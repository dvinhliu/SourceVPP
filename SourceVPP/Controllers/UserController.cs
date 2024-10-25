﻿using FirebaseAdmin.Auth;
using Newtonsoft.Json;
using SourceVPP.Models;
using SourceVPP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SourceVPP.Controllers
{
    public class UserController : Controller
    {
        laptopDataContext db = new laptopDataContext();
        // GET: User
        public async Task<ActionResult> GetUser()
        {
            var token = Request.Headers["Authorization"]?.ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, message = "Token không hợp lệ." });
            }

            try
            {
                FirebaseToken decodedToken = await FirebaseService.VerifyTokenAsync(token);
                string uid = decodedToken.Uid;

                var user = db.users.SingleOrDefault(u => u.MaTaiKhoan == uid);
                if (user != null)
                {
                    return Json(new { success = true, user.MaTaiKhoan, user.TenTaiKhoan, user.ImageProfile }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Người dùng không tìm thấy." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (FirebaseAuthException authEx)
            {
                return Json(new { success = false, message = "Lỗi xác thực: " + authEx.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CheckUsernameExists(string username)
        {
            var userExists = db.users.Any(u => u.TenTaiKhoan == username);
            return Json(new { exists = userExists }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckEmailUserExists(string email)
        {
            var emailExists = db.users.Any(u => u.Email == email);
            return Json(new { exists = emailExists }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpPost]
        public async Task<ActionResult> CheckEmailVerified(string email)
        {
            try
            {
                var userRecord = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
                return Json(new { emailVerified = userRecord.EmailVerified });
            }
            catch (FirebaseAuthException ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public ActionResult SaveUserData(user userData)
        {
            try
            {
                db.users.InsertOnSubmit(userData);
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
    public class TokenRequest
    {
        public string RefreshToken { get; set; }
    }
}