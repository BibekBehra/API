using AN360API.Areas.Shell.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AN360API.Areas.Shell.Entity;
using static AN360API.Validation.Shell.ShellValidator;

namespace AN360API.Validation.Shell
{
    public class ShellValidator : AbstractValidator<CreateShellAccountModel>
    {
        public enum EnumShellServiceType
        {
            CRL,
            APL,
            VAV,
            TWOWAYVOICE,
            SUPERVISION,
            TCPLAN,
            INFORMATION,
            AUTOMATION,
            VIDEO,
            ADVERTISING,
            VIDEODOORBELL
        }
        public ShellValidator()
        {
            RuleFor(x => x.ShellDetails.FirstName).NotEmpty().WithMessage("Please provide firstname.");
            RuleFor(x => x.ShellDetails.LastName).NotEmpty().WithMessage("Please provide lastname.");
            RuleFor(s => s.ShellDetails.Email).NotEmpty().WithMessage("Please provide email address.").EmailAddress().WithMessage("Please provide valid email address.");
            RuleFor(x => x.TotalConnectAccountDetails.MasterUserName).NotEmpty().WithMessage("Please provide MasterUserName"); ;
           // RuleFor(x => x.TotalConnectAccountDetails.Notification.ToString()).NotEmpty();

            RuleFor(x => x.ShellDetails.InstallationAddress).NotNull().WithMessage("Please enter iInstallation address");
            RuleFor(x => x.ShellDetails.InstallationAddress.AddressLine1).NotEmpty().WithMessage("Please enter AddressLine1");
            RuleFor(x => x.ShellDetails.InstallationAddress.AddressLine2).NotEmpty().WithMessage("Please enter AddressLine2"); ;
            RuleFor(x => x.ShellDetails.InstallationAddress.Country).NotEmpty().WithMessage("Please select the country.");
            RuleFor(x => x.ShellDetails.InstallationAddress.State).NotEmpty().WithMessage("Please enter the state."); ;
            RuleFor(x => x.ShellDetails.InstallationAddress.City).NotEmpty().WithMessage("Please enter the city."); ;
            RuleFor(x => x.ShellDetails.InstallationAddress.Zip).NotEmpty().WithMessage("Please enter the valid zip."); ;
            RuleFor(x => x.ShellDetails.InstallationAddress.Location).NotEmpty().WithMessage("Please enter your location."); ;
            RuleFor(x => x.AlarmNetAccountDetails).NotNull();

            RuleFor(x => x.TotalConnectAccountDetails).NotNull();

            RuleFor(x => x.AlarmNetAccountDetails.CityCSSub).NotNull();
            RuleFor(x => x.AlarmNetAccountDetails.CityCSSub.CityId).NotEmpty().WithMessage("CityID shall not be empty").Length(2).WithMessage("CityID shall be 2 characters length"); ;
            RuleFor(x => x.AlarmNetAccountDetails.CityCSSub.CsId).NotEmpty().WithMessage("CSID shall not be empty").Length(2).WithMessage("CSID shall be 2 characters length"); ;
            RuleFor(x => x.AlarmNetAccountDetails.CityCSSub.SubscriberId).NotEmpty().WithMessage("SubscriberID shall not be empty").Length(4).WithMessage("SubscriberID shall be 4 characters length"); ;
            RuleFor(x => x.TotalConnectAccountDetails.ServiceDetails).SetCollectionValidator(new ServiceValidator());

        }
       
        
    }
   
    public class ServiceValidator : AbstractValidator<ServiceDetails>
    {
        public ServiceValidator()
        {
            
            RuleFor(shellZone => IsServiceNameDefined(shellZone.Name)).Equal(true).WithMessage("Please provide service type {0}", shellzone => shellzone.Name);
            RuleFor(shellZone => IsServiceNameDefined(shellZone.Name) && !IsproperServiceValue(shellZone.Name, shellZone.Value)).Equal(false).WithMessage("Please provide service value {0} for {1}", shellzone => shellzone.Value, shellzone => shellzone.Name);

        }

        private bool IsproperServiceValue(string name,string value)
        {
            if (IsServiceNameDefined(name))
            {
                switch ((EnumShellServiceType)Enum.Parse(typeof(EnumShellServiceType), name, true))
                {
                    case EnumShellServiceType.ADVERTISING:
                        return (value == "1" || value == "0");
                        break;
                    case EnumShellServiceType.TCPLAN:
                        return (value == "Total Connect Basic" || value == "Total Connect Plus" || value == "Total Connect Premium");
                        break;
                    case EnumShellServiceType.VIDEO:
                        return (value == "None" || value == "7 Days" || value == "30 Days");
                        break;
                    case EnumShellServiceType.INFORMATION:
                        return (value == "1" || value == "0");
                        break;
                    case EnumShellServiceType.AUTOMATION:
                        return (value == "1" || value == "0");
                        break;
                    case EnumShellServiceType.VIDEODOORBELL:
                        return (value == "1" || value == "0");
                        break;
                    case EnumShellServiceType.SUPERVISION:
                        return (value == "Daily" || value == "Monthly" || value == "Unsupervised");
                        break;
                    case EnumShellServiceType.APL:
                        return (value == "1" || value == "0");
                        break;
                    case EnumShellServiceType.CRL:
                        return (value == "1" || value == "0");
                        break;
                    case EnumShellServiceType.VAV:
                        return (value == "1" || value == "0");
                        break;
                    case EnumShellServiceType.TWOWAYVOICE:
                        return (value == "1" || value == "0");
                        break;
                    default:
                        break;
                }
            }
            return false;
        }
        private bool IsServiceNameDefined(string name)
        {
            return  Enum.IsDefined(typeof(EnumShellServiceType), name);
        }
    }
}