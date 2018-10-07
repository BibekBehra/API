using AN360API.Areas.Shell.Entity;
using AN360API.Validation.Shell;
using FluentValidation.Attributes;
using System.Runtime.Serialization;

namespace AN360API.Areas.Shell.Models
{
    /// <summary>
    /// Contains all details about Lead
    /// </summary>
    [Validator(typeof(ShellValidator))]
    public class CreateShellAccountModel
    {
        [DataMember]
        public ShellDetails ShellDetails { get; set; }

        [DataMember]
        public AlarmNetAccountDetails AlarmNetAccountDetails { get; set; }

        [DataMember]
        public TotalConnectAccountDetails TotalConnectAccountDetails { get; set; }

       
    }
  
}