using System.Runtime.Serialization;

namespace AN360API.Areas.Shell.Entity
{
    public class ShellDetails
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public InstallationAddress InstallationAddress { get; set; }
    }
}