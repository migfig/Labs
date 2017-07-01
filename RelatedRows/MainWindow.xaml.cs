using RelatedRows.Domain;
using RelatedRows.Helpers;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;

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
            
            Loaded += (s, e) => {
                (DataContext as WindowViewModel).WindowSize = RenderSize;
            };
            Closing += (s, e) => {
                var windowsModel = DataContext as WindowViewModel;
                windowsModel?.OnWindowClosing();
            };
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            (DataContext as WindowViewModel).WindowSize = sizeInfo.NewSize;
        }

        private void OnPasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as CDatasource).password = (sender as PasswordBox).Password;
        }
    }
}
