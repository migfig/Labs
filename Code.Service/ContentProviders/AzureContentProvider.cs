using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainer.Domain;

namespace Code.Service.ContentProviders
{
    public class AzureContentProvider : IContentProvider<Presentation>
    {
        public void DeleteContent(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Presentation>> GetAllContent(string path, string pattern)
        {
            throw new NotImplementedException();
        }

        public Task<Presentation> GetContentById(object id)
        {
            throw new NotImplementedException();
        }

        public void UpdateContent(Presentation item)
        {
            throw new NotImplementedException();
        }
    }
}
