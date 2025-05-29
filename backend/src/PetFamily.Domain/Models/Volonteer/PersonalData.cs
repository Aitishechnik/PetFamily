using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volonteer
{
    public record PersonalData
    {
        private static readonly string _regexpFullNamePattern =
            @"^(?:(?:[А-ЯЁ][а-яё]+(?:-[А-ЯЁ][а-яё]+)?\s){1,2}[А-ЯЁ][а-яё]+(?:-[А-ЯЁ][а-яё]+)?|(?:[A-Z][a-z]+(?:-[A-Z][a-z]+)?\s){1,2}[A-Z][a-z]+(?:-[A-Z][a-z]+)?)$";

        private static readonly string _regexpEmailPattern =
            @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";

        private static readonly string _regexpPhoneNumberPattern =
            @"^\+?[0-9\s\-\(\)]{7,20}$";

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
                !Regex.IsMatch(fullName, _regexpFullNamePattern))
                return Errors.General.ValueIsInvalid("FullName");

            if (email is null ||
                email.Length > Constants.MAX_EMAIL_LENGTH ||
                !Regex.IsMatch(email, _regexpEmailPattern))
                return Errors.General.ValueIsInvalid("Email");

            if (phoneNumber is null || 
                phoneNumber.Length > Constants.MAX_PHONE_NUMBER_LENGTH ||
                !Regex.IsMatch(phoneNumber, _regexpPhoneNumberPattern))
                return Errors.General.ValueIsInvalid("PhoneNumber");

            return new PersonalData(fullName, email, phoneNumber);
        }
    }
}
