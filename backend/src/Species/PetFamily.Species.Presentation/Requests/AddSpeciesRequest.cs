using PetFamily.Species.Application.Commands.AddSpecies;

namespace PetFamily.Species.Requests
{
    public record AddSpeciesRequest(string Name)
    {
        public AddSpeciesCommand ToCommand() => new(Name);
    }
}
