using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.EventProcessor.WebJob.Processors
{
    class ResinApiClient
    {
        private dynamic _config;

        public ResinApiClient(dynamic config)
        {
            _config = config;
        }

        private string BuildUrl(string path, NameValueCollection queryParams = null)
        {
            var builder = new UriBuilder(String.Format("https://{0}/{1}", _config.ApiHost, path));

            var query = String.Format("apikey={0}", _config.ApiKey);
            if (queryParams != null)
            {
                foreach (string key in queryParams)
                {
                    query += "&" + key + "=" + queryParams[key];
                }
            }
            builder.Query = query;
            string url = builder.ToString();

            return url;
        }

        private async Task<JObject> ParseResponse(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseContent);
        }

        public async Task<JObject> GetAsync(string path, NameValueCollection queryParams = null)
        {
            var url = BuildUrl(path, queryParams);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                return await ParseResponse(response);
            }
        }

        public async Task<JObject> PostAsync(string path, NameValueCollection queryParams = null,
            object body = null)
        {
            var url = BuildUrl(path, queryParams);

            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(url, body);
                return await ParseResponse(response);
            }
        }

        public async Task CreateAppEnvVarAsync(string appId, string name, string value)
        {
            var url = "ewa/environment_variable";
            var body = new { application = appId, name = name, value = value };
            await PostAsync(url, body: body);
        }

        public async Task CreateDeviceEnvVarAsync(string deviceId, string name, string value)
        {
            var url = "ewa/device_environment_variable";
            var body = new { device = deviceId, env_var_name = name, value = value };
            await PostAsync(url, body: body);
        }
    }
}
