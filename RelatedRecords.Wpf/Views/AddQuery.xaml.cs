using System;
using System.Windows;
using WPFSpark;
using System.Collections.ObjectModel;
using RelatedRecords.Wpf.ViewModels;

namespace RelatedRecords.Wpf
{
    /// <summary>
    /// Interaction logic for AddQuery.xaml
    /// </summary>
    public partial class AddQuery : SparkWindow
    {
        public AddQuery()
        {
            InitializeComponent();
            DataContext = MainViewModel.ViewModel;
        }
    }
}
