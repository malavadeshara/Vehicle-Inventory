using System;

namespace Vehicle_Inventory.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationErrorCode Code { get; }

        public ValidationException(ValidationErrorCode code)
            : base(code.ToString())
        {
            Code = code;
        }
    }

    public enum ValidationErrorCode
    {
        PageNumberInvalid,
        PageSizeInvalid,
        MinPriceGreaterThanMaxPrice,
        UserAlreadyExists,
        InvalidCredentials,
        RefreshTokenExpired,
        UserNotFound,
        VehicleNotFound, // add this
        InvalidRefreshToken
    }
}