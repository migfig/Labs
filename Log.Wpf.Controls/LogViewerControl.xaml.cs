//------------------------------------------------------------------------------
// <copyright file="LogViewerControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Log.Wpf.Controls
{
    using Common;
    using Properties;
    using System.Windows.Controls;
    using ViewModels;
    using Visor.VStudio;
    using System.ComponentModel.Composition;
    using System.Windows;
    
    /// <summary>
    /// Interaction logic for LogViewerControl.
    /// </summary>
    [Export(typeof(IChildWindow))]
    [ExportMetadata("Title", "Log Viewer")]
    public partial class LogViewerControl : UserControl, IChildWindow, ITitledWindow
    {
        private IPlugableWindow _parentWindow;
        public string Title { get { return "Log Viewer"; } }
        UserControl IChildWindow.Content { get { return this; } }

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

        private void OnViewCodeCommand(object sender, RoutedEventArgs e)
        {
            var entry = ((sender as Button).Tag as LogEntry);
            if (!string.IsNullOrWhiteSpace(entry.ClassName))
            {
                if (null != _parentWindow)
                {
                    var errorMsg = _parentWindow.ViewCode(new ViewCodeArgs(Settings.Default.VisualStudioProgId, entry.ClassName, entry.LineNumber));
                    if(errorMsg.Length > 0)
                    {
                        MessageBox.Show(errorMsg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void OnAddToIgnoreValuesCommandClick(object sender, RoutedEventArgs e)
        {
            var message = (sender as Button).CommandParameter;
            if(LogViewModel.ViewModel.AddToIgnoreValuesCommand.CanExecute(message))
            {
                LogViewModel.ViewModel.AddToIgnoreValuesCommand.Execute(message);
            }
        }

        public void SetParentWindow(IPlugableWindow window)
        {
            _parentWindow = window;
        }
    }
}