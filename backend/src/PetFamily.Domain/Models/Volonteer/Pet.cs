using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volonteer
{
    public class Pet : Entity<Guid>, ISoftDeletable
    {
        protected Pet() { }
        public Pet(
            Guid id,
            Guid volonteerId,
            PetGeneralInfo petGeneralInfo,
            PetCharacteristics petCharacteristics,
            PetHealthInfo petHealthInfo,
            IEnumerable<DonationDetails> donationDetails,
            PetType petType
        ) : base(id)
        {
            VolonteerId = volonteerId;
            PetGeneralInfo = petGeneralInfo;
            PetCharacteristics = petCharacteristics;
            PetHealthInfo = petHealthInfo;
            _donationDetails = donationDetails.ToList();
            PetType = petType;
            CreatedAt = DateTime.Now;
        }

        public Guid VolonteerId { get; private set; } = default!;
        public SerialNumber SerialNumber { get; private set; } = default!;
        public PetGeneralInfo PetGeneralInfo { get; private set; } = default!;
        public PetCharacteristics PetCharacteristics { get; private set; } = default!;
        public PetHealthInfo PetHealthInfo { get; private set; } = default!;
        public IReadOnlyList<DonationDetails> DonationDetails => _donationDetails;
        private readonly List<DonationDetails> _donationDetails = [];
        public PetType PetType { get; private set; } = default!;
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletionDate { get; private set; } = null;
        public IReadOnlyList<FilePath> PetPhotos => _petPhotos;
        private readonly List<FilePath> _petPhotos = [];

        public void Delete()
        {
            IsDeleted = true;
            DeletionDate = DateTime.Now;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletionDate = null;
        }

        public void AddPhotos(IEnumerable<FilePath> petPhotos) => _petPhotos.AddRange(petPhotos);

        public void SetSerialNumber(SerialNumber serialNumber) => SerialNumber = serialNumber;
    }
}
