using System.Collections.Generic;

namespace M2MSystems.Insurance.WebService.Controllers.InsuranceSubmit.Models
{
    public class Response
    {
        public bool Ok { get; set; }

        public string Message { get; set; }
     
        public List<KeyValue> ValidatorResults { get; set; }

        public decimal? Fee { get; set; }
    }
}