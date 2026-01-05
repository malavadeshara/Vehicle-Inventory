//using CloudinaryDotNet;
//using CloudinaryDotNet.Actions;
//using Microsoft.Extensions.Options;
//using Vehicle_Inventory.Application.Common;
//using Vehicle_Inventory.Application.Interfaces.Services;
//using Vehicle_Inventory.Infrastructure.Settings;

//namespace Vehicle_Inventory.Infrastructure.Services;

//public class CloudinaryImageStorageService : IImageStorageService
//{
//    private readonly Cloudinary _cloudinary;

//    public CloudinaryImageStorageService(
//        IOptions<CloudinarySettings> options)
//    {
//        var settings = options.Value;

//        _cloudinary = new Cloudinary(new Account(
//            settings.CloudName,
//            settings.ApiKey,
//            settings.ApiSecret));
//    }

//    public async Task<UserImageUploadResult> UploadAsync(
//        Stream fileStream,
//        string folder,
//        string publicId)
//    {
//        var uploadParams = new ImageUploadParams
//        {
//            File = new FileDescription(publicId, fileStream),
//            Folder = folder,
//            PublicId = publicId,
//            Overwrite = false
//        };

//        var result = await _cloudinary.UploadAsync(uploadParams);

//        if (result.StatusCode != System.Net.HttpStatusCode.OK)
//            throw new Exception("Cloudinary upload failed");

//        return new UserImageUploadResult
//        {
//            PublicId = result.PublicId,
//            SecureUrl = result.SecureUrl.ToString()
//        };
//    }

//    public async Task DeleteAsync(string publicId)
//    {
//        var deleteParams = new DeletionParams(publicId);
//        await _cloudinary.DestroyAsync(deleteParams);
//    }
//}


using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Vehicle_Inventory.Application.Common;
using Vehicle_Inventory.Application.Interfaces.Services;
using Vehicle_Inventory.Infrastructure.Exceptions;
using Vehicle_Inventory.Infrastructure.Settings;

namespace Vehicle_Inventory.Infrastructure.Services;

public class CloudinaryImageStorageService : IImageStorageService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryImageStorageService(
        IOptions<CloudinarySettings> options)
    {
        var settings = options.Value;
        _cloudinary = new Cloudinary(new Account(
            settings.CloudName,
            settings.ApiKey,
            settings.ApiSecret));
    }

    public async Task<UserImageUploadResult> UploadAsync(
        Stream fileStream,
        string folder,
        string publicId)
    {
        try
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(publicId, fileStream),
                Folder = folder,
                PublicId = publicId,
                Overwrite = false
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                throw new InfrastructureException(InfrastructureErrorCode.CloudinaryUploadFailed);

            return new UserImageUploadResult
            {
                PublicId = result.PublicId,
                SecureUrl = result.SecureUrl.ToString()
            };
        }
        catch (InfrastructureException)
        {
            throw; // rethrow known infrastructure exceptions
        }
        catch (Exception ex)
        {
            throw new InfrastructureException(InfrastructureErrorCode.CloudinaryUploadFailed, ex);
        }
    }

    public async Task DeleteAsync(string publicId)
    {
        try
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                throw new InfrastructureException(InfrastructureErrorCode.CloudinaryDeleteFailed);
        }
        catch (InfrastructureException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InfrastructureException(InfrastructureErrorCode.CloudinaryDeleteFailed, ex);
        }
    }
}