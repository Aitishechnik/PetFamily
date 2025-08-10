using PetFamily.Species.Application.Commands.RemoveBreed;

namespace PetFamily.Species.Requests
{
    public record RemoveBreedByIdRequest(Guid BreedId)
    {
        public RemoveBreedByIdCommand ToCommand() =>
            new(BreedId);
    }
}
