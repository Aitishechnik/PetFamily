using PetFamily.Domain.Models.Volonteer;

namespace PetFamily.Contracts
{
    public record PetGeneralInfoDTO(
        string Name,
        string Description,
        string Address,
        string OwnerPhoneNumber,
        DateTime DateOfBirth,
        HelpStatus HelpStatus);
}
