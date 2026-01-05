//namespace Vehicle_Inventory.Domain.Entities;

//public class Vehicle
//{
//    public int Id { get; private set; }

//    public string Name { get; private set; }
//    public string Model { get; private set; }
//    public int Year { get; private set; }
//    public decimal Price { get; private set; }
//    public string Currency { get; private set; }
//    public bool InStock { get; private set; }

//    public string? ShortDescription { get; private set; }
//    public string? DetailedDescription { get; private set; }

//    public VehicleDimension? Dimensions { get; private set; }
//    public VehicleSpecification? Specifications { get; private set; }

//    private readonly List<VehicleImage> _images = new();
//    public IReadOnlyCollection<VehicleImage> Images => _images;

//    private readonly List<VehicleFeature> _features = new();
//    public IReadOnlyCollection<VehicleFeature> Features => _features;

//    protected Vehicle() { }

//    public Vehicle(string name, string model, int year, decimal price, string currency)
//    {
//        Name = name;
//        Model = model;
//        Year = year;
//        Price = price;
//        Currency = currency;
//        InStock = true;
//    }

//    // ---------------- DOMAIN BEHAVIOR ----------------

//    public void UpdateBasicInfo(
//        string name,
//        string model,
//        int year,
//        decimal price,
//        string currency,
//        bool inStock,
//        string? shortDescription,
//        string? detailedDescription)
//    {
//        Name = name;
//        Model = model;
//        Year = year;
//        Price = price;
//        Currency = currency;
//        InStock = inStock;
//        ShortDescription = shortDescription;
//        DetailedDescription = detailedDescription;
//    }

//    public void AddImage(VehicleImage image) => _images.Add(image);

//    public void RemoveImage(string imagePath)
//        => _images.RemoveAll(i => i.ImagePath == imagePath);

//    public void ClearImages() => _images.Clear();

//    public void AddFeature(VehicleFeature feature) => _features.Add(feature);

//    public void ClearFeatures() => _features.Clear();

//    public void SetDimensions(VehicleDimension dimensions) => Dimensions = dimensions;

//    public void SetSpecifications(VehicleSpecification specs) => Specifications = specs;
//}



//namespace Vehicle_Inventory.Domain.Entities;

//public class Vehicle
//{
//    public int Id { get; private set; }

//    public string Name { get; private set; }
//    public string Model { get; private set; }
//    public int Year { get; private set; }
//    public decimal Price { get; private set; }
//    public string Currency { get; private set; }
//    public bool InStock { get; private set; }

//    public string? ShortDescription { get; private set; }
//    public string? DetailedDescription { get; private set; }

//    public VehicleDimension? Dimensions { get; private set; }
//    public VehicleSpecification? Specifications { get; private set; }

//    private readonly List<VehicleImage> _images = new();
//    public IReadOnlyCollection<VehicleImage> Images => _images;

//    private readonly List<VehicleFeature> _features = new();
//    public IReadOnlyCollection<VehicleFeature> Features => _features;

//    protected Vehicle() { }

//    public Vehicle(string name, string model, int year, decimal price, string currency)
//    {
//        Name = name;
//        Model = model;
//        Year = year;
//        Price = price;
//        Currency = currency;
//        InStock = true;
//    }

//    public void UpdateBasicInfo(
//        string name,
//        string model,
//        int year,
//        decimal price,
//        string currency,
//        bool inStock,
//        string? shortDescription,
//        string? detailedDescription)
//    {
//        Name = name;
//        Model = model;
//        Year = year;
//        Price = price;
//        Currency = currency;
//        InStock = inStock;
//        ShortDescription = shortDescription;
//        DetailedDescription = detailedDescription;
//    }

//    public void AddImage(VehicleImage image) => _images.Add(image);

//    public void RemoveImage(string publicId)
//        => _images.RemoveAll(i => i.PublicId == publicId);

//    public void ClearImages() => _images.Clear();

//    public void AddFeature(VehicleFeature feature) => _features.Add(feature);

//    public void ClearFeatures() => _features.Clear();

//    public void SetDimensions(VehicleDimension dimensions) => Dimensions = dimensions;

//    public void SetSpecifications(VehicleSpecification specs) => Specifications = specs;
//}


