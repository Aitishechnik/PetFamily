namespace PetFamily.Volonteers.Domain.ValueObjects
{
    public record DonationDetailsWrapper
    {
        public DonationDetailsWrapper() { }
        public DonationDetailsWrapper(IEnumerable<DonationDetails> donationDetails)
        {
            DonationDetails = donationDetails.ToList();
        }
        public IReadOnlyList<DonationDetails> DonationDetails { get; } = default!;
    }
}
