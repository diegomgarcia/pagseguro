using System;
using System.Threading.Tasks;
using PagSeguro;


namespace PagSeguroTest
{
    class Program
    {
        static async Task Main()
        {
            const bool isSandBox = true;
            const string accountEmail = "example@account.com";
            const string sandBoxApiKey = "Your API Key";

            PagSeguroClient pagSeguro = new PagSeguroClient(isSandBox, accountEmail, sandBoxApiKey);
            
            PagSeguroPreApprovalPlanDetails details = new PagSeguroPreApprovalPlanDetails();
            details.Reference = "RF0001";
            details.Name = "GOLD Plan";
            details.Charge = PagSeguroPreApprovalPlanChargeType.Auto;
            details.ChargePeriod = PagSeguroApprovalChargePeriodType.Monthly;
            details.AmountPerPayment = 349.99;
            details.TrialPeriodDuration = 21;
            details.ExpirationValue = 1;
            details.ExpirationUnit = PagSeguroPreApprovalExpirationUnitType.Months;
            details.Details = "GOLD plan for monthly payment!";
            
            PagSeguroPreApprovalPlanResponse result = await pagSeguro.SendPreApproval(details);
            
            Console.WriteLine($"Code: {result.Code}, Checkout URL: {result.CheckoutUrl}");

            //At this point you should redirect the user to the result.CheckoutUrl
        }
    }
}