using System.ComponentModel.DataAnnotations;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Web.Models
{
    public class ResinConfigModel
    {
        [Required]
        public string AppId { get; set; }

        [Required]
        public string ApiKey { get; set; }

        public string ApiHost { get; set; }

        public ResinConfigModel()
        {
            ApiHost = "api.resin.io";
        }
    }
}