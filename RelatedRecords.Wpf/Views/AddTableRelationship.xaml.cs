using System;
using System.Windows;
using WPFSpark;
using System.Collections.ObjectModel;
using RelatedRecords.Wpf.ViewModels;

namespace RelatedRecords.Wpf
{
    /// <summary>
    /// Interaction logic for TableRelationships.xaml
    /// </summary>
    public partial class AddTableRelationship : SparkWindow
    {
        public AddTableRelationship()
        {
            InitializeComponent();
            DataContext = MainViewModel.ViewModel;
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            MainViewModel.ViewModel.SaveRelationshipCommand.Execute(null);
            Close();
        }
    }
}
