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
    public class ShipperController : Controller
    {
        private const int PAGE_SIZE = 5;
        private const string SHIPPER_SERACH = "ShipperCondition";
        // GET: Shipper
        public ActionResult Index()
        {
            Models.PaginationSearchInput condition = Session[SHIPPER_SERACH] as Models.PaginationSearchInput;

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
            var data = CommonDataService.ListOfShippers(condition.Page,
                                                        condition.PageSize,
                                                        condition.SearchValue,
                                                        out rowCount);
            Models.ShipperSearchOutput result = new Models.ShipperSearchOutput()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                RowCount = rowCount,
                Data = data,
            };

            Session[SHIPPER_SERACH] = condition;

            return View(result);
        }

        /// <summary>
        /// Thêm Người giao hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var data = new Shipper()
            {
                ShipperID = 0,
            };

            ViewBag.Title = "Thêm Người giao hàng";
            return View("Edit", data);
        }
        /// <summary>
        /// Sửa Người giao hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            if (id <= 0)
                return RedirectToAction("Index");

            var data = CommonDataService.GetShipper(id);
            if (data == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Sửa Người giao hàng";
            return View(data);
        }

        // Attribute
        [ValidateAntiForgeryToken]  // Kiểm tra Token không hợp lệ
        [HttpPost]  // Chỉ nhận phương thức post
        public ActionResult Save(Shipper data)
        {
            // Kiểm soát dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(data.ShipperName))
                ModelState.AddModelError(nameof(data.ShipperName), "Tên không được để trống!");
            if (string.IsNullOrWhiteSpace(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không được để trống!");

            if (ModelState.IsValid == false)    // Kiểm tra dữ liệu đầu vào có hợp lệ hay không
            {
                ViewBag.Title = data.ShipperID == 0 ? "Bổ sung Người giao hàng" : "Cập nhật Người giao hàng";
                return View("Edit", data);
            }

            //
            if (data.ShipperID == 0)
            {
                CommonDataService.AddShipper(data);
            }
            else
            {
                CommonDataService.UpdateShipper(data);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Xóa Người giao hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(int id = 0)
        {
            if (id <= 0)
                return RedirectToAction("Index");

            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteShipper(id);
                return RedirectToAction("Index");
            }

            var data = CommonDataService.GetShipper(id);
            if (data == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Xóa Người giao hàng";
            return View(data);
        }
    }
}