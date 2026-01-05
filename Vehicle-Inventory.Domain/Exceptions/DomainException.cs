//using System;

//namespace Vehicle_Inventory.Domain.Exceptions
//{
//    public enum DomainErrorCode
//    {
//        // User
//        UserNameRequired,
//        EmailRequired,
//        PasswordRequired,
//        InvalidRefreshToken,
//        RefreshTokenExpiryInvalid,

//        // Vehicle
//        VehicleNameRequired,
//        VehicleModelRequired,
//        VehicleYearInvalid,
//        VehiclePriceInvalid,
//        VehicleCurrencyRequired,
//        VehicleImageCannotBeNull,
//        VehicleDuplicateImage,
//        VehicleImageNotFound,
//        VehicleFeatureCannotBeNull,
//        VehicleDimensionsRequired,
//        VehicleSpecificationsRequired,

//        // VehicleFeature
//        VehicleFeatureNameRequired,

//        // VehicleImage
//        VehicleImagePublicIdRequired,
//        VehicleImageUrlRequired,
//        VehicleImageDisplayOrderInvalid,

//        // VehicleSpecification
//        VehicleSpecificationSeatingInvalid
//    }

//    public class DomainException : Exception
//    {
//        public DomainErrorCode ErrorCode { get; }

//        public DomainException(DomainErrorCode code)
//            : base(GetMessage(code))
//        {
//            ErrorCode = code;
//        }

//        private static string GetMessage(DomainErrorCode code) => code switch
//        {
//            // User
//            DomainErrorCode.UserNameRequired => "User name is required.",
//            DomainErrorCode.EmailRequired => "Email is required.",
//            DomainErrorCode.PasswordRequired => "Password is required.",
//            DomainErrorCode.InvalidRefreshToken => "Refresh token is invalid.",
//            DomainErrorCode.RefreshTokenExpiryInvalid => "Refresh token expiry date is invalid.",

//            // Vehicle
//            DomainErrorCode.VehicleNameRequired => "Vehicle name is required.",
//            DomainErrorCode.VehicleModelRequired => "Vehicle model is required.",
//            DomainErrorCode.VehicleYearInvalid => "Vehicle year must be greater than 0.",
//            DomainErrorCode.VehiclePriceInvalid => "Vehicle price must be greater than 0.",
//            DomainErrorCode.VehicleCurrencyRequired => "Vehicle currency is required.",
//            DomainErrorCode.VehicleImageCannotBeNull => "Vehicle image cannot be null.",
//            DomainErrorCode.VehicleDuplicateImage => "Duplicate vehicle image is not allowed.",
//            DomainErrorCode.VehicleImageNotFound => "Vehicle image not found.",
//            DomainErrorCode.VehicleFeatureCannotBeNull => "Vehicle feature cannot be null.",
//            DomainErrorCode.VehicleDimensionsRequired => "Vehicle dimensions are required.",
//            DomainErrorCode.VehicleSpecificationsRequired => "Vehicle specifications are required.",

//            // VehicleFeature
//            DomainErrorCode.VehicleFeatureNameRequired => "Vehicle feature name is required.",

//            // VehicleImage
//            DomainErrorCode.VehicleImagePublicIdRequired => "Vehicle image PublicId is required.",
//            DomainErrorCode.VehicleImageUrlRequired => "Vehicle image URL is required.",
//            DomainErrorCode.VehicleImageDisplayOrderInvalid => "Vehicle image display order must be 0 or greater.",

//            // VehicleSpecification
//            DomainErrorCode.VehicleSpecificationSeatingInvalid => "Vehicle seating must be greater than 0.",

//            _ => "A domain error occurred."
//        };
//    }
//}


using System;

namespace Vehicle_Inventory.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainErrorCode Code { get; }

        public DomainException(DomainErrorCode code)
            : base(code.ToString()) // Optional: message is enum name
        {
            Code = code;
        }
    }

    public enum DomainErrorCode
    {
        UserNameRequired,
        EmailRequired,
        PasswordRequired,
        InvalidRefreshToken,
        RefreshTokenExpiryInvalid,

        VehicleNameRequired,
        VehicleModelRequired,
        VehicleYearInvalid,
        VehiclePriceInvalid,
        VehicleCurrencyRequired,
        VehicleImageCannotBeNull,
        VehicleDuplicateImage,
        VehicleImageNotFound,
        VehicleFeatureCannotBeNull,
        VehicleDimensionsRequired,
        VehicleSpecificationsRequired,

        VehicleFeatureNameRequired,
        VehicleFeatureNotFound,

        VehicleImagePublicIdRequired,
        VehicleImageUrlRequired,
        VehicleImageDisplayOrderInvalid,

        VehicleSpecificationSeatingInvalid
    }
}