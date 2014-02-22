using System.Web.Http;
using System.Web.Mvc;
using M2MSystems.Insurance.WebService.Endpoints;

namespace M2MSystems.Insurance.WebService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}