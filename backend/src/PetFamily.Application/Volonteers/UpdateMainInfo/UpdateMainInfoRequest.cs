using PetFamily.Contracts;

namespace PetFamily.Application.Volonteers.UpdateMainInfo
{
    public record UpdateMainInfoRequest(
        Guid VolonteerId,
        MainInfoDTO MainInfoDTO);
}
