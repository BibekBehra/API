using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace AN360API.Areas.Shell.Entity
{
    public class CityCSSub
    {
        /// <summary>
        /// The City ID.
        /// </summary>        
        [DataMember]
        public string CityId { get; set; }
        /// <summary>
        /// The Central Station ID.
        /// </summary>
        [DataMember]
        public string CsId { get; set; }
        /// <summary>
        /// The Subscriber ID.
        /// </summary>
        [DataMember]
        public string SubscriberId { get; set; }
    }
}