using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace M2MSystems.Insurance.WebService.Controllers
{
    public class InsuranceGetController : ApiController
    {
        public HttpResponseMessage Get(long partnerId, long formId)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new MemoryStream(
                Encoding.UTF8.GetBytes(
                string.Format(
                    @"<html><body>Hello, {0} partner!<br/> This is form with id {1}!</body></html", 
                        partnerId, formId)));
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return result;
        }
    }
}