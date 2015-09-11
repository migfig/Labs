using System;
using System.Windows;
using WPFSpark;
using System.Collections.ObjectModel;
using ApiTester.Wpf.ViewModels;

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
    }
}
