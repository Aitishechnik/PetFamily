using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.FileManagement.Delete;
using PetFamily.SharedKernal;

namespace PetFamily.Volonteers.Application.Commands.RemovePetPhotos
{
    public class RemovePetPhotosHandler : ICommandHandler<RemovePetPhotosCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly DeleteFilesHandler _deleteFilesHandler;
        private readonly IValidator<RemovePetPhotosCommand> _validator;
        private readonly ILogger<RemovePetPhotosHandler> _logger;

        public RemovePetPhotosHandler(
            [FromKeyedServices(Modules.Volonteers)] IUnitOfWork unitOfWork,
            IVolonteersRepository volonteersRepository,
            DeleteFilesHandler deleteFilesHandler,
            IValidator<RemovePetPhotosCommand> validator,
            ILogger<RemovePetPhotosHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _volonteersRepository = volonteersRepository;
            _deleteFilesHandler = deleteFilesHandler;
            _validator = validator;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorList>> Handle(
            RemovePetPhotosCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = _validator.Validate(command);

            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volonteerResult = await _volonteersRepository.GetById(
                command.VolonteerId,
                cancellationToken);
            if (volonteerResult.IsFailure)
                return volonteerResult.Error.ToErrorList();

            var volonteer = volonteerResult.Value;

            var petResult = volonteer.GetPetById(command.PetId);
            if (petResult.IsFailure)
                return petResult.Error.ToErrorList();

            var pet = petResult.Value;

            var paths = command.FileInfoPath.Select(fileInfo => fileInfo.FilePath);

            foreach (var path in paths)
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

                var deleteFilesRequest = new DeleteFilesCommand(command.FileInfoPath);

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
