using Vehicle_Inventory.Application.Common;

namespace Vehicle_Inventory.Application.Interfaces.Services;

public interface IImageStorageService
{
    Task<UserImageUploadResult> UploadAsync(
        Stream fileStream,
        string folder,
        string publicId);

    Task DeleteAsync(string publicId);
}