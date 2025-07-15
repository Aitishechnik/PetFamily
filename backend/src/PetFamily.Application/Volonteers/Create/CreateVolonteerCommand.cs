using PetFamily.Contracts;

namespace PetFamily.Application.Volonteers.Create
{
    public record CreateVolonteerCommand(
        PersonalDataDTO PersonalDataDTO,
        ProfessionalDataDTO ProfessionalDataDTO,
        IEnumerable<SocialNetworkDTO> SocialNetworks,
        IEnumerable<DonationDetailsDTO> DonationDetails);
}
