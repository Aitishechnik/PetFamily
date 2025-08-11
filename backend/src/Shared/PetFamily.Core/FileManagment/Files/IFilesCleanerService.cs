namespace PetFamily.Core.FileManagement.Providers
{
    public interface IFilesCleanerService
    {
        Task Process(CancellationToken cancelationToken = default);
    }
}
