using System.Threading.Tasks;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Models;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Repository
{
    /// <summary>
    /// Interface to expose methods that can be called against the underlying identity repository
    /// CRUD operations.
    /// </summary>
    public interface IResinDeviceRegistryCrudRepository
    {
        /// <summary>
        /// Adds a resin device record asynchronously.
        /// </summary>
        /// <param name="resinDevice">The resin device record.</param>
        /// <returns></returns>
        Task<dynamic> AddResinDeviceAsync(dynamic resinDevice);

        /// <summary>
        /// Gets a resin device record asynchronously.
        /// </summary>
        /// <param name="resinDeviceId">The resin device record identifier.</param>
        /// <returns></returns>
        Task<dynamic> GetResinDeviceAsync(string resinDeviceId);
    }
}
