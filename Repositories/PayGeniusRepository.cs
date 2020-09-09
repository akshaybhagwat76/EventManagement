using MiidWeb.Helpers;
using MiidWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;

namespace MiidWeb.Repositories
{
    public static class PayGeniusRepository
    {

        public static PaymentPageResponse SendToPaygenius(
            PaymentPageRequest paymentPageRequest
            )
        {

            string secret = ConfigRepo.Get("PGsecret").ToString();
            string token = ConfigRepo.Get("PGtoken").ToString();
            string endpoint = ConfigRepo.Get("PGendpoint").ToString();
            //start here

            /* PaymentPageRequest paymentPageRequest = new PaymentPageRequest(


                   new Transaction() { amount = 1, currency = "ZAR", description = "Spare parts for machine", reference = "#INV0021" },
                    new Consumer()
                    {
                        email = "dsouchon@gmail.com",
                        name = "Daniel",
                        surname = "Souchon",
                        address = new Address()
                        {
                            addressLineOne = "Apple Weg 1",
                            city = "East London",
                            country = "South Africa",
                            postCode = "5432"
                        }


                    },
                   new Urls() { cancel = "https://miid.co.za/", error = "https://miid.co.za/", success = "https://miid.co.za/" },
                   false,
                   "goodtree"

                   );
             */

            string data = JsonConvert.SerializeObject(paymentPageRequest);
            string fullEndpoint = endpoint;
            var nullJsonCheck = data.ToString().Replace("{", "").Replace("}", "").Trim();
            var signiture = sign(fullEndpoint, String.IsNullOrEmpty(nullJsonCheck) ? null : data, secret);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(fullEndpoint);
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers["X-Token"] = token;
            httpWebRequest.Headers["X-Signature"] = signiture;

            if (!String.IsNullOrEmpty(nullJsonCheck))
            {
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(data);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            else
            {
                httpWebRequest.Method = "GET";
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();


                PaymentPageResponse response = JsonConvert.DeserializeObject<PaymentPageResponse>(result);

                return response;
            }



        }


        public static LookupResponse LookupPaygenius(
           string payment_reference
           )
        {

            string secret = ConfigRepo.Get("PGsecret").ToString();
            string token = ConfigRepo.Get("PGtoken").ToString();
            string endpoint = ConfigRepo.Get("PGlookupendpoint").ToString();


            // /pg/api/v2/redirect/{payment_reference}

            string data = null;
            string fullEndpoint = endpoint + payment_reference;
            var nullJsonCheck = "";
            var signiture = sign(fullEndpoint, String.IsNullOrEmpty(nullJsonCheck) ? null : data, secret);



            var httpWebRequest = (HttpWebRequest)WebRequest.Create(fullEndpoint);
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers["X-Token"] = token;
            httpWebRequest.Headers["X-Signature"] = signiture;

            if (!String.IsNullOrEmpty(nullJsonCheck))
            {
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(data);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            else
            {
                httpWebRequest.Method = "GET";
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();

                LookupResponse response = JsonConvert.DeserializeObject<LookupResponse>(result);
                //PaymentPageResponse response = JsonConvert.DeserializeObject<PaymentPageResponse>(result);

                return response;
            }



        }


        public static PaymentPageRequest AssembleRequest( int amount, string description, string reference,
            string email, string name, string surname,  string addressLineOne, string city, string country, string postCode          )
        {
            string cancelUrl = ConfigRepo.Get("PGcancelUrl").ToString();
            string errorUrl = ConfigRepo.Get("PGerrorUrl").ToString();
            string successUrl = ConfigRepo.Get("PGsuccessUrl").ToString();

            PaymentPageRequest paymentPageRequest = new PaymentPageRequest(
                  new Transaction()
                  {
                      amount = amount,
                      currency = "ZAR",
                      description = description,
                      reference = reference
                  },
                   new Consumer()
                   {
                       email = email,
                       name = name,
                       surname = surname,
                       address = new Address()
                       {
                           addressLineOne = addressLineOne,
                           city = city,
                           country = country,
                           postCode = postCode
                       }
                   },
                  new Urls() { cancel = cancelUrl + reference, error = errorUrl + reference, success = successUrl + reference },
                  false,
                  "goodtree"

                  );

            return paymentPageRequest;

        }










        public static string sign(string endpoint, string data = null, string secret = null)
        {
            string toSign = endpoint;
            if (data != null)
            {
                toSign += "\n" + data;
            }

            HMACSHA256 hmac = new HMACSHA256(System.Text.Encoding.Default.GetBytes(secret));

            byte[] hash = hmac.ComputeHash(System.Text.Encoding.Default.GetBytes(toSign.Trim()));

            return ByteToString(hash);
        }

        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return sbinary.ToLower();
        }


        public static void ProcessPaygeniusOrder(string reference, bool success)
        {
            // Determine from payment status if we are supposed to credit or not

            string path = "";
            string text = "";
           
            try
            {
                int amountGross = 0;
                var bt1 = BankTransactionRepository.GetByUniquePaymentID(reference);
                if (bt1 != null)
                {
                    amountGross = (int)(bt1.Amount ?? 0.00M) + (int)(bt1.AdminFee ?? 0.00M);
                }
                if (success)
                {
                    LogHelper.Log("\nTRY Paygenius Notify: ProcessOrder paymentStatus == COMPLETE", "\n");


                    //Update Ticket Status to Paid

                    try
                    {

                        bool haveMyTicketsExpired = TicketRepository.HaveMyTicketsExpired(reference);
                        BankTransaction bt = new BankTransaction();
                        if (!haveMyTicketsExpired)
                        {
                            bt = BankTransactionRepository.UpdateBankTransactionStatus("Approved Ticket Purchase", reference, "Paygenius System Response");
                        }
                        else
                        {
                            bt = BankTransactionRepository.UpdateBankTransactionStatus("Tickets Already Expired On Payment Receipt", reference, "Paygenius System Response");
                        }

                        LogHelper.Log("Paygenius Notify", text);
                        LogHelper.Log("Paygenius Notify", "TRY \nYou got to ticket transaction");


                        string baseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";

                        if (!MyMoneyRepository.HasEmailBeenSentForThisTicket(reference))
                        {
                            List<TicketViewModel> boughtTickets = TicketRepository.ConfirmTickets(reference);
                            string EventName = boughtTickets.First().Event.EventName;

                            if (!haveMyTicketsExpired)
                            {

                                MyMoneyRepository.SendConfirmationPaygenius_TicketPurchase(bt.EndUserID, amountGross, reference, bt.TransactionDate, boughtTickets, EventName, baseUrl, subdomain: "miid");
                            }
                            else
                            {
                                MyMoneyRepository.SendConfirmationPaygenius_TicketPurchaseExpired(bt.EndUserID, amountGross, reference, bt.TransactionDate, boughtTickets, EventName, baseUrl, subdomain: "miid");

                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        LogHelper.Log("\ncatch Paygenius Notify - ProcessOrder  Payment complete", ee.Message.ToString());
                        LogHelper.Log("Paygenius Notify", ee.Message);

                    }

                }
                else if (!success)
                {
                    //LogHelper.Log("Paygenius Notify", text);
                    LogHelper.Log("Paygenius Notify", "Transaction failed");

                    // Update order to failed
                    var bt = BankTransactionRepository.UpdateBankTransactionStatus("Failed", reference, "Paygenius System Response");
                    if (bt != null)
                    {
                        TicketRepository.ExpireTickets(reference);
                        TicketRepository.ClearExpiredTicketSeats(reference);
                        //DO NOT Add Miifunds record
                        MyMoneyRepository.SendFailurePaygenius(bt.EndUserID, amountGross, reference, bt.TransactionDate);

                    }
                }
                else
                {
                    // Log for investigation
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("Paygenius Notify", ex.Message);
            }
        }



    }

    // {"error":{"code":2000,"message":"Transaction not found"},"success":false}

    public class LookupResponse
    {
        public error error { get; set; }
        public bool success { get; set; }
    }

    public class error
    {

        public string code { get; set; }
        public string message { get; set; }
    }


    public class PaymentPageResponse
    {
        public bool success { get; set; }
        public string redirectUrl { get; set; }
        public string reference { get; set; }
    }
    public class PaymentPageRequest
    {
        public Transaction transaction { get; set; }
        public Consumer consumer { get; set; }
        public Urls urls { get; set; }

        public bool auth { get; set; }
        public string pageUrlKey { get; set; }


        public PaymentPageRequest() { }

        public PaymentPageRequest(Transaction transaction, Consumer consumer, Urls urls, bool auth, string pageUrlKey)
        {
            this.transaction = transaction;
            this.consumer = consumer;
            this.urls = urls;
            this.auth = auth;
            this.pageUrlKey = pageUrlKey;

        }


        public string LookupStatusRequest(string payment_reference)
        {
            // /pg/api/v2/redirect/{payment_reference}

            return "";
        }

    }



}


public class Transaction
{
    public string reference { get; set; }
    public string description { get; set; }
    public string currency { get; set; } = "ZAR";
    public double amount { get; set; }
}
public class Consumer
{
    public string name { get; set; }
    public string surname { get; set; }
    public string email { get; set; }

    public Address address { get; set; }
}
public class Address
{
    public string addressLineOne { get; set; }
    public string city { get; set; }
    public string postCode { get; set; }
    public string country { get; set; }

}

public class Urls
{
    public string success { get; set; }
    public string cancel { get; set; }
    public string error { get; set; }
}

