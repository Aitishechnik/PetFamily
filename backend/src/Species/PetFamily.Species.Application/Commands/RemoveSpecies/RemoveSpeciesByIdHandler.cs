using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernal;
using PetFamily.Volonteers.Contracts;
using PetFamily.Volonteers.Contracts.Requests;

namespace PetFamily.Species.Application.Commands.RemoveSpecies
{
    public class RemoveSpeciesByIdHandler : ICommandHandler<RemoveSpeciesByIdCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IVolonteerContract _volonteerContract;
        private readonly IValidator<RemoveSpeciesByIdCommand> _validator;
        private readonly ILogger<RemoveSpeciesByIdHandler> _logger;

        public RemoveSpeciesByIdHandler(
            IUnitOfWork unitOfWork,
            ISpeciesRepository speciesRepository,
            IVolonteerContract volonteerContract,
            IValidator<RemoveSpeciesByIdCommand> validator,
            ILogger<RemoveSpeciesByIdHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _speciesRepository = speciesRepository;
            _volonteerContract = volonteerContract;
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

            var isPetWithSpecies = await _volonteerContract.HasAnimalsWithSpecies(
                new HasPetsWithSpeciesRequest(command.SpeciesId),
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
