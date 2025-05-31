using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volonteer
{
    public record PetGeneralInfo
    {
        public string Name { get; } = default!;
        public string Description { get; } = default!;
        public string Address { get; } = default!;
        public string OwnerPhoneNumber { get; } = default!;
        public DateTime DateOfBirth { get; }
        public HelpStatus HelpStatus { get; }

        private PetGeneralInfo(
            string name,
            string description,
            string address,
            string ownerPhoneNumber,
            DateTime dateOfBirth,
            HelpStatus helpStatus)
        {
            Name = name;
            Description = description;
            Address = address;
            OwnerPhoneNumber = ownerPhoneNumber;
            DateOfBirth = dateOfBirth;
            HelpStatus = helpStatus;
        }

        public static Result<PetGeneralInfo, Error> Create(string name,
            string description,
            string address,
            string ownerPhoneNumber,
            DateTime dateOfBirth,
            HelpStatus helpStatus)
        {
            if(string.IsNullOrWhiteSpace(description) ||
                description.Length > Constants.MAX_TEXT_DESCRIPTION_LENGTH)
                return Errors.General.ValueIsInvalid("Description");

            if (string.IsNullOrWhiteSpace(name) ||
                name.Length > Constants.MAX_NAME_LENGTH)
                return Errors.General.ValueIsInvalid("Name");

            if (string.IsNullOrWhiteSpace(address) ||
                address.Length > Constants.MAX_TEXT_DESCRIPTION_LENGTH)
                return Errors.General.ValueIsInvalid("Address");

            if (string.IsNullOrWhiteSpace(ownerPhoneNumber) ||
                ownerPhoneNumber.Length > Constants.MAX_PHONE_NUMBER_LENGTH)
                return Errors.General.ValueIsInvalid("OwnerPhoneNumber");

            if (dateOfBirth > DateTime.Now ||
                dateOfBirth < DateTime.Now.AddYears(-Constants.MAX_DOMASTIC_PET_AGE))
                return Errors.General.ValueIsInvalid("DateOfBirth");

            if(helpStatus == HelpStatus.Undefined)
                return Errors.General.ValueIsInvalid("HelpStatus");

            return new PetGeneralInfo(name, description, address, ownerPhoneNumber, dateOfBirth, helpStatus);
        }
    }
}
