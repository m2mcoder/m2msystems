using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using M2MSystems.Database;
using M2MSystems.Insurance.WebService.Controllers.InsuranceSubmit;
using M2MSystems.Insurance.WebService.Controllers.InsuranceSubmit.Models;
using Newtonsoft.Json;

namespace M2MSystems.Insurance.WebService.Controllers
{
    public class InsuranceSubmitController : ApiController
    {
        private readonly IApplicationExtractorsManager _applicationExtractorsManager;
        private readonly IValidatorsManager _validatorsManager;
        private readonly IFeeCalculatorsManager _feeCalculatorsManager;

        public InsuranceSubmitController()
        {
            _applicationExtractorsManager = new ApplicationExtractorsManager();
            _validatorsManager = new ValidatorsManager();
            _feeCalculatorsManager = new FeeCalculatorsManager();
        }

        public Response Post(long partnerId, long formId, [FromBody] List<KeyValue> formsData)
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
            var validator = _validatorsManager.GetValidator(form.validator);
            if (validator == null)
                return new Response
                {
                    Ok = false,
                    Message = "Non properly configured form - missing valid validator."
                };
            var dataMapper = _applicationExtractorsManager.GetApplicationExtractor(form.datamapper);
            if (dataMapper == null)
                return new Response
                {
                    Ok = false,
                    Message = "Non properly configured form - missing data mapper."
                };
            var feeCalculator = _feeCalculatorsManager.GetFeeCalculator(form.feecalculator);
            if (feeCalculator == null)
                return new Response
                {
                    Ok = false,
                    Message = "Non properly configured form - missing fee calculator."
                };
            Response response;
            if (validator.Validate(formsData, out response) == false)
                return response;
            Application application;
            if ((application = dataMapper.ExtractApplication(formsData, out response)) == null)
                return response;
            var fee = feeCalculator.CalculateFee(application);
            var answersJson = JsonConvert.SerializeObject(formsData);
            using (db.Transaction = db.Connection.BeginTransaction())
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
                            email = application.Email
                        };
                        db.Applicants.InsertOnSubmit(applicant);   
                    }
                    var policy = new Policy
                    {
                        cost = fee,
                        Product = form.Product,
                        enddate = application.EndDate,
                        startdate = application.StartDate
                    };
                    db.Policies.InsertOnSubmit(policy);
                    var savedForm = new SavedForm
                    {
                        answers = answersJson,
                        Applicant = applicant,
                        Form = form,
                        Policy = policy,
                    };
                    applicant.SavedForms.Add(savedForm);
                    policy.SavedForms.Add(savedForm);
                    
                    db.Transaction.Commit();
                }
                catch (Exception)
                {
                    db.Transaction.Dispose();
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