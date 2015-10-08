using System;
using System.Runtime.Serialization;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Models
{
    /// <summary>
    /// Model object that extends ActionMapping with additional data
    /// </summary>
    [Serializable]
    [DataContract(Name = "ActionMappingExtended")]
    public class ActionMappingExtended : ActionMapping
    {
        [DataMember(Name = "numberOfDevices")]
        public int NumberOfDevices { get; set; }
    }
}
