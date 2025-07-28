using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Commands.Delete.Hard
{
    public class HardDeleteVolonteerHandler : ICommandHandler<Guid, HardDeleteVolonteerCommand>
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<DeleteVolonteerCommand> _validator;
        private readonly ILogger<HardDeleteVolonteerHandler> _logger;

        public HardDeleteVolonteerHandler(
            IVolonteersRepository volonteersRepository,
            IUnitOfWork unitOfWork,
            IValidator<DeleteVolonteerCommand> validator,
            ILogger<HardDeleteVolonteerHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            HardDeleteVolonteerCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(
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
