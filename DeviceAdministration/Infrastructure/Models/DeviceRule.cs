using System;
using System.Runtime.Serialization;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Models
{
    /// <summary>
    /// Container object for a DeviceRule
    /// </summary>
    [Serializable]
    [DataContract(Name = "DeviceRule")]
    public class DeviceRule
    {
        public DeviceRule() { }

        public DeviceRule(string ruleId)
        {
            RuleId = ruleId;
        }

        [DataMember(Name = "ruleId")]
        public string RuleId { get; set; }

        [DataMember(Name = "enabledState")]
        public bool EnabledState { get; set; }

        [DataMember(Name = "deviceID")]
        public string DeviceID { get; set; }

        [DataMember(Name = "dataField")]
        public string DataField { get; set; }

        [DataMember(Name = "operator")]
        public string Operator { get; set; }

        [DataMember(Name = "threshold")]
        public double? Threshold { get; set; }

        [DataMember(Name = "ruleOutput")]
        public string RuleOutput { get; set; }

        [DataMember(Name = "etag")]
        public string Etag { get; set; }

        /// <summary>
        /// This method will initialize any required, and automatically-built properties for a new rule
        /// </summary>
        public void InitializeNewRule(string deviceId)
        {
            DeviceID = deviceId;
            RuleId = Guid.NewGuid().ToString();
            EnabledState = true;
            Operator = ">";
        }
    }
}
