namespace PetFamily.Application.FileManagement.Providers
{
    public interface IFilesCleanerService
    {
        Task Process(CancellationToken cancelationToken = default);
    }
}
