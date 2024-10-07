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
        laptopEntities db = new laptopEntities();
        public ActionResult LaptopPartial()
        {
            return PartialView(db.sanpham.ToList());
        }
        public ActionResult LaptopDetails()
        {
            return View();
        }
    }
}