using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Visor.VStudio;
using Visor.Wpf.TodoCoder.ViewModels;

namespace Visor.Wpf.TodoCoder
{
    /// <summary>
    /// Interaction logic for CoderControl.xaml
    /// </summary>
    [Export(typeof(IChildWindow))]
    [ExportMetadata("Title", "Todo Coder")]
    public partial class CoderControl : UserControl, IChildWindow, ITitledWindow
    {
        public CoderControl()
        {
            DataContext = TodoViewModel.ViewModel;
            InitializeComponent();
        }

        public string Title { get { return "Todo Coder"; } }
        UserControl IChildWindow.Content { get { return this; } }
    }
}
