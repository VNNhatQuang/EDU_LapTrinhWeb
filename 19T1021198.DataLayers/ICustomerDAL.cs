using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _19T1021198.DomainModels;

namespace _19T1021198.DataLayers
{
    interface ICustomerDAL
    {
        int Add(Customer data);
        int Count(string searchValue);
        bool Delete(int id);
        Customer Get(int id);
        bool InUsed(int id);
        IList<Customer> List(int page, int pageSize, string searchValue);
        bool Update(Customer data);

    }
}
