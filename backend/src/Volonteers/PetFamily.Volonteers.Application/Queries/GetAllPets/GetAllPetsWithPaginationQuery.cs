using PetFamily.Core.Abstractions;

namespace PetFamily.Volonteers.Application.Queries.GetAllPets
{
    public record GetAllPetsWithPaginationQuery(
        int Page,
        int PageSize,
        Guid? VolonteerId,
        string? Name,
        string? Description,
        string? Address,
        string? OwnerPhoneNumber,
        DateTime? DateOfBirth,
        string? HelpStatus,
        string? Color,
        double? Weight,
        double? Height,
        bool? IsNeutered,
        bool? IsVaccinated,
        string? Species,
        string? Breed,
        int? SerialNumber) : IQuery;
}
