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
    using System;
    using System.ComponentModel.Composition;
    /// <summary>
    /// Interaction logic for LogViewerControl.
    /// </summary>
    [Export(typeof(IChildWindow))]
    [ExportMetadata("Title", "Log Viewer")]
    public partial class LogViewerControl : UserControl, IChildWindow, ITitledWindow
    {
        public string Title { get { return "Log Viewer"; } }
        UserControl IChildWindow.Content { get { return this; } }

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
                    var progId = Settings.Default.VisualStudioProgId ?? "VisualStudio.DTE.14.0";
                    OnViewCodeRequest(this, 
                        new ViewCodeArgs(progId, entry.ClassName, entry.LineNumber));
                }
            }
        }

        private void OnAddToIgnoreValuesCommandClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var message = (sender as Button).CommandParameter;
            if(LogViewModel.ViewModel.AddToIgnoreValuesCommand.CanExecute(message))
            {
                LogViewModel.ViewModel.AddToIgnoreValuesCommand.Execute(message);
            }
        }
    }
}