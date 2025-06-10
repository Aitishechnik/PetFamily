namespace PetFamily.Application.Volonteers.CreateVolonteer
{
    public record CreateVolonteerDTO(
        string FullName,
        string Email,
        string Description,
        int ExperienceInYears,
        string PhoneNumber,
        IEnumerable<SocialNetworkDTO> SocialNetworks,
        IEnumerable<DonationDetailsDTO> DonationDetails);
}
