using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trainer.Domain;
using Trainer.Wpf.VStudio.Tools.ViewModels;

namespace Trainer.Wpf.VStudio.Tools.Controls
{
    /// <summary>
    /// Interaction logic for ComponentsControl.xaml
    /// </summary>
    public partial class ComponentsControl : UserControl
    {
        public ComponentsControl()
        {
            InitializeComponent();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var component = (sender as System.Windows.Controls.Image).Tag as Component;
            if (ComponentsViewModel.ViewModel.AddCodeCommand.CanExecute(component))
            {
                ComponentsViewModel.ViewModel.AddCodeCommand.Execute(component);
            }
        }

        private void OnExpanderDetailsCollapsed(object sender, RoutedEventArgs e)
        {
            this.Width = 200;
            this.Height = 100;
        }

        private void OnExpanderDetailsExpanded(object sender, RoutedEventArgs e)
        {
            this.Width = 600;
            this.Height = 500;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var component = (sender as Button).Tag as Component;
            if (ComponentsViewModel.ViewModel.RemoveComponentCommand.CanExecute(component))
            {
                ComponentsViewModel.ViewModel.RemoveComponentCommand.Execute(component);
            }
        }
    }
}
