using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Extensions;
using PetFamily.Core.FileManagement.Add;
using PetFamily.Core.FileManagment.Files;
using PetFamily.Core.Messaging;
using PetFamily.SharedKernal;
using FilePath = PetFamily.SharedKernal.FilePath;

namespace PetFamily.Volonteers.Application.Commands.AddPetPhotos
{
    public class AddPetPhotosHandler : ICommandHandler<IReadOnlyList<FilePath>, AddPetPhotosCommand>
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AddFilesHandler _addFilesHandler;
        private readonly IValidator<AddPetPhotosCommand> _validator;
        private readonly IMessageQueue<IEnumerable<FileInfoPath>> _messageQueue;
        private readonly ILogger<AddPetPhotosHandler> _logger;

        public AddPetPhotosHandler(
            IVolonteersRepository volonteersRepository,
            [FromKeyedServices(Modules.Volonteers)] IUnitOfWork unitOfWork,
            AddFilesHandler addFilesHandler,
            IValidator<AddPetPhotosCommand> validator,
            IMessageQueue<IEnumerable<FileInfoPath>> messageQueue,
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
                pet.GetLastPhotoIndex() + 1,
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

                var fileDTOsResult = GenerateFileDTOList(
                    command.Content,
                    filePathList);

                if (fileDTOsResult.IsFailure)
                    return fileDTOsResult.Error.ToErrorList();

                var result = await _addFilesHandler.Handle(
                    new AddFilesCommand(fileDTOsResult.Value),
                    cancellationToken);

                if (result.IsFailure)
                {
                    await _messageQueue.WriteAsync(
                        fileDTOsResult.Value.Select(
                            dto => new FileInfoPath(
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

            for (int i = startCountIndex; i < startCountIndex + filesCount; i++)
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

        private Result<List<FileDto>, Error> GenerateFileDTOList(
            IEnumerable<Stream> content,
            IEnumerable<FilePath> filePaths)
        {
            if (content.Count() != filePaths.Count())
                return Errors.General.ValueIsInvalid("Content and file paths count mismatch.");

            var fileDTOs = new List<FileDto>();

            for (int i = 0; i < content.Count(); i++)
            {
                var fileDTO = new FileDto(content.ElementAt(i), filePaths.ElementAt(i).Path);
                fileDTOs.Add(fileDTO);
            }

            return fileDTOs;
        }
    }
}
