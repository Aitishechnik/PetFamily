using CSharpFunctionalExtensions;

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

        public static Result<PetType> Create(long speciesID, long breedID)
        {
            if (speciesID <= 0)
                Result.Failure("Species ID cannot be less than or equal to 0");
            if (breedID <= 0)
                Result.Failure("Breed ID cannot be less than or equal to 0");
            var result = new PetType(speciesID, breedID);
            return Result.Success(result);
        }
    }
}
