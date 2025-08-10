namespace PetFamily.Volonteers.Domain.ValueObjects
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
