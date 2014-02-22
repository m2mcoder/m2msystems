using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using M2MSystems.Database;

namespace M2MSystems.Insurance.WebService.Controllers
{
    public class InsuranceGetController : ApiController
    {
        public HttpResponseMessage Get(long partnerId, long formId)
        {
            var connectionString = System.Configuration.ConfigurationManager.
                ConnectionStrings["M2MSystemsConnectionString"].ConnectionString;

            var db = new M2MSystemsDataContext(connectionString);
           
            byte[] document = null;
            try
            {
                document = (from d in db.Documents
                    join f in db.Forms on d.id equals f.id
                    join p in db.Products on f.productid equals p.id
                    join ipp in db.InsurerPartnerProducts on p.id equals ipp.productid
                    join ip in db.InsurerPartners on ipp.insurerpartnerid equals ip.id
                    join partner in db.Partners on ip.partnerid equals partner.id
                    where f.id == formId && partner.id == partnerId
                    select d.bin).FirstOrDefault();
            }
            catch(Exception e) {}
            if (document == null)
            {
                //document =
                //Encoding.UTF8.GetBytes(
                //                "<html><body>No form for specified partner or form at this time.</body></html>");
                document = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Forms/ArtInsurance.html"));
            }
            

            var stream = new MemoryStream(document);

            var result = new HttpResponseMessage(HttpStatusCode.OK) {Content = new StreamContent(stream)};
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return result;
        }
    }
}