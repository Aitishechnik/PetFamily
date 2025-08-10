namespace PetFamily.Core.Dtos
{
    public record PetHealthInfoDto(
        string HealthInfo,
        bool IsNeutered,
        bool IsVaccinated);
}
