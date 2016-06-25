using System.Collections.Generic;
using Trainer.Domain;
using Log.Common.Services;
using System.Threading.Tasks;

namespace Trainer.ViewModels
{
    public interface ICodeServices
    {
        Task<bool> CopyCode(IEnumerable<Domain.Component> components);
    }

    public class CodeServices: ICodeServices
    {
        public async Task<bool> CopyCode(IEnumerable<Domain.Component> components)
        {
            var list = new Components();
            foreach(var c in components)
            {
                list.Component.Add(c);
            }

            using (var service = ApiServiceFactory.CreateService<Domain.Components>(useJson: false))
            {
                return await service.AddItem(list);
            }
        }
    }
}