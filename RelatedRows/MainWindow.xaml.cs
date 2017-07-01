using RelatedRows.Domain;
using RelatedRows.Helpers;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System;

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

        private void OnNavigateUri(object sender, RoutedEventArgs e)
        {
            (DataContext as WindowViewModel).ShowInGitHubCommand.Execute((sender as Hyperlink).NavigateUri);
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var rtb = sender as RichTextBox;
            var ptr = rtb.GetPositionFromPoint(e.GetPosition(rtb), true);
            if (null != ptr)
            {
                var url = ptr.GetTextInRun(LogicalDirection.Backward);
                url += ptr.GetTextInRun(LogicalDirection.Forward);
                (DataContext as WindowViewModel).ShowInGitHubCommand.Execute(new Uri(url));
            }
            e.Handled = true;
        }
    }
}
