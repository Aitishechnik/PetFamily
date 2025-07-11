using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Delete
{
    public class SoftDeleteVolonteerHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SoftDeleteVolonteerHandler> _logger;

        public SoftDeleteVolonteerHandler(
            IVolonteersRepository volonteersRepository,
            IUnitOfWork unitOfWork,
            ILogger<SoftDeleteVolonteerHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(
            DeleteVolonteerRequest request,
            CancellationToken cancellationToken = default)
        {
            var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

            try
            {
                var result = await _volonteersRepository.GetById(request.VolonteerId);
                if(result.IsFailure)
                    return result.Error;

                if (result.Value.IsDeleted)
                    return Errors.General.AlreadyDeleted(request.VolonteerId);

                result.Value.Delete();

                await _unitOfWork.SaveChanges();

                transaction.Commit();

                _logger.LogInformation("Volonteer with id {request.VolonteerId) was softly deleted}", request.VolonteerId);

                return result.Value.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(
                    ex, 
                    "Error occurred while soft deleting volonteer with id {request.VolonteerId}"
                    , request.VolonteerId);
                return Errors.General.ValueIsInvalid("volonteer soft deletion");
            }
        }        
    }
}
