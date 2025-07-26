using PetFamily.Application.Species.Commands.RemoveBreed;

namespace PetFamily.API.Controllers.Species.Requests
{
    public record RemoveBreedByIdRequest(Guid BreedId)
    {
        public RemoveBreedByIdCommand ToCommand() =>
            new(BreedId);
    }
}
