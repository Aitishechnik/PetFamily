using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileManagement.Delete;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.RemovePetPhotos
{
    public class RemovePetPhotosHandler
    {
        IUnitOfWork _unitOfWork;
        IVolonteersRepository _volonteersRepository;
        DeleteFilesHandler _deleteFilesHandler;
        ILogger<RemovePetPhotosHandler> _logger;
        public RemovePetPhotosHandler(
            IUnitOfWork unitOfWork,
            IVolonteersRepository volonteersRepository,
            DeleteFilesHandler deleteFilesHandler,
            ILogger<RemovePetPhotosHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _volonteersRepository = volonteersRepository;
            _deleteFilesHandler = deleteFilesHandler;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorList>> Handle(
            IValidator<RemovePetPhotosCommand> validator,
            RemovePetPhotosCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = validator.Validate(command);

            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();


            var volonteerResult = await _volonteersRepository.GetById(command.VolonteerId, cancellationToken);
            if(volonteerResult.IsFailure)
                return volonteerResult.Error.ToErrorList();

            var volonteer = volonteerResult.Value;

            var petResult = volonteer.GetPetById(command.PetId);
            if (petResult.IsFailure)
                return petResult.Error.ToErrorList();

            var pet = petResult.Value;

            var paths = command.FileInfo.Select(fileInfo => fileInfo.FilePath);

            foreach(var path in paths)
            {
                if (pet.PetPhotos.Any(p => p == path) == false)
                    return Errors.General
                        .ValueIsInvalid("Pet photo path is not valid")
                        .ToErrorList();
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                pet.RemovePhotos(paths);

                await _unitOfWork.SaveChangesAsync();

                var deleteFilesRequest = new DeleteFilesRequest(command.FileInfo);

                var deletionResult = await _deleteFilesHandler.Handle(deleteFilesRequest, cancellationToken);

                if (deletionResult.IsFailure)
                {
                    transaction.Rollback();
                    _logger.LogError("Failed to delete pet photos: {ErrorMessage}", deletionResult.Error.Message);
                    return deletionResult.Error.ToErrorList();
                }

                transaction.Commit();
                
                return Result.Success<ErrorList>();
            }
            catch
            {
                transaction.Rollback();
                
                return Errors.General
                    .ValueIsInvalid("Failed to remove pet photos")
                    .ToErrorList();
            }
        }
    }
}
