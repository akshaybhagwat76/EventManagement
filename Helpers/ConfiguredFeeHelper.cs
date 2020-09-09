using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Helpers
{
    public static class ConfiguredFeeHelper
    {
        public static decimal FeeAmount(string PaymentType, decimal Amount, int? subdomainID)
        {
            //ID	TransactionType	PaymentType	MinimumCharge	PercentageCharge	FixedFee	Active
            //1	Online	EFT	3.00	2.50	0.00	1

            decimal result = 0.00M;



            MiidEntities db = new MiidEntities();

            var checkIfSubdomainConfigured = db.FeeConfigurations.Where(x => x.subdomainID == subdomainID);
            if (checkIfSubdomainConfigured != null && checkIfSubdomainConfigured.Count() > 0)
            {

            }
            else
            {
                subdomainID = null;
            }

            var fee = db.FeeConfigurations.Where(u => u.PaymentType.ToLower() == PaymentType.ToLower() && u.subdomainID == subdomainID).FirstOrDefault();


            if (fee != null)
            {
                //If there is a minimum charge
                if (fee.MinimumCharge != null && fee.MinimumCharge > 0)
                {
                    //and a percentage charge
                    if (fee.Active == true)
                    {
                        if (fee.PercentageCharge != null && fee.PercentageCharge > 0)
                        {
                            //if the resulting charge is less than the minimum charge
                            if ((Amount * ((decimal)fee.PercentageCharge / 100.00M)) < fee.MinimumCharge)
                            {
                                //result is minimum charge
                                result = (decimal)fee.MinimumCharge;
                                //LogHelper.Log(String.Format("FeeAmount={0}", result), "result is minimum charge");
                            }
                            else
                            {
                                //result is percentage of amount
                                result = Amount * ((decimal)fee.PercentageCharge / 100.00M);
                                //LogHelper.Log(String.Format("fee.PercentageCharge={0}", fee.PercentageCharge), "result is percentage of amount");
                                //LogHelper.Log(String.Format("Amount={0}", Amount), "result is percentage of amount");
                                //LogHelper.Log(String.Format("FeeAmount={0}", result), "result is percentage of amount");
                            }
                        }
                        else
                        {
                            result = (decimal)fee.MinimumCharge;
                        }
                    }

                }
                else
                {
                    if (fee.PercentageCharge != null && fee.PercentageCharge > 0)
                    {
                        result = Amount * ((decimal)fee.PercentageCharge / 100.00M);
                        //LogHelper.Log(String.Format("FeeAmount={0}", result), "no min charge");
                    }
                }
                //Fixed Fee takes priority over the rest
                if (fee.Active == true && fee.FixedFee > 0)
                {
                    result = (decimal)fee.FixedFee;
                }

            }


            return result;
        }

    }
}