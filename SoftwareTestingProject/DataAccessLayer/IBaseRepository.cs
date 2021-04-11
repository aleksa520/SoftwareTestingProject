using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTestingProject.DataAccessLayer
{
    interface IBaseRepository<T> where T : class
    {
        string Insert(T entity);
        string Update(T entity);
        string Delete(T entity);
        T SelectById(int? id);
        void Save();
        IEnumerable<T> SelectAll();
    }
}
