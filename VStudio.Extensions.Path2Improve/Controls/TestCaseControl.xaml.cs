using System.Windows;
using System.Windows.Controls;
using VStudio.Extensions.Path2Improve.ViewModels;

namespace VStudio.Extensions.Path2Improve.Controls
{
    /// <summary>
    /// Interaction logic for StoryControl.xaml
    /// </summary>
    public partial class TestCaseControl : UserControl
    {
        public TestCaseControl()
        {
            InitializeComponent();
        }       

        private void OnExpanderDetailsCollapsed(object sender, RoutedEventArgs e)
        {
            if (((Control)sender).Name.Equals(expanderTestcase.Name))
            {
                this.Height = 25;
            }
            e.Handled = true;
        }

        private void OnExpanderDetailsExpanded(object sender, RoutedEventArgs e)
        {
            var expander = sender as Expander;
            if (expander.Name.Equals(expanderTestcase.Name))
            {
                this.Height = 500;
            }
            if (MainViewModel.ViewModel.SelectedTestcase == null)
                MainViewModel.ViewModel.SelectedTestcase = expander.DataContext as Testcase;

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
