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
    using Wpf.Controls;
    using System.Windows;
    using Wpf.Controls.ViewModels;
    using System.Collections.Generic;

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

        private void LogViewerOnViewCodeRequest(object sender, ViewCodeArgs e)
        {
            var found = false;
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
                        var projects = from project in solution.Projects.Cast<EnvDTE.Project>()
                                       where project.Name.Split('\\').First()
                                            .Equals(e.NameSpace.Split('\\').First())
                                       select project;
                        if (projects.Any())
                        {
                            var items = from project in projects
                                        from pi in project.ProjectItems.Cast<EnvDTE.ProjectItem>()
                                        let item = pi.Name.Equals(e.ClassName)
                                            ? pi
                                            : FindItem(output, pi.ProjectItems == null
                                                ? null
                                                : pi.ProjectItems.Cast<EnvDTE.ProjectItem>(), e)
                                        where item != null
                                        select item;

                            if (items.Count() == 1)
                            {
                                found = OpenItem(output, items.First(), e);
                            }
                            else
                            {
                                Log(output, string.Format("Looking for Code item in {0} projects. Namespace {1}", items.Count(), e.NameSpace));
                                foreach (var item in items)
                                {
                                    var parts = e.NameSpace.Split('\\');
                                    for (var i = parts.Length - 1; i >= 0; i--)
                                    {
                                        var name = string.Join("\\", parts, 0, i);
                                        Log(output, string.Format("Looking for Code item {0} as named {1}", i, name));
                                        if (item.ContainingProject.Name.Equals(name))
                                        {
                                            found = OpenItem(output, item, e);
                                            break;
                                        }
                                    }
                                    if (found) break;
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
                errMsg = ex.Message + ex.StackTrace;
            }

            if (errMsg.Length > 0)
            {
                MessageBox.Show(errMsg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if(!found)
                {
                    MessageBox.Show(string.Format("Class {0} not found in namespace {1}.", e.ClassName, e.NameSpace), "Code Item not found!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private bool OpenItem(EnvDTE.OutputWindowPane pane, EnvDTE.ProjectItem item, ViewCodeArgs e)
        {
            Log(pane, "Code item found [" + item.Name + "] in project [" + item.ContainingProject.Name + "]");
            var window = item.Open();
            if (null != window)
            {
                window.Activate();
                var selection = (EnvDTE.TextSelection)window.Document.Selection;
                selection.GotoLine(e.LineNumber, Select: true);
            }

            return true;
        }
        private EnvDTE.ProjectItem FindItem(EnvDTE.OutputWindowPane pane, IEnumerable<EnvDTE.ProjectItem> items, ViewCodeArgs e)
        {
            if (items == null || !items.Any()) return null;

            var item = items.FirstOrDefault(x => x.Name.Equals(e.ClassName));
            if (item != null) return item;

            foreach (var i in items.Where(x => x.Kind.Equals(EnvDTE.Constants.vsProjectItemKindPhysicalFolder)
                            || x.Kind.Equals(EnvDTE.Constants.vsProjectItemKindVirtualFolder)))
            {
                Log(pane, string.Format("looking for class {0} in project {1} at folder {2}", e.ClassName, i.ContainingProject.Name, i.Name));
                var itemx = FindItem(pane, i.ProjectItems.Cast<EnvDTE.ProjectItem>(), e);
                if (itemx != null)
                    return itemx;
            }

            return null;
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
