using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Models;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Web.Models
{
    [Serializable]
    [DataContract(Name = "AlertHistoryResultsModel")]
    public class AlertHistoryResultsModel
    {
        [DataMember(Name = "totalAlertCount")]
        public int TotalAlertCount { get; set; }

        [DataMember(Name = "totalFilteredCount")]
        public int TotalFilteredCount { get; set; }

        [DataMember(Name = "data")]
        public List<AlertHistoryItemModel> Data { get; set; }
    }
}