using M2MSystems.DataAccess.Entities;
using M2MSystems.Insurance.WebService.Endpoints.Controllers.InsuranceCheck.InsuranceTypes.ArtInsurance;

namespace M2MSystems.Insurance.WebService.Endpoints.Controllers.InsuranceCheck
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