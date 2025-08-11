using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernal;
using PetFamily.Volonteers.Contracts;
using PetFamily.Volonteers.Contracts.Requests;

namespace PetFamily.Species.Application.Commands.RemoveBreed
{
    public class RemoveBreedByIdHandler : ICommandHandler<RemoveBreedByIdCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IVolonteerContract _volonteerContract;
        private readonly IValidator<RemoveBreedByIdCommand> _validator;
        private readonly ILogger<RemoveBreedByIdHandler> _logger;

        public RemoveBreedByIdHandler(
            [FromKeyedServices(Modules.Species)] IUnitOfWork unitOfWork,
            ISpeciesRepository speciesRepository,
            IVolonteerContract volonteerContract,
            IValidator<RemoveBreedByIdCommand> validator,
            ILogger<RemoveBreedByIdHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _speciesRepository = speciesRepository;
            _volonteerContract = volonteerContract;
            _validator = validator;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorList>> Handle(
            RemoveBreedByIdCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
            {
                _logger.LogError("Validation error");
                return validationResult.ToErrorList();
            }

            var speciesExists = await _speciesRepository.GetBreedById(command.BreedId);
            if (speciesExists.IsFailure)
            {
                _logger.LogError("Breed not found");
                return speciesExists.Error.ToErrorList();
            }

            var isPetWithBreed = await _volonteerContract.HasAnimalsWithBreed(
                new HasPetsWithBreedRequest(command.BreedId),
                cancellationToken);

            if (isPetWithBreed)
            {
                _logger.LogError("Cannot remove beed with existing pets");
                return Errors.General.ValueIsInvalid(
                    "Cannot remove breed with existing pets").ToErrorList();
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var result = await _speciesRepository.RemoveBreed(
                    command.BreedId,
                    cancellationToken);

                if (result.IsFailure)
                {
                    _logger.LogError("Failed to remove breed");
                    return result.Error.ToErrorList();
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Commit();

                return UnitResult.Success<ErrorList>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while removing breed");
                transaction.Rollback();
                return Error.Failure("interna.error.", ex.Message).ToErrorList();
            }
        }
    }
}
