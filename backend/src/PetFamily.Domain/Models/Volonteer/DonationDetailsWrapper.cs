namespace PetFamily.Domain.Models.Volonteer
{
    public record DonationDetailsWrapper
    {
        private DonationDetailsWrapper()
        {
        }
        public DonationDetailsWrapper(IReadOnlyCollection<DonationDetails> donationDetails)
        {
            DonationDetails = donationDetails;
        }
        public IReadOnlyCollection<DonationDetails> DonationDetails { get; } = default!;
    }
}
