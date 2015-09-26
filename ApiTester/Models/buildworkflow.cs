using Common;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Serialization;

namespace ApiTester.Models
{
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class buildWorkflow: BaseModel
    {

        private ObservableCollection<TaskItem> taskItemField;
        private string nameField;
        private string commandLineField;

        public buildWorkflow()
        {
            taskItemField = new ObservableCollection<TaskItem>();
        }

        [XmlElementAttribute("taskItem")]
        public ObservableCollection<TaskItem> taskItem
        {
            get
            {
                return this.taskItemField;
            }
            set
            {
                this.taskItemField = value;
            }
        }

        [XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [XmlAttributeAttribute()]
        public string commandLine
        {
            get
            {
                return this.commandLineField;
            }
            set
            {
                this.commandLineField = value;
            }
        }
    }

    [XmlTypeAttribute(AnonymousType=true)]
    [XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class TaskItem {
        
        private ObservableCollection<TaskParameter> taskParameterField;        
        private ObservableCollection<TaskItem> taskItemField;        
        private string nameField;        
        private string commandLineField;
        private bool foreachTypeField;
        private bool isDisabledField;

        public TaskItem()
        {
            taskParameterField = new ObservableCollection<TaskParameter>();
            taskItemField = new ObservableCollection<TaskItem>();
        }
        
        [XmlElementAttribute("taskParameter", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public ObservableCollection<TaskParameter> taskParameter {
            get {
                return this.taskParameterField;
            }
            set {
                this.taskParameterField = value;
            }
        }
        
        [XmlElementAttribute("taskItem")]
        public ObservableCollection<TaskItem> taskItem {
            get {
                return this.taskItemField;
            }
            set {
                this.taskItemField = value;
            }
        }
        
        [XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        [XmlAttributeAttribute()]
        public string commandLine {
            get {
                return this.commandLineField;
            }
            set {
                this.commandLineField = value;
            }
        }

        [XmlAttributeAttribute()]
        public bool foreachType
        {
            get
            {
                return this.foreachTypeField;
            }
            set
            {
                this.foreachTypeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isDisabled
        {
            get
            {
                return this.isDisabledField;
            }
            set
            {
                this.isDisabledField = value;
            }
        }
    }

    [XmlTypeAttribute(AnonymousType=true)]
    public partial class TaskParameter {
        
        private string valueField;
        
        [XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    public static class BuildWorkflowExtensions
    {
        public static string ToArgs(this TaskItem task, params string[] values)
        {
            var value = new StringBuilder();
            foreach(var p in task.taskParameter)
            {
                value.AppendFormat(" " + p.Value, values);
            }

            return value.ToString();
        }
    }    
}
