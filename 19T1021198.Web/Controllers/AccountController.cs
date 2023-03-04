using _19T1021198.BusinessLayers;
using _19T1021198.DataLayers;
using _19T1021198.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace _19T1021198.Web.Controllers
{

    [Authorize] // Chỉ thị bắt buộc đăng nhập
    public class AccountController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Trang đăng nhập vào hệ thống
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]    // Cho phép không cần đăng nhập
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Xử lý đăng nhập
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]  // Ngăn chặn các cuộc tấn công giả mạo yêu cầu trên nhiều trang web
        [AllowAnonymous]    // Cho phép không cần đăng nhập
        [HttpPost]
        public ActionResult Login(string username="", string password = "")
        {
            // Kiểm tra tính hợp lệ của dữ liệu từ form
            ViewBag.UserName = username;
            if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Thông tin không đầy đủ!");
                return View();
            }

            // Kiểm tra đăng nhập
            var useAccount = UserAccountService.Authorize(AccountTypes.Employee, username, password);
            if(useAccount == null)
            {
                ModelState.AddModelError("", "Đăng nhập thất bại!");
                return View();
            }

            // Chuyển chuỗi thông tin đăng nhập thành json
            string cookieString = Newtonsoft.Json.JsonConvert.SerializeObject(useAccount);
            // Ghi cookie cho phiên đăng nhập
            FormsAuthentication.SetAuthCookie(cookieString, false);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Save(UserAccount data, string newPassword, string confirmNewPassword)
        {
            // Kiểm soát dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(data.Password))
                ModelState.AddModelError(nameof(data.Password), "Trường bắt buộc!");
            if (string.IsNullOrWhiteSpace(newPassword))
                ModelState.AddModelError(nameof(newPassword), "Trường bắt buộc!");
            if (string.IsNullOrWhiteSpace(confirmNewPassword))
                ModelState.AddModelError(nameof(confirmNewPassword), "Trường bắt buộc!");
            if (!newPassword.Equals(confirmNewPassword))
                ModelState.AddModelError(nameof(confirmNewPassword), "Mật khẩu xác nhận và Mật khẩu mới không trùng khớp!");

            if (ModelState.IsValid == false)    // Kiểm tra dữ liệu đầu vào có hợp lệ hay không
            {
                ModelState.AddModelError("", "Thay đổi thông tin thất bại!");
                return View("Index", data);
            }

            // Đổi mật khẩu
            var useAccount = UserAccountService.ChangePassword(AccountTypes.Employee, data.UserName, data.Password, newPassword);

            if (!useAccount)
            {
                ModelState.AddModelError("", "Thay đổi thông tin thất bại!");
                return View("Index", data);
            }
            else
            {
                ModelState.AddModelError("", "Cập nhật thông tin thành công!");
            }

            // Chuyển chuỗi thông tin đăng nhập thành json
            string cookieString = Newtonsoft.Json.JsonConvert.SerializeObject(new UserAccount
            {
                UserID = data.UserID,
                UserName = data.UserName,
                Photo = data.Photo,
                FullName = data.FullName,
                Email = data.Email,
                RoleNames = data.RoleNames,
                Password = newPassword
            });
            // Ghi cookie cho phiên đăng nhập
            FormsAuthentication.SetAuthCookie(cookieString, false);

            return View("Index", data);
        }

    }
}