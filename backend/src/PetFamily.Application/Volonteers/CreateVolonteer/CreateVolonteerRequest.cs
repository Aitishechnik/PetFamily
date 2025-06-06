namespace PetFamily.Application.Volonteers.CreateVolonteer
{
    public record CreateVolonteerRequest(
        string fullName,
        string email,
        string description,
        int experienceInYears,
        string phoneNumber,
        //string socialNetworkName,
        //string socialNetworkLink,
        //string donationDetailsName,
        //string donationDetailsDescription
        IEnumerable<string> socialNetworks,
        IEnumerable<string> donationDetails);
}
