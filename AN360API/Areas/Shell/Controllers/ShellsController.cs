using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Runtime.Serialization;
using FluentValidation;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using AN360API.Areas.Shell.Models;
using Marvin.JsonPatch;
//using Microsoft.AspNetCore.JsonPatch;

namespace AN360API.Controllers
{
    
    public class ShellsController : ApiController
    {
        private string jsonFile;
        public ShellsController()
        {
            jsonFile = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "") + @"\Lead.json";
        }
        /// <summary>
        ///  Get all shell accounts.
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            try
            {
                var json = File.ReadAllText(jsonFile);
                string val = json.Replace('\r', '\0').Replace('\n', '\0');
                var jObject = JObject.Parse(json);
                return this.Request.CreateResponse(HttpStatusCode.OK, jObject);
            }
            catch (Exception ex)
            {

                return this.Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
           
        }
        public HttpResponseMessage Getv2(string AlarmReportingNumber)
        {
            return this.Request.CreateResponse(HttpStatusCode.BadRequest, "This is version number 2. Now work on v2");
        }
            /// <summary>
            ///Gets shell account based on the Alarm Reporting Number.
            /// </summary>
            /// <param name="AlarmReportingNumber">AlarmReportingNumber shall be in city-cs-sub format</param>
            /// <returns></returns>
            public HttpResponseMessage Get(string AlarmReportingNumber)
        {
            try
            {
                if (!string.IsNullOrEmpty(AlarmReportingNumber))
                {
                    string[] CityCsSub = AlarmReportingNumber.Split('-');
                    if (IsExistingLead(CityCsSub[0], CityCsSub[1], CityCsSub[2]))
                    {
                        var json = File.ReadAllText(jsonFile);
                        var jObject = JObject.Parse(json);
                        JArray experiencesArrary = (JArray)jObject["Shells"];



                        var LeadstobeSelected = experiencesArrary.FirstOrDefault(obj => obj["AlarmNetAccountDetails"]["CityCSSub"]["CityId"].Value<string>() == CityCsSub[0]
                                                                                      && obj["AlarmNetAccountDetails"]["CityCSSub"]["CsId"].Value<string>() == CityCsSub[1]
                                                                                      && obj["AlarmNetAccountDetails"]["CityCSSub"]["SubscriberId"].Value<string>() == CityCsSub[2]
                                                                               );

                        return this.Request.CreateResponse(HttpStatusCode.OK, LeadstobeSelected);
                    }
                    else
                    {
                        string message = AlarmReportingNumber + " shell account doesn't exist.Please use proper AlarmReportingNumber.";
                        return this.Request.CreateResponse(HttpStatusCode.NotFound, message);
                    }
                }
                else
                {
                    string message = AlarmReportingNumber + " should not be empty.";
                    return this.Request.CreateResponse(HttpStatusCode.InternalServerError, message);
                }
            }
            catch (Exception ex)
            {

                return this.Request.CreateResponse(HttpStatusCode.BadRequest, ex);

            }
           
           
        }
        /// <summary>
        /// Update shell account based on the Alarm Reporting Number
        /// </summary>
        /// <param name="ShellObj"></param>
        /// <param name="AlarmReportingNumber">AlarmReportingNumber shall be in city-cs-sub format</param>
        /// <returns></returns>
        public HttpResponseMessage Put([FromBody]CreateShellAccountModel ShellObj, string AlarmReportingNumber)
        {
            try
            {
                string[] CityCsSub = AlarmReportingNumber.Split('-');
                if (!ModelState.IsValid)
                {
                    string message = string.Join(" | ", ModelState.Values
                     .SelectMany(v => v.Errors)
                     .Select(e => e.ErrorMessage));
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, message);
                }
                else if (IsExistingLead(CityCsSub[0], CityCsSub[1], CityCsSub[2]))
                {
                    //Delete First
                    var json = File.ReadAllText(jsonFile);
                    var jObject = JObject.Parse(json);
                    JArray experiencesArrary = (JArray)jObject["Shells"];

                    var Leadstobedeleted = experiencesArrary.FirstOrDefault(obj => obj["AlarmNetAccountDetails"]["CityCSSub"]["CityId"].Value<string>() == CityCsSub[0]
                                                                              && obj["AlarmNetAccountDetails"]["CityCSSub"]["CsId"].Value<string>() == CityCsSub[1]
                                                                              && obj["AlarmNetAccountDetails"]["CityCSSub"]["SubscriberId"].Value<string>() == CityCsSub[2]
                                                                       );
                    experiencesArrary.Remove(Leadstobedeleted);

                    string newjson = JsonConvert.SerializeObject(ShellObj);
                    var updatedCustomer = JObject.Parse(newjson);
                    experiencesArrary.Add(updatedCustomer);

                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(jsonFile, output);

                    string message = AlarmReportingNumber + " shell account updated successfully";
                    return this.Request.CreateResponse(HttpStatusCode.Conflict, message);
                }
                else
                {
                    string message = AlarmReportingNumber + " doesn't exist in the system.Please use proper AlarmReportingNumber.";
                    return this.Request.CreateResponse(HttpStatusCode.NotFound, message);
                }
            }
            catch (Exception ex)
            {

                return this.Request.CreateResponse(HttpStatusCode.BadRequest, ex);

            }

        }
        [NonAction]
        public bool IsExistingLead(string CityId,string Csid,string SubId)
        {
            try
            {
                var json = File.ReadAllText(jsonFile);
                var jObject = JObject.Parse(json);
                JArray experiencesArrary = (JArray)jObject["Shells"];

                var objLead = experiencesArrary.FirstOrDefault(obj => obj["AlarmNetAccountDetails"]["CityCSSub"]["CityId"].Value<string>() == CityId
                                                                              && obj["AlarmNetAccountDetails"]["CityCSSub"]["CsId"].Value<string>() == Csid
                                                                              && obj["AlarmNetAccountDetails"]["CityCSSub"]["SubscriberId"].Value<string>() == SubId
                                                                       );

                if (objLead != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        /// <summary>
        /// Delete Shell account based on the Alarm Reporting Number.
        /// </summary>
        /// <param name="AlarmReportingNumber">AlarmReportingNumber shall be in city-cs-sub format</param>
        /// <returns></returns>
        public HttpResponseMessage Delete(string AlarmReportingNumber)
        {
            try
            {
                string[] CityCsSub = AlarmReportingNumber.Split('-');

                if (IsExistingLead(CityCsSub[0], CityCsSub[1], CityCsSub[2]))
                {
                    var json = File.ReadAllText(jsonFile);
                    var jObject = JObject.Parse(json);
                    JArray experiencesArrary = (JArray)jObject["Shells"];

                    var Leadstobedeleted = experiencesArrary.FirstOrDefault(obj => obj["AlarmNetAccountDetails"]["CityCSSub"]["CityId"].Value<string>() == CityCsSub[0]
                                                                              && obj["AlarmNetAccountDetails"]["CityCSSub"]["CsId"].Value<string>() == CityCsSub[1]
                                                                              && obj["AlarmNetAccountDetails"]["CityCSSub"]["SubscriberId"].Value<string>() == CityCsSub[2]
                                                                       );
                    experiencesArrary.Remove(Leadstobedeleted);

                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(jsonFile, output);

                    return this.Request.CreateResponse(HttpStatusCode.OK, AlarmReportingNumber + " Shell account deleted successfully");
                }
                else
                {
                    string message = AlarmReportingNumber + "  doesn't exist in system.Please use proper AlarmReportingNumber.";
                    return this.Request.CreateResponse(HttpStatusCode.NotFound, message);
                }
            }
            catch (Exception ex)
            {

                return this.Request.CreateResponse(HttpStatusCode.BadRequest,ex);
            }
            
        }

        /// <summary>
        /// Creates shell account using Customer details along with Alarmnet and Total Connect information.
        /// </summary>
        /// <param name="ShellObj"></param>
        /// <returns></returns>
        public HttpResponseMessage Post([FromBody]CreateShellAccountModel ShellObj)
        {
            try
            {
                string AlarmReportingNumber = ShellObj.AlarmNetAccountDetails.CityCSSub.CityId + "-" + ShellObj.AlarmNetAccountDetails.CityCSSub.CsId + "-" + ShellObj.AlarmNetAccountDetails.CityCSSub.SubscriberId;
                if (!ModelState.IsValid)
                {
                    string message = string.Join(" | ", ModelState.Values
                     .SelectMany(v => v.Errors)
                     .Select(e => e.ErrorMessage));
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, message);
                }
                else if (IsExistingLead(ShellObj.AlarmNetAccountDetails.CityCSSub.CityId, ShellObj.AlarmNetAccountDetails.CityCSSub.CsId, ShellObj.AlarmNetAccountDetails.CityCSSub.SubscriberId))
                {
                    string message = AlarmReportingNumber + " already present in the system. Please use different subscriberid. Unable to create shell account , please try again.";
                    return this.Request.CreateResponse(HttpStatusCode.Conflict, message);
                }
                else
                {

                    //Adding Data 
                    var existingjson = File.ReadAllText(jsonFile);
                    var jObject1 = JObject.Parse(existingjson);
                    JArray experiencesArrary = (JArray)jObject1["Shells"];

                    string newjson = JsonConvert.SerializeObject(ShellObj);
                    var newCustomer = JObject.Parse(newjson);
                    experiencesArrary.Add(newCustomer);


                    jObject1["Shells"] = experiencesArrary;

                    string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jObject1,
                                           Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(jsonFile, newJsonResult);

                    //End

                    //Getting Data 

                    var json = File.ReadAllText(jsonFile);
                    var jObject = JObject.Parse(json);

                    //End


                    var message = Request.CreateResponse(HttpStatusCode.Created, "Shell account successfully created for" + AlarmReportingNumber);
                    message.Headers.Location = new Uri(Request.RequestUri + "/" + AlarmReportingNumber);

                    return message;
                }
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest,ex);
            }
           

        }


        public HttpResponseMessage Patch([FromBody]JsonPatchDocument<CreateShellAccountModel> patch)
        {
            var json = File.ReadAllText(jsonFile);
            var jObject = JObject.Parse(json);
            JArray experiencesArrary = (JArray)jObject["Shells"];



            var lead = experiencesArrary.FirstOrDefault(obj => obj["AlarmNetAccountDetails"]["CityCSSub"]["CityId"].Value<string>() == "89"
                                                                          && obj["AlarmNetAccountDetails"]["CityCSSub"]["CsId"].Value<string>() == "05"
                                                                          && obj["AlarmNetAccountDetails"]["CityCSSub"]["SubscriberId"].Value<string>() == "0212"
                                                                   );


            string jsonObjShellAct = lead.ToString();
            CreateShellAccountModel contact = JsonConvert.DeserializeObject<CreateShellAccountModel>(jsonObjShellAct);


            //var patched = CreateShellAccountModel.Copy();
            patch.ApplyTo(contact);// ModelState);
            return Request.CreateResponse(HttpStatusCode.OK, contact);

            //var model = new
            //{
            //    original = contact,
            //    patched = patched
            //};

            //return Ok();
        }
    }
    
}
