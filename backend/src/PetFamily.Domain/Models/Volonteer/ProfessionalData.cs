using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volonteer
{
    public record ProfessionalData
    {
        public string Description { get; }

        public int ExperienceInYears { get; }
        private ProfessionalData(string description, int experienceInYears)
        {
            Description = description;
            ExperienceInYears = experienceInYears;
        }

        public static Result<ProfessionalData, Error> Create(string description, int experienceInYears)
        {
            if (description.Length > Constants.MAX_TEXT_DESCRIPTION_LENGTH)
                return Errors.General.ValueIsInvalid("Description");

            if (experienceInYears < 0 || experienceInYears > Constants.MAX_YEARS_OF_EXPERIENCE)
                return Errors.General.ValueIsInvalid("ExperienceInYears");

            return new ProfessionalData(description, experienceInYears);
        }
    }
}
