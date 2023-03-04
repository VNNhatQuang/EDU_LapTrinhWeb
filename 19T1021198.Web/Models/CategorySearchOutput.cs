using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _19T1021198.DomainModels;

namespace _19T1021198.Web.Models
{
    /// <summary>
    /// Kết quả tìm kiếm phân trang đối với Loại hàng
    /// </summary>
    public class CategorySearchOutput : PaginationSearchOutput
    {
        public List<Category> Data { get; set; }
    }
}