using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Specialized;
using MiidWeb.Repositories;
using MiidWeb.Models;
using System.IO;
namespace MiidWeb
{
    public partial class IveriSuccess : System.Web.UI.Page
    {
        private void GetLayoutFiles()
        {

            string host = Request.Url.Host;

            // string company = host.Split('.')[0].ToString();
            string company = host;

            //  if (company.Contains("www")) {  company = "training.miid.co.za"; }
           if (company.Contains("localhost")) { company = "miid.co.za"; }

            GlobalVariables.Company = company;

            int subDomainID = Helpers.LookupHelper.GetSubdomainID(company);
            using (var db = new MiidEntities())
            {
                Subdomain subDomain = db.Subdomains.Find(subDomainID);
                if (subDomain != null)
                {
                    GlobalVariables.SubdomainID = subDomain.ID.ToString();
                    GlobalVariables.Text1 = subDomain.Text1;
                    GlobalVariables.Text2 = subDomain.Text2;
                    GlobalVariables.Text3 = subDomain.Text3;
                    GlobalVariables.Text4 = subDomain.Text4;
                    GlobalVariables.Text5 = subDomain.Text5;
                    GlobalVariables.Text6 = subDomain.Text6;
					


				}
            }
            if (company == "miid")
            {
                GlobalVariables.LayoutLogin = "~/Views/Shared/_LayoutLogin.cshtml";
                GlobalVariables.LayoutAdmin = "~/Views/Shared/_LayoutAdmin.cshtml";
                GlobalVariables.Layout = MiidWeb.Repositories.TicketRepository.GetLayoutFile("Layout");

            }
            else
            {
                GlobalVariables.LayoutLogin = String.Format("~/Views/Shared/_LayoutLogin{0}.cshtml", company);
                GlobalVariables.LayoutAdmin = String.Format("~/Views/Shared/_LayoutAdmin{0}.cshtml", company);
                GlobalVariables.Layout = String.Format("~/Views/Shared/_Layout{0}.cshtml", company);

            }




        }
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                //NEW
                string status;
                string description;
                string transactionType;
                string amount;
                string authorisationCode;
                string merchantReference;

                status = Request.Form["LITE_PAYMENT_CARD_STATUS"].ToString();
                description = Request.Form["LITE_RESULT_DESCRIPTION"].ToString();
                transactionType = Request.Form["LITE_AUTHORISATION"].ToString();
                amount = Request.Form["LITE_ORDER_AMOUNT"].ToString();
                authorisationCode = Request.Form["LITE_ORDER_AUTHORISATIONCODE"].ToString();
                merchantReference = Request.Form["ECOM_CONSUMERORDERID"].ToString();

                decimal fee = 0;
                int amountminusfee = 0;

                var db = new MiidEntities();
                var bts = db.BankTransactions.Where(x => x.Reference == merchantReference);
                BankTransaction bt1 = new BankTransaction();
                if (bts != null && bts.Count() > 0)
                {
                    bt1 = bts.First();
                    fee = (decimal)bt1.AdminFee;
                    amountminusfee = (int)bt1.Amount;
                }
                string text = String.Format(@"Iveri Success Page Loaded {0} - Status {1} Description - {2} transaction type - {3}
            ", DateTime.Now.ToString(), status, description, transactionType);
                string path = Server.MapPath("~/Uploads");
                System.IO.File.AppendAllText(Path.Combine(path, "IveriLog.txt"), text);
                int intAmountInclFee = (int)(decimal.Parse(amount)) / 100;

                if (transactionType == "TRUE")
                {
                    transactionType = "Authorisation";
                    lbltransactionType.Text = transactionType;
                }
                else
                {
                    transactionType = "Sale";
                    lbltransactionType.Text = transactionType;
                    
                }

                System.IO.File.AppendAllText(Path.Combine(path, "IveriLog.txt"), "line 73");

                if (status == "0")
                {
                    //Payment was successful
                    //Do the database thing and send the emails

                    pnlSuccess.Visible = true;
                    pnlFailed.Visible = false;
                    lblamount.Text = amountminusfee.ToString();
                    lblfee.Text = fee.ToString();
                    lblauthorisationCode.Text = authorisationCode;
                    lblmerchantReference.Text = merchantReference;

                    //System.IO.File.AppendAllText(Path.Combine(path, "IveriLog.txt"), "line 87");

                    var bt = BankTransactionRepository.UpdateBankTransactionStatus("payment received", merchantReference, "Iveri System Response");

                    //System.IO.File.AppendAllText(Path.Combine(path, "IveriLog.txt"), "line 90");

                    // Update order to successful
                    if (bt1 != null)
                    {
                        System.IO.File.AppendAllText(Path.Combine(path, "IveriLog.txt"), "\\n bt1 is not null");
                        System.IO.File.AppendAllText(Path.Combine(path, "IveriLog.txt"), bt1.ID.ToString());

                        if (merchantReference != null)
                        {

                            if (merchantReference.Contains("_TT"))//its a ticket purchase
                            {
                                List<TicketViewModel> boughtTickets = TicketRepository.ConfirmTickets(merchantReference);
                                string EventName = boughtTickets.First().Event.EventName;
                                //System.IO.File.AppendAllText(Path.Combine(path, "IveriLog.txt"), "line 96");
                                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                                MyMoneyRepository.SendConfirmationCardPayment_TicketPurchase(bt.EndUserID, intAmountInclFee, merchantReference, bt.TransactionDate, boughtTickets, EventName, baseUrl, subdomain: "miid");
                                lblAmountTitle.Text = "Ticket Amount";
                                //System.IO.File.AppendAllText(Path.Combine(path, "IveriLog.txt"), "line 99");
                            }
                            else
                            {
                                //Add Miifunds record
                                lblAmountTitle.Text = "Topup Amount";
                                System.IO.File.AppendAllText(Path.Combine(path, "IveriLog.txt"), " miifunds topup line 106");
                                MyMoneyRepository.SuccessfulTopup(bt1.EndUserID, (decimal)intAmountInclFee, merchantReference, String.Format("Card Payment Topup #{0}", merchantReference));
                                MyMoneyRepository.SendConfirmationCardPayment(bt1.EndUserID, intAmountInclFee, merchantReference, bt1.TransactionDate);


                            }
                        }
                        else
                        {
                            System.IO.File.AppendAllText(Path.Combine(path, "IveriLog.txt"), "merchantReference is null");
                        }
                    }
                    else
                    {
                        System.IO.File.AppendAllText(Path.Combine(path, "IveriLog.txt"), "\\n bt1 is null");
                    
                    }
                    



                }
                else
                {
                    pnlSuccess.Visible = false;
                    pnlFailed.Visible = true;
                    lblstatus.Text = status;
                    lbldescription.Text = description;

                    // Update order to failed
                    var bt = BankTransactionRepository.UpdateBankTransactionStatus("Failed", merchantReference, "Iveri System Response");
                    if (bt != null)
                    {
                        //DO NOT Add Miifunds record
                        MyMoneyRepository.SendFailureCardPayment(bt.EndUserID, (int)(decimal.Parse(amount)), merchantReference, bt.TransactionDate);

                    }

                }
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
                Response.Write(ex.StackTrace.ToString());
                if (ex.InnerException != null)
                {
                    Response.Write(ex.InnerException.StackTrace.ToString());
                    Response.Write(ex.InnerException.Message.ToString());
                }
            }





