namespace PetFamily.Application.Volonteers.CreateVolonteer
{
    public record CreateVolonteerDTO(
        PersonalDataDTO PersonalDataDTO,
        ProfessionalDataDTO ProfessionalDataDTO,
        IEnumerable<SocialNetworkDTO> SocialNetworks,
        IEnumerable<DonationDetailsDTO> DonationDetails);
}
