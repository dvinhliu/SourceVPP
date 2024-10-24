﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using SourceVPP.Models;

namespace SourceVPP.Controllers
{
    public class ProfileController : Controller
    {
        laptopDataContext db = new laptopDataContext();
        // GET: Profile
        public ActionResult Index(string view, string MaTaiKhoan)
        {
            ViewBag.PartialView = string.IsNullOrEmpty(view) ? "ProfilePartial" : view;
            ViewBag.MaTaiKhoan = MaTaiKhoan;

            if (!string.IsNullOrEmpty(MaTaiKhoan))
            {
                var dbUser = db.users.FirstOrDefault(u => u.MaTaiKhoan == MaTaiKhoan);
                if (dbUser != null)
                {
                    return View(dbUser); // Trả về view với đối tượng người dùng
                }
                else
                {
                    // Trường hợp không tìm thấy tài khoản
                    return View("Error", new { message = "Không tìm thấy tài khoản" });
                }
            }

            return View();
        }

        public ActionResult ProfilePartial(string MaTaiKhoan)
        {
            if (!string.IsNullOrEmpty(MaTaiKhoan))
            {
                var dbUser = db.users.FirstOrDefault(u => u.MaTaiKhoan == MaTaiKhoan);

                if (dbUser != null)
                {
                    return PartialView(dbUser); // Truyền thông tin user từ database vào view
                }
            }

            return PartialView();
        }

        public ActionResult AddressPartial(string MaTaiKhoan) // Thêm tham số MaTaiKhoan
        {
            if (!string.IsNullOrEmpty(MaTaiKhoan))
            {
                var dbUser = db.users.Include("addresses").FirstOrDefault(u => u.MaTaiKhoan == MaTaiKhoan);
                var adrs = db.addresses.Where(a => a.MaTaiKhoan == MaTaiKhoan).ToList();

                if (dbUser != null)
                {
                    return PartialView(adrs); // Truyền thông tin user từ database vào view
                }
            }

            return PartialView();
        }

        public ActionResult ChangePasswordPartial(string MaTaiKhoan) // Thêm tham số MaTaiKhoan
        {
            if (!string.IsNullOrEmpty(MaTaiKhoan))
            {
                var dbUser = db.users.FirstOrDefault(u => u.MaTaiKhoan == MaTaiKhoan);

                if (dbUser != null)
                {
                    return PartialView(dbUser); // Truyền thông tin user từ database vào view
                }
            }

            return PartialView();
        }

        public ActionResult OrderPartial(string MaTaiKhoan) // Thêm tham số MaTaiKhoan
        {
            if (!string.IsNullOrEmpty(MaTaiKhoan))
            {
                var dbUser = db.users.FirstOrDefault(u => u.MaTaiKhoan == MaTaiKhoan);

                if (dbUser != null)
                {
                    return PartialView(dbUser); // Truyền thông tin user từ database vào view
                }
            }

            return PartialView();
        }

        [HttpPost]
        public ActionResult UpdateProfile(user updateUser)
        {
            try
            {
                var currentUser = db.users.FirstOrDefault(u => u.MaTaiKhoan == updateUser.MaTaiKhoan);
                if (currentUser != null)
                {
                    currentUser.TenKhachHang = updateUser.TenKhachHang;
                    currentUser.Gender = updateUser.Gender;
                    currentUser.DateOfBirth = updateUser.DateOfBirth;
                    currentUser.ImageProfile = updateUser.ImageProfile;

                    db.SubmitChanges();
                }
                return RedirectToAction("Index", new { view = "ProfilePartial", MaTaiKhoan = updateUser.MaTaiKhoan });
            }
            catch (Exception ex)
            {
                return View("Error", new { message = ex.Message });
            }
        }
    }
}