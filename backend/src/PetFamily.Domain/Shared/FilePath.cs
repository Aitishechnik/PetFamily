using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared
{
    public record FilePath
    {
        public FilePath() { }
        private FilePath(string path)
        {
            Path = path;
        }
        public string Path { get; }

        public static Result<FilePath, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Errors.General.ValueIsInvalid("File path cannot be null or empty.");
            }
            return new FilePath(value);
        }
    }
}
