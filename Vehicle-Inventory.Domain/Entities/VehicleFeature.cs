//namespace Vehicle_Inventory.Domain.Entities;

//public class VehicleFeature
//{
//    public int Id { get; private set; }
//    public int VehicleId { get; private set; }

//    public string FeatureName { get; private set; }

//    protected VehicleFeature() { }

//    public VehicleFeature(string featureName)
//    {
//        FeatureName = featureName;
//    }
//}

using System.Xml.Linq;
using Vehicle_Inventory.Domain.Exceptions;

namespace Vehicle_Inventory.Domain.Entities;

public class VehicleFeature
{
    public int Id { get; private set; }
    public int VehicleId { get; private set; }

    public string FeatureName { get; private set; }

    protected VehicleFeature() { }

    public VehicleFeature(string featureName)
    {
        if (string.IsNullOrWhiteSpace(featureName))
            throw new DomainException(DomainErrorCode.VehicleFeatureNameRequired);

        FeatureName = featureName;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException(DomainErrorCode.VehicleFeatureCannotBeNull);

        FeatureName = newName;
    }

}
