using Common;
using System.Collections.Generic;
using System.Linq;
using Trainer.Domain;

namespace Trainer.Wpf.VStudio.Tools.ViewModels
{
    public class ComponentVarsViewModel : Common.BaseModel
    {
        public IList<Parameter> Parameters { get; private set; }
        public IEnumerable<Parameter> VisibleParameters { get { return Parameters.Where(x => x.IsVisible.Equals(true)); } }
        public IList<string> Projects { get; private set; }

        private string _selectedProject;
        public string SelectedProject {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                var p = Parameters.FirstOrDefault(x => x.IsProjectName.Equals(true));
                if(p != null)
                {
                    p.Value = value;
                }
            }
        }

        public bool IsValid {
            get
            {
                return Parameters.All(x => x.IsValid) && !string.IsNullOrEmpty(SelectedProject);
            }
        }

        public ComponentVarsViewModel(IList<Parameter> parameters, IList<string> projects)
        {
            Parameters = parameters;
            Projects = projects;
            SelectedProject = Projects.FirstOrDefault();
        }
    }
}
