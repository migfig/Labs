using System;
using EnvDTE80;
using EnvDTE;
using Trainer.Domain;
using System.Linq;
using System.IO;

namespace Visor.VStudio
{
    public partial class PlugableWindow
    {
        private void Log(string message)
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
                    //_outputPane.Clear();
                }
            }

            try
            {
                _outputPane.OutputString(message + Environment.NewLine);
            }
            catch (Exception) {; }
        }

        public string ViewCode(ViewCodeArgs e)
        {
            var found = false;
            var errMsg = string.Empty;
            try
            {
                if (Dte != null)
                {
                    var solution = (Solution2)Dte.Solution;

                    var prjItem = solution.FindProjectItem(e.ClassName);
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

            if (!found)
            {
                errMsg = string.Format("Class {0} not found in namespace {1}.", e.ClassName, e.NameSpace);
            }

            if(errMsg.Length > 0)
            {
                Log(errMsg);
            }

            return errMsg;
        }

        private bool OpenItem(ProjectItem item, ViewCodeArgs e)
        {
            Log("Code item found [{0}] in project [{1}]", item.Name, item.ContainingProject.Name);
            var window = item.Open();
            if (null != window)
            {
                if(!window.Visible)
                    window.Activate();
                var selection = (TextSelection)window.Document.Selection;
                if(e.LineNumber > 0)
                    selection.GotoLine(e.LineNumber, Select: true);
            }

            return true;
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

            Log("Project {0} found for Project target {1} at Component {2}", project.Name, component.TargetProject, component.Name);

            foreach (var dep in component.Dependency)
            {
                Log("Adding code for Component {0} dependency in Project {1}", dep.Component.Name, project.Name);
                AddCode(dep.Component, project);
            }

            if (!string.IsNullOrWhiteSpace(component.Code.Value))
            {
                var parts = component.TargetFile.Split('\\');
                var file = parts.Last();
                var folderItem = EnsureFoldersExist(project, component);

                ProjectItem item = null;
                if(folderItem == null)
                {
                    item = project.ProjectItems.Cast<ProjectItem>().FirstOrDefault(x => x.Name.Equals(file));
                }
                else
                {
                    item = folderItem.ProjectItems.Cast<ProjectItem>().FirstOrDefault(x => x.Name.Equals(file));
                }

                if (item != null)
                {
                    InsertCode(item, component);
                }
                else
                {
                    var fileName = GetFileName(project, component);
                    using(var stream = File.CreateText(fileName))
                    {
                        stream.Write(component.Code.Value);
                    }

                    component.SourcePath = fileName;
                    InsertCodeFromFile(project, component, isLinked: true);
                }
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
                selection.Insert(component.Code.Value);
                Log("Inserting code for Component {0} into Item {1} at Line {2}", component.Name, item.Name, component.Line.ToString());
                return true;
            }
            catch (Exception e)
            {
                Log("Error: {0}\nStack: {1}", e.Message, e.StackTrace);
            }

            return false;
        }

        private bool InsertCodeFromFile(Project project, Component component, bool isLinked = false)
        {
            try
            {
                var folders = component.TargetFile.Split('\\');
                var file = folders.Last();
                var folderItem = EnsureFoldersExist(project, component);
                ProjectItem item = null;

                if (folderItem != null)
                {
                    if(isLinked)
                        item = folderItem.ProjectItems.AddFromFile(Path.Combine(component.SourcePath, component.Code.SourceFile));
                    else
                        item = folderItem.ProjectItems.AddFromFileCopy(Path.Combine(component.SourcePath, component.Code.SourceFile));

                    Log("Inserting code for Component {0} into Folder Item {1} from Source file {2}", component.Name, folderItem.Name, component.Code.SourceFile);
                    OpenItem(item, new ViewCodeArgs(string.Empty, string.Empty, 1));
                }
                else
                {
                    if(isLinked)
                        item = project.ProjectItems.AddFromFile(Path.Combine(component.SourcePath, component.Code.SourceFile));
                    else
                        item = project.ProjectItems.AddFromFileCopy(Path.Combine(component.SourcePath, component.Code.SourceFile));

                    Log("Inserting code for Component {0} into Project {1} from Source file {2}", component.Name, project.Name, component.Code.SourceFile);
                    OpenItem(item, new ViewCodeArgs(string.Empty, string.Empty, 1));
                }

                return true;
            }
            catch (Exception e)
            {
                Log("Error: {0}\nStack: {1}", e.Message, e.StackTrace);
            }

            return false;
        }

        private ProjectItem EnsureFoldersExist(Project project, Component component)
        {
            ProjectItem folderItem = null;

            try
            {
                var folders = component.TargetFile.Split('\\');
                var file = folders.Last();
                foreach (var folder in folders.Where(x => !x.Equals(file)))
                {
                    if (folderItem == null)
                    {
                        folderItem = project.ProjectItems.Cast<ProjectItem>().FirstOrDefault(x => x.Name.Equals(folder));
                        if (folderItem == null)
                        {
                            Log("Adding Folder Item {0} into Project {1} for Component {2}", folder, project.Name, component.Name);
                            folderItem = project.ProjectItems.AddFolder(folder);
                        }
                    }
                    else
                    {
                        folderItem = folderItem.ProjectItems.Cast<ProjectItem>().FirstOrDefault(x => x.Name.Equals(folder));
                        if (folderItem == null)
                        {
                            Log("Adding Folder {0} into Folder Item {1} for Component {2}", folder, folderItem.Name, component.Name);
                            folderItem = folderItem.ProjectItems.AddFolder(folder);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log("Error: {0}\nStack: {1}", e.Message, e.StackTrace);
            }

            return folderItem;
        }

        private string GetFileName(Project project, Component component)
        {
            var fileName = project.FullName;
            var folders = component.TargetFile.Split('\\');
            var file = folders.Last();
            ProjectItem folderItem = null;

            foreach (var folder in folders.Where(x => !x.Equals(file)))
            {
                if (folderItem == null)
                {
                    folderItem = project.ProjectItems.Cast<ProjectItem>().FirstOrDefault(x => x.Name.Equals(folder));
                    if (folderItem != null)
                    {
                        fileName += "\\" + folderItem.Name;
                    }
                }
                else
                {
                    folderItem = folderItem.ProjectItems.Cast<ProjectItem>().FirstOrDefault(x => x.Name.Equals(folder));
                    if (folderItem != null)
                    {
                        fileName += "\\" + folderItem.Name;
                    }
                }
            }

            return fileName += "\\" + file;
        }
    }
}
