using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Models
{
    /// <summary>
    /// Stores all data needed to query the devices (sorting, filtering, searching, etc)
    /// </summary>
    [Serializable]
    [DataContract(Name = "DeviceListQuery")]
    public class DeviceListQuery
    {
        /// <summary>
        /// Column-level filter values (can have zero or more)
        /// </summary>
        [DataMember(Name = "filters")]
        public List<FilterInfo> Filters { get; set; }

        /// <summary>
        /// General, overarching search query (not specific to a column)
        /// </summary>
        [DataMember(Name = "searchQuery")]
        public string SearchQuery { get; set; }
        
        /// <summary>
        /// Requested sorting column
        /// </summary>
        [DataMember(Name = "sortColumn")]
        public string SortColumn { get; set; }
        
        /// <summary>
        /// Requested sorting order
        /// </summary>
        [DataMember(Name = "sortOrder")]
        public QuerySortOrder SortOrder { get; set;}
        
        /// <summary>
        /// Number of devices to skip at start of list (if Skip = 50, then 
        /// the first 50 devices will be omitted from the list, and devices will
        /// be returned starting with #51 and on)
        /// </summary>
        [DataMember(Name = "skip")]
        public int Skip { get; set; }

        /// <summary>
        /// Number of devices to return/display
        /// </summary>
        [DataMember(Name = "take")]
        public int Take { get; set; }
    }
}
