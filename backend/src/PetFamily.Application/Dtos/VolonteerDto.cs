namespace PetFamily.Application.Dtos
{
    public class VolonteerDto
    {
        public VolonteerDto() { }
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ExperienceInYears { get; set; }
        public SocialNetworkDto[] SocialNetworks { get; set; } = [];
        public DonationDetailsDto[] DonationDetails { get; set; } = [];
    }
}
