using System;
using System.Runtime.Serialization;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Models
{
    /// <summary>
    /// A model that summarizes a period of Device telemetry.
    /// </summary>
    [Serializable]
    [DataContract(Name = "DeviceTelemetrySummaryModel")]
    public class DeviceTelemetrySummaryModel
    {
        /// <summary>
        /// Gets or sets the covered period's average humidity.
        /// </summary>
        [DataMember(Name = "averageHumidity")]
        public double? AverageHumidity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ID of the Device, covered by the summarized 
        /// telemetry.
        /// </summary>
        [DataMember(Name = "deviceId")]
        public string DeviceId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the covered period's maximum humidity.
        /// </summary>
        [DataMember(Name = "maximumHumidity")]
        public double? MaximumHumidity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the covered period's minimum humidity.
        /// </summary>
        [DataMember(Name = "minimumHumidity")]
        public double? MinimumHumidity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of minutes the represented period covers.
        /// </summary>
        [DataMember(Name = "timeFrameMinutes")]
        public double? TimeFrameMinutes
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
