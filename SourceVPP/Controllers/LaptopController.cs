using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SourceVPP.Models;

namespace SourceVPP.Controllers
{
    public class LaptopController : Controller
    {
        // GET: Book
        laptopDataContext db = new laptopDataContext();
        public ActionResult LaptopPartial()
        {
            return PartialView(db.sanphams.ToList());
        }
        public ActionResult LaptopDetails()
        {
            return View();
        }
    }
}