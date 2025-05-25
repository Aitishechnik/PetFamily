using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Volonteer
{
    public record DonationDetailsWrapper
    {
        private DonationDetailsWrapper()
        {
        }
        private DonationDetailsWrapper(IReadOnlyCollection<DonationDetails> donationDetails)
        {
            DonationDetails = donationDetails;
        }
        public IReadOnlyCollection<DonationDetails> DonationDetails { get; } = default!;

        public static Result<DonationDetailsWrapper> Create(IReadOnlyCollection<DonationDetails> donationDetails)
        {
            if (donationDetails == null || !donationDetails.Any())
                return Result.Failure<DonationDetailsWrapper>("Donation details cannot be null or empty");
            return Result.Success(new DonationDetailsWrapper(donationDetails));
        }
    }
}
