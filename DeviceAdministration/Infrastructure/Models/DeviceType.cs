using System;
using System.Runtime.Serialization;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Models
{
    /// <summary>
    /// Represents a type of device that can be selected during the add device wizard
    /// </summary>
    [Serializable]
    [DataContract(Name = "DeviceType")]
    public class DeviceType
    {
        [DataMember(Name = "deviceTypeId")]
        public int DeviceTypeId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "imageUrl")]
        public Uri ImageUrl { get; set; }

        [DataMember(Name = "instructionsUrl")]
        public string InstructionsUrl { get; set; }

        [DataMember(Name = "isSimulatedDevice")]
        public bool IsSimulatedDevice { get; set; }
    }
}
