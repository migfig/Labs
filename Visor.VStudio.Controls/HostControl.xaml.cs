﻿using System.ComponentModel.Composition.Hosting;
using System.Windows;
using System.IO;
using System.ComponentModel.Composition;

namespace Visor.VStudio.Controls
{
    /// <summary>
    /// Interaction logic for ParentControl.xaml
    /// </summary>
    public partial class HostControl : PlugableWindow
    {
        public HostControl()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            var folder = Properties.Settings.Default.ExtensionsDirectory;
            try
            {
                var assembly = GetType().Assembly;
                var catalog = new AggregateCatalog();
                catalog.Catalogs.Add(new AssemblyCatalog(assembly));

                if (!Directory.Exists(folder))
                {
                    folder = Path.Combine(Path.GetDirectoryName(assembly.Location), "extensions");
                }
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
