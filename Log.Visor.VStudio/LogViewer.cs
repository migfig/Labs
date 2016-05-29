//------------------------------------------------------------------------------
// <copyright file="LogViewer.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Log.Visor.VStudio
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    using Wpf.Controls;
    using System.Windows;    
    
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("02bee7af-68af-40ec-8e32-ea7a53105041")]
    public class LogViewer : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogViewer"/> class.
        /// </summary>
        public LogViewer() : base(null)
        {
            this.Caption = "Log Viewer";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new LogViewerControl();
            (this.Content as LogViewerControl).OnViewCodeRequest += LogViewerOnViewCodeRequest;
        }

        private void LogViewerOnViewCodeRequest(object sender, Wpf.Controls.ViewModels.ViewCodeArgs e)
        {
            var errMsg = string.Empty;
            try
            {
                var vsInstance = Marshal.GetActiveObject(e.ProgId);
                if (null != vsInstance)
                {
                    var dte = (EnvDTE80.DTE2)vsInstance;
                    var solution = dte.Solution;
                    if (null != solution)
                    {
                        foreach (EnvDTE.Project project in solution.Projects)
                        {
                            foreach (EnvDTE.ProjectItem item in project.ProjectItems)
                            {
                                if (project.Name.Contains(e.NameSpace) && item.Name.Equals(e.ClassName))
                                {
                                    var window = item.Open();
                                    if (null != window)
                                    {
                                        window.Activate();
                                        var selection = (EnvDTE.TextSelection)window.Document.Selection;
                                        selection.GotoLine(e.LineNumber, Select: true);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        errMsg = "Solution not available";
                    }
                }
                else
                {
                    errMsg = "Visual Studio Instance " + e.ProgId + " is not available!";
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            if (errMsg.Length > 0)
            {
                MessageBox.Show(errMsg,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
