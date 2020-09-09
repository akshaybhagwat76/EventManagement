using MiidWeb.Controllers;
using MiidWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MiidWeb.Repositories
{
    public static class ConfigRepo
    {
        public static int GetSubdomainID()
        {
            int subDomainID = 0;

            ErrorController e = new ErrorController();
            

            if (HttpContext.Current != null)
            {
                string host = "";
                try
                {
                    host = HttpContext.Current.Request.Url.Host;
                }
                catch (NullReferenceException nullex)
                {
                    host = HttpContext.Current.Request.Url.OriginalString;
                }
                    
                //  string company = host.Split('.')[0].ToString();
                string company = host;
                company = company.Replace(":80", "");

                // if (company.Contains("www")) {  company = "training.miid.co.za"; }
                 if (company.Contains("localhost")) { company = "miid.co.za"; }


                subDomainID = LookupHelper.GetSubdomainID(company);

                if (subDomainID == 0)
                {
                    subDomainID = LookupHelper.GetSubdomainID(host);
                }
            }
            else
            {
                
            }

            return subDomainID;

        }


        public static string Get(string Name, int subdomainID=0)
        {

            if (Name.ToLower().Contains("connectionstring"))
            {
                return ConfigurationManager.ConnectionStrings[Name].ConnectionString;

            }


            if (subdomainID == 0)
            {
                subdomainID = GetSubdomainID();
                
            }


            string result = "";
            using (var db = new MiidEntities())
            {
                var config = db.Configurations.Where(x => x.Key.ToUpper() == Name.ToUpper() && x.SubdomainID==subdomainID);

                if (config != null && config.Count() > 0)
                {
                    result = config.First().Value;
                }
                else //try get default
                {
                    var defaultconfig = db.Configurations.Where(x => x.Key.ToUpper() == Name.ToUpper() && x.SubdomainID == 1);
                    if (defaultconfig != null && defaultconfig.Count() > 0)
                    {
                        result = defaultconfig.First().Value;
                    }
                    else//try config file
                    {
                        if (ConfigurationManager.AppSettings[Name] != null)
                        {
                            result = ConfigurationManager.AppSettings[Name].ToString();
                            
                            if (String.IsNullOrEmpty(result))//try connection strings
                            {
                                result = ConfigurationManager.ConnectionStrings[Name].ConnectionString;
                            }
                        }

                    }
                }

            }

            return result;

        }
    }
}