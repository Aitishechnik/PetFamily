using PetFamily.Species.Contracts.Requests;

namespace PetFamily.Species.Contracts
{
    public interface ISpeciesContract
    {
        Task<bool> IsSpeciesAndBreedExist(
        IsSpeciesAndBreedExistRequest request,
        CancellationToken cancellationToken);
    }
}
