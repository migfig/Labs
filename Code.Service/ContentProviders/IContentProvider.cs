using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainer.Domain;

namespace Code.Service
{
    public interface IContentProvider<T> where T: class
    {
        Task<IEnumerable<T>> GetAllContent(string path, string pattern);
        Task<T> GetContentById(object id);
        void DeleteContent(object id);
        void UpdateContent(T item);
    }
}
