using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SourceVPP.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Cart()
        {
            return View();
        }
        public ActionResult CartPartial()
        {
            ViewBag.TongSL = 0;
            ViewBag.TongTT = 0;
            return View();
        }
    }
}