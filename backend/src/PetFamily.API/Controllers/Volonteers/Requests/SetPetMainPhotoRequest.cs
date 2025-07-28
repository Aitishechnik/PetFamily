using PetFamily.Application.Volonteers.Commands.SetPetMainPhoto;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record SetPetMainPhotoRequest(string MainPhotoPath)
    {
        public SetPetMainPhotoCommand ToCommand(Guid volonteerId, Guid petId)
            => new(volonteerId, petId, MainPhotoPath);
    }

}
