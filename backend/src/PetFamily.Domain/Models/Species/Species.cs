using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Species
{
    public class Species : Entity
    {
        public Species(long id, string name, List<Breed> breeds)
        {
            Id = id;
            Name = name;
            Breeds = breeds;
        }
        public string Name { get; private set; } = default!;
        public List<Breed> Breeds { get; private set; } = new();
    }
}
