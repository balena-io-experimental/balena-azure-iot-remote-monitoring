using System;
using System.Runtime.Serialization;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Models;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Models
{
    [Serializable]
    [DataContract(Name = "DeviceWithKeys")]
    public class DeviceWithKeys
    {
        [DataMember(Name = "device")]
        public dynamic Device { get; set; }

        [DataMember(Name = "securityKeys")]
        public SecurityKeys SecurityKeys { get; set; }

        public DeviceWithKeys(dynamic device, SecurityKeys securityKeys)
        {
            Device = device;
            SecurityKeys = securityKeys;
        }
    }
}
