using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiidWeb;
using MiidWeb.Models;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;

namespace MiidWeb.Repositories
{
    public class CMSRepository
    {

        public static string GetContent(string PagePath, int ParagraphNo)
        {
            string result = "";
             using (MiidEntities db = new MiidEntities())
            {
                var cmses = db.CMS.Where(x => x.PagePath.ToLower() == PagePath.ToLower());
                if (cmses != null && cmses.Count() > 0)
                {
                    var cms = cmses.First();
                    switch (ParagraphNo)
                    {

                        case 1: result = cms.Paral; break;
                        case 2: result = cms.Para2;  break;
                        case 3: result = cms.Para3; break;
                        case 4: result = cms.Para4; break;
                        default: break;
                    
                    }
                }
            }
                return result;
        }
    }
}