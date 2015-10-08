using System;
using System.Runtime.Serialization;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Models
{
    /// <summary>
    /// Represents a single set of filtering data for a device
    /// </summary>
    [Serializable]
    [DataContract(Name = "FilterInfo")]
    public class FilterInfo
    {
        [DataMember(Name = "columnName")]
        public string ColumnName { get; set; }

        [DataMember(Name = "filterType")]
        public FilterType FilterType { get; set; }

        [DataMember(Name = "filterValue")]
        public string FilterValue { get; set; }
    }
}
