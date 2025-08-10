using PetFamily.Domain.Models.Volonteer;

namespace PetFamily.Application.Dtos
{
    public class PetDto
    {
        public Guid Id { get; set; }
        public Guid VolonteerId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string OwnerPhoneNumber { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public HelpStatus HelpStatus { get; set; }
        public string Color { get; set; } = default!;
        public double Weight { get; set; } = default!;
        public double Height { get; set; } = default!;
        public string HealthInfo { get; set; } = default!;
        public bool IsNeutered { get; set; }
        public bool IsVaccinated { get; set; }
        public DonationDetailsDto[] DonationDetails { get; set; } = [];
        public Guid SpeciesId { get; set; }
        public Guid BreedId { get; set; }
        public int SerialNumber { get; set; }
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletionDate { get; private set; } = null;
        public string? MainPhoto { get; set; }
    }
}
