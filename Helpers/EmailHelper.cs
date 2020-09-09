using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Threading;
using System.Net.Mail;
using System.Net;

using System.Net.Mime;

using System.IO;
using System.Text;
using System.Configuration;
using MiidWeb.Repositories;
using MiidWeb.Controllers;

namespace MiidWeb.Helpers
{



    public interface IMailable
    {
        string Subject { get; set; }
        string Body { get; set; }
    }

    public class Mailable : IMailable
    {

        public Mailable()
        {
            Subject = "";
            Body = "";
        }

        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class FileAttachment
    {
        public string FileContentBase64 { get; set; }

        public FileInfo Info { get; set; }
    }

    public interface IMailSettings
    {

        bool Enabled { get; }

        string Host { get; }

        int Port { get; }

        string Username { get; }

        string Password { get; }

        string From { get; }

        int Retries { get; }

        TimeSpan RetryWait { get; }

        bool SslEnabled { get; }


    }

    public static class EmailHelper
    {
        public static int _subdomainid = 1;

        private static bool _enabled = true;
        // private static string _host = ConfigRepo.Get("EmailHost");//"mail.miid.co.za";
        // private static int _port = int.Parse(ConfigRepo.Get("EmailPort"));//25;
        // private static string _username = ConfigRepo.Get("EmailUserName");//"info@miid.co.za";
        // private static string _password = ConfigRepo.Get("EmailPassword");//"Cel1808!";



        //private static string _contactEmailAddress = ConfigRepo.Get("EmailContactEmail");//["info@miid.co.za");

        // private static string _from = String.Format("{0} <{1}>",ConfigRepo.Get("EmailFromAddressTitle"), ConfigRepo.Get("EmailFromAddress"));// "MiiD HelpDesk <info@miid.co.za>";
        private static int _retries = 1;
        private static TimeSpan _retryWait = TimeSpan.FromSeconds(5);
        // private static bool _sslEnabled = bool.Parse(ConfigRepo.Get("EmailSslEnabled"));//25;


        /*   public static void Configure(IMailSettings settings)
           {
               //_password = System.Web.Configuration.WebConfigRepo.Get("email-password"].ToString();

               lock (typeof(EmailHelper))
               {

                   _enabled = settings.Enabled;
                   _host = settings.Host;
                   _port = settings.Port;
                   _username = settings.Username;
                   _password = settings.Password;
                   _from = settings.From;
                   _retries = settings.Retries;
                   _retryWait = settings.RetryWait;
                   _sslEnabled = settings.SslEnabled;

                   //clear cached properties
                   _fromAddress = null;
                   _credentials = null;
               }

           }

       */

        public static bool SendMail(string recipients, string subject, string body, MemoryStream attachment = null, string attachmentPath = null, List<string> attachmentList = null, int subdomainid = 1)
        {
            _subdomainid = subdomainid;
            return SendMail(recipients.Split(';'), subject, body, false, null, null, attachmentPath, attachmentList);
        }




        public static bool SendContactUsMail(string subject, string body)
        {
            return SendMail(ConfigRepo.Get("EmailContactEmail", _subdomainid).Split(';'), subject, body, false, null);
        }

        public static bool SendMail(string[] recipients, IMailable mailable)
        {
            return SendMail(recipients, mailable.Subject, mailable.Body, false, null);
        }

        public static bool SendMail(string recipients, string subject, string body, Dictionary<string, string> embeddedImages)
        {
            return SendMail(recipients.Split(';'), subject, body, false, embeddedImages);
        }


