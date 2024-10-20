using FirebaseAdmin.Auth;
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
                    return Json(new { success = true, user }, JsonRequestBehavior.AllowGet);
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

        // POST: User/RefreshToken
        [System.Web.Http.HttpPost]
        public async Task<ActionResult> RefreshToken([FromBody] TokenRequest request)
        {
            // Kiểm tra refresh token và lấy ID token mới
            try
            {
                string newToken = await RefreshTokenAsync(request.RefreshToken);
                return Json(new { success = true, token = newToken });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task<string> RefreshTokenAsync(string refreshToken)
        {
            // Xác minh và làm mới token
            // Thực hiện yêu cầu đến Firebase Authentication để lấy token mới
            var auth = FirebaseAuth.DefaultInstance;
            var newToken = await auth.CreateCustomTokenAsync(refreshToken); // Đây chỉ là ví dụ, bạn cần điều chỉnh cho đúng
            return newToken;
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