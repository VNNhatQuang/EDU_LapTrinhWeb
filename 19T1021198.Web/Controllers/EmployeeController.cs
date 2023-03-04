using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _19T1021198.DomainModels;
using _19T1021198.BusinessLayers;

namespace _19T1021198.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private const int PAGE_SIZE = 5;
        private const string EMPLOYEE_SEARCH = "EmployeeCondition";
        // GET: Employee
        public ActionResult Index()
        {
            Models.PaginationSearchInput condition = Session[EMPLOYEE_SEARCH] as Models.PaginationSearchInput;

            if (condition == null)
            {
                condition = new Models.PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                };
            }

            return View(condition);
        }

        public ActionResult Search(Models.PaginationSearchInput condition)  // (int Page, int PageSize, string SearchValue)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfEmployees(condition.Page,
                                                        condition.PageSize,
                                                        condition.SearchValue,
                                                        out rowCount);
            Models.EmployeeSearchOutput result = new Models.EmployeeSearchOutput()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                RowCount = rowCount,
                Data = data,
            };

            Session[EMPLOYEE_SEARCH] = condition;

            return View(result);
        }

        /// <summary>
        /// Thêm Nhân viên
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var data = new Employee()
            {
                EmployeeID = 0,
            };

            ViewBag.Title = "Thêm Nhân viên";
            return View("Edit", data);
        }
        /// <summary>
        /// Sửa Nhân viên
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            if (id <= 0)
                return RedirectToAction("Index");

            var data = CommonDataService.GetEmployee(id);
            if (data == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Sửa Nhân viên";
            return View(data);
        }

        // Attribute
        [ValidateAntiForgeryToken]  // Kiểm tra Token không hợp lệ
        [HttpPost]  // Chỉ nhận phương thức post
        public ActionResult Save(Employee data, string birthday, HttpPostedFileBase uploadPhoto)
        {
            DateTime? d = Converter.DMYStringToDateTime(birthday);
            if (d == null)
                ModelState.AddModelError("Birthdate", "*");
            else
                data.BirthDate = d.Value;

            //// check
            if (string.IsNullOrWhiteSpace(data.FirstName))
                ModelState.AddModelError(nameof(data.FirstName), "Họ đệm không được để trống!");
            if (string.IsNullOrWhiteSpace(data.LastName))
                ModelState.AddModelError(nameof(data.LastName), "Tên không được để trống!");
            if (string.IsNullOrWhiteSpace(data.Email))
                ModelState.AddModelError(nameof(data.Email), "Email không được để trống!");
            data.Photo = data.Photo ?? "https://www.w3schools.com/bootstrap4/img_avatar1.png";
            data.Notes = data.Notes ?? "";

            if (ModelState.IsValid == false)    // Kiểm tra dữ liệu đầu vào có hợp lệ hay không
            {
                ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung Nhân viên" : "Cập nhật Nhân viên";
                return View("Edit", data);
            }

            if (uploadPhoto != null)
            {
                string path = Server.MapPath("~/Images/Employees");
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string filePath = System.IO.Path.Combine(path, fileName);
                uploadPhoto.SaveAs(filePath);
                data.Photo = $"Images/Employees/{fileName}";
            }

            //
            if (data.EmployeeID == 0)
            {
                CommonDataService.AddEmployee(data);
            }
            else
            {
                CommonDataService.UpdateEmployee(data);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Xóa Nhân viên
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(int id = 0)
        {
            if (id <= 0)
                return RedirectToAction("Index");

            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }

            var data = CommonDataService.GetEmployee(id);
            if (data == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Xóa Nhân viên";
            return View(data);
        }
    }
}