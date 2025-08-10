using PetFamily.Core.Dtos.Enums;
using PetFamily.Volonteers.Application.Queries.GetAllPets;

namespace PetFamily.Volonteers.Presentation.Requests
{
    public record GetAllPetsWithPaginationRequest(
        int Page,
        int PageSize,
        Guid? VolonteerId,
        string? Name,
        string? Description,
        string? Address,
        string? OwnerPhoneNumber,
        DateTime? DateOfBirth,
        HelpStatus? HelpStatus,
        string? Color,
        double? Weight,
        double? Height,
        bool? IsNeutered,
        bool? IsVaccinated,
        string? Species,
        string? Breed,
        int? SerialNumber)
    {
        public GetAllPetsWithPaginationQuery ToQuery() => new(
            Page,
            PageSize,
            VolonteerId,
            Name,
            Description,
            Address,
            OwnerPhoneNumber,
            DateOfBirth,
            HelpStatus.ToString(),
            Color,
            Weight,
            Height,
            IsNeutered,
            IsVaccinated,
            Species,
            Breed,
            SerialNumber);
    }
}