        public static bool SendMail(string[] recipients, string subject, string body, bool throwFriendlyExceptionOnFail, Dictionary<string, string> embeddedImages, MemoryStream attachment = null, string pathToAttachment = null, List<string> attachmentList = null)
        {

            try
            {

                AlternateView alternateView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

                if (embeddedImages != null)
                {
                    foreach (KeyValuePair<string, string> image in embeddedImages)
                    {
                        LinkedResource logo = new LinkedResource(image.Value, "image/png");
                        logo.ContentId = image.Key;
                        alternateView.LinkedResources.Add(logo);
                    }
                }

                MailMessage mm = new MailMessage();

                mm.From = FromAddress;
                mm.Subject = subject;
                mm.AlternateViews.Add(alternateView);
                mm.IsBodyHtml = true;

                //        var pdfAttachment = new System.Net.Mail.Attachment(attachment, System.Net.Mime.MediaTypeNames.Application.Pdf);
                //        pdfAttachment.ContentDisposition.FileName = "YourTicket.pdf";
                //mm.Attachments.Add(pdfAttachment);

                if (!String.IsNullOrEmpty(pathToAttachment))
                {
                    mm.Attachments.Add(new Attachment(pathToAttachment));
                }

                if (attachmentList != null && attachmentList.Count() > 0)
                {
                    foreach (string att in attachmentList)
                    {
                        mm.Attachments.Add(new Attachment(att));
                    }
                }

                foreach (string s in recipients)
                {
                    try
                    {
                        mm.To.Add(new MailAddress(s));
                    }
                    catch { }
                }

                SmtpClient c = new SmtpClient();
                c.Host = ConfigRepo.Get("EmailHost", _subdomainid); //_host;
                c.Port = int.Parse(ConfigRepo.Get("EmailPort", _subdomainid)); //_port;

                string emailPassword = ConfigRepo.Get("EmailPassword", _subdomainid);
                NetworkCredential _credentials = new NetworkCredential(ConfigRepo.Get("EmailUserName", _subdomainid), emailPassword);

                c.Credentials = _credentials;
                //c.EnableSsl = _sslEnabled;
                //c.DeliveryMethod = SmtpDeliveryMethod.Network;


                c.Send(mm);

                //_logger.Debug(string.Format("Mail Sent To:{0}\n\nSubject:\n{1}\n\nBody:\n{2}", recipients.Delimit(), subject, body));
                mm.Attachments.Dispose();
                mm.Dispose();

                return true;

            }
            catch (Exception e)
            {
                // ErrorController e3 = new ErrorController();
                //  e3.Response.Write(e.Message);
                Helpers.LogHelper.Log(e.Message, "Send Mail");
                if (e.InnerException != null)
                {
                    Helpers.LogHelper.Log(e.InnerException.Message, "Send Mail");
                }


                return false;

            }

        }




        static MailAddress _fromAddress = null;
        public static MailAddress FromAddress
        {
            get
            {
                // if (_fromAddress == null)
                //{
                _fromAddress = new MailAddress(String.Format("{0} <{1}>", ConfigRepo.Get("EmailFromAddressTitle", _subdomainid), ConfigRepo.Get("EmailFromAddress", _subdomainid)));// "MiiD HelpDesk <info@miid.co.za>";);
                //}
                return _fromAddress;
            }

        }

        static NetworkCredential _credentials = null;
        public static NetworkCredential Credentials
        {
            get
            {
                //if (_credentials == null)
                // {
                string emailPassword = ConfigRepo.Get("EmailPassword", _subdomainid);
                _credentials = new NetworkCredential(ConfigRepo.Get("EmailUserName", _subdomainid), emailPassword);
                //}
                return _credentials;
            }
        }

        public static string AssembleBody(string ReturnLink, string UserName, string Role, string ProjectName, string Message)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("Hi {0}", UserName));
            sb.AppendLine(string.Format("You are involved on project {0}", ProjectName));
            sb.AppendLine(string.Format("in the role of {0}", Role));
            sb.AppendLine(string.Format("Please note: {0}", Message));
            sb.AppendLine(string.Format("Click here to log in to the Ease of Shop workflow site: {0}", ReturnLink));

            return sb.ToString();
        }


        public static string AssembleReturnLink(string URL, string Controller, string Action, string UserName)
        {
            string userid = UserHelper.UserID(UserName).ToString();

            return String.Format("<a href=\"http://{0}/{1}/{2}?u={3}\"> Click here to return to site </a>", URL, Controller, Action, userid);
        }


        //public void OpenOutlookMessage()
        //{
        //    Microsoft.Office.Interop.Outlook.Application oApp = new Microsoft.Office.Interop.Outlook.Application();
        //    Microsoft.Office.Interop.Outlook.MailItem mail = (Microsoft.Office.Interop.Outlook.MailItem)oApp.Session.OpenSharedItem(path);

        //    mail.Display(true);



        //    mail = null;

        //    oApp = null; 

        //}

    }
}



