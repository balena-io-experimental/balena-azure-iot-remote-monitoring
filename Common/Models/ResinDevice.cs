namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Models
{
    /// <summary>
    /// Represents a Resin Device record as stored in the system
    /// </summary>
    public class ResinDevice
    {
        public string ResinDeviceId { get; set; }

        public string ResinDeviceUuid { get; set; }

        public string DeviceId { get; set; }

        public ResinDevice(string resinDeviceId, string resinDeviceUuid, string deviceId)
        {
            ResinDeviceId = resinDeviceId;
            ResinDeviceUuid = resinDeviceUuid;
            DeviceId = deviceId;
        }

        public ResinDevice()
        {

        }
    }
}
