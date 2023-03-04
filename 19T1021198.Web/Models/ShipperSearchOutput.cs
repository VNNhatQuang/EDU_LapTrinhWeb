using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _19T1021198.DomainModels;

namespace _19T1021198.Web.Models
{
    /// <summary>
    /// Kết quả tìm kiếm phân trang đối với Người giao hàng
    /// </summary>
    public class ShipperSearchOutput : PaginationSearchOutput
    {
        public List<Shipper> Data { get; set; }
    }
}