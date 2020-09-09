using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiidWeb.Models;
using System.Security.Principal;

namespace MiidWeb.Helpers
{
    public static class UserHelper
    {

        public static bool IsBandActive(string UserName)
        {

            bool isBandActive = false;
            MiidEntities db = new MiidEntities();
            int userID = UserID(UserName);
            int TagActive = db.Status.Where(c=>c.Description == "Active" && c.Context == "NFCTag").First().ID;

            var Tags = db.NFCTags.Where(x => x.EndUserID == userID && x.StatusID ==TagActive);
            if (Tags != null && Tags.Count() > 0)
            {
                isBandActive = true;
            }

            return isBandActive;
        }

        public static bool HasSiteRole(int UserID, string Permission)
        {
            bool _HasPermission = false;
            //MiidEntities db = new MiidEntities();
            //var RoleForPermission = db.SiteRoles.Where(r => r.RoleName.ToLower() == Permission.ToLower() && r.Context.ToLower() == "Site");
            // int RoleID = 0;
            // if (RoleForPermission != null && RoleForPermission.Count() > 0)
            // {
            //     RoleID = RoleForPermission.First().ID;
            //     var User = db.EndUsers.Find(UserID);
            //     if (User != null && User.SiteRoleID == RoleID)
            //     {
            //         _HasPermission = true;
            //     }
            // }
            return _HasPermission;
        }

        public static bool HasPermission(int UserID, string Permission)
        {
            bool _HasPermission = false;
            //MiidEntities db = new MiidEntities();
            //var RoleForPermission = db.SiteRoles.Where(r => r.RoleName.ToLower() == Permission.ToLower() && r.Context.ToLower() == "permission");
            //int RoleID = 0;
            //if (RoleForPermission != null && RoleForPermission.Count() > 0)
            //{
            //    RoleID = RoleForPermission.First().ID;
            //    var ProjectRolesForUser = db.UserProjectRoles.Where(p => p.Project.ProjectCode.ToLower() == "rolesetter" && p.EndUserID == UserID && p.SiteRoleID == RoleID);
            //    if (ProjectRolesForUser != null && ProjectRolesForUser.Count() > 0)
            //    {
            //        _HasPermission = true;
            //    }
            //    else
            //    {
            //        _HasPermission = false;
            //    }
            //}
            //else
            //{
            //    _HasPermission = false;
            //}
            
            return _HasPermission;
        
        }
        
        public static int UserID(IPrincipal User)
        {
            int result = 0;

            MiidEntities db = new MiidEntities();

            var loggedinuser = db.EndUsers.Where(u => u.Email.ToLower() == User.Identity.Name.ToLower() && u.Status.Code.ToLower()=="active").FirstOrDefault();

            if (loggedinuser != null)
            {
                result = loggedinuser.ID;
            }


            return result;
        }

        public static string UserEmailConfirmed(string UserName)
        {
            var db = new MiidEntities();
            
            var users = db.EndUsers.Where(x => x.Email == UserName); 

            if (users.Count() > 0)
            {
                return "Active";
                /*DANIEL: All users are Active - no need to confirm email
                 * var usersActive = db.EndUsers.Where(x => x.Email == UserName && x.StatusID == 1);

                if (usersActive.Count() > 0)
                    return "Active";
                else
                    return "Pending";*/
            }



            return "Not Registered";
    
        }



        public static int UserID(string UserName)
        {
            
            int result = 0;

            MiidEntities db = new MiidEntities();

            var loggedinuser = db.EndUsers.Where(u => u.Email.ToLower() == UserName.ToLower() && u.Status.Code.ToLower()=="active").FirstOrDefault();

            if (loggedinuser != null)
            {
                result = loggedinuser.ID;
            }


            return result;
        }

        public static string UserName(int? UserID)
        {

            string result = "";

            MiidEntities db = new MiidEntities();

            var loggedinuser = db.EndUsers.Find(UserID);

            if (loggedinuser != null)
            {
                result = loggedinuser.Firstname + " " + loggedinuser.Surname;
            }


            return result;
        }
    }
}