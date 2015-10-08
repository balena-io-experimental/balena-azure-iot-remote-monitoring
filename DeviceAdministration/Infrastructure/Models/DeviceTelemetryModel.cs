using System;
using System.Runtime.Serialization;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Models
{
    /// <summary>
    /// A model that represents a Device's telemetry recording.
    /// </summary>
    [Serializable]
    [DataContract(Name = "DeviceTelemetryModel")]
    public class DeviceTelemetryModel
    {
        /// <summary>
        /// Gets or sets the ID of the Device for which telemetry applies.
        /// </summary>
        [DataMember(Name = "deviceId")]
        public string DeviceId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the represented telemetry recording's external 
        /// temperature value.
        /// </summary>
        [DataMember(Name = "externalTemperature")]
        public double? ExternalTemperature
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the represented telemetry recording's humidity 
        /// value.
        /// </summary>
        [DataMember(Name = "humidity")]
        public double? Humidity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the represented telemetry recording's temperature 
        /// value.
        /// </summary>
        [DataMember(Name = "temperature")]
        public double? Temperature
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time of record for the represented telemetry 
        /// recording.
        /// </summary>
        [DataMember(Name = "timestamp")]
        public DateTime? Timestamp
        {
            get;
            set;
        }
    }
}
