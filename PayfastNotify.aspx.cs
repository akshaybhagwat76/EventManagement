using MiidWeb.Helpers;
using MiidWeb.Models;
using MiidWeb.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiidWeb
{
    public partial class PayfastNotify : System.Web.UI.Page
    {
       
        string orderId = "";
        string processorOrderId = "";
        string strPostedVariables = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            // Can't have a postback on this page, it is called
            // once by the payment processor.
            if (Page.IsPostBack) return;

            string text = "";
            string path = "";

            try
            {
                text = String.Format(@"Notify Page Loaded {0}
            ", DateTime.Now.ToString());
                path = Server.MapPath("~/Uploads");
                System.IO.File.AppendAllText(Path.Combine(path, "PayfastLog.txt"), text);
                LogHelper.Log("/nTRY/nPayfast Notify", text);
            }
            catch (Exception gg)
            {
                LogHelper.Log("/nCATCH/nPayfast Notify", gg.ToString());

            }
            try
            {
                // Get the posted variables. Exclude the signature (it must be excluded when we hash and also when we validate).
                text = "// Get the posted variables. Exclude the signature (it must be excluded when we hash and also when we validate).";
                LogHelper.Log("/nTRY/nPayfast Notify", text);
                NameValueCollection arrPostedVariables = new NameValueCollection(); // We will use later to post data
                NameValueCollection req = Request.Form;
                string key = "";
                string value = "";
                for (int i = 0; i < req.Count; i++)
                {
                    key = req.Keys[i];
                    value = req[i];

                    if (key != "signature")
                    {
                        strPostedVariables += key + "=" + value + "&";
                        arrPostedVariables.Add(key, value);
                    }
                }

                // Remove the last &
                strPostedVariables = strPostedVariables.TrimEnd(new char[] { '&' });

                // Also get the Ids early. They are used to log errors to the orders table.
                orderId = Request.Form["m_payment_id"];
                processorOrderId = Request.Form["pf_payment_id"];

                //LogHelper.Log("Payfast Notify OrderId", orderId);


                // Are we testing or making live payments
                string site = "";
                string merchant_id = "";
                string paymentMode = ConfigRepo.Get("PaymentMode");

                if (paymentMode == "test")
                {
                    site = "https://sandbox.payfast.co.za/eng/query/validate";
                    merchant_id = "10000100";
                }
                else if (paymentMode == "live")
                {
                    site = "https://www.payfast.co.za/eng/query/validate";
                    merchant_id = ConfigRepo.Get("PF_MerchantID");
                }
                else
                {
                    text += "throw new InvalidOperationException(\"Cannot process payment if PaymentMode(in web.config) value is unknown.\");";
                    //throw new InvalidOperationException("Cannot process payment if PaymentMode (in web.config) value is unknown.");
                    LogHelper.Log("/nCATCH/nPayfast Notify", text.ToString());
                }

                // Get the posted signature from the form.
                string postedSignature = Request.Form["signature"];

                if (string.IsNullOrEmpty(postedSignature))
                {
                    text = "throw new Exception(\"Signature parameter cannot be null\")";
                    LogHelper.Log("/nCATCH/nPayfast Notify 106", text.ToString());
                }
                // Check if this is a legitimate request from the payment processor
                PerformSecurityChecks(arrPostedVariables, merchant_id);

                // The request is legitimate. Post back to payment processor to validate the data received
                ValidateData(site, arrPostedVariables);


                // All is valid, process the order
                ProcessOrder(arrPostedVariables);
            }
            catch (Exception ex)
            {
                // An error occurred
                text = ex.Message.ToString();
                LogHelper.Log("/nCATCH/nPayfast Notify", text.ToString());
            }
        }

        private void PerformSecurityChecks(NameValueCollection arrPostedVariables, string merchant_id)
        {

            try
            {
                string text = "PerformSecurityChecks";
                foreach (var x in arrPostedVariables.Keys)
                {
                    text += x.ToString() + ":" + arrPostedVariables[x.ToString()].ToString() + "\n";
                }
                LogHelper.Log("/TRY/nPayfast Notify - PerformSecurityChecks", text.ToString());
                // Verify that we are the intended merchant
                string receivedMerchant = arrPostedVariables["merchant_id"];

                if (receivedMerchant != merchant_id)
                {
                    text = "throw new Exception(\"Mechant ID mismatch\");";
                    LogHelper.Log("/nCATCH/nPayfast Notify", text.ToString());
                }
                // Verify that the request comes from the payment processor's servers.

                // Get all valid websites from payment processor
                string[] validSites = new string[] { "www.payfast.co.za", "sandbox.payfast.co.za", "w1w.payfast.co.za", "w2w.payfast.co.za" };

                // Get the requesting ip address
                string requestIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (string.IsNullOrEmpty(requestIp))

                {
                    text = "throw new Exception(\"IP address cannot be null\"); ";
                    LogHelper.Log("/nCATCH/nPayfast Notify", text.ToString());
                }
                // Is address in list of websites
                if (!IsIpAddressInList(validSites, requestIp))

                {
                    text = "throw new Exception(\"IP address invalid\"); ";
                    LogHelper.Log("/nCATCH/nPayfast Notify", text.ToString());
                }
            }
            catch (Exception ex)
            {
                // An error occurred
                string text = ex.Message.ToString();
                LogHelper.Log("/nCATCH/nPayfast Notify", text.ToString());
            }
        }

        private bool IsIpAddressInList(string[] validSites, string ipAddress)
        {
            try
            {
                LogHelper.Log("/TRY/nPayfast Notify - IsIpAddressInList", "");
                // Get the ip addresses of the websites
                ArrayList validIps = new ArrayList();

                for (int i = 0; i < validSites.Length; i++)
                {
                    validIps.AddRange(System.Net.Dns.GetHostAddresses(validSites[i]));
                    LogHelper.Log("Valid Sites: ", validSites[i]);
                }

                IPAddress ipObject = IPAddress.Parse(ipAddress);

                if (validIps.Contains(ipObject))
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                // An error occurred
                LogHelper.Log("Payfast Notify - IsIpAddressInList", ex.ToString());
                LogHelper.Log("Payfast Notify - IsIpAddressInList", ex.Message.ToString());
                return false;
            }

        }

        private void ValidateData(string site, NameValueCollection arrPostedVariables)
        {
            WebClient webClient = null;

            try
            {
                webClient = new WebClient();
                byte[] responseArray = webClient.UploadValues(site, arrPostedVariables);
                LogHelper.Log("TRY Payfast Notify - ValidateData", "\n");

                // Get the response and replace the line breaks with spaces
                string result = Encoding.ASCII.GetString(responseArray);
                result = result.Replace("\r\n", " ").Replace("\r", "").Replace("\n", " ");

                // Was the data valid?
                if (result == null || !result.StartsWith("VALID"))
                {

                    string text = "throw new Exception(\"Data validation failed\");";
                    LogHelper.Log("TRY Payfast Notify - ValidateData", text);
                }
            }
            catch (Exception ex)
            {

                LogHelper.Log("\ncatch Payfast Notify", ex.ToString());
                LogHelper.Log("\ncatch Payfast Notify - ValidateData", ex.Message.ToString());
                // throw ex;
            }
            finally
            {
                if (webClient != null)
                    webClient.Dispose();
            }
        }

        private void ProcessOrder(NameValueCollection arrPostedVariables)
        {
            // Determine from payment status if we are supposed to credit or not
            string paymentStatus = "";
            string path = "";
            string text = "";
            try
            {

                paymentStatus = arrPostedVariables["payment_status"];

                text = String.Format(@"Notify Page Result: {1} - {0} ", DateTime.Now.ToString(), paymentStatus);
                path = Server.MapPath("~/Uploads");
                System.IO.File.AppendAllText(Path.Combine(path, "PayfastLog.txt"), text);

                LogHelper.Log("\nTRY Payfast Notify: ProcessOrder", text.ToString());

            }
            catch (Exception ex)
            {

                LogHelper.Log("Payfast Notify", ex.ToString());
                LogHelper.Log("\ncatch Payfast Notify - ProcessOrder", ex.Message.ToString());
                //throw ex;
            }


            try
            {
                if (paymentStatus == "COMPLETE")
                {
                    LogHelper.Log("\nTRY Payfast Notify: ProcessOrder paymentStatus == COMPLETE", "\n");
                    // Update order to successful
                    //if (bt != null)
                    //{
                    //Update Ticket Status to Paid
                    if (arrPostedVariables["m_payment_id"].ToLower().Contains("_tt"))
                    {
                        try
                        {
                            var bt = BankTransactionRepository.UpdateBankTransactionStatus("Approved Ticket Purchase", arrPostedVariables["m_payment_id"], "Payfast System Response");
                            text = String.Format(@"Notify Page PaymentID: {1} - {0} ", DateTime.Now.ToString(), arrPostedVariables["m_payment_id"]);
                            path = Server.MapPath("~/Uploads");
                            System.IO.File.AppendAllText(Path.Combine(path, "PayfastLog.txt"), text);


                            LogHelper.Log("Payfast Notify", "TRY \nYou got to ticket transaction");


                            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";

                            if (!MyMoneyRepository.HasEmailBeenSentForThisTicket(arrPostedVariables["m_payment_id"]))
                            {
                                List<TicketViewModel> boughtTickets = TicketRepository.ConfirmTickets(arrPostedVariables["m_payment_id"]);
                                string EventName = boughtTickets.First().Event.EventName;


                                var numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "." };
                                var amount_gross = Decimal.Parse(arrPostedVariables["amount_gross"], numberFormatInfo);
                              
                                MyMoneyRepository.SendConfirmationInstantEFT_TicketPurchase(bt.EndUserID, (int)(amount_gross), arrPostedVariables["m_payment_id"], bt.TransactionDate, boughtTickets, EventName, baseUrl, subdomain:"miid");
                            }
                        }
                        catch (Exception ee)
                        {
                            LogHelper.Log("\ncatch Payfast Notify - ProcessOrder  Payment complete", ee.Message.ToString());
                            LogHelper.Log("Payfast Notify", ee.Message);

                        }
                    }
                    else
                    {
                        try
                        {
                            var bt = BankTransactionRepository.UpdateBankTransactionStatus("payment received", arrPostedVariables["m_payment_id"], "Payfast System Response");
                            //OR Add Miifunds record
                            LogHelper.Log("Payfast Notify", "TRY \nYou got to  update bank transaction TOPUP");
                            //LogHelper.Log("Payfast Notify","You got to update bank transaction");


                            if (!MyMoneyRepository.HasEmailBeenSentForThisTopup(arrPostedVariables["m_payment_id"]))
                            {
                                MyMoneyRepository.SuccessfulTopup(bt.EndUserID, decimal.Parse(arrPostedVariables["amount_gross"]), arrPostedVariables["m_payment_id"], String.Format("Instant EFT Topup #{0}", arrPostedVariables["m_payment_id"]));
                                MyMoneyRepository.SendConfirmationInstantEFT(bt.EndUserID, (int)(decimal.Parse(arrPostedVariables["amount_gross"])), arrPostedVariables["m_payment_id"], bt.TransactionDate);
                            }
                        }
                        catch (Exception ee)
                        {
                            LogHelper.Log("\ncatch Payfast Notify - ProcessOrder  payment received", ee.Message.ToString());
                            LogHelper.Log("Payfast Notify", ee.Message);

                        }
                    }



                    // }

                }
                else if (paymentStatus == "FAILED")
                {
                    // Update order to failed
                    var bt = BankTransactionRepository.UpdateBankTransactionStatus("Failed", arrPostedVariables["m_payment_id"], "Payfast System Response");
                    if (bt != null)
                    {
                        //DO NOT Add Miifunds record
                        MyMoneyRepository.SendFailureInstantEFT(bt.EndUserID, (int)(decimal.Parse(arrPostedVariables["amount_gross"])), arrPostedVariables["m_payment_id"], bt.TransactionDate);

                    }
                }
                else
                {
                    // Log for investigation
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("Payfast Notify", ex.Message);
            }
        }


    }


}