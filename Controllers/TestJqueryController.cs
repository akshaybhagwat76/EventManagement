using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiidWeb.Controllers
{
    public class TestJqueryController : Controller
    {
        // GET: TestJquery
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult PopulateDetails(UserModel model)
        {
            UserResultModel userResultModel = new UserResultModel();
            if (String.IsNullOrEmpty(model.UserId))
            {
                userResultModel.Message = "UserId can not be blank";
                return Json(userResultModel);
            }

            UserModel user = EndUserRepository.GetUser(model.UserId);

            if (user == null)
            {
                userResultModel.Message = String.Format("No UserId found for {0}", model.UserId);
                return Json(userResultModel);
            }

            userResultModel.LastName = user.LastName;
            userResultModel.FirstName = user.FirstName;
            userResultModel.Message = String.Empty; //success message is empty in this case

            return Json(userResultModel);
        }

    }


}