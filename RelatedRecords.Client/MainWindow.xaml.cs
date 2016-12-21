using RelatedRecords.Data.ViewModels;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RelatedRecords.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainViewModel.ViewModel;
        }

        private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                var cell = (sender as DataGrid).CurrentCell;
                var colName = cell.Column.Header.ToString();
                MainViewModel.ViewModel.CurrentColumnValue = colName;
                MainViewModel.ViewModel.CurrentColumnValue = (cell.Item as DataRowView)[colName].ToString();
            }
            catch (Exception) {; }
        }

        private void ComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && !string.IsNullOrWhiteSpace(MainViewModel.ViewModel.Command))
            {
                MainViewModel.ViewModel.ExecuteCommand();
            }
            e.Handled = true;
        }
    }
}
