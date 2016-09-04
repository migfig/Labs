using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Trainer.Domain;

namespace Code.Service.ContentProviders
{
    public abstract class CloudProvider
    {
        /// <summary>
        /// needed to start cloud collections based in local storage
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public virtual IEnumerable<Presentation> GetAllLocalContent(string path, string pattern)
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

            return list;
        }
    }
}
