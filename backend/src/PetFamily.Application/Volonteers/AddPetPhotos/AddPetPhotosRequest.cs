using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.AddPetPhotos
{
    public record AddPetPhotosRequest(Guid VolonteerId, Guid PetId, IEnumerable<Stream> Content);
}
