using System.ComponentModel.Composition;
using System.Windows.Controls;
using Visor.VStudio;
using Visor.Wpf.TodoCoder.ViewModels;

namespace Trainer.Wpf.VStudio.Tools
{
    /// <summary>
    /// Interaction logic for AppAccelerator.xaml
    /// </summary>
    [Export(typeof(IChildWindow))]
    [ExportMetadata("Title", "App Accelerator")]
    public partial class AppAccelerator : UserControl, IChildWindow, ITitledWindow
    {
        public AppAccelerator()
        {
            DataContext = ComponentsViewModel.ViewModel;
            InitializeComponent();
        }

        private IPlugableWindow _parentWindow;
        public string Title { get { return "App Accelerator"; } }
        UserControl IChildWindow.Content { get { return this; } }

        public void SetParentWindow(IPlugableWindow window)
        {
            _parentWindow = window;
            ComponentsViewModel.ViewModel.ParentWindow = _parentWindow;
        }        
    }
}
