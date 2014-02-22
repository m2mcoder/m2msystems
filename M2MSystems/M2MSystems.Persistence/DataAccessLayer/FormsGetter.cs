using System.Configuration;
using System.Linq;
using M2MSystems.Access.Linq2Sql;
using Form = M2MSystems.DataAccess.Entities.Form;

namespace M2MSystems.DataAccess.DataAccessLayer
{
    public interface IFormGetter
    {
        Form GetForm(long partnerId, long formId);
    }

    public class FormGetter : IFormGetter
    {
        public Form GetForm(long partnerId, long formId)
        {
            var connectionString = ConfigurationManager.
                ConnectionStrings["M2MSystemsConnectionString"].ConnectionString;

            var db = new M2MSystemsDataContext(connectionString);

            var form = (from f in db.Forms
                        join p in db.Products on f.productid equals p.id
                        join ipp in db.InsurerPartnerProducts on p.id equals ipp.productid
                        join ip in db.InsurerPartners on ipp.insurerpartnerid equals ip.id
                        join partner in db.Partners on ip.partnerid equals partner.id
                        where f.id == formId && partner.id == partnerId
                        select f).FirstOrDefault();
            if (form == null) return null;
            return new Form
            {
                ApplicationExtractor = form.applicationextractor,
                FeeCalculator = form.feecalculator
            };
        }
    }
}
