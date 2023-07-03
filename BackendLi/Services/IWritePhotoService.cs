namespace BackendLi.Services;

public interface IWritePhotoService
{
    public void AddImageToIndex(string imagePath, string imageName);
}