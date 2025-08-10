using CSharpFunctionalExtensions;
using PetFamily.SharedKernal;

namespace PetFamily.Volonteers.Domain.ValueObjects
{
    public record PetCharacteristics
    {
        public string Color { get; } = default!;
        public double Weight { get; } = default!;
        public double Height { get; } = default!;

        private PetCharacteristics(string color, double weight, double height)
        {
            Color = color;
            Weight = weight;
            Height = height;
        }

        public static Result<PetCharacteristics, Error> Create(string color, double weight, double height)
        {
            if (string.IsNullOrWhiteSpace(color) || color.Length > Constants.MAX_NAME_LENGTH)
                return Errors.General.ValueIsInvalid("Color");
            if (weight <= 0 || weight > Constants.MAX_DOMASTIC_PET_WEIGHT_KG)
                return Errors.General.ValueIsInvalid("Weight");
            if (height <= 0 || height > Constants.MAX_DOMASTIC_PET_HEIGHT_SM)
                return Errors.General.ValueIsInvalid("Height");

            return new PetCharacteristics(color, weight, height);
        }
    }
}
