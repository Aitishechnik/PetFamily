namespace PetFamily.Application.Volonteers.AddPetPhotos
{
    public record AddPetPhotosRequest(Guid VolonteerId, Guid PetId, string Bucket, IEnumerable<Stream> Content);
}
