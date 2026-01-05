//namespace Vehicle_Inventory.Domain.Entities;

//public class VehicleImage
//{
//    public int Id { get; private set; }
//    public int VehicleId { get; private set; }

//    public string ImagePath { get; private set; }
//    public int DisplayOrder { get; private set; }

//    protected VehicleImage() { }

//    public VehicleImage(string imagePath, int displayOrder)
//    {
//        ImagePath = imagePath;
//        DisplayOrder = displayOrder;
//    }
//}

//namespace Vehicle_Inventory.Domain.Entities;

//public class VehicleImage
//{
//    public int Id { get; private set; }
//    public int VehicleId { get; private set; }

//    public string PublicId { get; private set; }
//    public string ImageUrl { get; private set; }
//    public int DisplayOrder { get; private set; }

//    protected VehicleImage() { }

//    public VehicleImage(string publicId, string imageUrl, int displayOrder)
//    {
//        PublicId = publicId;
//        ImageUrl = imageUrl;
//        DisplayOrder = displayOrder;
//    }
//}


using Vehicle_Inventory.Domain.Exceptions;

namespace Vehicle_Inventory.Domain.Entities;

public class VehicleImage
{
    public int Id { get; private set; }
    public int VehicleId { get; private set; }

    public string PublicId { get; private set; }
    public string ImageUrl { get; private set; }
    public int DisplayOrder { get; private set; }

    protected VehicleImage() { }

    public VehicleImage(string publicId, string imageUrl, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(publicId))
            throw new DomainException(DomainErrorCode.VehicleImagePublicIdRequired);

        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new DomainException(DomainErrorCode.VehicleImageUrlRequired);

        if (displayOrder < 0)
            throw new DomainException(DomainErrorCode.VehicleImageDisplayOrderInvalid);

        PublicId = publicId;
        ImageUrl = imageUrl;
        DisplayOrder = displayOrder;
    }
}