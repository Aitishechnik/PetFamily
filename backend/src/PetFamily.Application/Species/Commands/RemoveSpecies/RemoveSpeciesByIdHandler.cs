using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Commands.RemoveSpecies
{
    public class RemoveSpeciesByIdHandler : ICommandHandler<RemoveSpeciesByIdCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IReadDbContext _readDbContext;
        private readonly IValidator<RemoveSpeciesByIdCommand> _validator;
        private readonly ILogger<RemoveSpeciesByIdHandler> _logger;

        public RemoveSpeciesByIdHandler(
            IUnitOfWork unitOfWork,
            ISpeciesRepository speciesRepository,
            IReadDbContext readDbContext,
            IValidator<RemoveSpeciesByIdCommand> validator,
            ILogger<RemoveSpeciesByIdHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _speciesRepository = speciesRepository;
            _readDbContext = readDbContext;
            _validator = validator;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorList>> Handle(
            RemoveSpeciesByIdCommand command, 
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
            {
                _logger.LogError("Validation error");
                return validationResult.ToErrorList();
            }

            var speciesExists = await _speciesRepository.GetSpeciesById(command.SpeciesId);

            if (speciesExists.IsFailure)
            {
                _logger.LogError("Species not found");
                return speciesExists.Error.ToErrorList();
            }

            var isPetWithSpecies = await _readDbContext.Pets.AnyAsync(
                p => p.SpeciesId == command.SpeciesId, 
                cancellationToken);

            if (isPetWithSpecies)
            {
                _logger.LogError("Cannot remove species with existing pets");
                return Errors.General.ValueIsInvalid(
                    "Cannot remove species with existing pets").ToErrorList();
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var result = await _speciesRepository.RemoveSpecies(command.SpeciesId);
                if (result.IsFailure)
                {
                    _logger.LogError("Failed to remove species");
                    return result.Error.ToErrorList();
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                transaction.Commit();

                return UnitResult.Success<ErrorList>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing species");
                transaction.Rollback();
                return Error.Failure("interna.error.", ex.Message).ToErrorList();
            }
        }
    }
}
