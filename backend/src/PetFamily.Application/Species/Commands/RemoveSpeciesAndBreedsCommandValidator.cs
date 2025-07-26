using FluentValidation;

namespace PetFamily.Application.Species.Commands
{
    public class RemoveSpeciesAndBreedsCommandValidator : AbstractValidator<RemoveSpeciesAndBreedsCommand>
    {
        public RemoveSpeciesAndBreedsCommandValidator()
        {
            RuleFor(command => command.SpeciesId)
                .NotEmpty();

            RuleFor(command => command.BreedId)
                .NotEmpty();
        }
    }
}
