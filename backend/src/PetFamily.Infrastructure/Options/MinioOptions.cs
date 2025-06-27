namespace PetFamily.Infrastructure.Options
{
    public class MinioOptions
    {
        public const string MINIO = "Minio";
        public string Endpoint { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public bool WithSsl { get; set; } = false;
    }
}
