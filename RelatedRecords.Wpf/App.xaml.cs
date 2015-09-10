using System.Windows;
using System.Windows.Threading;

namespace RelatedRecords.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += OnUnhandledException;
        }

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Common.Extensions.ErrorLog.Error(e.Exception, "Unhandled Exception");
        }
    }
}
