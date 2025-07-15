using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Delete
{
    public class HardDeleteVolonteerHandler
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HardDeleteVolonteerHandler> _logger;

        public HardDeleteVolonteerHandler(
            IVolonteersRepository volonteersRepository,
            IUnitOfWork unitOfWork,
            ILogger<HardDeleteVolonteerHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(
            DeleteVolonteerRequest request,
            CancellationToken cancellationToken = default)
        {
            using var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

            try
            {
                var result = await _volonteersRepository.GetById(request.VolonteerId);
                if (result.IsFailure)
                    return result.Error;

                await _volonteersRepository.Delete(result.Value);

                transaction.Commit();

                _logger.LogInformation("Volonteer with id {request.VolonteerId) was permenantly deleted}", request.VolonteerId);

                return result.Value.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting volonteer with id {request.VolonteerId}", request.VolonteerId);
                return Errors.General.ValueIsInvalid("volonteer deletion");
            }
        }
    }
}
