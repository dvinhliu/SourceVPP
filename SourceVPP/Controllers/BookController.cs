using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SourceVPP.Models;

namespace SourceVPP.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        public ActionResult BookPartial()
        {
            return PartialView(db.Sach.ToList());
        }
    }
}