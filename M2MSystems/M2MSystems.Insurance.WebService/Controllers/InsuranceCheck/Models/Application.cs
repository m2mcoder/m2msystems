using System;

namespace M2MSystems.Insurance.WebService.Controllers.InsuranceCheck.Models
{
    public class Application
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string ThirdPartyKey { get; set; }
    }
}