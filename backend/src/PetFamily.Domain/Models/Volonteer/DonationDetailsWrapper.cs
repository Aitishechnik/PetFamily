namespace PetFamily.Domain.Models.Volonteer
{
    public record DonationDetailsWrapper
    {
        public DonationDetailsWrapper()
        {
        }
        public DonationDetailsWrapper(IEnumerable<DonationDetails> donationDetails)
        {
            DonationDetails = donationDetails.ToList();
        }
        public IReadOnlyList<DonationDetails> DonationDetails { get; } = default!;
    }
}
