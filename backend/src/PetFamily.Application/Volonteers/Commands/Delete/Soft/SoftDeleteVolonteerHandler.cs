using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Commands.Delete.Soft
{
    public class SoftDeleteVolonteerHandler : ICommandHandler<Guid, SoftDeleteVolonteerCommand>
    {
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<DeleteVolonteerCommand> _validator;
        private readonly ILogger<SoftDeleteVolonteerHandler> _logger;

        public SoftDeleteVolonteerHandler(
            IVolonteersRepository volonteersRepository,
            IUnitOfWork unitOfWork,
            IValidator<DeleteVolonteerCommand> validator,
            ILogger<SoftDeleteVolonteerHandler> logger)
        {
            _volonteersRepository = volonteersRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            
            SoftDeleteVolonteerCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(
                command,
                cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var result = await _volonteersRepository.GetById(command.VolonteerId);
            if(result.IsFailure)
                return result.Error.ToErrorList();

            if (result.Value.IsDeleted)
                return Errors.General
                    .AlreadyDeleted(command.VolonteerId)
                    .ToErrorList();

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                result.Value.Delete();

                await _unitOfWork.SaveChangesAsync();

                transaction.Commit();

                _logger.LogInformation("Volonteer with id {request.VolonteerId) was softly deleted}", command.VolonteerId);

                return result.Value.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(
                    ex, 
                    "Error occurred while soft deleting volonteer with id {request.VolonteerId}"
                    , command.VolonteerId);
                return Errors.General
                    .ValueIsInvalid("volonteer soft deletion")
                    .ToErrorList();
            }
        }        
    }
}
