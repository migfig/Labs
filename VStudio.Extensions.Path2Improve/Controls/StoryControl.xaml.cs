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
    public partial class StoryControl : UserControl
    {
        public StoryControl()
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
                this.Width = 490;
                this.Height = 25;
            }
            e.Handled = true;
        }

        private void OnExpanderDetailsExpanded(object sender, RoutedEventArgs e)
        {
            var expander = sender as Expander;
            if (expander.Name.Equals(expanderStory.Name))
            {
                this.Width = 490;
                this.Height = 500;
            }
            if (MainViewModel.ViewModel.SelectedStory == null)
                MainViewModel.ViewModel.SelectedStory = expander.DataContext as Story;

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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Common.Extensions.runProcess(@"C:\Program Files\Internet Explorer\iexplore.exe", (sender as Button).Tag.ToString());
        }
    }
}
