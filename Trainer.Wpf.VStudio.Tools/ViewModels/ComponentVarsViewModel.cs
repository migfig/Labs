using Common;
using System.Collections.Generic;
using System.Linq;

namespace Trainer.Wpf.VStudio.Tools.ViewModels
{
    public class ComponentVarsViewModel: BaseModel
    {
        public string ClassName { get; private set; }
        public IList<string> Projects { get; private set; }

        private string _selectedProject;
        public string SelectedProject {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
            }
        }

        public bool IsValid {
            get
            {
                return !string.IsNullOrEmpty(ClassName) && !string.IsNullOrEmpty(SelectedProject);
            }
        }

        public ComponentVarsViewModel(string className, IList<string> projects)
        {
            ClassName = className;
            Projects = projects;
            SelectedProject = Projects.FirstOrDefault();
        }
    }
}
