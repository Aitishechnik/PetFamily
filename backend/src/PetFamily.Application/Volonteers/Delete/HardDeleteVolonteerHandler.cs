using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
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

        public async Task<Result<Guid, ErrorList>> Handle(
            IValidator<DeleteVolonteerCommand> validator,
            DeleteVolonteerCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await validator.ValidateAsync(
                command,
                cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var result = await _volonteersRepository.GetById(command.VolonteerId);
            if (result.IsFailure)
                return result.Error.ToErrorList();

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                await _volonteersRepository.Delete(result.Value);

                transaction.Commit();

                _logger.LogInformation("Volonteer with id {request.VolonteerId) was permenantly deleted}", command.VolonteerId);

                return result.Value.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting volonteer with id {request.VolonteerId}", command.VolonteerId);
                return Errors.General
                    .ValueIsInvalid("volonteer deletion")
                    .ToErrorList();
            }
        }
    }
}
