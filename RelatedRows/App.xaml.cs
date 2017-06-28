using RelatedRows.Domain;
using System.Windows;

namespace RelatedRows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += (s, e) =>
            {
                Logger.Log.Error(e.Exception, s.ToString());
                e.Handled = true;
            };
        }
    }
}
