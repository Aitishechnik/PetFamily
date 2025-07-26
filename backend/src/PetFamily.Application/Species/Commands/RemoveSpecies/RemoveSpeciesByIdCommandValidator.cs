using FluentValidation;

namespace PetFamily.Application.Species.Commands.RemoveSpecies
{
    public class RemoveSpeciesByIdCommandValidator : AbstractValidator<RemoveSpeciesByIdCommand>
    {
        public RemoveSpeciesByIdCommandValidator()
        {
            RuleFor(command => command.SpeciesId)
                .NotEmpty();
        }
    }
}
