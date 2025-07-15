namespace PetFamily.Application.Volonteers.AddPetPhotos
{
    public record AddPetPhotosCommand(Guid VolonteerId, Guid PetId, string Bucket, IEnumerable<Stream> Content);
}
