using System.Collections.Generic;
using Trainer.Domain;
using Log.Common.Services;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Trainer.ViewModels
{
    public interface ICodeServices
    {
        Task<bool> CopyCode(IEnumerable<Component> components);
    }

    public class CodeServices: ICodeServices
    {
        public async Task<bool> CopyCode(IEnumerable<Component> components)
        {
            var list = new Components { Component = new ObservableCollection<Component>( components )};

            using (var service = ApiServiceFactory.CreateService<Components>())
            {
                return await service.AddItem(list);
            }
        }
    }
}