using PetFamily.Core.Dtos.Enums;

namespace PetFamily.Core.Dtos
{
    public record PetGeneralInfoDto(
        string Name,
        string Description,
        string Address,
        string OwnerPhoneNumber,
        DateTime DateOfBirth,
        HelpStatus HelpStatus);
}
