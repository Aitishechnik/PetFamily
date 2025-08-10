using CSharpFunctionalExtensions;
using PetFamily.SharedKernal;
using System.Text.RegularExpressions;

namespace PetFamily.Volonteers.Domain.ValueObjects
{
    public record PersonalData
    {
        public string FullName { get; }

        public string Email { get; }

        public string PhoneNumber { get; }

        private PersonalData(string fullName, string email, string phoneNumber)
        {
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public static Result<PersonalData, Error> Create(string fullName, string email, string phoneNumber)
        {
            if (fullName is null ||
                fullName.Length > Constants.MAX_NAME_LENGTH ||
                !Regex.IsMatch(fullName, Constants.REGEX_FULLNAME_PATTERN))
                return Errors.General.ValueIsInvalid("FullName");

            if (email is null ||
                email.Length > Constants.MAX_EMAIL_LENGTH ||
                !Regex.IsMatch(email, Constants.REGEX_EMAIL_PATTERN))
                return Errors.General.ValueIsInvalid("Email");

            if (phoneNumber is null ||
                phoneNumber.Length > Constants.MAX_PHONE_NUMBER_LENGTH ||
                !Regex.IsMatch(phoneNumber, Constants.REGEX_PHONE_NUMBER_PATTERN))
                return Errors.General.ValueIsInvalid("PhoneNumber");

            return new PersonalData(fullName, email, phoneNumber);
        }
    }
}
