using CSharpFunctionalExtensions;
using PetFamily.SharedKernal;

namespace PetFamily.Volonteers.Domain.ValueObjects
{
    public record PetHealthInfo
    {
        public string HealthInfo { get; } = default!;
        public bool IsNeutered { get; }
        public bool IsVaccinated { get; }

        private PetHealthInfo(string healthInfo, bool isNeutered, bool isVaccinated)
        {
            this.HealthInfo = healthInfo;
            this.IsNeutered = isNeutered;
            this.IsVaccinated = isVaccinated;
        }

        public static Result<PetHealthInfo, Error> Create(string healthInfo, bool isNeutered, bool isVaccinated)
        {
            if (string.IsNullOrWhiteSpace(healthInfo) || healthInfo.Length > Constants.MAX_TEXT_DESCRIPTION_LENGTH)
                return Errors.General.ValueIsInvalid("HealthInfo");

            return new PetHealthInfo(healthInfo, isNeutered, isVaccinated);
        }
    }
}
