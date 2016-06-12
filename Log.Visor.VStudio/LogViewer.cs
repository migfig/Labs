//------------------------------------------------------------------------------
// <copyright file="LogViewer.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Log.Visor.VStudio
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    //using Wpf.Controls;
    using System.Windows;
    //using Wpf.Controls.ViewModels;
    using System.Collections.Generic;
    using global::Visor.VStudio.Controls;

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
        private EnvDTE.OutputWindowPane _outputPane = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="LogViewer"/> class.
        /// </summary>
        public LogViewer() : base(null)
        {
            this.Caption = "Viewer";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            //this.Content = new LogViewerControl();
            this.Content = new ParentControl();
            //(this.Content as LogViewerControl).OnViewCodeRequest += LogViewerOnViewCodeRequest;
        }

        //private void LogViewerOnViewCodeRequest(object sender, ViewCodeArgs e)
        //{
        //    var found = false;
        //    var errMsg = string.Empty;
        //    var progId = e.ProgId;
        //    try
        //    {
        //        var vsInstance = Marshal.GetActiveObject(progId);
        //        if (null != vsInstance)
        //        {
        //            var dte = (EnvDTE80.DTE2)vsInstance;
        //            var dte90 = (EnvDTE80.Solution2)dte.Solution;
        //            try
        //            {
        //                _outputPane = dte.ToolWindows.OutputWindow.OutputWindowPanes.Item("LogVisorX");
        //            }
        //            catch (Exception)
        //            {
        //                _outputPane = dte.ToolWindows.OutputWindow.OutputWindowPanes.Add("LogVisorX");
        //            }
        //            if (_outputPane != null)
        //            {
        //                _outputPane.Activate();
        //                _outputPane.Clear();
        //            }

        //            var prjItem = dte90.FindProjectItem(e.ClassName);
        //            if (prjItem != null)
        //            {
        //                found = OpenItem(prjItem, e);
        //            }
        //        }
        //        else
        //        {
        //            errMsg = "Visual Studio Instance " + e.ProgId + " is not available!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errMsg = ex.Message + " ProgId [" + progId + "] " + ex.StackTrace;
        //    }

        //    if (errMsg.Length > 0)
        //    {
        //        MessageBox.Show(errMsg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    else
        //    {
        //        if (!found)
        //        {
        //            MessageBox.Show(string.Format("Class {0} not found in namespace {1}.", e.ClassName, e.NameSpace), "Code Item not found!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //        }
        //    }
        //}

        //private bool OpenItem(EnvDTE.ProjectItem item, ViewCodeArgs e)
        //{
        //    Log("Code item found [" + item.Name + "] in project [" + item.ContainingProject.Name + "]");
        //    var window = item.Open();
        //    if (null != window)
        //    {
        //        window.Activate();
        //        var selection = (EnvDTE.TextSelection)window.Document.Selection;
        //        selection.GotoLine(e.LineNumber, Select: true);
        //    }

        //    return true;
        //}

        private void Log(string message)
        {
            try
            {
                _outputPane.OutputString(message + Environment.NewLine);
            }
            catch (Exception) {; }
        }
    }
}
