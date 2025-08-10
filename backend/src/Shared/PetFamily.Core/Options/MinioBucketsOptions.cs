namespace PetFamily.Core.Options
{
    public class MinioBucketOptions
    {
        public const int EXPIRY_TIME_IN_SECONDS = 60 * 60 * 24; // 24 hours   
        public const string MINIO_BUCKETS = "MinioBuckets";
        public string Photos { get; set; } = string.Empty;
        public int MaxWriteConcurrency { get; set; }
        public int MaxDeleteConcurrency { get; set; }
    }
}
