﻿//------------------------------------------------------------------------------
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
    using Wpf.Controls;
    using System.Windows;
    using System.Diagnostics;
    using Wpf.Controls.ViewModels;
    using System.Collections.Generic;/// <summary>
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

        private void LogViewerOnViewCodeRequest(object sender, ViewCodeArgs e)
        {
            var errMsg = string.Empty;
            try
            {
                var vsInstance = Marshal.GetActiveObject(e.ProgId);
                if (null != vsInstance)
                {
                    var dte = (EnvDTE80.DTE2)vsInstance;
                    var output = dte.ToolWindows.OutputWindow.ActivePane;
                    var solution = dte.Solution;
                    if (null != solution)
                    {

                        var items = from project in solution.Projects.Cast<EnvDTE.Project>()
                                    let item = FindItem(project.ProjectItems.Cast<EnvDTE.ProjectItem>(), e)
                                    where item != null
                                    select item;

                        if (items.Count() == 1)
                        {
                            var item = items.First();
                            Log(output, "Code item found: " + item.Name);

                            var window = item.Open();
                            if (null != window)
                            {
                                window.Activate();
                                var selection = (EnvDTE.TextSelection)window.Document.Selection;
                                selection.GotoLine(e.LineNumber, Select: true);
                            }
                        }
                        else
                        {
                            var found = false;
                            Log(output, string.Format("Looking for Code item in {0} projects. Namespace {1}", items.Count(), e.NameSpace));
                            foreach (var item in items)
                            {
                                var parts = e.NameSpace.Split('\\');
                                for (var i=parts.Length-1;i>0;i--)
                                {
                                    var name = string.Join("\\", parts, 0, i);
                                    Log(output, string.Format("Looking for Code item {0} as named {1}", i, name));
                                    if (item.ContainingProject.Name.Equals(name))
                                    {
                                        Log(output, "Code item found in project " + item.ContainingProject.Name);
                                        var window = item.Open();
                                        if (null != window)
                                        {
                                            window.Activate();
                                            var selection = (EnvDTE.TextSelection)window.Document.Selection;
                                            selection.GotoLine(e.LineNumber, Select: true);
                                        }
                                        found = true;
                                        break;
                                    }
                                }
                                if (found)
                                    break;
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
                errMsg = ex.Message + ex.StackTrace;
            }

            if (errMsg.Length > 0)
            {
                MessageBox.Show(errMsg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private EnvDTE.ProjectItem FindItem(IEnumerable<EnvDTE.ProjectItem> items, ViewCodeArgs args)
        {
            var item = items.FirstOrDefault(x => x.Name.Equals(args.ClassName));
            if (item != null) return item;

            return FindItem(items
                    .Where(x => x.Kind.Equals(EnvDTE.Constants.vsProjectItemKindPhysicalFolder)
                        || x.Kind.Equals(EnvDTE.Constants.vsProjectItemKindVirtualFolder))
                , args);
        }

        private void Log(EnvDTE.OutputWindowPane pane, string message)
        {
            try
            {
                pane.OutputString(message + Environment.NewLine);
            } catch(Exception) {;} 
        }
    }
}
