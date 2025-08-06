using PetFamily.Application.Abstraction;
using PetFamily.Domain.Models.Volonteer;

namespace PetFamily.Application.Volonteers.Queries.GetAllPets
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
