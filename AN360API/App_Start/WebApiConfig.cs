using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Extensions;
using AN360API.Formatter;
using FluentValidation.WebApi;
using Microsoft.AspNetCore.Mvc.Formatters.Json;
namespace AN360API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            FluentValidationModelValidatorProvider.Configure(config);
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{AlarmReportingNumber}",
                defaults: new { AlarmReportingNumber = RouteParameter.Optional }
            );

            config.Formatters.Add(new ProductFormatter());
            //config.AddODataQueryFilter();
        }
    }
}
