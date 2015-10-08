using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Models
{
    [Serializable]
    [DataContract(Name = "SecurityKeys")]
    public class SecurityKeys
    {
        public SecurityKeys(string primaryKey, string secondaryKey)
        {
            PrimaryKey = primaryKey;
            SecondaryKey = secondaryKey;
        }

        [DataMember(Name = "primaryKey")]
        public string PrimaryKey { get; set; }

        [DataMember(Name = "secondaryKey")]
        public string SecondaryKey { get; set; }
    }

    public enum SecurityKey
    {
        None = 0,
        [Display(Name = "primary")]
        Primary,
        [Display(Name = "secondary")]
        Secondary
    }
}
