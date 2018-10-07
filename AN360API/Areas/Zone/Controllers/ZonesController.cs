using AN360API.APIEnum;
using AN360API.Areas.Shell.Entity;
using AN360API.Areas.Zone.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AN360API.Controllers
{
    /// <summary>
    /// This obejct is used to create/search/delete a particular zones
    /// </summary>
    public class ZonesController : ApiController
    {
        private ShellsController objLead;
        private string jsonVlidationFile;
        private string jsonLeadZonesFile;
        public ZonesController()
        {
            objLead = new ShellsController();
            jsonVlidationFile = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "") + @"\LeadZoneValidator.json";
            jsonLeadZonesFile = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "") + @"\LeadZones.json";
        }
        /// <summary>
        /// Gets all the zones based on AlarmReportingNumber
        /// </summary>
        /// <param name="AlarmReportingNumber">AlarmReportingNumber shall be in city-cs-sub format</param>
        /// <returns></returns>
        public HttpResponseMessage Get(string AlarmReportingNumber)
        {
            try
            {
                string message = "";
                if (!string.IsNullOrEmpty(AlarmReportingNumber))
                {
                    string[] CityCsSub = AlarmReportingNumber.Split('-');
                    if (objLead.IsExistingLead(CityCsSub[0], CityCsSub[1], CityCsSub[2]))
                    {
                        var json = File.ReadAllText(jsonLeadZonesFile);
                        var jObject = JObject.Parse(json);
                        JArray LeadZoneArrary = (JArray)jObject["Zones"];


                        var LeadsZonetobeSelected = LeadZoneArrary.Where(obj => obj["CityCsSubId"]["CityId"].Value<string>() == CityCsSub[0]
                                                                                      && obj["CityCsSubId"]["CsId"].Value<string>() == CityCsSub[1]
                                                                                      && obj["CityCsSubId"]["SubscriberId"].Value<string>() == CityCsSub[2]);

                        if (LeadsZonetobeSelected == null || LeadsZonetobeSelected.Count() == 0)
                        {
                            message = "No zones available for shell account :: " + AlarmReportingNumber;
                            return this.Request.CreateResponse(HttpStatusCode.NotFound, message);
                        }
                        return this.Request.CreateResponse(HttpStatusCode.OK, LeadsZonetobeSelected);
                    }
                    else
                    {
                        message = AlarmReportingNumber + " shell account doesn't exist in AN360.Please use proper AlarmReportingNumber.";
                        return this.Request.CreateResponse(HttpStatusCode.NotFound, message);
                    }
                }
                else
                {
                    message = AlarmReportingNumber + " should not be empty.";
                    return this.Request.CreateResponse(HttpStatusCode.InternalServerError, message);
                }
            }
            catch (Exception ex)
            {

                return this.Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
            
        }
        /// <summary>
        /// Delete zones based on AlarmReportingNumber
        /// </summary>
        /// <param name="AlarmReportingNumber">AlarmReportingNumber shall be in city-cs-sub format</param>
        /// <returns></returns>
        public HttpResponseMessage Delete(string AlarmReportingNumber)
        {
            try
            {
                string[] CityCsSub = AlarmReportingNumber.Split('-');

                if (objLead.IsExistingLead(CityCsSub[0], CityCsSub[1], CityCsSub[2]))
                {

                    var json = File.ReadAllText(jsonLeadZonesFile);
                    var jObject = JObject.Parse(json);
                    JArray experiencesArrary = (JArray)jObject["Zones"];

                    var LeadZonestobedeleted = experiencesArrary.Where(obj => obj["CityCsSubId"]["CityId"].Value<string>() == CityCsSub[0]
                                                                                      && obj["CityCsSubId"]["CsId"].Value<string>() == CityCsSub[1]
                                                                                      && obj["CityCsSubId"]["SubscriberId"].Value<string>() == CityCsSub[2]).ToList();


                    foreach (var item in LeadZonestobedeleted)
                    {
                        experiencesArrary.Remove(item);

                    }

                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(jsonLeadZonesFile, output);

                    return this.Request.CreateResponse(HttpStatusCode.OK, "Zones deleted successfully for shell account:: " + AlarmReportingNumber);
                }
                else
                {
                    string message = AlarmReportingNumber + "  doesn't exist in system.Please use proper AlarmReportingNumber.";
                    return this.Request.CreateResponse(HttpStatusCode.NotFound, message);
                }
            }
            catch (Exception  ex)
            {

                return this.Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
           
        }
        /// <summary>
        /// Add zone to mentioned AlarmReportingNumber
        /// </summary>
        /// <param name="AlarmReportingNumber">AlarmReportingNumber shall be in city-cs-sub format</param>
        /// /// <param name="ObjZone"></param>
        /// <returns></returns>
        public HttpResponseMessage Post([FromBody]CreateZoneModel ObjZone,string AlarmReportingNumber)
        {
            try
            {
                bool exists = Enum.IsDefined(typeof(DeviceType), ObjZone.DeviceType);
                exists = Enum.IsDefined(typeof(ResponseTypes), ObjZone.ResponseType);
                string[] CityCsSub = AlarmReportingNumber.Split('-');
                string message = "";
                if (exists)
                {
                    if (objLead.IsExistingLead(CityCsSub[0], CityCsSub[1], CityCsSub[2]))
                    {
                        //Vaidation Start
                        var validationjson = File.ReadAllText(jsonVlidationFile);
                        var jvalidationObject = JObject.Parse(validationjson);

                        JArray ZoneValidationMappingArray = (JArray)jvalidationObject["DeviceTypeWithResponseTypeMapper"];

                        string strDeviceType = ObjZone.DeviceType.ToString(); //below while condition doesn't work with this. See later
                        string strResponseType = ObjZone.ResponseType.ToString(); //below while condition doesn't work with this. See later
                        var JToken = ZoneValidationMappingArray.Where(obj => obj["DeviceType"].Value<string>() == strDeviceType).ToList();
                        var isrespTypeThere = JToken[0].Last().Values().Any(x => x.Value<string>() == strResponseType);
                        //End

                        //Appending

                        var LeadZonesjson = File.ReadAllText(jsonLeadZonesFile);
                        var jLeadZonesObject = JObject.Parse(LeadZonesjson);

                        JArray LeadZonesArrary = (JArray)jLeadZonesObject["Zones"];

                        CityCSSub objARM = new CityCSSub
                        {
                            CityId = CityCsSub[0],
                            CsId = CityCsSub[1],
                            SubscriberId = CityCsSub[2]
                        };
                        ObjZone.CityCsSubId = objARM;


                        string newjson = JsonConvert.SerializeObject(ObjZone);
                        var newZone = JObject.Parse(newjson);
                        LeadZonesArrary.Add(newZone);


                        jLeadZonesObject["Zones"] = LeadZonesArrary;

                        string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jLeadZonesObject,
                                               Newtonsoft.Json.Formatting.Indented);
                        File.WriteAllText(jsonLeadZonesFile, newJsonResult);

                        message = "Zones got added for shell account : " + AlarmReportingNumber;
                        return this.Request.CreateResponse(HttpStatusCode.OK, message);
                    }
                    else
                    {
                        message = AlarmReportingNumber + " shell account doesn't exist.Please use proper AlarmReportingNumber.";
                        return this.Request.CreateResponse(HttpStatusCode.NotFound, message);
                    }
                    //End

                }
                else
                {
                    message = "Incorrect DeviceType/Response Type";
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, message);
                }
            }
            catch (Exception ex)
            {

                return this.Request.CreateResponse(HttpStatusCode.BadRequest, ex);

            }
            
        }

    }
}
