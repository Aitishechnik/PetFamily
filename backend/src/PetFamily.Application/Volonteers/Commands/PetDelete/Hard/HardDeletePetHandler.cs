using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileManagement.Delete;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Commands.PetDelete.Hard
{
    public class HardDeletePetHandler : ICommandHandler<HardDeletePetCommand>
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DeleteFilesHandler _deleteFilesHandler;
        private readonly IConfiguration _configuration;
        private readonly IValidator<DeletePetCommand> _validator;
        private readonly ILogger<HardDeletePetHandler> _logger;

        public HardDeletePetHandler(
            IVolonteersRepository volonteersRepository,
            IUnitOfWork unitOfWork,
            DeleteFilesHandler deleteFilesHandler,
            IConfiguration configuration,
            IValidator<DeletePetCommand> validator,
            ILogger<HardDeletePetHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _unitOfWork = unitOfWork;
            _deleteFilesHandler = deleteFilesHandler;
            _configuration = configuration;
            _validator = validator;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorList>> Handle(
            HardDeletePetCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = _validator.Validate(command);
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
                _logger.LogError("Pet with ID {PetId} not found in volonteer {VolonteerId}.",
                    command.PetId, command.VolonteerId);
                return petResult.Error.ToErrorList();
            }

            var pet = petResult.Value;

            if (pet.IsDeleted)
            {
                _logger.LogWarning("Pet with ID {PetId} is already deleted.", command.PetId);
                return Errors.General.ValueIsInvalid("Pet is already deleted").ToErrorList();
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                if (pet.PetPhotos.Count > 0)
                {
                    var bucketName = _configuration["MinioBuckets:Photos"];
                    if (string.IsNullOrEmpty(bucketName))
                    {
                        _logger.LogError("Bucket name for photos is not configured.");
                        return Error.Failure("configuration.error", "Bucket name for photos is not configured")
                            .ToErrorList();
                    }

                    var deleteFilesRequest = new DeleteFilesCommand(
                        pet.PetPhotos.Select(photo => new FileManagment.Files.FileInfoPath(bucketName, photo)));

                    var deletionResult = await _deleteFilesHandler.Handle(deleteFilesRequest, cancellationToken);
                    if (deletionResult.IsFailure)
                    {
                        transaction.Rollback();
                        _logger.LogError("Failed to delete pet photos: {ErrorMessage}", deletionResult.Error.Message);
                        return deletionResult.Error.ToErrorList();
                    }
                }

                volonteer.PetDelete(pet.Id);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Commit();

                _logger.LogInformation(
                    "Successfully hard deleted pet with ID {PetId} for volonteer {VolonteerId}.",
                    command.PetId, command.VolonteerId);
                return Result.Success<ErrorList>();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "An error occurred while hard deleting pet with ID {PetId} for volonteer {VolonteerId}.",
                    command.PetId, command.VolonteerId);
                return Error.Failure("internal.error", "An error occurred while hard deleting pet").ToErrorList();
            }
        }
    }
}
