using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Commands.PetDelete.Soft
{
    public class SoftDeletePetHandler : ICommandHandler<DeletePetCommand>
    {
        private readonly IVolonteersRepository _volonteerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<DeletePetCommand> _validator;
        private readonly ILogger<SoftDeletePetHandler> _logger;

        public SoftDeletePetHandler(
            IVolonteersRepository volonteerRepository,
            IUnitOfWork unitOfWork,
            IValidator<DeletePetCommand> validator,
            ILogger<SoftDeletePetHandler> logger)
        {
            _volonteerRepository = volonteerRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorList>> Handle(
            DeletePetCommand command, 
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for command: {Command}. Errors: {Errors}", 
                    command, validationResult.Errors);
                return validationResult.ToErrorList();
            }
            
            var volonteerResult = await _volonteerRepository.GetById(
                command.VolonteerId, cancellationToken);
            if (volonteerResult.IsFailure)
            {
                _logger.LogError("Volonteer with ID {VolonteerId} not found.", command.VolonteerId);
                return volonteerResult.Error.ToErrorList();
            }

            var petResult = volonteerResult.Value.GetPetById(command.PetId);
            if (petResult.IsFailure)
            {
                _logger.LogError("Pet with ID {PetId} not found in volonteer {VolonteerId}.", 
                    command.PetId, command.VolonteerId);
                return petResult.Error.ToErrorList();
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                petResult.Value.Delete();

                await _unitOfWork.SaveChangesAsync();

                transaction.Commit();

                _logger.LogInformation("Successfully soft deleted pet with ID {PetId} for volonteer {VolonteerId}.", 
                    command.PetId, command.VolonteerId);

                return Result.Success<ErrorList>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while soft deleting pet with ID {PetId} for volonteer {VolonteerId}.", 
                    command.PetId, command.VolonteerId);
                return Error.Failure("internal.error", "An error occurred while soft deleting pet").ToErrorList();
            }
        }
    }
}
