using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using EnvDTE80;
using EnvDTE;
using Trainer.Domain;
using System.Linq;

namespace Visor.VStudio
{
    public interface IChildWindow
    {
        UserControl Content { get; }
        void SetParentWindow(IPlugableWindow window);
    }

    public interface ITitledWindow
    {
        string Title { get; }
    }

    public interface IPlugableWindow
    {
        string ProgId { get; }
        void Attach();
        DTE2 Dte { get; set; }
        void Log(string format, params string[] message);
        bool AddCode(Component component);
    }

    [Export(typeof(IPlugableWindow))]
    public class PlugableWindow : UserControl, IPlugableWindow
    {
        [ImportMany]
        IEnumerable<Lazy<IChildWindow, ITitledWindow>> children;

        private OutputWindowPane _outputPane = null;

        public DTE2 Dte { get; set; }
        public string ProgId { get { return Properties.Settings.Default.VisualStudioProgId; } }

        public void Attach()
        {
            foreach(Lazy<IChildWindow, ITitledWindow> child in children)
            {
                var tab = this.FindName("tabControl") as TabControl;
                if(tab != null)
                {
                    child.Value.SetParentWindow(this);
                    tab.Items.Add(new TabItem { Header = child.Metadata.Title, Content = child.Value.Content });
                }
            }            
        }

        public void Log(string format, params string[] message)
        {
            if (null != Dte && null == _outputPane)
            {
                try
                {
                    _outputPane = Dte.ToolWindows.OutputWindow.OutputWindowPanes.Item("LogVisorX");
                }
                catch (Exception)
                {
                    _outputPane = Dte.ToolWindows.OutputWindow.OutputWindowPanes.Add("LogVisorX");
                }
                if (_outputPane != null)
                {
                    _outputPane.Activate();
                    _outputPane.Clear();
                }
            }

            try
            {
                _outputPane.OutputString(string.Format(format, message) + Environment.NewLine);
            }
            catch (Exception) {; }
        }

        public bool AddCode(Component component)
        {
            if (null == Dte) return false;
            var solution = (Solution2)Dte.Solution;
            if (solution == null) return false;

            var projects = solution.Projects.Cast<Project>();
            var project = projects
                .FirstOrDefault(x => x.Name.Equals(component.TargetProject));
            if (project == null)
                project = projects.FirstOrDefault();
            if (project == null) return false;

            //project.CodeModel.
            project.ProjectItems.AddFromFile(component.TargetFile);

            return true;
        }
    }
}
