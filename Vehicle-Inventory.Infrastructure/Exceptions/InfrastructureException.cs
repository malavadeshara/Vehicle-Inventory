using System;

namespace Vehicle_Inventory.Infrastructure.Exceptions
{
    public class InfrastructureException : Exception
    {
        public InfrastructureErrorCode Code { get; }

        public InfrastructureException(InfrastructureErrorCode code)
            : base(code.ToString())
        {
            Code = code;
        }

        public InfrastructureException(InfrastructureErrorCode code, Exception innerException)
            : base(code.ToString(), innerException)
        {
            Code = code;
        }
    }

    public enum InfrastructureErrorCode
    {
        DatabaseOperationFailed,
        DatabaseUpdateFailed,
        CloudinaryUploadFailed,
        CloudinaryDeleteFailed,
        DatabaseConcurrencyFailed,
    }
}
