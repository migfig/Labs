using System;
using System.Linq;
using System.Windows;
using WPFSpark;
using System.Collections.ObjectModel;
using ApiTester.Wpf.ViewModels;
using System.Windows.Controls;

namespace ApiTester.Wpf
{
    /// <summary>
    /// Interaction logic for LoadAssembly.xaml
    /// </summary>
    public partial class LoadAssembly : SparkWindow
    {
        public LoadAssembly()
        {
            InitializeComponent();
            DataContext = MainViewModel.ViewModel;
        }

        private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            MainViewModel.ViewModel.SelectedTypes = (sender as ListView).SelectedItems.OfType<Type>();
        }
    }
}
