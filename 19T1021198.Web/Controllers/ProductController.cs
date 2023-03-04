using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _19T1021198.DomainModels;
using _19T1021198.BusinessLayers;

namespace _19T1021198.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("product")]
    public class ProductController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private const int PAGE_SIZE = 5;
        private const string PRODUCT_SEARCH = "ProductCondition";
        public ActionResult Index()
        {
            Models.PaginationSearchInput condition = Session[PRODUCT_SEARCH] as Models.PaginationSearchInput;

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
            var data = CommonDataService.ListOfProducts(condition.Page,
                                                        condition.PageSize,
                                                        condition.SearchValue,
                                                        out rowCount);
            Models.ProductSearchOutput result = new Models.ProductSearchOutput()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                RowCount = rowCount,
                Data = data,
            };

            Session[PRODUCT_SEARCH] = condition;

            return View(result);
        }

        /// <summary>
        /// Tạo mặt hàng mới
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var data = new Product()
            {
                ProductID = 0,
            };

            ViewBag.Title = "Bổ sung mặt hàng";
            return View(data);
        }

        /// <summary>
        /// Cập nhật thông tin mặt hàng, 
        /// Hiển thị danh sách ảnh và thuộc tính của mặt hàng, điều hướng đến các chức năng
        /// quản lý ảnh và thuộc tính của mặt hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public ActionResult Edit(int id = 0)
        {
            if (id <= 0)
                return RedirectToAction("Index");

            var data = CommonDataService.GetProduct(id);
            if (data == null)
                return RedirectToAction("Index");

            ViewBag.ListOfPhotos = ProductDataService.ListPhotos(data.ProductID);
            ViewBag.ListOfAttributes = ProductDataService.ListAttributes(data.ProductID);


            ViewBag.Title = "Cập nhật mặt hàng";
            return View(data);
        }

        /// <summary>
        /// Lưu thông tin mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <param name="uploadPhoto"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]  // Kiểm tra Token không hợp lệ
        [HttpPost]  // Chỉ nhận phương thức post
        public ActionResult Save(Product data, HttpPostedFileBase uploadPhoto)
        {
            // Kiểm soát dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(data.ProductName))
                ModelState.AddModelError(nameof(data.ProductName), "Tên Mặt hàng không được để trống!");
            if (string.IsNullOrEmpty(data.Unit))
                ModelState.AddModelError(nameof(data.Unit), "Đơn vị tính không được để trống!");

            if (ModelState.IsValid == false)    // Kiểm tra dữ liệu đầu vào có hợp lệ hay không
            {
                ViewBag.Title = data.ProductID == 0 ? "Bổ sung Mặt hàng" : "Cập nhật Mặt hàng";
                return View("Edit", data);
            }

            if (uploadPhoto != null)
            {
                string path = Server.MapPath("~/Images/Products");
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string filePath = System.IO.Path.Combine(path, fileName);
                uploadPhoto.SaveAs(filePath);
                data.Photo = $"Images/Products/{fileName}";
            }

            if (data.ProductID == 0)
            {
                CommonDataService.AddProduct(data);
            }
            else
            {
                CommonDataService.UpdateProduct(data);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Xóa mặt hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public ActionResult Delete(int id = 0)
        {
            if (id <= 0)
                return RedirectToAction("Index");

            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }

            var data = CommonDataService.GetProduct(id);
            if (data == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Xóa Mặt hàng";
            return View(data);
        }

        /// <summary>
        /// Các chức năng quản lý ảnh của mặt hàng
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="photoID"></param>
        /// <returns></returns>
        [Route("photo/{method?}/{productID?}/{photoID?}")]
        public ActionResult Photo(string method = "add", int productID = 0, long photoID = 0)
        {
            switch (method)
            {
                case "add":
                    var data = new ProductPhoto()
                    {
                        PhotoID = 0,
                        ProductID = productID
                    };
                    ViewBag.Title = "Bổ sung ảnh";
                    return View(data);
                case "edit":
                    if (photoID <= 0)
                        return RedirectToAction("Index");
                    data = ProductDataService.GetPhoto(photoID);
                    if (data == null)
                        return RedirectToAction("Index");
                    ViewBag.Title = "Thay đổi ảnh";
                    return View(data);
                case "delete":
                    ProductDataService.DeletePhoto(photoID);
                    return RedirectToAction($"Edit/{productID}"); //return RedirectToAction("Edit", new { productID = productID });
                default:
                    return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Lưu thông tin Ảnh trong Mặt hàng
        /// </summary>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SavePhoto(ProductPhoto data, HttpPostedFileBase uploadPhoto, string isHidden)
        {
            data.Photo = data.Photo ?? "https://www.w3schools.com/bootstrap4/img_avatar1.png";
            data.IsHidden = false;
            if (isHidden == "on")
                data.IsHidden = true;

            if (uploadPhoto != null)
            {
                string path = Server.MapPath("~/Images/Products/Photos");
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string filePath = System.IO.Path.Combine(path, fileName);
                uploadPhoto.SaveAs(filePath);
                data.Photo = $"Images/Products/Photos/{fileName}";
            }

            //
            if (data.PhotoID == 0)
                ProductDataService.AddPhoto(data);
            else
                ProductDataService.UpdatePhoto(data);

            return RedirectToAction($"Edit/{data.ProductID}");
        }

        /// <summary>
        /// Các chức năng quản lý thuộc tính của mặt hàng
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        [Route("attribute/{method?}/{productID}/{attributeID?}")]
        public ActionResult Attribute(string method = "add", int productID = 0, long attributeID = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính";
                    return View();
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính";
                    return View();
                case "delete":
                    //ProductDataService.DeleteAttribute(attributeID);
                    return RedirectToAction($"Edit/{productID}"); //return RedirectToAction("Edit", new { productID = productID });
                default:
                    return RedirectToAction("Index");
            }
        }
    }
}