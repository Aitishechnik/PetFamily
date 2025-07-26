using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volonteers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Commands
{
    public class RemoveSpeciesAndBreedsHandler : ICommandHandler<RemoveSpeciesAndBreedsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IVolonteersRepository _volonteersRepository;
        private readonly IValidator<RemoveSpeciesAndBreedsCommand> _validator;
        private readonly ILogger<RemoveSpeciesAndBreedsHandler> _logger;

        public RemoveSpeciesAndBreedsHandler(
            IUnitOfWork unitOfWork,
            ISpeciesRepository speciesRepository,
            IVolonteersRepository volonteersRepository,
            IValidator<RemoveSpeciesAndBreedsCommand> validator,
            ILogger<RemoveSpeciesAndBreedsHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _speciesRepository = speciesRepository;
            _volonteersRepository = volonteersRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorList>> Handle(
            RemoveSpeciesAndBreedsCommand command, 
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var speciedResult = await _speciesRepository
                .IsSpeciesAndBreedExists(command.SpeciesId, command.BreedId, cancellationToken);

            if (speciedResult.IsFailure)
            {
                _logger.LogError("Species or breed not found: {Error}", speciedResult.Error);
                return speciedResult.Error.ToErrorList();
            }

            var pets = await _volonteersRepository.GetAllPets();

            if (pets.Any())
            {
                var petBelongsToSpecies = pets
                    .Any(p => p.PetType.SpeciesID == command.SpeciesId);

                if (petBelongsToSpecies)
                    {
                    _logger.LogError("Cannot remove species or breed because pets belong to it.");
                    return Errors.General.ValueIsInvalid("species or breed").ToErrorList();
                }
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var result = await _speciesRepository.RemoveSpecies(command.SpeciesId);
                if (result.IsFailure)
                {
                    _logger.LogError("Failed to remove species: {Error}", result.Error);
                    transaction.Rollback();
                    return result.Error.ToErrorList();
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Commit();

                _logger.LogInformation("Species and breeds removed successfully.");

                return UnitResult.Success<ErrorList>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing species and breeds.");
                transaction.Rollback();
                return Error.Failure("internal.error", ex.Message).ToErrorList();
            }
        }
    }
}
