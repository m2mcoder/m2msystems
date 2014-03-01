using System.Web.Http;
using M2MSystems.DataAccess.DataAccessLayer;
using M2MSystems.DataAccess.Entities;
using M2MSystems.Insurance.WebService.Endpoints.Controllers.InsuranceCheck;
using M2MSystems.Insurance.WebService.Endpoints.Controllers.InsuranceCheck.Models;
using Newtonsoft.Json;

namespace M2MSystems.Insurance.WebService.Endpoints.Controllers
{
    public class InsuranceCheckController : ApiController
    {
        private readonly IApplicationExtractorsManager _applicationExtractorsManager;
        private readonly IFeeCalculatorsManager _feeCalculatorsManager;
        private readonly IFormGetter _formGetter;
        private readonly IApplicantsUpdater _applicantsUpdater;

        public InsuranceCheckController()
        {
            _applicationExtractorsManager = new ApplicationExtractorsManager();
            _feeCalculatorsManager = new FeeCalculatorsManager();
            _formGetter = new FormGetter();
            _applicantsUpdater = new ApplicantsUpdater();
        }

        public Response Post(long partnerId, long formId, [FromBody] Request request)
        {
            var form = _formGetter.GetForm(partnerId, formId);
            if (form == null)
                return new Response
                {
                    Ok = false,
                    Message = "Unknown form."
                };
            var applicationExtractor = _applicationExtractorsManager.GetApplicationExtractor(form.ApplicationExtractor);
            if (applicationExtractor == null)
                return new Response
                {
                    Ok = false,
                    Message = "Non properly configured form - missing application extractor."
                };
            var feeCalculator = _feeCalculatorsManager.GetFeeCalculator(form.FeeCalculator);
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
            if (_applicantsUpdater.Update(application) == false)
                return new Response
                {
                    Ok = false,
                    Message = "Unable to save form at the current time. Try again later."
                };
            return new Response { Ok = true, Fee = fee };
        }
    }
}