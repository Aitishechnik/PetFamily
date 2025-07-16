using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileManagement.Add;
using PetFamily.Application.Messaging;
using PetFamily.Contracts;
using PetFamily.Domain.Shared;
using FileInfo = PetFamily.Application.FileManagment.Files.FileInfo;

namespace PetFamily.Application.Volonteers.AddPetPhotos
{
    public class AddPetPhotosHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AddFilesHandler _addFilesHandler;
        private readonly IValidator<AddPetPhotosCommand> _validator;
        private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
        private readonly ILogger<AddPetPhotosHandler> _logger;

        public AddPetPhotosHandler(
            IVolonteersRepository volonteersRepository,
            IUnitOfWork unitOfWork,
            AddFilesHandler addFilesHandler,
            IValidator<AddPetPhotosCommand> validator,
            IMessageQueue<IEnumerable<FileInfo>> messageQueue,
            ILogger<AddPetPhotosHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _addFilesHandler = addFilesHandler;
            _validator = validator;
            _unitOfWork = unitOfWork;
            _messageQueue = messageQueue;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyList<FilePath>, ErrorList>> Handle(
            AddPetPhotosCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volonteerResult = await _volonteersRepository.GetById(command.VolonteerId);

            if (volonteerResult.IsFailure)
                return volonteerResult.Error.ToErrorList();

            var volonteer = volonteerResult.Value;

            var petResult = volonteer.GetPetById(command.PetId);

            if (petResult.IsFailure)
                return petResult.Error.ToErrorList();

            var pet = petResult.Value;

            var filePathList = GenerateFilePathList(
                volonteer.Id,
                pet.Id,
                pet.GetLastPhotoIndex()+1,
                command.Content.Count(),
                Constants.PHOTO_FILE_EXTENSION);

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var addPhotoResult = volonteer.AddPetPhotos(
                    pet.Id, filePathList);

                if (addPhotoResult.IsFailure)
                    return addPhotoResult.Error.ToErrorList();

                await _unitOfWork.SaveChangesAsync();

                var fileDTOsResult = GenerateLileDTOList(
                    command.Content,
                    filePathList);

                if(fileDTOsResult.IsFailure)
                    return fileDTOsResult.Error.ToErrorList();

                var result = await _addFilesHandler.Handle(
                    new AddFilesRequest(fileDTOsResult.Value),
                    cancellationToken);

                if (result.IsFailure)
                {
                    await _messageQueue.WriteAsync(
                        fileDTOsResult.Value.Select(
                            dto => new FileInfo(
                                command.Bucket, FilePath.Create(
                                    dto.FileName).Value)));

                    return result.Error.ToErrorList();
                }

                transaction.Commit();
                return filePathList;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex.Message);
                return Errors.General.ValueIsInvalid(ex.Message).ToErrorList();
            }
        }

        private List<FilePath> GenerateFilePathList(
            Guid volunteerId,
            Guid petId,
            int startCountIndex,
            int filesCount,
            string extension)
        {
            var result = new List<FilePath>();

            for (int i = startCountIndex; i < startCountIndex+filesCount; i++)
            {
                var fileName = $"{volunteerId}/{petId}/{i}{extension}";
                var filePath = FilePath.Create(fileName);
                if (filePath.IsFailure)
                {
                    _logger.LogError($"Failed to create file path: {filePath.Error.Message}");
                    continue;
                }
                result.Add(filePath.Value);
            }

            return result;
        }

        private Result<List<FileDTO>, Error> GenerateLileDTOList(
            IEnumerable<Stream> content,
            IEnumerable<FilePath> filePaths)
        {
            if(content.Count() != filePaths.Count())
                return Errors.General.ValueIsInvalid("Content and file paths count mismatch.");

            var fileDTOs = new List<FileDTO>();

            for(int i = 0; i < content.Count(); i++)
            {
                var fileDTO = new FileDTO(content.ElementAt(i), filePaths.ElementAt(i).Path);
                fileDTOs.Add(fileDTO);
            }

            return fileDTOs;
        }
    }
}
