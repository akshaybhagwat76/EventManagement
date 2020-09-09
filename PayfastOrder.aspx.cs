using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using MiidWeb.Models;
using MiidWeb.Repositories;
using System.Globalization;
using MiidWeb.Helpers;

namespace MiidWeb
{
    public partial class PayfastOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BankTransaction model = BankTransactionRepository.GetByUniquePaymentID(Request.QueryString["on"]);

                    SendToPayFast(model);
                }
            }

            catch (Exception gg)
            {
                LogHelper.Log("Payfast Order", gg.ToString());

            }
            Response.Write("Something broke.");
        }

        protected void SendToPayFast(BankTransaction model)
        {
            try
            {

                // Create the order in your DB and get the ID
                var numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "." };
                var amount_gross = Decimal.Parse(model.Amount.Value.ToString("0.00"), numberFormatInfo);

                string amount = amount_gross.ToString("0.00");
                string orderId = model.Reference;
                string name = "Mii-Funds Topup, Order #" + orderId;
                string description = model.Description;

                string site = "";
                string merchant_id = "";
                string merchant_key = "";

                // Check if we are using the test or live system
                string paymentMode = ConfigRepo.Get("PaymentMode");

                if (paymentMode == "test")
                {
                    site = "https://sandbox.payfast.co.za/eng/process?";
                    merchant_id = "10000100";
                    merchant_key = "46f0cd694581a";
                }
                else if (paymentMode == "live")
                {
                    site = "https://www.payfast.co.za/eng/process?";
                    merchant_id = ConfigRepo.Get("PF_MerchantID");
                    merchant_key = ConfigRepo.Get("PF_MerchantKey");
                }
                else
                {
                    throw new InvalidOperationException("Cannot process payment if PaymentMode (in web.config) value is unknown.");
                }

                // Build the query string for payment site

                string transactionType = MiidWeb.Helpers.StatusHelper.TransactionTypeCode(model.TransactionTypeID ?? 0);


                StringBuilder str = new StringBuilder();
                str.Append("merchant_id=" + HttpUtility.UrlEncode(merchant_id));
                str.Append("&merchant_key=" + HttpUtility.UrlEncode(merchant_key));

                string PF_ReturnURL_Ticket = "https://" + ConfigRepo.Get("PF_ReturnURL_Ticket");
                string PF_ReturnURL = "https://" + ConfigRepo.Get("PF_ReturnURL");
                string PF_CancelURL = "https://" + ConfigRepo.Get("PF_CancelURL");
                string PF_NotifyURL = "https://" + ConfigRepo.Get("PF_NotifyURL");



                //if (GlobalVariables.Company != "miid")
                //{
                //    //https://www.miid.co.za/PayfastConfirm.aspx
                //    PF_ReturnURL_Ticket = PF_ReturnURL_Ticket.Replace("https://www", "https://" + GlobalVariables.Company);
                //    PF_ReturnURL = PF_ReturnURL.Replace("https://www", "https://" + GlobalVariables.Company);
                //    PF_CancelURL = PF_CancelURL.Replace("https://www", "https://" + GlobalVariables.Company);
                //    PF_NotifyURL = PF_NotifyURL.Replace("https://www", "https://" + GlobalVariables.Company);
                //}


                if (transactionType == "Instant EFT Ticket Purchase")
                {
                    str.Append("&return_url=" + HttpUtility.UrlEncode(PF_ReturnURL_Ticket));
                    name = "MiiD Ticket Purchase, Order #" + orderId;
                }
                else
                {
                    str.Append("&return_url=" + HttpUtility.UrlEncode(PF_ReturnURL));
                }
                str.Append("&cancel_url=" + HttpUtility.UrlEncode(PF_CancelURL));
                str.Append("&notify_url=" + HttpUtility.UrlEncode(PF_NotifyURL));

                str.Append("&m_payment_id=" + HttpUtility.UrlEncode(orderId));
                str.Append("&amount=" + HttpUtility.UrlEncode(amount));
                str.Append("&item_name=" + HttpUtility.UrlEncode(name));
                str.Append("&item_description=" + HttpUtility.UrlEncode(description));

                // Redirect to PayFast
                Response.Redirect(site + str.ToString());
            }
            catch (Exception ex)
            {
                // Handle your errors here (log them and tell the user that there was an error)
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
                LogHelper.Log("Payfast Order", ex.ToString());

            }

            
        

        
        }


    }
}