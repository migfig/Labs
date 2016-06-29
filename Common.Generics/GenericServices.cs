using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Generics
{
    public interface IGenericServices<T> where T : class
    {
        Task<IEnumerable<T>> GetItems();
        Task<bool> AddItem(T item);
        Task<bool> RemoveItem(string propertyName, object id);
        Task<bool> LoadItems();
        Task<bool> SaveItems();
    }

    public class GenericServices<T> : IGenericServices<T> where T : class
    {
        private readonly int _maxItems;
        private readonly string _path;
        private readonly string _pattern;

        private IList<T> _items;

        public GenericServices(int maxItems, string path, string pattern)
        {
            _maxItems = maxItems;
            _path = path;
            _pattern = pattern;
            _items = new List<T>();
        }

        public Task<IEnumerable<T>> GetItems()
        {
            return Task.FromResult(_items.Take(_maxItems));
        }

        public Task<bool> AddItem(T item)
        {
            _items.Add(item);
            return Task.FromResult(true);
        }

        public Task<bool> RemoveItem(string propertyName, object id)
        {
            var item = _items.FirstOrDefault(x =>
                x.GetType().GetRuntimeProperty(propertyName).GetValue(x).Equals(id));
            if (null != item)
            {
                _items.Remove(item);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public virtual Task<bool> LoadItems()
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> SaveItems()
        {
            return Task.FromResult(false);
        }
    }
}
