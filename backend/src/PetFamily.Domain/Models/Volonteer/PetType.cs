using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volonteer
{
    public record PetType
    {
        private PetType(Guid speciesId, Guid breedId)
        {
            SpeciesId = speciesId;
            BreedId = breedId;
        }

        public Guid SpeciesId { get; }
        public Guid BreedId { get; }

        public static Result<PetType, Error> Create(Guid speciesId, Guid breedId)
        {
            if (speciesId == Guid.Empty)
                return Errors.General.ValueIsInvalid("SpeciedId");
            if (breedId == Guid.Empty)
                return Errors.General.ValueIsInvalid("BreedId");

            return new PetType(speciesId, breedId);
        }
    }
}
