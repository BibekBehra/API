using AN360API.Areas.Shell.Entity;
using AN360API.APIEnum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AN360API.Areas.Zone.Models
{
    public class CreateZoneModel
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public DeviceType DeviceType { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ResponseTypes ResponseType { get; set; }
        public string ZoneDescriptor1 { get; set; }
        public string ZoneDescriptor2 { get; set; }

        public CityCSSub CityCsSubId { get; set; }
    }
}