using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Commands.SetPetMainPhoto
{
    public class SetPetMainPhotoHandler : ICommandHandler<SetPetMainPhotoCommand>
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<SetPetMainPhotoCommand> _validator;
        private readonly ILogger<SetPetMainPhotoHandler> _logger;

        public SetPetMainPhotoHandler(
            IVolonteersRepository volonteersRepository,
            IUnitOfWork unitOfWork,
            IValidator<SetPetMainPhotoCommand> validator,
            ILogger<SetPetMainPhotoHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorList>> Handle(
            SetPetMainPhotoCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for command: {Command}. Errors: {Errors}",
                    command, validationResult.Errors);
                return validationResult.ToErrorList();
            }

            var volonteerResult = await _volonteersRepository.GetById(
                command.VolonteerId, cancellationToken);
            if (volonteerResult.IsFailure)
            {
                _logger.LogError("Volonteer with ID {VolonteerId} not found.", command.VolonteerId);
                return volonteerResult.Error.ToErrorList();
            }

            var volonteer = volonteerResult.Value;

            var petResult = volonteer.GetPetById(command.PetId);
            if (petResult.IsFailure)
            {
                _logger.LogError("Pet with ID {PetId} not found for Volonteer {VolonteerId}.",
                    command.PetId, command.VolonteerId);
                return petResult.Error.ToErrorList();
            }

            var pet = petResult.Value;

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var setMainPhotoResult = pet
                    .SetMainPhoto(FilePath.Create(command.MainPhotoPath).Value);
                if (setMainPhotoResult.IsFailure)
                {
                    _logger.LogError("Failed to set main photo for pet with ID {PetId}. Error: {ErrorMessage}",
                        command.PetId, setMainPhotoResult.Error.Message);
                    return setMainPhotoResult.Error.ToErrorList();
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Commit();

                _logger.LogInformation("Main photo set successfully for pet with ID {PetId}.", command.PetId);

                return UnitResult.Success<ErrorList>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while setting the main photo for pet with ID {PetId}.", command.PetId);
                return Errors.General
                    .ValueIsInvalid("setting main photo for pet")
                    .ToErrorList();
            }
        }
    }
}
