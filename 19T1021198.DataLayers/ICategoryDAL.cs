using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _19T1021198.DomainModels;

namespace _19T1021198.DataLayers
{
    interface ICategoryDAL
    {
        int Add(Category data);
        int Count(string searchValue = "");
        bool Delete(int id);
        Category Get(int id);
        bool InUsed(int id);
        IList<Category> List(int page = 1, int pageSize = 0, string searchValue = "");
        bool Update(Category data);
    }
}
