namespace PetFamily.Contracts
{
    public record PetHealthInfoDTO(
        string HelthInfo,
        bool IsNeutered,
        bool IsVaccinated);
}
