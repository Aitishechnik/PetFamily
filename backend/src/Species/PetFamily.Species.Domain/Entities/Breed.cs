using CSharpFunctionalExtensions;

namespace PetFamily.Species.Domain.Entities
{
    public class Breed : Entity<Guid>
    {
        protected Breed() { }
        public Breed(string name)
        {
            Name = name;
        }
        public string Name { get; private set; } = default!;
        public Guid SpeciesId { get; private set; } = default!;
    }
}
