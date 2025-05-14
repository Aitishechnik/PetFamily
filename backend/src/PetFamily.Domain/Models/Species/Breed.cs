using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Species
{
    public class Breed : Entity
    {
        public Breed(long id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Name { get; private set; } = default!;
    }
}
