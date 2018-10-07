using System.Runtime.Serialization;

namespace AN360API.Areas.Shell.Entity
{
    public class InstallationAddress
    {
        /// <summary>
        /// AddressLine1
        /// </summary>
        [DataMember]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// AddressLine2
        /// </summary>
        [DataMember]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Name of the State
        /// </summary>
        [DataMember]
        public string State { get; set; }

        /// <summary>
        /// Name of the Country
        /// </summary>
        [DataMember]
        public string Country { get; set; }

        /// <summary>
        /// Name of the City
        /// </summary>
        [DataMember]
        public string City { get; set; }

        /// <summary>
        /// Zip
        /// </summary>
        [DataMember]
        public string Zip { get; set; }

        /// <summary>
        ///  Installation Address or Billing address
        /// </summary>

        [DataMember]
        public string Location { get; set; }
    }
}