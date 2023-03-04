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
    public class CategoryController : Controller
    {
        private const int PAGE_SIZE = 5;
        private const string CATEGORY_SEARCH = "CategoryCondition";
        // GET: Category
        public ActionResult Index()
        {
            Models.PaginationSearchInput condition = Session[CATEGORY_SEARCH] as Models.PaginationSearchInput;

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
            var data = CommonDataService.ListOfCategories(condition.Page,
                                                        condition.PageSize,
                                                        condition.SearchValue,
                                                        out rowCount);
            Models.CategorySearchOutput result = new Models.CategorySearchOutput()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                RowCount = rowCount,
                Data = data,
            };

            Session[CATEGORY_SEARCH] = condition;

            return View(result);
        }

        /// <summary>
        /// Thêm Loại hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var data = new Category()
            {
                CategoryID = 0,
            };

            ViewBag.Title = "Thêm Loại hàng";
            return View("Edit", data);
        }
        /// <summary>
        /// Sửa Loại hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            if (id <= 0)
                return RedirectToAction("Index");

            var data = CommonDataService.GetCategory(id);
            if (data == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Sửa Loại hàng";
            return View(data);
        }

        // Attribute
        [ValidateAntiForgeryToken]  // Kiểm tra Token không hợp lệ
        [HttpPost]  // Chỉ nhận phương thức post
        public ActionResult Save(Category data)
        {
            // Kiểm soát dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(data.CategoryName))
                ModelState.AddModelError(nameof(data.CategoryName), "Tên Loại hàng không được để trống!");

            data.Description = data.Description ?? "";

            if (ModelState.IsValid == false)    // Kiểm tra dữ liệu đầu vào có hợp lệ hay không
            {
                ViewBag.Title = data.CategoryID == 0 ? "Bổ sung Loại hàng" : "Cập nhật Loại hàng";
                return View("Edit", data);
            }

            if (data.CategoryID == 0)
            {
                CommonDataService.AddCategory(data);
            }
            else
            {
                CommonDataService.UpdateCategory(data);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Xóa Loại hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(int id = 0)
        {
            if (id <= 0)
                return RedirectToAction("Index");

            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteCategory(id);
                return RedirectToAction("Index");
            }

            var data = CommonDataService.GetCategory(id);
            if (data == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Xóa Loại hàng";
            return View(data);
        }

    }
}