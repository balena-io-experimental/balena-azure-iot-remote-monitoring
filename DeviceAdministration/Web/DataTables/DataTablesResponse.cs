using System;
using System.Runtime.Serialization;
namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Web.DataTables
{
    [Serializable]
    [DataContract(Name = "DataTablesResponse")]
    public class DataTablesResponse
    {
        [DataMember(Name = "draw")]
        public int Draw { get; set; }

        [DataMember(Name = "recordsTotal")]
        public int RecordsTotal { get; set; }

        [DataMember(Name = "recordsFiltered")]
        public int RecordsFiltered { get; set; }

        [DataMember(Name = "data")]
        public dynamic[] Data { get; set; }
    }
}