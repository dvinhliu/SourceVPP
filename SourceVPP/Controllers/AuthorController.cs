using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SourceVPP.Models;

namespace SourceVPP.Controllers
{
    public class AuthorController : Controller
    {
        // GET: Author
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        public ActionResult AuthorPartial()
        {
            return PartialView(db.NhaXuatBan.ToList());
        }
        public ActionResult BookByAuthor(int manxb)
        {
            var tmp = db.NhaXuatBan.Where(at => at.MaNXB == manxb).OrderBy(at => at.TenNXB).ToList();
            if (tmp.Count > 0)
            {
                return PartialView(tmp);
            }
            ViewBag.kq = "Không có sách nào để hiển thị";
            return PartialView();
        }
    }
}