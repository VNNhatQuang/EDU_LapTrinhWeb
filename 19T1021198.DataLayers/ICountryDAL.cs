using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _19T1021198.DomainModels;

namespace _19T1021198.DataLayers
{
    /// <summary>
    /// Định nghĩa phép xử lý dữ liệu liên quan đến Quốc gia
    /// </summary>
    public interface ICountryDAL
    {
        /// <summary>
        /// Lấy danh sách Quốc gia
        /// </summary>
        IList<Country> List();
    }
}
