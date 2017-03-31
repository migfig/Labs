using System.Windows;
using System.Windows.Controls;
using Trainer.Domain;
using VStudio.Extensions.Path2Improve.ViewModels;

namespace VStudio.Extensions.Path2Improve.Controls
{
    /// <summary>
    /// Interaction logic for StoryControl.xaml
    /// </summary>
    public partial class KeyIdentifierControl : UserControl
    {
        public KeyIdentifierControl()
        {
            InitializeComponent();
        }
        
        private void OnExpanderDetailsCollapsed(object sender, RoutedEventArgs e)
        {
            if (((Control)sender).Name.Equals(expanderKeyIdentifier.Name))
            {
                this.Height = 25;
            }
            e.Handled = true;
        }

        private void OnExpanderDetailsExpanded(object sender, RoutedEventArgs e)
        {
            var expander = sender as Expander;
            if (expander.Name.Equals(expanderKeyIdentifier.Name))
            {
                this.Height = 500;
            }
            if (MainViewModel.ViewModel.SelectedKeyidentifier == null)
                MainViewModel.ViewModel.SelectedKeyidentifier = expander.DataContext as Keyidentifier;

            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).Tag;
            if (MainViewModel.ViewModel.AddActionCommand.CanExecute(item))
            {
                MainViewModel.ViewModel.AddActionCommand.Execute(item);
            }
        }
    }
}
