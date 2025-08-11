using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernal;

namespace PetFamily.Species.Application.Commands.AddSpecies
{
    public class AddSpeciesHandler : ICommandHandler<Guid, AddSpeciesCommand>
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddSpeciesCommand> _validator;
        private readonly ILogger<AddSpeciesHandler> _logger;

        public AddSpeciesHandler(
            ISpeciesRepository speciesRepository,
            [FromKeyedServices(Modules.Species)] IUnitOfWork unitOfWork,
            IValidator<AddSpeciesCommand> validator,
            ILogger<AddSpeciesHandler> logger)
        {
            _speciesRepository = speciesRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            AddSpeciesCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (validationResult.IsValid == false)
            {
                _logger.LogError("False validation");
                return validationResult.ToErrorList();
            }

            var species = new Domain.Entities.Species(command.Name);

            using var transation = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                await _speciesRepository.AddSpecies(species);
                await _unitOfWork.SaveChangesAsync();
                transation.Commit();
                return species.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Error.Failure("internal,error", ex.Message).ToErrorList();
            }
        }
    }
}
