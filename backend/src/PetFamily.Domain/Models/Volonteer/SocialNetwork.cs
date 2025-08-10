using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

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

        public static Result<SocialNetwork, Error> Create(string name, string link)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                name.Length > Constants.MAX_NAME_LENGTH)
                return Errors.General.ValueIsInvalid("Name");
            if (string.IsNullOrWhiteSpace(link) || link.Length > Constants.MAX_LINK_LENGTH)
                return Errors.General.ValueIsInvalid("Link");

            return new SocialNetwork(name, link);
        }
    }
}
