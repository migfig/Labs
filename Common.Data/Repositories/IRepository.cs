using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public interface IRepository<T> where T: class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        T Add(T item);
        T Update(T item);
        T Delete(T item);
        bool Load();
        bool Save();
    }
}
