namespace PetFamily.Domain.Models.Volonteer
{
    public record SocialNetwokrsWrapper
    {
        public SocialNetwokrsWrapper()
        {
        }
        public SocialNetwokrsWrapper(IReadOnlyCollection<SocialNetwork> socialNetworks)
        {
            SocialNetworks = socialNetworks;
        }

        public IReadOnlyCollection<SocialNetwork> SocialNetworks { get; } = default!;
    }
}
