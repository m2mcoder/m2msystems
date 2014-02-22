using System;
using System.Collections.Generic;
using System.Linq;
using M2MSystems.DataAccess.Entities;
using M2MSystems.Insurance.WebService.Endpoints.Controllers.InsuranceCheck.Models;

namespace M2MSystems.Insurance.WebService.Endpoints.Controllers.InsuranceCheck.InsuranceTypes.ArtInsurance
{
    public class ArtInsuranceApplicationExtractor : IApplicationExtractor
    {
        public Application ExtractApplication(List<KeyValue> request, out Response response)
        {
            var app = new Application
            {
                Address1 = request.Where(x => x.Key == "address").Select(x => x.Value).FirstOrDefault(),
                //Address2 = request.Where(x => x.Key == "").Select(x => x.Value).FirstOrDefault(),
                //City = request.Where(x => x.Key == "").Select(x => x.Value).FirstOrDefault(),
                Email = request.Where(x => x.Key == "email").Select(x => x.Value).FirstOrDefault(),
                //StartDate = DateTimeOffset.Parse(request.Where(x => x.Key == "").Select(x => x.Value).FirstOrDefault()),
                //EndDate = DateTimeOffset.Parse(request.Where(x => x.Key == "").Select(x => x.Value).FirstOrDefault()),
                FirstName = request.Where(x => x.Key == "firstname").Select(x => x.Value).FirstOrDefault(),
                LastName = request.Where(x => x.Key == "lastname").Select(x => x.Value).FirstOrDefault(),
                ThirdPartyKey = request.Where(x => x.Key == "stunumber").Select(x => x.Value).FirstOrDefault(),

            };
            app.StartDate = DateTimeOffset.Now.AddDays(1).Date;
            app.EndDate = app.StartDate.AddDays(1);
            if (string.IsNullOrWhiteSpace(app.Email))
            {
                response = new Response
                {
                    Ok = false,
                    ValidationResults = new List<KeyValue>
                    {
                        new KeyValue
                        {
                            Key = "email",
                            Value = "Missing or invalid email."
                        }
                    }
                };
                return null;
            }
            response = null;
            return app;
        }
    }
}