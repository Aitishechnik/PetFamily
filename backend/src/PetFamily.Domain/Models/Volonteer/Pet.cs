using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Volonteer
{
    public class Pet : Entity
    {
        public Pet(
            long id, 
            string name, 
            string species, 
            string description, 
            string breed, 
            string color,
            string healthInfo, 
            string address, 
            double weight, 
            double height, 
            string ownerPhoneNumber,
            bool isNeutered, 
            DateTime dateOfBirth, 
            bool isVaccinated, 
            HelpStatus helpStatus,
            DonationDetails donationDetails, 
            PetType petType
        )
        {
            Id = id;
            Name = name;
            Species = species;
            Description = description;
            Breed = breed;
            Color = color;
            HealthInfo = healthInfo;
            Address = address;
            Weight = weight;
            Height = height;
            OwnerPhoneNumber = ownerPhoneNumber;
            IsNeutered = isNeutered;
            DateOfBirth = dateOfBirth;
            IsVaccinated = isVaccinated;
            HelpStatus = helpStatus;
            DonationDetails = donationDetails;
            PetType = petType;
            CreatedAt = DateTime.Now;
        }
        public string Name { get; private set; } = default!;
        public string Species { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public string Breed { get; private set; } = default!;
        public string Color { get; private set; } = default!;
        public string HealthInfo { get; private set; } = default!;
        public string Address { get; private set; } = default!;
        public double Weight { get; private set; }
        public double Height { get; private set; }
        public string OwnerPhoneNumber { get; private set; } = default!;
        public bool IsNeutered { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public bool IsVaccinated { get; private set; }
        public HelpStatus HelpStatus { get; private set; }
        public DonationDetails DonationDetails { get; private set; }
        public PetType PetType { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
    }
}
