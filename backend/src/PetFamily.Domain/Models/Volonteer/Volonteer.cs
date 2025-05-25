using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Volonteer
{
    public class Volonteer : Entity<Guid>
    {
        protected Volonteer() { }
        public Volonteer(
            Guid id, 
            string fullName, 
            string email, 
            string description, 
            int experienceInYears,
            List<Pet> pets, 
            string phoneNumber,
            SocialNetwokrsWrapper socialNetwokrs,
            DonationDetailsWrapper donationDetails)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            Description = description;
            ExperienceInYears = experienceInYears;
            Pets = pets;
            PhoneNumber = phoneNumber;
            SocialNetworks = socialNetwokrs;
            DonationDetails = donationDetails;
        }

        public string FullName { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public int ExperienceInYears { get; private set; }
        public List<Pet> Pets { get; private set; } = new();
        public string PhoneNumber { get; private set; } = default!;

        public SocialNetwokrsWrapper SocialNetworks { get; private set; } = default!;
        public DonationDetailsWrapper DonationDetails { get; private set; } = default!;
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
