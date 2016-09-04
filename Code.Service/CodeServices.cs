using Common.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainer.Domain;

namespace Code.Service
{
    public class CodeServices: ICodeServices
    {
        private int _maxItems;
        private Components _components;

        public CodeServices(int maxItems)
        {
            _maxItems = maxItems;
            _components = new Components();
        }

        public Task<IEnumerable<Component>> GetItems()
        {
            return Task.FromResult(_components.Component.Take(_maxItems));
        }

        public Task<bool> AddItems(IEnumerable<Component> items)
        {
            foreach (var item in items)
                _components.Component.Add(item);

            return Task.FromResult(true);
        }

        public Task<bool> RemoveItem(string id)
        {
            var component = _components.Component.FirstOrDefault(x => x.Id.Equals(id));
            if (null != component)
            {
                _components.Component.Remove(component);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
