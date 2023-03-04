using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _19T1021198.DomainModels;

namespace _19T1021198.DataLayers
{
    public interface ISupplierDAL
    {
        /// <summary>
        /// Lấy danh sách Nhà cung cấp
        /// </summary>
        IList<Supplier> List();
        int Add(Supplier data);
        int Count(string searchValue);
        bool Delete(int id);
        Supplier Get(int id);
        bool InUsed(int id);
        //IList<Supplier> List(int page, int pageSize, string searchValue);
        bool Update(Supplier data);

    }
}
