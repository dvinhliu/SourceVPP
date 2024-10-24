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
                if (useraddress.isDefault == true)
                {
                    var lstadrs = db.addresses.Where(u => u.MaTaiKhoan == useraddress.MaTaiKhoan).ToList();

                    foreach(var adrs in lstadrs)
                    {
                        if (adrs.address_id != useraddress.address_id)
                        {
                            adrs.isDefault = false;
                        }
                    }
                    db.SubmitChanges();
                }    
                db.addresses.InsertOnSubmit(useraddress);
                db.SubmitChanges();
                return Json(new { success = true, message = "Dữ liệu đã được lưu thành công." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public ActionResult SaveSetDefault(int address_id)
        {
            try
            {
                var address = db.addresses.FirstOrDefault(u => u.address_id == address_id);
                if (address.isDefault == false)
                {
                    var lstadrs = db.addresses.Where(u => u.MaTaiKhoan == address.MaTaiKhoan).ToList();

                    foreach (var adrs in lstadrs)
                    {
                        if (adrs.address_id != address.address_id)
                        {
                            adrs.isDefault = false;
                        }
                    }

                    address.isDefault = true;
                    db.SubmitChanges();
                }
                return Json(new { success = true, message = "Dữ liệu đã được lưu thành công." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public ActionResult DeleteUserAddress(int address_id)
        {
            try
            {
                var address = db.addresses.FirstOrDefault(u => u.address_id == address_id);
                if (address != null)
                {
                    db.addresses.DeleteOnSubmit(address);
                    db.SubmitChanges();
                }
                return Json(new { success = true, message = "Xóa dữ liệu thành công." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}