using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Volonteer
{
    public record class DonationDetails
    {
        private DonationDetails(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public static Result<DonationDetails> Create(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                Result.Failure("Name cannot be empty");
            if (string.IsNullOrWhiteSpace(description))
                Result.Failure("Name cannot be empty");

            var result = new DonationDetails(name, description);

            return Result.Success(result);
        }
        public string Name { get; }
        public string Description { get; }
    }
}
