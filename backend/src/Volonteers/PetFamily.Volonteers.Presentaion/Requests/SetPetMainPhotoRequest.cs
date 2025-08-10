using PetFamily.Volonteers.Application.Commands.SetPetMainPhoto;

namespace PetFamily.Volonteers.Presentation.Requests
{
    public record SetPetMainPhotoRequest(string MainPhotoPath)
    {
        public SetPetMainPhotoCommand ToCommand(Guid volonteerId, Guid petId)
            => new(volonteerId, petId, MainPhotoPath);
    }

}
