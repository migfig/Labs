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
            var assembly = typeof(ParentControl).Assembly;
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(assembly));
            catalog.Catalogs.Add(new DirectoryCatalog(Path.Combine(Path.GetDirectoryName(assembly.Location), "extensions")));
            var container = new CompositionContainer(catalog);
            try
            {
                container.ComposeParts(this);
            } catch(CompositionException e)
            {
                MessageBox.Show(e.Message);
            }

            Attach();
        }
    }
}
