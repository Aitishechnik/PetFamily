using PetFamily.Application.Volonteers.RemovePetPhotos;
using PetFamily.Domain.Shared;
using FileInfo = PetFamily.Application.FileManagment.Files.FileInfo;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record RemovePetPhotoRequest(IEnumerable<string> Paths)
    {
        public RemovePetPhotosCommand ToCommand(
            Guid volonteerId, 
            Guid petId,
            string bucket)
        {
            return new RemovePetPhotosCommand(
                volonteerId, 
                petId, 
                Paths.Select(
                    path => new FileInfo(
                        bucket, FilePath.Create(
                            path).Value)));
        } 
    }
}
