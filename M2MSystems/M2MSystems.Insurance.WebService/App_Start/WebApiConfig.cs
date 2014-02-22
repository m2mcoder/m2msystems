using System.Web.Http;

namespace M2MSystems.Insurance.WebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                "InsuranceCheck",
                "Forms/{partnerId}/{formId}/Check",
                new { controller = "InsuranceCheck" }
            );
        }
    }
}
