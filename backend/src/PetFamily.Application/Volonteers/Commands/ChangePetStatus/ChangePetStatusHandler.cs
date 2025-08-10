using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Commands.ChangePetStatus
{
    public class ChangePetStatusHandler : ICommandHandler<ChangePetStatusCommand>
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ChangePetStatusCommand> _validator;
        private readonly ILogger<ChangePetStatusHandler> _logger;

        public ChangePetStatusHandler(
            IVolonteersRepository volonteersRepository,
            IUnitOfWork unitOfWork,
            IValidator<ChangePetStatusCommand> validator,
            ILogger<ChangePetStatusHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorList>> Handle(
            ChangePetStatusCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(
                command,
                cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation failed for command: {Command}", command);
                return validationResult.ToErrorList();
            }

            var volonteerResult = await _volonteersRepository.GetById(
                command.VolonteerId, cancellationToken);
            if (volonteerResult.IsFailure)
            {
                _logger.LogError("Volonteer with ID {VolonteerId} not found", command.VolonteerId);
                return volonteerResult.Error.ToErrorList();
            }

            var volonteer = volonteerResult.Value;

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var result = volonteer.ChangePetStatus(
                    command.PetId,
                    command.NewPetStatus);
                if (result.IsFailure)
                {
                    _logger.LogError("Failed to change pet status for PetId {PetId} by VolonteerId {VolonteerId}: {ErrorMessage}",
                        command.PetId, command.VolonteerId, result.Error.Message);
                    return result.Error.ToErrorList();
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Commit();

                _logger.LogInformation("Successfully changed pet status for PetId {PetId} by VolonteerId {VolonteerId}",
                    command.PetId, command.VolonteerId);

                return UnitResult.Success<ErrorList>();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error changing pet status for PetId {PetId} by VolonteerId {VolonteerId}",
                    command.PetId, command.VolonteerId);
                return Error.Failure("internal.error", "Error changing pet status")
                    .ToErrorList();
            }
        }
    }
}
