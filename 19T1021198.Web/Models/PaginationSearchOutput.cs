using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _19T1021198.Web.Models
{
    /// <summary>
    /// Lớp cơ sở dùng để biểu diễn kết quả tìm kiếm dưới dạng phân trang
    /// </summary>
    public abstract class PaginationSearchOutput
    {
        /// <summary>
        /// Trang đang được hiển thị
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Số dòng trên mỗi trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Giá trị tìm kiếm
        /// </summary>
        public string SearchValue { get; set; }
        /// <summary>
        /// Số dòng dữ liệu tìm được
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// Số trang 
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                    return 1;
                int p = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                    p += 1;
                return p;
            }
        }
    }
}