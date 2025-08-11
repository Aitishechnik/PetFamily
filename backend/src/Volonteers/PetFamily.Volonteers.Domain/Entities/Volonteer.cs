using CSharpFunctionalExtensions;
using PetFamily.Core.Dtos.Enums;
using PetFamily.SharedKernal;
using PetFamily.Volonteers.Domain.ValueObjects;

namespace PetFamily.Volonteers.Domain.Entities
{
    public class Volonteer : Entity<Guid>, ISoftDeletable
    {
        protected Volonteer() { }
        public Volonteer(
            //Guid id,
            PersonalData personalData,
            ProfessionalData professionalData,
            List<Pet> pets,
            IEnumerable<SocialNetwork> socialNetwokrs,
            IEnumerable<DonationDetails> donationDetails)
        {
            //Id = id;
            PersonalData = personalData;
            ProfessionalData = professionalData;
            Pets = pets;
            _socialNetworks = socialNetwokrs.ToList();
            _donationDetails = donationDetails.ToList();
        }

        public PersonalData PersonalData { get; private set; } = default!;
        public ProfessionalData ProfessionalData { get; private set; } = default!;
        public List<Pet> Pets { get; private set; } = new();
        public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
        private List<SocialNetwork> _socialNetworks = [];
        public IReadOnlyList<DonationDetails> DonationDetails => _donationDetails;
        private List<DonationDetails> _donationDetails = [];
        public bool IsDeleted { get; private set; } = false;

        public DateTime? DeletionDate { get; private set; } = null;

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

        public void UpdateMainInfo(
            PersonalData personalData,
            ProfessionalData professionalData)
        {
            PersonalData = personalData;
            ProfessionalData = professionalData;
        }

        public void UpdateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
        {
            _socialNetworks = [.. socialNetworks];
        }

        public void UpdateDonationDetails(IEnumerable<DonationDetails> donationDetails)
        {
            _donationDetails = [.. donationDetails];
        }

        public void Delete()
        {
            IsDeleted = true;

            foreach (var pet in Pets)
                pet.Delete();

            DeletionDate = DateTime.Now;
        }

        public void Restore()
        {
            IsDeleted = false;

            foreach (var pet in Pets)
                pet.Restore();

            DeletionDate = null;
        }

        public UnitResult<Error> AddPet(Pet pet)
        {
            var serialNumberResult = SerialNumber.Create(Pets.Count + 1);
            if (serialNumberResult.IsFailure)
                return serialNumberResult.Error;

            pet.SetSerialNumber(serialNumberResult.Value);

            Pets.Add(pet);

            return Result.Success<Error>();
        }

        public Result<Pet, Error> GetPetById(Guid petId)
        {
            if (petId == Guid.Empty)
                return Errors.General.ValueIsInvalid(petId.ToString());

            var result = Pets.FirstOrDefault(p => p.Id == petId);

            if (result == null)
                return Errors.General.ValueIsInvalid(petId.ToString());

            return result;
        }

        public UnitResult<Error> AddPetPhotos(Guid petId, IEnumerable<FilePath> petPhotos)
        {
            if (petPhotos is null || petPhotos.ToList().Count == 0)
                return Errors.General.ValueIsInvalid("pet photos collection");

            if (petId == Guid.Empty)
                return Errors.General.ValueIsInvalid("pet is null");

            var pet = Pets.FirstOrDefault(p => p.Id == petId);

            if (pet is null)
                return Errors.General.ValueIsInvalid("pet is not in volunteer's list");

            pet.AddPhotos(petPhotos);

            return Result.Success<Error>();
        }

        public UnitResult<Error> MovePet(SerialNumber current, SerialNumber target)
        {
            var pet = Pets.FirstOrDefault(p => p.SerialNumber == current);
            if (pet is null)
                return Errors.General.ValueIsInvalid("serial number");

            if (target.Value <= 0 || target.Value > Pets.Count)
                return Errors.General.ValueIsInvalid("new position");

            int oldIndex = Pets.IndexOf(pet);
            int targetIndex = target.Value - 1;

            if (oldIndex == targetIndex)
                return Result.Success<Error>();

            Pets.RemoveAt(oldIndex);
            Pets.Insert(targetIndex, pet);

            int startIndex = oldIndex < targetIndex ? oldIndex : targetIndex;

            for (int i = startIndex; i < Pets.Count; i++)
            {
                var serialResult = SerialNumber.Create(i + 1);
                if (serialResult.IsFailure)
                    return serialResult.Error;

                Pets[i].SetSerialNumber(serialResult.Value);
            }

            return Result.Success<Error>();
        }

        public UnitResult<Error> UpdatePetInfo(
            Guid petId,
            int serialNumber,
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
            var petResult = GetPetById(petId);
            if (petResult.IsFailure)
                return petResult.Error;

            var updateResult = petResult.Value.UpdateInfo(
                name,
                description,
                address,
                ownerPhoneNumber,
                dateOfBirth,
                HelpStatus,
                color,
                weight,
                height,
                healthInfo,
                isNeutered,
                isVaccinated,
                speciesId,
                breedId,
                donationDetails);
            if (updateResult.IsFailure)
                return updateResult.Error;

            if (serialNumber != petResult.Value.SerialNumber.Value)
            {
                var moveResult = MovePet(
                    petResult.Value.SerialNumber,
                    SerialNumber.Create(serialNumber).Value);
                if (moveResult.IsFailure)
                    return moveResult.Error;
            }

            return Result.Success<Error>();
        }

        public UnitResult<Error> ChangePetStatus(
            Guid petId,
            HelpStatus helpStatus)
        {
            var petResult = GetPetById(petId);
            if (petResult.IsFailure)
                return petResult.Error;

            if (helpStatus != HelpStatus.NeedsHelp &&
                helpStatus != HelpStatus.LookingForHome)
                return Errors.General.ValueIsInvalid("help status");

            petResult.Value.ChangeHelpStatus(helpStatus);

            return Result.Success<Error>();
        }

        public UnitResult<Error> PetSoftDelete(Guid petId)
        {
            var petResult = GetPetById(petId);
            if (petResult.IsFailure)
                return petResult.Error;

            var petDeleteResult = petResult.Value.SoftDelete();
            if (petDeleteResult.IsFailure)
                return petDeleteResult.Error;

            return Result.Success<Error>();
        }

        public UnitResult<Error> PetDelete(Guid petId)
        {
            var petResult = GetPetById(petId);
            if (petResult.IsFailure)
                return petResult.Error;
            Pets.Remove(petResult.Value);
            return Result.Success<Error>();
        }
    }
}