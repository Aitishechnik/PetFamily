namespace PetFamily.Domain.Models.Volonteer
{
    public record SocialNetwokrsWrapper
    {
        public SocialNetwokrsWrapper()
        {
        }
        public SocialNetwokrsWrapper(IEnumerable<SocialNetwork> socialNetworks)
        {
            SocialNetworks = socialNetworks.ToList();
        }

        public IReadOnlyCollection<SocialNetwork> SocialNetworks { get; } = default!;
    }
}
