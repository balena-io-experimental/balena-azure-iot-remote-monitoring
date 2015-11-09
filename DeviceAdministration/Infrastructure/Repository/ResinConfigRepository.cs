using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Models;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Configurations;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Utility;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.DeviceSchema;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Repository
{
    /// <summary>
    /// Wraps calls to the DocumentDB resin config store
    /// </summary>
    public class ResinConfigRepository : IResinConfigRepository
    {
        // Configuration strings for use in accessing the DocumentDB, Database and DocumentCollection
        readonly string _endpointUri;
        readonly string _authorizationKey;
        readonly string _databaseId;
        readonly string _resinConfigCollectionId;

        IDocDbRestUtility _docDbRestUtil;

        public ResinConfigRepository(IConfigurationProvider configProvider)
        {
            _endpointUri = configProvider.GetConfigurationSettingValue("docdb.EndpointUrl");
            _authorizationKey = configProvider.GetConfigurationSettingValue("docdb.PrimaryAuthorizationKey");
            _databaseId = configProvider.GetConfigurationSettingValue("docdb.DatabaseId");
            _resinConfigCollectionId = configProvider.GetConfigurationSettingValue("docdb.ResinConfigCollectionId");

            _docDbRestUtil = new DocDbRestUtility(_endpointUri, _authorizationKey,
                _databaseId, _resinConfigCollectionId);

            Task.Run(() => _docDbRestUtil.InitializeDatabase()).Wait();
            Task.Run(() => _docDbRestUtil.InitializeCollection()).Wait();
        }

        /// <summary>
        /// Gets the config asynchronously.
        /// </summary>
        /// <returns>The config object, or null if it's not found</returns>
        public async Task<dynamic> GetConfigAsync()
        {
            dynamic result = null;

            string query = "SELECT VALUE root FROM root";
            DocDbRestQueryResult foundConfig = await _docDbRestUtil.QueryCollectionAsync(query, null);

            JArray docs = foundConfig.ResultSet;

            if (docs != null && docs.Count > 0)
            {
                result = docs.Children().ElementAt(0);
            }

            return result;
        }

        /// <summary>
        /// Adds the config to the DocumentDB.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private async Task<dynamic> CreateConfigAsync(dynamic config)
        {
            return await _docDbRestUtil.SaveNewDocumentAsync(config);
        }

        /// <summary>
        /// Updates the config asynchronously.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <returns>The updated config object</returns>
        public async Task<dynamic> UpdateConfigAsync(dynamic config)
        {
            dynamic existingConfig = await GetConfigAsync();

            if (existingConfig == null)
            {
                return await CreateConfigAsync(config);
            }

            string incomingRid = DeviceSchemaHelper.GetDocDbRid(config);

            if (string.IsNullOrWhiteSpace(incomingRid))
            {
                // copy the existing _rid onto the incoming data if needed
                var existingRid = DeviceSchemaHelper.GetDocDbRid(existingConfig);
                if (string.IsNullOrWhiteSpace(existingRid))
                {
                    throw new InvalidOperationException("Could not find _rid property on existing config");
                }
                config._rid = existingRid;
            }

            string incomingId = DeviceSchemaHelper.GetDocDbId(config);

            if (string.IsNullOrWhiteSpace(incomingId))
            {
                // copy the existing id onto the incoming data if needed
                var existingId = DeviceSchemaHelper.GetDocDbId(existingConfig);
                if (string.IsNullOrWhiteSpace(existingId))
                {
                    throw new InvalidOperationException("Could not find id property on existing config");
                }
                config.id = existingId;
            }

            return await _docDbRestUtil.UpdateDocumentAsync(config);
        }

        public static ResinConfig ConvertConfig(dynamic config)
        {
            JObject configObject = config;
            return new ResinConfig()
            {
                ApiHost = ((JValue) config.ApiHost).Value as string,
                AppId = ((JValue)config.AppId).Value as string,
                ApiKey = ((JValue)config.ApiKey).Value as string
            };
        }

    }
}
