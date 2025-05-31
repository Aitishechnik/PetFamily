using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Volonteer
{
    public class Pet : Entity<Guid>
    {
        protected Pet() { }
        public Pet(
            Guid id,
            PetGeneralInfo petGeneralInfo,
            BreedAndSpecies breedAndSpecies,
            PetCharacteristics petCharacteristics,
            PetHealthInfo petHealthInfo,
            DonationDetailsWrapper donationDetails,
            PetType petType
        ) : base(id)
        {
            PetGeneralInfo = petGeneralInfo;
            BreedAndSpecies = breedAndSpecies;
            PetCharacteristics = petCharacteristics;
            PetHealthInfo = petHealthInfo;
            DonationDetails = donationDetails;
            PetType = petType;
            CreatedAt = DateTime.Now;
        }

        public PetGeneralInfo PetGeneralInfo { get; } = default!;
        public BreedAndSpecies BreedAndSpecies { get; private set; } = default!;
        public PetCharacteristics PetCharacteristics { get; private set; } = default!;
        public PetHealthInfo PetHealthInfo { get; private set; } = default!;
        public DonationDetailsWrapper DonationDetails { get; private set; } = default!;
        public PetType PetType { get; private set; } = default!;
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
    }
}
