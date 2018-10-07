using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AN360API.Areas.Shell.Entity
{
    public class TotalConnectAccountDetails
    {
        [DataMember]
        public string MasterUserName { get; set; }

        [DataMember]
        public string Notification { get; set; }

        [DataMember]
        public IList<ServiceDetails> ServiceDetails { get; set; }
    }
}