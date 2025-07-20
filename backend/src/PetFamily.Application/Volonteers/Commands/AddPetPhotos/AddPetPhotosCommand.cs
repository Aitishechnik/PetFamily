using PetFamily.Application.Abstraction;

namespace PetFamily.Application.Volonteers.Commands.AddPetPhotos
{
    public record AddPetPhotosCommand(
        Guid VolonteerId, 
        Guid PetId, 
        string Bucket, 
        IEnumerable<Stream> Content) : ICommand;
}
