using PetFamily.Core.Abstractions;

namespace PetFamily.Volonteers.Application.Commands.AddPetPhotos
{
    public record AddPetPhotosCommand(
        Guid VolonteerId,
        Guid PetId,
        string Bucket,
        IEnumerable<Stream> Content) : ICommand;
}
