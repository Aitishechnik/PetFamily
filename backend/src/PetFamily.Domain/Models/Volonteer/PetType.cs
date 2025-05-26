using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volonteer
{
    public record PetType
    {
        private PetType(long speciesID, long breedID)
        {
            SpeciesID = speciesID;
            BreedID = breedID;
        }

        public long SpeciesID { get; }
        public long BreedID { get; }

        public static Result<PetType, Error> Create(long speciesID, long breedID)
        {
            if (speciesID <= 0)
                return Errors.General.ValueIsInvalid("SpeciedID");
            if (breedID <= 0)
                return Errors.General.ValueIsInvalid("BreedID");

            return new PetType(speciesID, breedID);
        }
    }
}