using Vehicle_Inventory.Domain.Exceptions;

namespace Vehicle_Inventory.Domain.Entities;

public class Vehicle
{
    public int Id { get; private set; }

    public string Name { get; private set; }
    public string Model { get; private set; }
    public int Year { get; private set; }
    public decimal Price { get; private set; }
    public string Currency { get; private set; }
    public bool InStock { get; private set; }

    public string? ShortDescription { get; private set; }
    public string? DetailedDescription { get; private set; }

    public VehicleDimension? Dimensions { get; private set; }
    public VehicleSpecification? Specifications { get; private set; }

    private readonly List<VehicleImage> _images = new();
    public IReadOnlyCollection<VehicleImage> Images => _images;

    private readonly List<VehicleFeature> _features = new();
    public IReadOnlyCollection<VehicleFeature> Features => _features;

    protected Vehicle() { }

    public Vehicle(string name, string model, int year, decimal price, string currency)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException(DomainErrorCode.VehicleNameRequired);

        if (string.IsNullOrWhiteSpace(model))
            throw new DomainException(DomainErrorCode.VehicleModelRequired);

        if (year <= 0)
            throw new DomainException(DomainErrorCode.VehicleYearInvalid);

        if (price <= 0)
            throw new DomainException(DomainErrorCode.VehiclePriceInvalid);

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException(DomainErrorCode.VehicleCurrencyRequired);

        Name = name;
        Model = model;
        Year = year;
        Price = price;
        Currency = currency;
        InStock = true;
    }

    public void UpdateBasicInfo(
        string name,
        string model,
        int year,
        decimal price,
        string currency,
        bool inStock,
        string? shortDescription,
        string? detailedDescription)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException(DomainErrorCode.VehicleNameRequired);

        if (string.IsNullOrWhiteSpace(model))
            throw new DomainException(DomainErrorCode.VehicleModelRequired);

        if (year <= 2000)
            throw new DomainException(DomainErrorCode.VehicleYearInvalid);

        if (price <= 0)
            throw new DomainException(DomainErrorCode.VehiclePriceInvalid);

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException(DomainErrorCode.VehicleCurrencyRequired);

        Name = name;
        Model = model;
        Year = year;
        Price = price;
        Currency = currency;
        InStock = inStock;
        ShortDescription = shortDescription;
        DetailedDescription = detailedDescription;
    }

    public void AddImage(VehicleImage image)
    {
        if (image == null)
            throw new DomainException(DomainErrorCode.VehicleImageCannotBeNull);

        if (_images.Any(i => i.PublicId == image.PublicId))
            throw new DomainException(DomainErrorCode.VehicleDuplicateImage);

        _images.Add(image);
    }

    //public void RemoveImage(string publicId)
    //{
    //    if (!_images.Any(i => i.PublicId == publicId))
    //        throw new DomainException(DomainErrorCode.VehicleImageNotFound);

    //    _images.RemoveAll(i => i.PublicId == publicId);
    //}

    public void RemoveImage(string imageUrl)
    {
        if (!_images.Any(i => i.ImageUrl == imageUrl))
            throw new DomainException(DomainErrorCode.VehicleImageNotFound);

        _images.RemoveAll(i => i.ImageUrl == imageUrl);
    }

    public void ClearImages() => _images.Clear();

    public void AddFeature(VehicleFeature feature)
    {
        if (feature == null)
            throw new DomainException(DomainErrorCode.VehicleFeatureCannotBeNull);

        _features.Add(feature);
    }

    public void RemoveFeature(int featureId)
    {
        var feature = _features.FirstOrDefault(f => f.Id == featureId);
        if (feature == null)
            throw new DomainException(DomainErrorCode.VehicleFeatureNotFound);

        _features.Remove(feature);
    }


    public void ClearFeatures() => _features.Clear();

    public void SetDimensions(VehicleDimension dimensions)
    {
        if (dimensions == null)
            throw new DomainException(DomainErrorCode.VehicleDimensionsRequired);

        Dimensions = dimensions;
    }

    public void SetSpecifications(VehicleSpecification specs)
    {
        if (specs == null)
            throw new DomainException(DomainErrorCode.VehicleSpecificationsRequired);

        Specifications = specs;
    }
}