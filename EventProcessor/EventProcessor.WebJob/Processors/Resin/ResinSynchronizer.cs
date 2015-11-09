using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Configurations;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.DeviceSchema;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Models;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Repository;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Utility;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.BusinessLogic;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Models;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Repository;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.EventProcessor.WebJob.Processors
{
    class ResinSynchronizer
    {
        private ResinConfig _config;
        private ResinApiClient _apiClient;
        private IConfigurationProvider _configProvider;
        private IResinDeviceRegistryCrudRepository _resinDeviceRepository;
        private IDeviceLogic _deviceLogic;

        private const string IOT_HUB_HOST = "IOT_HUB_HOST";
        private const string IOT_HUB_NAME = "IOT_HUB_NAME";
        private const string IOT_HUB_SUFFIX = "IOT_HUB_SUFFIX";
        private const string IOT_HUB_DEVICE_ID = "IOT_HUB_DEVICE_ID";
        private const string IOT_HUB_DEVICE_KEY = "IOT_HUB_DEVICE_KEY";


        public ResinSynchronizer(ResinConfig config, IConfigurationProvider configProvider)
        {
            _config = config;
            _configProvider = configProvider;
            _apiClient = new ResinApiClient(_config);
            _resinDeviceRepository = new ResinDeviceRepository(configProvider);

            var iotHubRepository = new IotHubRepository(configProvider);
            var deviceDocDbUtil = new DocDbRestUtility(configProvider);
            var deviceRegistryRepository = new DeviceRegistryRepository(configProvider, deviceDocDbUtil);
            var virtualDeviceTableStorage = new VirtualDeviceTableStorage(configProvider);
            var securityKeyGenerator = new SecurityKeyGenerator();
            var deviceRulesRepository = new DeviceRulesRepository(configProvider);
            var actionMappingRepository = new ActionMappingRepository(configProvider);
            var actionMappingLogic = new ActionMappingLogic(actionMappingRepository, deviceRulesRepository);
            var deviceRulesLogic = new DeviceRulesLogic(deviceRulesRepository, actionMappingLogic);

            _deviceLogic = new DeviceLogic(iotHubRepository,
                deviceRegistryRepository,
                deviceRegistryRepository,
                virtualDeviceTableStorage,
                securityKeyGenerator,
                configProvider,
                deviceRulesLogic);
        }

        private async Task<JObject> GetApplicationData()
        {
            var path = String.Format("resin/application({0})", _config.AppId);
            var queryParams = new NameValueCollection
            {
                { "$expand", "device,environment_variable" }
            };

            var result = await _apiClient.GetAsync(path, queryParams);

            return result["d"][0] as JObject;
        }

        private bool JArrayContainsElement(JArray array, string key, string value)
        {
            if (array == null)
            {
                return false;
            }

            foreach (var item in array.Children())
            {
                var element = item as JObject;
                if ((string)element[key] == value)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task CreateMissingAppEnvVar(JArray existingEnvVars, string name, string value)
        {
            if (JArrayContainsElement(existingEnvVars, "name", name))
            {
                Trace.TraceInformation("ResinSynchronizer.CreateMissingAppEnvVar name: {0} - exists, skipping", name);
                return;
            }
            Trace.TraceInformation("ResinSynchronizer.CreateMissingAppEnvVar name: {0} - creating", name);
            await _apiClient.CreateAppEnvVarAsync(_config.AppId, name, value);
            Trace.TraceInformation("ResinSynchronizer.CreateMissingAppEnvVar name: {0} - OK", name);
        }

        private async Task FixAppEnvVars(JArray existingEnvVars)
        {
            var iotHubHost = _configProvider.GetConfigurationSettingValue("iotHub.HostName");
            var dotIndex = iotHubHost.IndexOf('.');
            var iotHubName = iotHubHost.Substring(0, dotIndex);
            var iotHubSuffix = iotHubHost.Substring(dotIndex + 1);
            Trace.TraceInformation("ResinSynchronizer.FixAppEnvVars iotHubHost: {0}", iotHubHost);
            Trace.TraceInformation("ResinSynchronizer.FixAppEnvVars iotHubName: {0}", iotHubName);
            Trace.TraceInformation("ResinSynchronizer.FixAppEnvVars iotHubSuffix: {0}", iotHubSuffix);
            var createIotHubHost = CreateMissingAppEnvVar(existingEnvVars, IOT_HUB_HOST, iotHubHost);
            var createIotHubName = CreateMissingAppEnvVar(existingEnvVars, IOT_HUB_NAME, iotHubName);
            var createIotHubSuffix = CreateMissingAppEnvVar(existingEnvVars, IOT_HUB_SUFFIX, iotHubSuffix);
            await Task.WhenAll(new Task[] { createIotHubHost, createIotHubName, createIotHubSuffix });
        }

        private async Task<DeviceWithKeys> CreateIoTDevice(string deviceType)
        {
            var deviceId = Guid.NewGuid().ToString();
            var device = DeviceSchemaHelper.BuildDeviceStructure(deviceId, false, null);
            device[DeviceModelConstants.DEVICE_PROPERTIES].Add(
                DevicePropertiesConstants.MANUFACTURER, "resin.io");
            device[DeviceModelConstants.DEVICE_PROPERTIES].Add(
                DevicePropertiesConstants.MODEL_NUMBER, deviceType);

            var deviceWithKeys = await _deviceLogic.AddDeviceAsync(device);
            return deviceWithKeys;
        }

        private async Task HandleResinDevice(JObject device, string deviceType)
        {
            var resinDeviceId = (string)device["id"];
            var resinDeviceUuid = (string)device["uuid"];
            Trace.TraceInformation("ResinSynchronizer.HandleResinDevice ID: {0}", resinDeviceId);

            var existingRecord = await _resinDeviceRepository.GetResinDeviceAsync(resinDeviceId);
            if (existingRecord != null)
            {
                Trace.TraceInformation("ResinSynchronizer.HandleResinDevice ID: {0} - exists, skipping", resinDeviceId);
                return;
            }

            Trace.TraceInformation("ResinSynchronizer.HandleResinDevice ID: {0} - creating", resinDeviceId);
            var deviceWithKeys = await CreateIoTDevice(deviceType);
            var deviceId = ((JValue)
                deviceWithKeys.Device.DeviceProperties.DeviceID).Value as string;
            Trace.TraceInformation("ResinSynchronizer.HandleResinDevice Created IoT device, ID: {0}", deviceId);

            var resinDeviceProperties = new JObject();
            resinDeviceProperties.Add("DeviceId", deviceId);
            resinDeviceProperties.Add("ResinDeviceId", resinDeviceId);
            resinDeviceProperties.Add("ResinDeviceUuid", resinDeviceUuid);
            var resinDevice = _resinDeviceRepository.AddResinDeviceAsync(resinDeviceProperties);

            var createDeviceHubDeviceId = _apiClient.CreateDeviceEnvVarAsync(resinDeviceId,
                IOT_HUB_DEVICE_ID, deviceId);
            var key = deviceWithKeys.SecurityKeys.PrimaryKey;
            var createDeviceHubDeviceKey = _apiClient.CreateDeviceEnvVarAsync(resinDeviceId,
                IOT_HUB_DEVICE_KEY, deviceWithKeys.SecurityKeys.PrimaryKey);

            await Task.WhenAll(new Task[] { resinDevice, createDeviceHubDeviceId, createDeviceHubDeviceKey });
            Trace.TraceInformation("ResinSynchronizer.HandleResinDevice ID: {0} - OK", resinDeviceId);
        }

        private async Task CreateMissingDevices(JArray devices, string deviceType)
        {
            Trace.TraceInformation("ResinSynchronizer.CreateMissingDevices Started");
            var tasks = new Task[devices.Count];
            var i = 0;

            foreach (var item in devices.Children())
            {
                tasks[i++] = HandleResinDevice(item as JObject, deviceType);
            }

            await Task.WhenAll(tasks);
            Trace.TraceInformation("ResinSynchronizer.CreateMissingDevices Done");
        }

        public async Task Run()
        {
            Trace.TraceInformation("ResinSynchronizer.Run Started");

            var application = await GetApplicationData();
            var deviceType = (string)application["device_type"];

            Trace.TraceInformation("ResinSynchronizer.Run Application device type: {0}", deviceType);

            var environmentVariables = application["environment_variable"] as JArray;
            var fixApEnvVars = FixAppEnvVars(environmentVariables);

            var devices = application["device"] as JArray;
            Trace.TraceInformation("ResinSynchronizer.Run Total devices found: {0}", devices.Count);
            var createMissingDevices = CreateMissingDevices(devices, deviceType);

            await Task.WhenAll(new Task[] { fixApEnvVars, createMissingDevices });

            Trace.TraceInformation("ResinSynchronizer.Run Done");
        }
    }
}
