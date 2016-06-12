using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trainer.Domain;
using Visor.Wpf.TodoCoder.ViewModels;

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
            //var component = (sender as Image).Tag as Components;
            //if (ComponentsViewModel.ViewModel.AddComponent.CanExecute(null))
            //{
            //    ComponentsViewModel.ViewModel.AddComponent.Execute(component);
            //}
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
    }
}
