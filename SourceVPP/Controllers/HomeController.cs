using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SourceVPP.Services;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;
using System.Web.Security;
using SourceVPP.Models;

namespace SourceVPP.Controllers
{
    public class HomeController : Controller
    {
        laptopDataContext db = new laptopDataContext();

        // GET: Home
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Products()
        {
            return View();
        }
        public ActionResult Contacts()
        {
            return View();
        }
        public ActionResult Introduce()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult SignUp()
        {
            return View();
        }
        public ActionResult SearchProduct()
        {
            return View();
        }

        public async Task<ActionResult> GoogleSignIn(string token)
        {
            try
            {
                FirebaseToken decodedToken = await FirebaseService.VerifyTokenAsync(token);
                string uid = decodedToken.Uid; // UID từ Firebase
                string email = decodedToken.Claims["email"].ToString();
                string displayName = decodedToken.Claims["name"].ToString();
                string userName = email.Substring(0, email.IndexOf('@'));
                // Kiểm tra hoặc tạo người dùng mới dựa trên email
                var user = db.users.SingleOrDefault(u => u.Email == email);
                if (user == null)
                {
                    user = new user
                    {
                        MaTaiKhoan = uid,
                        Email = email,
                        TenKhachHang = displayName,
                        TenTaiKhoan = userName
                    };
                    db.users.InsertOnSubmit(user);
                    db.SubmitChanges();
                }
                else
                {
                    user.TenKhachHang = displayName;
                    db.SubmitChanges();
                }

                return Json(new { success = true, token = token });
            }
            catch (FirebaseAuthException authEx)
            {
                return Json(new { success = false, message = "Lỗi xác thực: " + authEx.Message });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chung
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }
    }
}