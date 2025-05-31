using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volonteer
{
    public record BreedAndSpecies
    {
        public string Breed { get; } = default!;
        public string Species { get; } = default!;

        private BreedAndSpecies(string breed, string species)
        {
            Breed = breed;
            Species = species;
        }

        public static Result<BreedAndSpecies, Error> Create(string breed, string species)
        {
            if (string.IsNullOrWhiteSpace(breed) ||
                breed.Length > Constants.MAX_NAME_LENGTH)
                return Errors.General.ValueIsInvalid("Breed");
            if (string.IsNullOrWhiteSpace(species) ||
                species.Length > Constants.MAX_NAME_LENGTH)
                return Errors.General.ValueIsInvalid("Species");

            return new BreedAndSpecies(breed, species);
        }
    }
}
