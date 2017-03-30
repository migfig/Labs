using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trainer.Domain;
using VStudio.Extensions.Path2Improve.ViewModels;

namespace VStudio.Extensions.Path2Improve.Controls
{
    /// <summary>
    /// Interaction logic for StoryControl.xaml
    /// </summary>
    public partial class CheckupControl : UserControl
    {
        public CheckupControl()
        {
            InitializeComponent();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var component = (sender as System.Windows.Controls.Image).Tag as Component;
            if (MainViewModel.ViewModel.AddKeyIdentifierCommand.CanExecute(component))
            {
                MainViewModel.ViewModel.AddKeyIdentifierCommand.Execute(component);
            }
        }

        private void OnExpanderDetailsCollapsed(object sender, RoutedEventArgs e)
        {
            if (((Control)sender).Name.Equals(expanderStory.Name))
            {
                this.Height = 25;
            }
            e.Handled = true;
        }

        private void OnExpanderDetailsExpanded(object sender, RoutedEventArgs e)
        {
            if (((Control)sender).Name.Equals(expanderStory.Name))
            {
                this.Height = 500;
            }
            e.Handled = true;
        }        
    }
}
