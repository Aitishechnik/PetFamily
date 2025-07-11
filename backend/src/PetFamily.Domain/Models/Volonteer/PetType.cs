using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volonteer
{
    public record PetType
    {
        private PetType(Guid speciesID, Guid breedID)
        {
            SpeciesID = speciesID;
            BreedID = breedID;
        }

        public Guid SpeciesID { get; }
        public Guid BreedID { get; }

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
