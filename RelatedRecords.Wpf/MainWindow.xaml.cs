using Fluent;
using RelatedRecords.Wpf.ViewModels;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace RelatedRecords.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainViewModel.ViewModel;
        }

        private void btnDrillDown_Click(object sender, RoutedEventArgs e)
        {
            var table = (sender as Fluent.Button).Tag as DatatableEx;
            if (null != table)
            {
                MainViewModel.ViewModel.TableNavigation.Push(MainViewModel.ViewModel.SelectedDataTable);
                MainViewModel.ViewModel.SelectedDataTable = table;
            }
        }

        private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            try {
                var cell = (sender as DataGrid).CurrentCell;
                var colName = cell.Column.Header.ToString();
                MainViewModel.ViewModel.SelectedColumn = colName;
                MainViewModel.ViewModel.SearchCriteria = (cell.Item as DataRowView)[colName].ToString();
            } catch(Exception) {; }
        }

        private void AddTableRelation_Click(object sender, RoutedEventArgs e)
        {
            var table = (sender as Fluent.Button).Tag as DatatableEx;
            if (null != table)
            {
                MainViewModel.ViewModel.SelectedParentTable = table.Root.ConfigTable;
                MainViewModel.ViewModel.AddTableRelationshipCommand.Execute(null);
            }
        }

        private void RemoveTableRelation_Click(object sender, RoutedEventArgs e)
        {
            var table = (sender as Fluent.Button).Tag as DatatableEx;
            if (null != table)
            {
                MainViewModel.ViewModel.SelectedChildTable = table.Root.ConfigTable;
                MainViewModel.ViewModel.RemoveTableRelationshipCommand.Execute(null);
            }
        }

        private void cmdReloadDataset_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.ViewModel.ReLoadDatasetCommand.Execute(null);
        }

        private void cmdRemoveDataset_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.ViewModel.RemoveDatasetCommand.Execute(null);
        }
    }
}
