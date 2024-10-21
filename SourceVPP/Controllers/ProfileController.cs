using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SourceVPP.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index(string view)
        {
            ViewBag.PartialView = string.IsNullOrEmpty(view) ? "ProfilePartial" : view;
            return View();
        }

        public ActionResult ProfilePartial()
        {
            return PartialView();
        }

        public ActionResult AddressPartial()
        {
            return PartialView();
        }

        public ActionResult ChangePasswordPartial()
        {
            return PartialView();
        }

        public ActionResult OrderPartial()
        {
            return PartialView();
        }
    }
}