using System.Windows;
using System.Windows.Controls;
using VStudio.Extensions.Path2Improve.ViewModels;

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
                //var item = ((Button)sender).Tag as StringValue;
                //switch(item.Type)
                //{
                //    case "Url":
                //        break;
                //    case "Step":
                //        break;
                //    case "Question":
                //        break;
                //    case "Attachment":
                //        break;
                //    case "":
                //        break;
                //    case "":
                //        break;
                //}
            }
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = (((Button)sender).Tag as StringValue).Type;
            if (MainViewModel.ViewModel.AddActionCommand.CanExecute(item))
            {
                MainViewModel.ViewModel.AddActionCommand.Execute(item);
            }
        }

        private void Button_RemoveItemClick(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).Tag;
            if (MessageBox.Show("Are you sure you want to remove this " + item.GetType().Name + "?", "Remove Item", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (MainViewModel.ViewModel.RemoveActionCommand.CanExecute(item))
                {
                    MainViewModel.ViewModel.RemoveActionCommand.Execute(item);
                }
            }
        }
    }
}
