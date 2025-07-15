using PetFamily.Contracts;

namespace PetFamily.Application.Volonteers.UpdateMainInfo
{
    public record UpdateMainInfoCommand(
        Guid VolonteerId,
        MainInfoDTO MainInfoDTO);
}
