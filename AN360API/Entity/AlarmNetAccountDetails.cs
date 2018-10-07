using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AN360API.Areas.Shell.Entity
{
    public class AlarmNetAccountDetails
    {
        [DataMember]
        public CityCSSub CityCSSub { get; set; }

        [DataMember]
        public IList<ServiceDetails> ServiceDetails { get; set; }
    }
}