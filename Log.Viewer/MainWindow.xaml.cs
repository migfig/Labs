using System.Windows;

namespace Log.Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            logViewerControl.OnViewCodeRequest += LogViewerControlOnViewCodeRequest;
        }

        private void LogViewerControlOnViewCodeRequest(object sender, Wpf.Controls.ViewModels.ViewCodeArgs e)
        {
            MessageBox.Show(string.Format("View code request:\n{0}.{1}\n@ Line number {2}", e.NameSpace, e.ClassName, e.LineNumber), sender.ToString());
        }
    }
}
