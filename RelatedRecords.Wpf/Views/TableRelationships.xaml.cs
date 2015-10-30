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
    public partial class TableRelationships : SparkWindow
    {
        public TableRelationships()
        {
            InitializeComponent();
            DataContext = MainViewModel.ViewModel;
        }

        void OnNotify(string message)
        {
            customStatusBar.SetStatus(message, true);
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            MainViewModel.ViewModel.SaveDatasourceSchemaCommand.Execute(null);
            Close();
        }
    }
}
