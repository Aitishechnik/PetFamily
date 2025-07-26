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

        public static Result<PetType, Error> Create(Guid speciesID, Guid breedID)
        {
            if (speciesID == Guid.Empty)
                return Errors.General.ValueIsInvalid("SpeciedID");
            if (breedID == Guid.Empty)
                return Errors.General.ValueIsInvalid("BreedID");

            return new PetType(speciesID, breedID);
        }
    }
}
