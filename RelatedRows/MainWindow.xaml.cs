using RelatedRows.Domain;
using RelatedRows.Helpers;
using System.ComponentModel;
using System.Windows.Controls;

namespace RelatedRows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = WindowViewModel.GetViewModel(new SchedulerProvider(Dispatcher));
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            var windowsModel = DataContext as WindowViewModel;
            windowsModel?.OnWindowClosing();
        }

        private void OnPasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as CDatasource).password = (sender as PasswordBox).Password;
        }
    }
}
