using PetFamily.Species.Application.Commands.AddBreed;

namespace PetFamily.Species.Requests
{
    public record AddBreedRequest(string Name)
    {
        public AddBreedCommand ToCommand(Guid speciesId) => new(speciesId, Name);
    }
}
