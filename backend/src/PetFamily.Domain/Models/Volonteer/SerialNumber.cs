using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volonteer
{
    public record SerialNumber
    {
        public static SerialNumber First => new (1);
        public int Value { get; }
        private SerialNumber(int value)
        {
            Value = value;
        }

        public static Result<SerialNumber, Error> Create(int number)
        {
            if (number <= 0)
                return Errors.General.ValueIsInvalid("serial number");

            return new SerialNumber(number);
        }
    }
}