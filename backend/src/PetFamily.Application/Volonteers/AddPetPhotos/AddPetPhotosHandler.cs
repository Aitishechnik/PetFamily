using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileManagement.Add;
using PetFamily.Contracts;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.AddPetPhotos
{
    public class AddPetPhotosHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AddFilesHandler _addFilesHandler;
        private readonly ILogger<AddPetPhotosHandler> _logger;

        public AddPetPhotosHandler(
            IVolonteersRepository volonteersRepository,
            IUnitOfWork unitOfWork,
            AddFilesHandler addFilesHandler,
            ILogger<AddPetPhotosHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _addFilesHandler = addFilesHandler;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyList<FilePath>, Error>> Handle(
            AddPetPhotosRequest request,
            CancellationToken cancellationToken = default)
        {
            var transaction = await _unitOfWork.BeginTransaction();

            try
            {
                var volonteerResult = await _volonteersRepository.GetById(request.VolonteerId);

                if (volonteerResult.IsFailure)
                {
                    transaction.Rollback();
                    return volonteerResult.Error;
                }

                var volonteer = volonteerResult.Value;

                var petResult = volonteer.GetPetById(request.PetId);

                if (petResult.IsFailure)
                {
                    transaction.Rollback();
                    return petResult.Error;
                }

                var pet = petResult.Value;

                var filePathList = GenerateFilePathList(
                    volonteer.Id,
                    pet.Id,
                    pet.PetPhotos.Count,
                    request.Content.Count(),
                    Constants.PHOTO_FILE_EXTENSION);

                var addPhotoResult = volonteer.AddPetPhotos(
                    pet.Id, filePathList);

                if (addPhotoResult.IsFailure)
                {
                    transaction.Rollback();
                    return addPhotoResult.Error;
                }

                //_unitOfWork.EntryChangeStateOnModified(volonteer);
                _unitOfWork.EntryChangeStateOnModified(pet);

                await _unitOfWork.SaveChanges();

                var fileDTOsResult = GenerateLileDTOList(
                    request.Content,
                    filePathList);

                if(fileDTOsResult.IsFailure)
                {
                    transaction.Rollback();
                    return fileDTOsResult.Error;
                }

                var result = await _addFilesHandler.Handle(
                    new AddFilesRequest(fileDTOsResult.Value),
                    cancellationToken);

                if (result.IsFailure)
                {
                    transaction.Rollback();
                    return result.Error;
                }

                return filePathList;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex.Message);
                return Errors.General.ValueIsInvalid(ex.Message);
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
