using PetFamily.Volonteers.Contracts.Requests;

namespace PetFamily.Volonteers.Contracts
{
    public interface IVolonteerContract
    {
        Task<bool> HasAnimalsWithBreed(
            HasPetsWithBreedRequest request,
            CancellationToken cancellationToken);

        Task<bool> HasAnimalsWithSpecies(
            HasPetsWithSpeciesRequest request,
            CancellationToken cancellationToken);
    }
}
