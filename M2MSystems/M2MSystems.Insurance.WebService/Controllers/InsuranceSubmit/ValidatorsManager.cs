using System.Collections.Generic;
using M2MSystems.Insurance.WebService.Controllers.InsuranceSubmit.InsuranceTypes.ArtInsurance;
using M2MSystems.Insurance.WebService.Controllers.InsuranceSubmit.Models;

namespace M2MSystems.Insurance.WebService.Controllers.InsuranceSubmit
{
    public interface IValidatorsManager
    {
        IValidator GetValidator(string validator);
    }

    public interface IValidator
    {
        bool Validate(List<KeyValue> request, out Response response);
    }

    public class ValidatorsManager : IValidatorsManager
    {
        public IValidator GetValidator(string validator)
        {
            if (validator == "ArtInsurance")
                return new ArtInsuranceValidator();
            return null;
        }
    }
}