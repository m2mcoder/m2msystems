using System.Web.Http;

namespace M2MSystems.Insurance.WebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //name: "DefaultApi",
            //routeTemplate: "api/{controller}/{id}",
            //defaults: new { id = RouteParameter.Optional }
            config.Routes.MapHttpRoute(
                "InsuranceGet",
                "api/1/insurance/{partnerId}/{formId}",
                new { controller = "InsuranceGet" }
            );
            config.Routes.MapHttpRoute(
                "InsuranceQuote",
                "api/1/insurance/{partnerId}/{formId}/quote",
                new { controller = "InsuranceQuote" }
            );
        }
    }
}
