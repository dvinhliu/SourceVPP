using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SourceVPP.Models;

namespace SourceVPP.Controllers
{
    public class TopicController : Controller
    {
        // GET: Topic
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        public ActionResult TopicPartial()
        {
            return PartialView(db.ChuDe.ToList());
        }
        public ActionResult BookByTopic(int macd)
        {
            var tmp = db.Sach.Where(s => s.MaChuDe == macd).OrderBy(s => s.GiaBan).ToList();
            if (tmp.Count > 0)
            {
                return PartialView(tmp);
            }
            ViewBag.kq = "Không có sách nào để hiển thị";
            return PartialView();
        }
    }
}