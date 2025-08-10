using CSharpFunctionalExtensions;
using PetFamily.SharedKernal;

namespace PetFamily.Species.Domain.Entities
{
    public class Species : Entity<Guid>
    {
        protected Species() { }
        public Species(string name)
        {
            Name = name;
        }
        public string Name { get; private set; } = default!;
        public IReadOnlyList<Breed> Breeds => _breeds;
        private readonly List<Breed> _breeds = [];

        public UnitResult<Error> AddBreeds(IEnumerable<Breed> breeds)
        {
            if (breeds.Distinct().Count() != breeds.Count())
                return Error.Validation("not.unique.elemets", "Breeds must be unique");

            foreach (var breed in breeds)
            {
                if (_breeds.Contains(breed))
                {
                    return Error.Conflict("breed.already.exists", $"Breed {breed.Name} already exists");
                }
            }

            _breeds.AddRange(breeds);

            return UnitResult.Success<Error>();
        }
    }
}