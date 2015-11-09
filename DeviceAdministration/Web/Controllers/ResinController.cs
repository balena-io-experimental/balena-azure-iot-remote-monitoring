using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Web.Models;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Web.Security;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Repository;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Web.Controllers
{
    [Authorize]
    public class ResinController : Controller
    {
        readonly IResinConfigRepository _resinConfigRepository;

        public ResinController(IResinConfigRepository resinConfigRepository)
        {
            _resinConfigRepository = resinConfigRepository;
        }

        [RequirePermission(Permission.EditResinConfig)]
        public async Task<ActionResult> EditConfig()
        {
            var config = await _resinConfigRepository.GetConfigAsync();
            var configModel = new ResinConfigModel
            {
                AppId = config != null ? config.AppId : "",
                ApiKey = config != null ? config.ApiKey : ""
            };
            return View(configModel);
        }

        [HttpPost]
        [RequirePermission(Permission.EditResinConfig)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateConfig(ResinConfigModel model)
        {
            var config = new JObject();
            config.Add("AppId", model.AppId);
            config.Add("ApiKey", model.ApiKey);
            config.Add("ApiHost", model.ApiHost);

            var resinConfig = await _resinConfigRepository.UpdateConfigAsync(config);
            return Json(new { data = resinConfig });
        }
    }
}