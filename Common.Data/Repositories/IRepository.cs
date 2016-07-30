using System.Collections.Generic;

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
