using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volonteer
{
    public class Volonteer : Entity<Guid>, ISoftDeletable
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

        public void UpdateSocialNetworks(SocialNetwokrsWrapper socialNetworks)
        {
            SocialNetworks = socialNetworks;
        }

        public void UpdateDonationDetails(DonationDetailsWrapper donationDetails)
        {
            DonationDetails = donationDetails;
        }

        public void Delete()
        {
            IsDeleted = true;

            foreach(var pet in Pets)
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

        public UnitResult<Error> MovePet(SerialNumber current, SerialNumber target)
        {
            var pet = Pets.FirstOrDefault(p => p.SerialNumber == current);
            if(pet is null)
                return Errors.General.ValueIsInvalid("serial number");

            if (target.Value <= 0 || target.Value > Pets.Count)
                return Errors.General.ValueIsInvalid("new position");

            int oldIndex = Pets.IndexOf(pet);
            int targetIndex = target.Value - 1;

            if (oldIndex == targetIndex)
                return Result.Success<Error>();

            Pets.RemoveAt(oldIndex);
            Pets.Insert(targetIndex, pet);

            for (int i = targetIndex; i < Pets.Count; i++)
            {
                var serialResult = SerialNumber.Create(i + 1);
                if (serialResult.IsFailure)
                    return serialResult.Error;

                Pets[i].SetSerialNumber(serialResult.Value);
            }

            return Result.Success<Error>();
        }
    }
}