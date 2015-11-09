using System.Threading.Tasks;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Models;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Repository
{
    /// <summary>
    /// Interface to expose methods that can be called against the underlying identity repository
    /// CRUD operations.
    /// </summary>
    public interface IResinConfigRepository
    {
        /// <summary>
        /// Gets the config asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<dynamic> GetConfigAsync();

        /// <summary>
        /// Updates the config asynchronously.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <returns></returns>
        Task<dynamic> UpdateConfigAsync(dynamic config);
    }
}
