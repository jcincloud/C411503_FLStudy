using Newtonsoft.Json;
using System.Web.Http;

namespace DotWeb.AppStart
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "GetActionApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: null, 
                constraints: new { controller = "GetAction" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional
                }
            );

            config.Formatters.JsonFormatter.SerializerSettings =
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        }
    }
}
