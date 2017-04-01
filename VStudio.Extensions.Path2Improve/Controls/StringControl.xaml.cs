using System.Windows;
using System.Windows.Controls;

namespace VStudio.Extensions.Path2Improve.Controls
{
    /// <summary>
    /// Interaction logic for StoryControl.xaml
    /// </summary>
    public partial class StringControl : UserControl
    {
        public StringControl()
        {
            InitializeComponent();
        }
                
        private void OnExpanderDetailsCollapsed(object sender, RoutedEventArgs e)
        {
            if (((Control)sender).Name.Equals(expanderString.Name))
            {
                this.Height = 25;
            }
            e.Handled = true;
        }

        private void OnExpanderDetailsExpanded(object sender, RoutedEventArgs e)
        {
            if (((Control)sender).Name.Equals(expanderString.Name))
            {
                this.Height = 500;
            }
            e.Handled = true;
        }        
    }
}
