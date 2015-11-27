using RelatedRecords.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        }

        private void ComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && !string.IsNullOrWhiteSpace(MainViewModel.ViewModel.Command))
            {
                MainViewModel.ViewModel.ExecuteCommand();
                e.Handled = true;
            }
        }
    }
}
