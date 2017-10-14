using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace RelatedRows.Domain
{
    public interface IConnectable
    {
        IConnectable Get();
    }

    public interface IPlugable
    {
        void OnPlugedIn();
    }

    [Export(typeof(IPlugable))]
    public abstract class PlugableExtension: IPlugable
    {
        [ImportMany]
        IEnumerable<Lazy<IConnectable>> plugins;

        private IList<IConnectable> _plugins = Enumerable.Empty<IConnectable>().ToList();
        public IEnumerable<IConnectable> Plugins { get { return _plugins; } }

        public abstract string Load();

        public void OnPlugedIn()
        {
            foreach(Lazy<IConnectable> plugin in plugins)
            {
                _plugins.Add(plugin.Value.Get());
            }
        }
    }

    public class PlugableProviders : PlugableExtension
    {
        public override string Load()
        {
            var error = string.Empty;
            try
            {
                var assembly = GetType().Assembly;
                var catalog = new AggregateCatalog();
                catalog.Catalogs.Add(new AssemblyCatalog(assembly));

                var folder = Path.Combine(Path.GetDirectoryName(assembly.Location), "extensions");
                if (Directory.Exists(folder))
                {
                    catalog.Catalogs.Add(new DirectoryCatalog(folder));
                    var container = new CompositionContainer(catalog);
                    container.ComposeParts(this);
                    OnPlugedIn();
                    return string.Empty;
                }
                error = string.Format("Folder '{0}' not found. Trying to load extensions.", folder);
            }
            catch (CompositionException e)
            {
                error = e.Message + ". Trying to load extensions.";
            }
            catch (Exception e)
            {
                error = e.Message + ". Trying to load extensions.";
            }

            return error;
        }
    }
}
