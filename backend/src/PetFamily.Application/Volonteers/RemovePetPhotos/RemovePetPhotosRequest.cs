using PetFamily.Contracts;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.RemovePetPhotos
{
    public record RemovePetPhotosRequest(Guid VolonteerId, Guid PetId, IEnumerable<FilePath> Paths);
}
