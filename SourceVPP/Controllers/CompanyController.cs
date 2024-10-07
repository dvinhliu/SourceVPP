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
        laptopEntities db = new laptopEntities();

        public ActionResult CompanyPartial()
        {
            return PartialView(db.hang.ToList());
        }
        public ActionResult LaptopByCompany(int mahang)
        {
            var tmp = db.sanpham.Where(l => l.MaHang == mahang).OrderBy(l => l.GiaBan).ToList();
            if (tmp.Count > 0)
            {
                ViewBag.Company = db.hang.Single(h => h.MaHang == mahang).TenHang;
                return View(tmp);
            }
            ViewBag.kq = "Không có laptop nào để hiển thị";
            return View();
        }
    }
}