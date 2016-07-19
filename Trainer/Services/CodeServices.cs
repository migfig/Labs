using System.Collections.Generic;
using Trainer.Domain;
using Log.Common.Services;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;

namespace Trainer.ViewModels
{
    public interface ICodeServices
    {
        Task<bool> CopyCode(IEnumerable<Component> components);
        Task<bool> ViewCode(IEnumerable<Component> components);
    }

    public class CodeServices: ICodeServices
    {
        public async Task<bool> CopyCode(IEnumerable<Component> components)
        {
            var list = new Components { Component = new ObservableCollection<Component>(components.Where(x => x.Action.Equals(ComponentAction.Copy))) };

            using (var service = ApiServiceFactory.CreateService<Components>(Services.SettingsServices.SettingsService.Instance.CodeServicesUrl))
            {
                return await service.AddItem(list);
            }
        }

        public async Task<bool> ViewCode(IEnumerable<Component> components)
        {
            var list = new Components { Component = new ObservableCollection<Component>(components.Where(x => x.Action.Equals(ComponentAction.View))) };

            using (var service = ApiServiceFactory.CreateService<Components>(Services.SettingsServices.SettingsService.Instance.CodeServicesUrl))
            {
                return await service.AddItem(list);
            }
        }
    }
}