using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Trainer.Domain;

namespace Code.Service.ContentProviders
{
    public class FileSystemContentProvider : IContentProvider<Presentation>
    {
        public void DeleteContent(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Presentation>> GetAllContent(string path, string pattern)
        {
            var list = new List<Presentation>();
            try
            {
                var files = Directory.GetFiles(path, pattern);
                foreach (var file in files)
                {
                    var p = XmlHelper<Presentation>.Load(file);
                    if (p != null && p.Slide.Any())
                    {
                        list.Add(p);
                    }
                }
            }
            catch (Exception)
            {
            }

            return Task.FromResult(list.AsEnumerable());
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
