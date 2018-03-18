using EventSub.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace EventSub
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{eventId}",
                defaults: new { eventId = RouteParameter.Optional }
            );

            // Using the custom Json Converter allows us to take a dynamic set
            // of data, and populate an object, while still being able to use
            // the ASP.NET Web API validation tools found in the ApiController.
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.Converters.Add(new LiveEventConverter());
        }
    }
}
