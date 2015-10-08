using System;
using System.Runtime.Serialization;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Models
{
    /// <summary>
    /// A model representing an Alert History item.
    /// </summary>
    [Serializable]
    [DataContract(Name = "AlertHistoryItemModel")]
    public class AlertHistoryItemModel
    {
        /// <summary>
        /// Gets or sets the ID of the Device that the represented Alert 
        /// History item covers.
        /// </summary>
        [DataMember(Name = "deviceId")]
        public string DeviceId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the represented Alert History item's Rule Output.
        /// </summary>
        [DataMember(Name = "ruleOutput")]
        public string RuleOutput
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time at whichthe Alert History item occurred.
        /// </summary>
        [DataMember(Name = "timestamp")]
        public DateTime? Timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the represented Alert History item's Value.
        /// </summary>
        [DataMember(Name = "value")]
        public string Value
        {
            get;
            set;
        }
    }
}
