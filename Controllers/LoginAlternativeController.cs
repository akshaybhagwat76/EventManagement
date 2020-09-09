using MiidWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiidWeb.Controllers
{
    public class LoginAlternativeController : Controller
    {


		private void GetLayoutFiles()
		{

			string host = HttpContext.Request.Url.Host;

            //	string company = host.Split('.')[0].ToString();
            string company = host;

            //	if (company.Contains("www")) {  company = "training.miid.co.za"; }
            if (company.Contains("localhost")) { company = "miid.co.za"; }
            GlobalVariables.Company = company;

			int subDomainID = LookupHelper.GetSubdomainID(company);
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
					GlobalVariables.LoginParaOne = subDomain.LoginParaOne;
					GlobalVariables.LoginParaOne = subDomain.LoginParatwo;
					GlobalVariables.HomeParaOne = subDomain.HomeParaOne;
					GlobalVariables.HomeParaTwo = subDomain.HomeParatwo;


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


			GetLogosAndTagLines(company);

		}
		private void GetLogosAndTagLines(string company)
		{
			switch (company)
			{
				case "foundation": GlobalVariables.LogoFile = "../Content/images/logofoundation.jpg"; break;
				case "wrhi": GlobalVariables.LogoFile = "../Content/images/logowrhi.jpg"; break;
				case "righttocare": GlobalVariables.LogoFile = "../Content/images/logorighttocare.png"; break;
				case "match": GlobalVariables.LogoFile = "../Content/images/logomatch.jpg"; break;
				case "khethimpilo": GlobalVariables.LogoFile = "../Content/images/logokhethimpilo.jpg"; break;
				case "anovahealth": GlobalVariables.LogoFile = "../Content/images/logoanovahealth.jpg"; break;
				case "broadreach": GlobalVariables.LogoFile = "../Content/images/logobroadreach.jpg"; break;
				case "nhls": GlobalVariables.LogoFile = "../Content/images/logonhls.jpg"; break;
				default: GlobalVariables.LogoFile = "http://www.sead.co.za/wp-content/uploads/portraitlogolight.png"; break;

			}
			switch (company)
			{
				case "foundation": GlobalVariables.CompanyTagLine = "Health Strategy | Insight | Implementation"; break;
				case "wrhi": GlobalVariables.CompanyTagLine = "Health Strategy | Insight | Implementation"; break;
				case "righttocare": GlobalVariables.CompanyTagLine = "Health Strategy | Insight | Implementation"; break;

				case "match": GlobalVariables.CompanyTagLine = "Maternal Adolescent and Child Health Systems"; break;
				case "khethimpilo": GlobalVariables.CompanyTagLine = "Choose Life for an AIDS Free Generation."; break;
				case "anovahealth": GlobalVariables.CompanyTagLine = "Anova is dedicated to strengthening quality public health services…"; break;
				case "broadreach": GlobalVariables.CompanyTagLine = "Unlock Human & Economic Potential"; break;
				case "nhls": GlobalVariables.CompanyTagLine = "Africa's centre of excellence for innovative laboratory medicine."; break;
				default: GlobalVariables.CompanyTagLine = "Health Strategy | Insight | Implementation"; break;

			}
		}



		// GET: LoginAlternative
		public ActionResult LoginAlternative()
        {
			GetLayoutFiles();
			return View();
        }
    }
}