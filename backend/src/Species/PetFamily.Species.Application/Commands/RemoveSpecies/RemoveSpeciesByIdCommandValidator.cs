using FluentValidation;

namespace PetFamily.Species.Application.Commands.RemoveSpecies
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
