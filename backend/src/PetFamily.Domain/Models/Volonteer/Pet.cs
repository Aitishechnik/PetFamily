using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volonteer
{
    public class Pet : Entity<Guid>, ISoftDeletable
    {
        protected Pet() { }
        public Pet(
            PetGeneralInfo petGeneralInfo,
            PetCharacteristics petCharacteristics,
            PetHealthInfo petHealthInfo,
            IEnumerable<DonationDetails> donationDetails,
            PetType petType
        ) : base()
        {
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
        private List<DonationDetails> _donationDetails = [];
        public PetType PetType { get; private set; } = default!;
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletionDate { get; private set; } = null;
        public IReadOnlyList<FilePath> PetPhotos => _petPhotos;
        private List<FilePath> _petPhotos = [];
        public FilePath? MainPhoto { get; private set; } = null;

        public void RemovePhotos(IEnumerable<FilePath> petPhotos)
        {
            foreach (var photo in petPhotos)
            {
                if (_petPhotos.Contains(photo))
                    _petPhotos.Remove(photo);

                if (MainPhoto == photo)
                {
                    MainPhoto = null;
                }
            }
        }

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

        public int GetLastPhotoIndex()
        {
            var lastPhoto = _petPhotos.LastOrDefault();
            if (lastPhoto == null)
                return -1;

            return GetPhotoIndex(lastPhoto);
        }

        public UnitResult<Error> UpdateInfo(
            string name,
            string description,
            string address,
            string ownerPhoneNumber,
            DateTime dateOfBirth,
            HelpStatus HelpStatus,
            string color,
            double weight,
            double height,
            string healthInfo,
            bool isNeutered,
            bool isVaccinated,
            Guid speciesId,
            Guid breedId,
            IEnumerable<DonationDetails> donationDetails)
        {
            var newPetGeneralInfo = PetGeneralInfo.Create(
                name,
                description,
                address,
                ownerPhoneNumber,
                dateOfBirth,
                HelpStatus
            );
            if (newPetGeneralInfo.IsFailure)
                return newPetGeneralInfo.Error;

            PetGeneralInfo = newPetGeneralInfo.Value;

            var newPetCharacteristics = PetCharacteristics.Create(
                color,
                weight,
                height);
            if (newPetCharacteristics.IsFailure)
                return newPetCharacteristics.Error;

            PetCharacteristics = newPetCharacteristics.Value;

            var newPetHealthInfo = PetHealthInfo.Create(
                healthInfo,
                isNeutered,
                isVaccinated);
            if (newPetHealthInfo.IsFailure)
                return newPetHealthInfo.Error;

            _donationDetails = donationDetails.ToList();

            var newPetType = PetType.Create(speciesId, breedId);
            if (newPetType.IsFailure)
                return newPetType.Error;

            return Result.Success<Error>();
        }

        public void AddPhotos(IEnumerable<FilePath> petPhotos) => _petPhotos.AddRange(petPhotos);

        public void SetSerialNumber(SerialNumber serialNumber) => SerialNumber = serialNumber;

        public void ChangeHelpStatus(HelpStatus helpStatus)
        {
            PetGeneralInfo = PetGeneralInfo.Create(
                PetGeneralInfo.Name,
                PetGeneralInfo.Description,
                PetGeneralInfo.Address,
                PetGeneralInfo.OwnerPhoneNumber,
                PetGeneralInfo.DateOfBirth,
                helpStatus).Value;
        }

        public UnitResult<Error> SoftDelete()
        {
            if (IsDeleted)
                return Error.Failure("pet.was.already.deleted", "This pet was already deleted");
            IsDeleted = true;
            DeletionDate = DateTime.Now;

            return Result.Success<Error>();
        }

        public UnitResult<Error> SetMainPhoto(FilePath mainPhoto)
        {
            if (_petPhotos.Count == 0 ||
                !_petPhotos.Contains(mainPhoto))
                return Errors.General.ValueIsInvalid("No photos available");

            MainPhoto = mainPhoto;

            return Result.Success<Error>();
        }

        private int GetPhotoIndex(FilePath photo)
        {
            var indexString = photo.Path
                .Split('/')[2]
                .Split('.')[0]
                .Trim();
            if (int.TryParse(indexString, out var index))
                return index;
            else
                return -1;
        }
    }
}
