using SourceVPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SourceVPP.Controllers
{
    public class AddressController : Controller
    {
        laptopDataContext db = new laptopDataContext();
        // GET: Address
        public ActionResult SaveUserAddress(address useraddress)
        {
            try
            {
                db.addresses.InsertOnSubmit(useraddress);
                db.SubmitChanges();
                return Json(new { success = true, message = "Dữ liệu đã được lưu thành công." });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}