using FluentValidation;
using PetFamily.SharedKernal;

namespace PetFamily.Species.Application.Commands.AddSpecies
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
