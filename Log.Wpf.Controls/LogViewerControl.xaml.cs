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
    using EnvDTE80;
    using EnvDTE;
    using System;
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
                LogViewerOnViewCodeRequest(sender, 
                    new ViewCodeArgs(Settings.Default.VisualStudioProgId, entry.ClassName, entry.LineNumber));
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

        private void LogViewerOnViewCodeRequest(object sender, ViewCodeArgs e)
        {
            var found = false;
            var errMsg = string.Empty;
            try
            {
                if (null != _parentWindow && null != _parentWindow.Dte)
                {
                    var dte90 = (Solution2)_parentWindow.Dte.Solution;

                    var prjItem = dte90.FindProjectItem(e.ClassName);
                    if (prjItem != null)
                    {
                        found = OpenItem(prjItem, e);
                    }
                }
                else
                {
                    errMsg = "Visual Studio Instance " + e.ProgId + " is not available!";
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + " ProgId [" + e.ProgId + "] " + ex.StackTrace;
            }

            if (errMsg.Length > 0)
            {
                MessageBox.Show(errMsg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (!found)
                {
                    MessageBox.Show(string.Format("Class {0} not found in namespace {1}.", e.ClassName, e.NameSpace), "Code Item not found!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private bool OpenItem(ProjectItem item, ViewCodeArgs e)
        {
            _parentWindow.Log("Code item found [{0}] in project [{1}]", item.Name, item.ContainingProject.Name);
            var window = item.Open();
            if (null != window)
            {
                window.Activate();
                var selection = (TextSelection)window.Document.Selection;
                selection.GotoLine(e.LineNumber, Select: true);
            }

            return true;
        }
    }
}