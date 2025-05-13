using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Volonteer
{
    public class Volonteer : Entity
    {
        public Volonteer(long id, string fullName, string email, string description, int experienceInYears,
            List<Pet> pets, string phoneNumber, List<SocialNetwork> socialNetworks,
            List<DonationDetails> donationDetails)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            Description = description;
            ExperienceInYears = experienceInYears;
            Pets = pets;
            PhoneNumber = phoneNumber;
            SocialNetworks = socialNetworks;
            DonationDetails = donationDetails;
        }

        public string FullName { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public int ExperienceInYears { get; private set; }
        public List<Pet> Pets { get; private set; } = new();
        public string PhoneNumber { get; private set; } = default!;
        public List<SocialNetwork> SocialNetworks { get; private set; } = new();
        public List<DonationDetails> DonationDetails { get; private set; } = new();
        private int GetPetsCountByStatus(HelpStatus status) => Pets.Count(p => p.HelpStatus == status);

        public int PetsFoundHome()
        {
            return GetPetsCountByStatus(HelpStatus.FoundHome);
        }

        public int PetLookingForHome()
        {
            return GetPetsCountByStatus(HelpStatus.LookingForHome);
        }

        public int PetsUnderTreatment()
        {
            return GetPetsCountByStatus(HelpStatus.NeedsHelp);
        }
    }
}
