using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Volonteer
{
    public class Volonteer : Entity<Guid>
    {
        protected Volonteer() { }
        public Volonteer(
            Guid id, 
            PersonalData personalData,
            ProfessionalData professionalData,
            List<Pet> pets, 
            SocialNetwokrsWrapper socialNetwokrs,
            DonationDetailsWrapper donationDetails)
        {
            Id = id;
            PersonalData = personalData;
            ProfessionalData = professionalData;
            Pets = pets;
            SocialNetworks = socialNetwokrs;
            DonationDetails = donationDetails;
        }

        public PersonalData PersonalData { get; private set; } = default!;
        public ProfessionalData ProfessionalData { get; private set; } = default!;
        public List<Pet> Pets { get; private set; } = new();
        public SocialNetwokrsWrapper SocialNetworks { get; private set; } = default!;
        public DonationDetailsWrapper DonationDetails { get; private set; } = default!;
        private int GetPetsCountByStatus(HelpStatus status) => Pets.Count(p => p.PetGeneralInfo.HelpStatus == status);

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
