using System.Collections.Generic;

namespace M2MSystems.Insurance.WebService.Controllers.InsuranceCheck.Models
{
    public class Response
    {
        public bool Ok { get; set; }

        public string Message { get; set; }

        public List<KeyValue> ValidationResults { get; set; }

        public decimal? Fee { get; set; }
    }
}