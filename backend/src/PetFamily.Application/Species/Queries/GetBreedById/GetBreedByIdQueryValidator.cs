using FluentValidation;

namespace PetFamily.Application.Species.Queries.GetBreedById
{
    public class GetBreedByIdQueryValidator : AbstractValidator<GetBreedByIdQuery>
    {
        public GetBreedByIdQueryValidator()
        {
            RuleFor(g => g.BreedId)
                .NotEmpty();
        }
    }
}
