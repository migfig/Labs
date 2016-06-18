using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Trainer.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Trainer.Controls
{
    public sealed partial class SlideControl : UserControl
    {
        public SlideControl()
        {            
            this.InitializeComponent();
            var vm = new SlidePageViewModel();
            if (vm.Presentation != null)
            {
                icContainer.ItemsSource = vm.CurrentSlide.Block.Select(x => x.Control);
            }
        }
    }
}
