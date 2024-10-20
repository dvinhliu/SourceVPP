using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SourceVPP.Models;

namespace SourceVPP.Controllers
{
    public class CompanyController : Controller
    {
        // GET: Topic
        laptopDataContext db = new laptopDataContext();

        public ActionResult CompanyPartial()
        {
            return PartialView(db.hangs.ToList());
        }
        public ActionResult LaptopByCompany(int mahang)
        {
            var tmp = db.sanphams.Where(l => l.MaHang == mahang).OrderBy(l => l.GiaBan).ToList();
            if (tmp.Count > 0)
            {
                ViewBag.Company = db.hangs.Single(h => h.MaHang == mahang).TenHang;
                return View(tmp);
            }
            ViewBag.kq = "Không có laptop nào để hiển thị";
            return View();
        }
    }
}