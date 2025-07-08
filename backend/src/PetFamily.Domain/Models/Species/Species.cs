using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Species
{
    public class Species : Entity<Guid>
    {
        protected Species() { }
        public Species(Guid id, string name, List<Breed> breeds)
        {
            Id = id;
            Name = name;
            _breeds = breeds;
        }
        public string Name { get; private set; } = default!;
        public IReadOnlyList<Breed> Breeds => _breeds;
        private readonly List<Breed> _breeds = [];
    }
}