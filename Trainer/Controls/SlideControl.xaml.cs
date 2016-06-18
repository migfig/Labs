using System.Linq;
using Trainer.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
                var controls = vm.CurrentSlide.Block.Select(x => x.Control);
                var i = 0;
                for (i = 0; i < controls.Count(); i++)
                {
                    var c = controls.ElementAt(i);
                    gdContainer.RowDefinitions.Add(new RowDefinition
                    {
                        //Height = new GridLength(c.Height*c.Blocks.Count())
                        Height = GridLength.Auto
                    });
                }

                i = 0;
                foreach (var c in controls)
                {
                    gdContainer.Children.Add(c);
                    Grid.SetRow(c, i++);
                }
            }
        }
    }
}
