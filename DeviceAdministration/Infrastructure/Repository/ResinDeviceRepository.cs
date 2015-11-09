using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Models;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Configurations;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Utility;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Exceptions;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Repository
{
    /// <summary>
    /// Wraps calls to the DocumentDB resin config store
    /// </summary>
    public class ResinDeviceRepository : IResinDeviceRegistryCrudRepository
    {
        // Configuration strings for use in accessing the DocumentDB, Database and DocumentCollection
        readonly string _endpointUri;
        readonly string _authorizationKey;
        readonly string _databaseId;
        readonly string _resinDevicesCollectionId;

        IDocDbRestUtility _docDbRestUtil;

        public ResinDeviceRepository(IConfigurationProvider configProvider)
        {
            _endpointUri = configProvider.GetConfigurationSettingValue("docdb.EndpointUrl");
            _authorizationKey = configProvider.GetConfigurationSettingValue("docdb.PrimaryAuthorizationKey");
            _databaseId = configProvider.GetConfigurationSettingValue("docdb.DatabaseId");
            _resinDevicesCollectionId = configProvider.GetConfigurationSettingValue("docdb.ResinDevicesCollectionId");

            _docDbRestUtil = new DocDbRestUtility(_endpointUri, _authorizationKey,
                _databaseId, _resinDevicesCollectionId);

            Task.Run(() => _docDbRestUtil.InitializeDatabase()).Wait();
            Task.Run(() => _docDbRestUtil.InitializeCollection()).Wait();
        }

        /// <summary>
        /// Gets the Resin Device record asynchronously.
        /// </summary>
        /// <returns>The resin device object, or null if it's not found</returns>
        public async Task<dynamic> GetResinDeviceAsync(string resinDeviceId)
        {
            dynamic result = null;

            Dictionary<string, Object> queryParams = new Dictionary<string, Object>();
            queryParams.Add("@id", resinDeviceId);
            DocDbRestQueryResult response = await _docDbRestUtil.QueryCollectionAsync(
                "SELECT VALUE root FROM root WHERE (root.ResinDeviceId = @id)", queryParams);
            JArray foundDevices = response.ResultSet;

            if (foundDevices != null && foundDevices.Count > 0)
            {
                result = foundDevices.Children().ElementAt(0);
            }

            return result;
        }

        /// <summary>
        /// Adds a resin device record asynchronously.
        /// </summary>
        /// <param name="resinDevice">The resin device record.</param>
        /// <returns></returns>
        public async Task<dynamic> AddResinDeviceAsync(dynamic resinDevice)
        {
            string deviceId = resinDevice.ResinDeviceId;
            dynamic existingDevice = await GetResinDeviceAsync(deviceId);

            if (existingDevice != null)
            {
                throw new ResinDeviceAlreadyRegisteredException(deviceId);
            }

            return await _docDbRestUtil.SaveNewDocumentAsync(resinDevice);
        }
    }
}
