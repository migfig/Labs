using System;
using System.Collections.Generic;
using System.Data;
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

namespace RelatedRecords.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainViewModel.Instance;
        }

        private void btnDrillDown_Click(object sender, RoutedEventArgs e)
        {
            var table = (sender as Button).Tag as DatatableEx;
            if (null != table)
            {
                MainViewModel.Instance.TableNavigation.Push(MainViewModel.Instance.SelectedDataTable);
                MainViewModel.Instance.SelectedDataTable = table;
            }
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.SelectedDataTable = MainViewModel.Instance.TableNavigation.Pop();
        }
    }
}
