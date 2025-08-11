using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernal;
using PetFamily.Species.Domain.Entities;

namespace PetFamily.Species.Application.Commands.AddBreed
{
    public class AddBreedHandler : ICommandHandler<Guid, AddBreedCommand>
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddBreedCommand> _validator;
        private readonly ILogger<AddBreedHandler> _logger;

        public AddBreedHandler(
            ISpeciesRepository speciesRepository,
            [FromKeyedServices(Modules.Species)] IUnitOfWork unitOfWork,
            IValidator<AddBreedCommand> validator,
            ILogger<AddBreedHandler> logger)
        {
            _speciesRepository = speciesRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            AddBreedCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(
                command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for AddBreedCommand: {Errors}", validationResult.Errors);
                return validationResult.ToErrorList();
            }

            var speciesResult = await _speciesRepository.GetSpeciesById(command.speciesId, cancellationToken);
            if (speciesResult.IsFailure)
            {
                _logger.LogWarning(
                    "Species with ID {SpeciesId} not found",
                    command.speciesId);
                return speciesResult.Error.ToErrorList();
            }

            using var transaction = await _unitOfWork
                .BeginTransactionAsync(cancellationToken);

            try
            {
                var result = speciesResult.Value.AddBreeds([new Breed(command.Name)]);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Commit();

                _logger.LogInformation(
                    "Breed '{BreedName}' added to species with ID {SpeciesId}",
                    command.Name,
                    command.speciesId);

                var breedId = speciesResult.Value.Breeds
                    .FirstOrDefault(b => b.Name == command.Name)!.Id;

                return breedId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(
                    ex,
                    "An error occurred while adding breed '{BreedName}' to species with ID {SpeciesId}",
                    command.Name,
                    command.speciesId);
                return Error.Failure("internal.error", "An unexpected error occurred while adding the breed").ToErrorList();
            }
        }
    }
}
