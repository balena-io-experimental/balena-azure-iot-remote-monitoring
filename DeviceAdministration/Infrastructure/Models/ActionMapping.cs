using System;
using System.Runtime.Serialization;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Models
{
    /// <summary>
    /// Defines a mapping between a RuleOutput value (output from the ASA job looking
    /// for alarm-like conditions), and an ActionId (which is currently
    /// associated with a logic app).
    /// </summary>
    [Serializable]
    [DataContract(Name = "ActionMapping")]
    public class ActionMapping
    {
        [DataMember(Name = "ruleOutput")]
        public string RuleOutput { get; set; }

        [DataMember(Name = "actionId")]
        public string ActionId { get; set; }
    }
}
