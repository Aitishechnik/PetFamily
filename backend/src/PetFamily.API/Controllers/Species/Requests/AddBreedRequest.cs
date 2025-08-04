using PetFamily.Application.Species.Commands.AddBreed;

namespace PetFamily.API.Controllers.Species.Requests
{
    public record AddBreedRequest(string Name)
    {
        public AddBreedCommand ToCommand(Guid speciesId) => new(speciesId, Name);
    }
}
