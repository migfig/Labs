using System.ComponentModel.Composition.Hosting;
using System.Windows;
using System.IO;
using System.ComponentModel.Composition;

namespace Visor.VStudio.Controls
{
    /// <summary>
    /// Interaction logic for ParentControl.xaml
    /// </summary>
    public partial class ParentControl : PlugableWindow
    {
        public ParentControl()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            var folder = string.Empty;
            try
            {
                var assembly = GetType().Assembly;
                var catalog = new AggregateCatalog();
                catalog.Catalogs.Add(new AssemblyCatalog(assembly));
                folder = Path.Combine(Path.GetDirectoryName(assembly.Location), "extensions");
                catalog.Catalogs.Add(new DirectoryCatalog(folder));
                var container = new CompositionContainer(catalog);
                container.ComposeParts(this);
                Attach();
            }
            catch (CompositionException e)
            {
                MessageBox.Show(e.Message + " extensions folder: " + folder);
            }
        }
    }
}
