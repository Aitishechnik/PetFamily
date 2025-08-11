using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernal;
using PetFamily.Volonteers.Domain.ValueObjects;

namespace PetFamily.Volonteers.Application.Commands.ShiftPetPosition
{
    public class ShiftPetPositionHandler : ICommandHandler<ShiftPetPositionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IValidator<ShiftPetPositionCommand> _validator;
        private readonly ILogger<ShiftPetPositionHandler> _logger;

        public ShiftPetPositionHandler(
            [FromKeyedServices(Modules.Volonteers)] IUnitOfWork unitOfWork,
            IVolonteersRepository volonteersRepository,
            IValidator<ShiftPetPositionCommand> validator,
            ILogger<ShiftPetPositionHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _volonteersRepository = volonteersRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorList>> Handle(
            ShiftPetPositionCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(command);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volonteerResult = await _volonteersRepository.GetById(command.VolonteerId);
            if (volonteerResult.IsFailure)
                return volonteerResult.Error.ToErrorList();

            var petResult = volonteerResult.Value.GetPetById(command.PetId);
            if (petResult.IsFailure)
                return petResult.Error.ToErrorList();

            var newPositionResult = SerialNumber.Create(command.NewPosition);
            if (newPositionResult.IsFailure)
                return newPositionResult.Error.ToErrorList();

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var result = volonteerResult.Value.MovePet(
                    petResult.Value.SerialNumber,
                    newPositionResult.Value);

                if (result.IsFailure)
                    return result.Error.ToErrorList();

                await _unitOfWork.SaveChangesAsync();
                transaction.Commit();

                return Result.Success<ErrorList>();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(
                    ex,
                    "Error occurred while changing pet position");
                return Errors.General
                    .ValueIsInvalid(ex.Message)
                    .ToErrorList();
            }
        }
    }
}
