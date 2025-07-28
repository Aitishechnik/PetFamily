namespace PetFamily.Contracts
{
    public record PetHealthInfoDTO(
        string HealthInfo,
        bool IsNeutered,
        bool IsVaccinated);
}
