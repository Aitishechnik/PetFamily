namespace PetFamily.Contracts
{
    public record 
        VolonteerDTO(
        PersonalDataDTO PersonalDataDTO,
        ProfessionalDataDTO ProfessionalDataDTO,
        IEnumerable<SocialNetworkDTO> SocialNetworks,
        IEnumerable<DonationDetailsDTO> DonationDetails);
}
