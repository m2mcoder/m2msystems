using System.Collections.Generic;
using System.Web.Http;

namespace M2MSystems.Insurance.WebService.Controllers
{
    public class InsuranceQuoteKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class InsuranceQuoteResponse
    {
        public bool Ok { get; set; }
        public List<InsuranceQuoteKeyValue> ValidationResults { get; set; }
    }

    public class InsuranceQuoteRequest
    {

        public List<InsuranceQuoteKeyValue> Data { get; set; }
    }

    public class InsuranceValidator
    {
        
    }

    public class InsuranceQuoteController : ApiController
    {

        public InsuranceQuoteResponse Post(long partnerId, long formId, [FromBody] InsuranceQuoteRequest request)
        {
            return new InsuranceQuoteResponse
            {
                Ok = true,
            };
        }
    }
}