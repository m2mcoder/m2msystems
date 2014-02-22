using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Http;
using M2MSystems.Database;
using M2MSystems.Insurance.WebService.Controllers.InsuranceCheck;
using M2MSystems.Insurance.WebService.Controllers.InsuranceCheck.Models;
using Newtonsoft.Json;

namespace M2MSystems.Insurance.WebService.Controllers
{
    public class InsuranceCheckController : ApiController
    {
        private readonly IApplicationExtractorsManager _applicationExtractorsManager;
        private readonly IFeeCalculatorsManager _feeCalculatorsManager;

        public InsuranceCheckController()
        {
            _applicationExtractorsManager = new ApplicationExtractorsManager();
            _feeCalculatorsManager = new FeeCalculatorsManager();
        }

        public Response Post(long partnerId, long formId, [FromBody] Request request)
        {
            var connectionString = System.Configuration.ConfigurationManager.
                ConnectionStrings["M2MSystemsConnectionString"].ConnectionString;
            var db = new M2MSystemsDataContext(connectionString);
            var form = (from f in db.Forms
                        join p in db.Products on f.productid equals p.id
                        join ipp in db.InsurerPartnerProducts on p.id equals ipp.productid
                        join ip in db.InsurerPartners on ipp.insurerpartnerid equals ip.id
                        join partner in db.Partners on ip.partnerid equals partner.id 
                        where f.id == formId && partner.id == partnerId
                        select f).FirstOrDefault();
            if (form == null)
                return new Response
                {
                    Ok = false,
                    Message = "Unknown form."
                };
            var applicationExtractor = _applicationExtractorsManager.GetApplicationExtractor(form.applicationextractor);
            if (applicationExtractor == null)
                return new Response
                {
                    Ok = false,
                    Message = "Non properly configured form - missing application extractor."
                };
            var feeCalculator = _feeCalculatorsManager.GetFeeCalculator(form.feecalculator);
            if (feeCalculator == null)
                return new Response
                {
                    Ok = false,
                    Message = "Non properly configured form - missing fee calculator."
                };
            Response response;
            Application application;
            if ((application = applicationExtractor.ExtractApplication(request.FormsData, out response)) == null)
                return response;
            var fee = feeCalculator.CalculateFee(application);
            var answersJson = JsonConvert.SerializeObject(request.FormsData); 
            using (var transaction = new TransactionScope())
            {
                try
                {
                    var applicant = db.Applicants.FirstOrDefault(x => x.email == application.Email);
                    if (applicant != null)
                    {
                        applicant.address1 = application.Address1;
                        applicant.address2 = application.Address2;
                        applicant.city = application.City;
                        applicant.firstname = application.FirstName;
                        applicant.lastname = application.LastName;
                        application.ThirdPartyKey = application.ThirdPartyKey;
                    }
                    else
                    {
                        applicant = new Applicant
                        {
                            address1 = application.Address1,
                            address2 = application.Address2,
                            city = application.City,
                            firstname = application.FirstName,
                            lastname = application.LastName,
                            thirdpartykey = application.ThirdPartyKey,
                            email = application.Email
                        };
                        db.Applicants.InsertOnSubmit(applicant);   
                    }
                    //var policy = new Policy
                    //{
                    //    cost = fee,
                    //    Product = form.Product,
                    //    enddate = application.EndDate,
                    //    startdate = application.StartDate
                    //};
                    //db.Policies.InsertOnSubmit(policy);
                    //var savedForm = new SavedForm
                    //{
                    //    answers = answersJson,
                    //    Applicant = applicant,
                    //    Form = form,
                    //    Policy = policy,
                    //};
                    //applicant.SavedForms.Add(savedForm);
                    //policy.SavedForms.Add(savedForm);

                    db.SubmitChanges();
                    transaction.Complete();
                }
                catch (Exception e)
                {
                    return new Response
                    {
                        Ok = false,
                        Message = "Unable to save form at the current time. Try again later."
                    };
                }
            }

            return new Response { Ok = true, Fee = fee };
        }
    }
}