using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
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

        public async Task<UnitResult<Error>> Handle(
            RemovePetPhotosRequest request,
            CancellationToken cancellationToken = default)
        {
            var transaction = await _unitOfWork.BeginTransaction();
            try
            {
                var volonteerResult = await _volonteersRepository.GetById(request.VolonteerId, cancellationToken);
                if(volonteerResult.IsFailure)
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
                var t1 = _unitOfWork.ChangeTrackerEntry();
                foreach(var path in request.Paths)
                {
                    if (pet.PetPhotos.Any(p => p == path) == false)
                    {
                        transaction.Rollback();
                        return Errors.General.ValueIsInvalid("Pet photo path is not valid");
                    }
                }

                pet.RemovePhotos(request.Paths);
                var t2 = _unitOfWork.ChangeTrackerEntry();

                await _unitOfWork.SaveChanges();
                var t3 = _unitOfWork.ChangeTrackerEntry();

                var deleteFilesRequest = new DeleteFilesRequest(request.Paths);

                var deletionResult = await _deleteFilesHandler.Handle(deleteFilesRequest, cancellationToken);

                if (deletionResult.IsFailure)
                {
                    transaction.Rollback();
                    _logger.LogError("Failed to delete pet photos: {ErrorMessage}", deletionResult.Error.Message);
                    return deletionResult.Error;
                }

                transaction.Commit();
                
                return Result.Success<Error>();
            }
            catch
            {
                transaction.Rollback();
                
                return Errors.General.ValueIsInvalid("Failed to remove pet photos");
            }
        }
    }
}
