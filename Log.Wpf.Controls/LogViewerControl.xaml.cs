//------------------------------------------------------------------------------
// <copyright file="LogViewerControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Log.Wpf.Controls
{
    using Common;
    using System.Configuration;
    using System.Windows.Controls;
    using ViewModels;
    /// <summary>
    /// Interaction logic for LogViewerControl.
    /// </summary>
    public partial class LogViewerControl : UserControl
    {
        public delegate void ViewCodeRequestHandler(object sender, ViewCodeArgs e);
        public event ViewCodeRequestHandler OnViewCodeRequest;
        /// <summary>
        /// Initializes a new instance of the <see cref="LogViewerControl"/> class.
        /// </summary>
        public LogViewerControl()
        {
            DataContext = LogViewModel.ViewModel;
            this.InitializeComponent();
        }

        private void OnComboBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                LogViewModel.ViewModel.SelectedFilter = (sender as ComboBox).Text;
            }
        }

        private void OnViewCodeCommand(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OnViewCodeRequest != null)
            {
                var entry = ((sender as Button).Tag as LogEntry);
                if (!string.IsNullOrWhiteSpace(entry.ClassName))
                {
                    OnViewCodeRequest(this, 
                        new ViewCodeArgs(ConfigurationManager.AppSettings["VisualStudio.ProgId"], 
                            entry.ClassName, 
                            entry.LineNumber));
                }
            }
        }
    }
}