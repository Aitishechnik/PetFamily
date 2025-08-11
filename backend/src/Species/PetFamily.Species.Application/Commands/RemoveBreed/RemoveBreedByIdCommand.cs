using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.RemoveBreed
{
    public record RemoveBreedByIdCommand(Guid BreedId) : ICommand;
}
