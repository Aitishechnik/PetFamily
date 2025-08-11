using FluentValidation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Commands.AddSpecies
{
    public class AddSpeciesCommandValidator : AbstractValidator<AddSpeciesCommand>
    {
        public AddSpeciesCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty()
                .MaximumLength(Constants.MAX_NAME_LENGTH);
        }
    }
}
