using CSharpFunctionalExtensions;
using PetFamily.SharedKernal;

namespace PetFamily.Volonteers.Domain.ValueObjects
{
    public record class DonationDetails
    {
        private DonationDetails(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }
        public string Description { get; }

        public static Result<DonationDetails, Error> Create(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                name.Length > Constants.MAX_NAME_LENGTH)
                return Errors.General.ValueIsInvalid("Name");
            if (string.IsNullOrWhiteSpace(description) ||
                description.Length > Constants.MAX_TEXT_DESCRIPTION_LENGTH)
                return Errors.General.ValueIsInvalid("Description");

            return new DonationDetails(name, description);
        }
    }
}
