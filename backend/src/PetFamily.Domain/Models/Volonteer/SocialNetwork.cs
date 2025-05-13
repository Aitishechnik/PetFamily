using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Volonteer
{
    public record SocialNetwork
    {
        private SocialNetwork(string name, string link)
        {
            Name = name;
            Link = link;
        }

        public string Name { get; }
        public string Link { get; }

        public Result<SocialNetwork> Create(string name, string link)
        {
            if (string.IsNullOrWhiteSpace(name))
                Result.Failure("Name cannot be empty");
            if (string.IsNullOrWhiteSpace(link))
                Result.Failure("Name cannot be empty");
            var result = new SocialNetwork(name, link);
            return Result.Success(result);
        }
    }
}
