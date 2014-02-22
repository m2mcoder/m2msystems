using M2MSystems.Insurance.WebService.Controllers.InsuranceCheck.InsuranceTypes.ArtInsurance;
using M2MSystems.Insurance.WebService.Controllers.InsuranceCheck.Models;

namespace M2MSystems.Insurance.WebService.Controllers.InsuranceCheck
{
    public interface IFeeCalculatorsManager
    {
        IFeeCalculator GetFeeCalculator(string feeCalculator);
    }

    public interface IFeeCalculator
    {
        decimal CalculateFee(Application request);
    }

    public class FeeCalculatorsManager : IFeeCalculatorsManager
    {
        public IFeeCalculator GetFeeCalculator(string feeCalculator)
        {
            if (feeCalculator == "ArtInsurance")
                return new ArtInsuranceFeeCalculator();
            return null;
        }
    }
}