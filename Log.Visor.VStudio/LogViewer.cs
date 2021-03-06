﻿//------------------------------------------------------------------------------
// <copyright file="LogViewer.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Log.Visor.VStudio
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    using System.Windows;
    using global::Visor.VStudio.Controls;
    using global::Visor.VStudio;

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
            this.Caption = "Viewer";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            //this.Content = new LogViewerControl();
            this.Content = new HostControl();
            var control = (IPlugableWindow)this.Content;
            try
            {
                var vsInstance = Marshal.GetActiveObject(control.ProgId);
                if (null != vsInstance)
                {
                    control.Dte = (EnvDTE80.DTE2)vsInstance;
                }
            } catch(Exception e)
            {
                MessageBox.Show(e.Message + " ProgId [" + control.ProgId + "] " + e.StackTrace, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
