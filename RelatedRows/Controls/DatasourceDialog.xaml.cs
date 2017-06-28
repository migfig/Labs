using RelatedRows.Domain;
using System.Windows;
using System.Windows.Controls;

namespace RelatedRows.Controls
{
    /// <summary>
    /// Interaction logic for SampleDialog.xaml
    /// </summary>
    public partial class DatasourceDialog : UserControl
    {
        public DatasourceDialog()
        {
            InitializeComponent();
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as CDatasource).password = (sender as PasswordBox).Password;
        }
    }
}
