using System.Collections.Generic;
using M2MSystems.Insurance.WebService.Controllers.InsuranceCheck.InsuranceTypes.ArtInsurance;
using M2MSystems.Insurance.WebService.Controllers.InsuranceCheck.Models;

namespace M2MSystems.Insurance.WebService.Controllers.InsuranceCheck
{
    public interface IApplicationExtractorsManager
    {
        IApplicationExtractor GetApplicationExtractor(string apllicationExtractor);
    }

    public interface IApplicationExtractor
    {
        Application ExtractApplication(List<KeyValue> request, out Response response);
    }

    public class ApplicationExtractorsManager : IApplicationExtractorsManager
    {
        public IApplicationExtractor GetApplicationExtractor(string apllicationExtractor)
        {
            if (apllicationExtractor == "ArtInsurance")
                return new ArtInsuranceApplicationExtractor();
            return null;
        }
    }
}