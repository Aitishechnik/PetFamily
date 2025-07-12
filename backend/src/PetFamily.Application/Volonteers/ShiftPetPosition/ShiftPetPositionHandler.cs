using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.ShiftPetPosition
{
    public class ShiftPetPositionHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly ILogger<ShiftPetPositionHandler> _logger;

        public ShiftPetPositionHandler(
            IUnitOfWork unitOfWork, 
            IVolonteersRepository volonteersRepository, 
            ILogger<ShiftPetPositionHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _volonteersRepository = volonteersRepository;
            _logger = logger;
        }

        public async Task<UnitResult<Error>> Handle(
            ShiftPetPositionRequest request,
            CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransaction();

            var volonteerResult = await _volonteersRepository.GetById(request.VoloteerId);
            if (volonteerResult.IsFailure)
                return volonteerResult.Error;

            var petResult = volonteerResult.Value.GetPetById(request.PetId);
            if (petResult.IsFailure)
                return petResult.Error;

            var newPositionResult = SerialNumber.Create(request.NewPosition);
            if (newPositionResult.IsFailure)
                return newPositionResult.Error;

            try
            {
                var result = volonteerResult.Value.MovePet(
                    petResult.Value.SerialNumber, 
                    newPositionResult.Value);

                if (result.IsFailure)
                    return result.Error;

                await _unitOfWork.SaveChanges();
                transaction.Commit();

                return Result.Success<Error>();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(
                    ex, 
                    "Error occurred while changing pet position");
                return Errors.General.ValueIsInvalid(ex.Message);
            }
        }
    }
}
