using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _19T1021198.BusinessLayers;
using _19T1021198.DomainModels;
using System.Web.Mvc;

namespace _19T1021198.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class SelectListHelper
    {
        /// <summary>
        /// Danh sách Quốc gia
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Countries()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "---Chọn Quốc gia---",
            });

            foreach (var item in CommonDataService.ListOfCountries())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.CountryName,
                    Text = item.CountryName,
                });
            }

            return list;
        }

        /// <summary>
        /// Danh sách Loại hàng
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Categories()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "---Chọn Loại hàng---",
            });

            foreach (var item in CommonDataService.ListOfCategories())
            {
                list.Add(new SelectListItem()
                {
                    Value = Convert.ToString(item.CategoryID),
                    Text = item.CategoryName,
                });
            }

            return list;
        }

        /// <summary>
        /// Danh sách Nhà cung cấp
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Suppliers()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "---Chọn Nhà cung cấp---",
            });

            foreach (var item in CommonDataService.ListOfSuppliers())
            {
                list.Add(new SelectListItem()
                {
                    Value = Convert.ToString(item.SupplierID),
                    Text = item.SupplierName,
                });
            }

            return list;
        }

        public static List<SelectListItem> Status()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "---Trạng thái---",
            });

            foreach (var item in OrderService.ListOfStatus())
            {
                if (item.Description.Equals("Rejected"))
                    item.Description = "Đơn hàng bị từ chối";
                if (item.Description.Equals("Cancel"))
                    item.Description = "Đơn hàng bị hủy";
                if (item.Description.Equals("Init"))
                    item.Description = "Đơn hàng mới (chờ duyệt)";
                if (item.Description.Equals("Accepted"))
                    item.Description = "Đơn hàng đã duyệt (chờ chuyển hàng)";
                if (item.Description.Equals("Shipping"))
                    item.Description = "Đơn hàng đang được giao";
                if (item.Description.Equals("Finished"))
                    item.Description = "Đơn hàng đã hoàn tất thành công";

                list.Add(new SelectListItem()
                {
                    Value = Convert.ToString(item.Status),
                    Text = item.Description,
                });
            }

            return list;
        }

    }
}