using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using EnvDTE80;
using EnvDTE;
using Trainer.Domain;
using System.Linq;
using System.IO;

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
            return AddCode(component, null);
        }

        private bool AddCode(Component component, Project project = null)
        {
            if (project == null)
            {
                if (null == Dte) return false;
                var solution = (Solution2)Dte.Solution;
                if (solution == null) return false;

                project = solution.Projects.Cast<Project>()
                    .FirstOrDefault(x => x.Name.Equals(component.TargetProject));
                if (project == null) return false;
            }

            foreach (var dep in component.Dependency)
            {
                AddCode(dep.Component, project);
            }

            if (!string.IsNullOrWhiteSpace(component.Code.Value))
            {
                var solution = (Solution2)Dte.Solution;
                var item = solution.FindProjectItem(component.TargetFile.Split('\\').Last());
                InsertCode(item, component);
            }
            else
            {
                InsertCodeFromFile(project, component);
            }

            return true;
        }

        private bool InsertCode(ProjectItem item, Component component)
        {
            if (item == null) return false;

            var window = item.Open();
            if (window == null) return false;

            try
            {
                window.Activate();
                window.Document.Activate();
                var selection = (TextSelection)window.Document.Selection;
                selection.GotoLine(component.Line, Select: false);
                selection.EndOfLine();
                selection.NewLine();
                //var edit = selection.Parent.CreateEditPoint();
                //edit.Insert(component.Code.Value);
                selection.Insert(component.Code.Value);
                return window.Document.Save() == vsSaveStatus.vsSaveSucceeded;
            }
            catch (Exception e)
            {
                Log("Error: {0}\nStack: {1}", e.Message, e.StackTrace);
            }

            return false;
        }

        private bool InsertCodeFromFile(Project project, Component component)
        {
            try
            {
                var folders = component.TargetFile.Split('\\');
                var file = folders.Last();
                ProjectItem folderItem = null;
                foreach (var folder in folders.Where(x => !x.Equals(file)))
                {
                    if (folderItem == null)
                        folderItem = project.ProjectItems.AddFolder(folder);
                    else
                        folderItem = folderItem.ProjectItems.AddFolder(folder);
                }

                if (folderItem != null)
                {
                    folderItem.ProjectItems.AddFromFile(Path.Combine(component.SourcePath, component.Code.SourceFile));
                    folderItem.Save();
                }
                else
                {
                    project.ProjectItems.AddFromFile(Path.Combine(component.SourcePath, component.Code.SourceFile));
                    project.Save();
                }

                return true;
            } catch(Exception e)
            {
                Log("Error: {0}\nStack: {1}", e.Message, e.StackTrace);
            }

            return false;
        }
    }
}
