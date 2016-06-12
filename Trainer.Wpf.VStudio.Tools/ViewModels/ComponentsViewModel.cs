using Common;
using System.Collections.ObjectModel;
using System.IO;
using Trainer.Domain;
using Trainer.Wpf.VStudio.Tools.Properties;
using System.Linq;

namespace Visor.Wpf.TodoCoder.ViewModels
{
    public class ComponentsViewModel: BaseModel
    {
        private static ComponentsViewModel _viewModel = new ComponentsViewModel();
        public static ComponentsViewModel ViewModel { get { return _viewModel; } }

        private ObservableCollection<Component> _items;
        public ObservableCollection<Component> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new ObservableCollection<Component>();
                    var files = Directory.GetFiles(Settings.Default.SourcePath, Settings.Default.FileMask);
                    foreach(var file in files)
                    {
                        var item = XmlHelper<Components>.Load(file);
                        if(null != item)
                        {
                            foreach(var i in item.Component)
                                _items.Add(i);
                        }
                    }
                }

                return _items;
            }
        }
    }
}
