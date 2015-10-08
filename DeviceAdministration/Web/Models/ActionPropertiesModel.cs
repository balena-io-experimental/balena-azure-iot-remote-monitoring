using System;
using System.Runtime.Serialization;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Web.Security;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Web.Models
{
    [Serializable]
    [DataContract(Name = "ActionPropertiesModel")]
    public class ActionPropertiesModel
    {
        [DataMember(Name = "ruleOutput")]
        public string RuleOutput { get; set; }

        [DataMember(Name = "actionId")]
        public string ActionId { get; set; }

        public bool HasAssignActionPerm
        {
            get
            {
                return PermsChecker.HasPermission(Permission.AssignAction);
            }
        }

        [DataMember(Name = "updateActionModel")]
        public UpdateActionModel UpdateActionModel { get; set; }
    }
}