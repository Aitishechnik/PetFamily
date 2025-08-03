using PetFamily.Application.Species.Commands.AddSpecies;

namespace PetFamily.API.Controllers.Species.Requests
{
    public record AddSpeciesRequest(string Name)
    {
        public AddSpeciesCommand ToCommand() => new(Name);
    }
}
