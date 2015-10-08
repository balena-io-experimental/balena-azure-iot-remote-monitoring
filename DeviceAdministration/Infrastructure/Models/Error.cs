using System;
using System.Runtime.Serialization;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Models
{
    /// <summary>
    /// Wraps error details to pass back to the caller of a WebAPI
    /// </summary>
    [Serializable()]
    [DataContract(Name = "Error")]
    public class Error
    {
        public enum ErrorType
        {
            Exception = 0,
            Validation = 1
        }

        [DataMember(Name = "type")]
        public ErrorType Type { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        public Error(Exception exception)
        {
            Type = ErrorType.Exception;
            Message = Strings.UnexpectedErrorOccurred;
        }

        public Error(string validationError)
        {
            Type = ErrorType.Validation;
            Message = validationError;
        }
    }
}