            //END NEW


            /*	
		
                NameValueCollection nvc = (NameValueCollection)Session["resultNVC"];
                this.NPay_Status.Value = nvc["NPAYSTATUS"].ToString();

                // Display the result to the screen
                // This is an example display, you can choose to display the result in any format you choose
                WriteLine(nvc);

                if (nvc["RESULT"] == "UnauthorizedAccessException")
                    WriteUAException(nvc);

                if (this.NPay_Status.Value == "ONLINE")
                {
                    this.NPay_Customer_Print_Data.Value = (Session["CustomerPrint"] == null) ? "" : Session["CustomerPrint"].ToString();
                    this.NPay_Merchant_Print_Data.Value = (Session["MerchantPrint"] == null) ? "" : Session["MerchantPrint"].ToString();
                    this.NPay_Declined_Print_Data.Value = (Session["DeclinedPrint"] == null) ? "" : Session["DeclinedPrint"].ToString();
                    this.btn_Print.Visible = true;
                    WriteLine("<br />");
                    WriteLine("<b>Click 'Print' to print receipt<b>");
                }

                // Remove the session state once you have completed the transaction for security purposes
                this.Session.Clear();
		
                */
        }

        private void WriteUAException(NameValueCollection nvc)
        {
            WriteLine("** NOTE ***");
            WriteLine("An UnauthorizedAccessException occurred.");
            WriteLine("It may have been caused by using the EnterpriseSC class and not configuring an Administration identity within ComponentServices.");
            WriteLine("Either configure the identity, or use Enterprise class instead.");
            WriteLine("The EnterpriseSC class is recommended for production ASP.NET websites (as it means the website can run under a restricted user).");
        }

        private void WriteLine(string txt)
        {

            // Add the line to the output
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell td = new HtmlTableCell();
            td.ColSpan = 2;
            td.Attributes.CssStyle.Add("text-align", "center");
            td.InnerHtml = txt;
            tr.Cells.Add(td);
            //this.tblResult.Rows.Add(tr);
        }

        private void WriteLine(NameValueCollection nvc)
        {
            for (int i = 0; i < nvc.Count; i++)
            {
                if (nvc.GetKey(i) != "NPAYSTATUS")
                {
                    HtmlTableRow tr = new HtmlTableRow();
                    HtmlTableCell td1 = new HtmlTableCell();
                    td1.Attributes.Add("class", "clsInformationHeading");
                    td1.InnerHtml = nvc.GetKey(i);
                    HtmlTableCell td2 = new HtmlTableCell();
                    td2.Attributes.Add("class", "clsInformation");
                    td2.InnerHtml = nvc[i];

                    tr.Cells.Add(td1);
                    tr.Cells.Add(td2);
                    //this.tblResult.Rows.Add(tr);
                }
            }
        }
    }
}