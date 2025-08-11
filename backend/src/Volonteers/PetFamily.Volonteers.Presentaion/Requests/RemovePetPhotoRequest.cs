using PetFamily.Core.FileManagment.Files;
using PetFamily.SharedKernal;
using PetFamily.Volonteers.Application.Commands.RemovePetPhotos;

namespace PetFamily.Volonteers.Presentation.Requests
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
                    path => new FileInfoPath(
                        bucket, FilePath.Create(
                            path).Value)));
        }
    }
}
