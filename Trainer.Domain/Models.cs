using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Trainer.Domain
{
    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    [System.Xml.Serialization.XmlRoot(Namespace = "", IsNullable = false)]
    public partial class Components
    {
        private ObservableCollection<Component> componentField;
        public Components()
        {
            componentField = new ObservableCollection<Component>();
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("Component")]
        public ObservableCollection<Component> Component
        {
            get
            {
                return this.componentField;
            }
            set
            {
                this.componentField = value;
            }
        }
    }

    public enum ComponentAction
    {
        None,
        Copy,
        View,
        Remove
    }

    public enum Language
    {
        All,
        CSharp,
        Xml,
        Html,
        JScript,
        JSon,
        Java,
        Sql
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    public partial class Component
    {
        private ObservableCollection<Dependency> dependencyField;
        private ObservableCollection<Parameter> parameterField;
        private Code codeField;

        private string idField;
        private string nameField;
        private string imageField;
        private string targetFileField;
        private string sourcePathField;
        private byte lineField;
        private bool lineFieldSpecified;
        private bool isBrowsableField;
        private string targetProjectField;
        private ComponentAction actionField;
        private bool isDirtyField;
        private Language languageField;

        public Component()
        {
            codeField = new Code();
            dependencyField = new ObservableCollection<Dependency>();
            parameterField = new ObservableCollection<Parameter>();
            actionField = ComponentAction.None;
            languageField = Language.CSharp;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("Dependency", Order=0)]
        public ObservableCollection<Dependency> Dependency
        {
            get
            {
                return this.dependencyField;
            }
            set
            {
                this.dependencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("Parameter", Order = 1)]
        public ObservableCollection<Parameter> Parameter
        {
            get
            {
                return this.parameterField;
            }
            set
            {
                this.parameterField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("Code", Order = 2)]
        public Code Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Name
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Image
        {
            get
            {
                return this.imageField;
            }
            set
            {
                this.imageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string TargetFile
        {
            get
            {
                return this.targetFileField;
            }
            set
            {
                this.targetFileField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string SourcePath
        {
            get
            {
                return this.sourcePathField;
            }
            set
            {
                this.sourcePathField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public byte Line
        {
            get
            {
                return this.lineField;
            }
            set
            {
                this.lineField = value;
            }
        }

        /// <remarks/>
        [JsonIgnore]
        [System.Xml.Serialization.XmlIgnore()]
        public bool LineSpecified
        {
            get
            {
                return this.lineFieldSpecified;
            }
            set
            {
                this.lineFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public bool IsBrowsable
        {
            get
            {
                return this.isBrowsableField;
            }
            set
            {
                this.isBrowsableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string TargetProject
        {
            get
            {
                return this.targetProjectField;
            }
            set
            {
                this.targetProjectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public ComponentAction Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool IsDirty
        {
            get
            {
                return this.isDirtyField;
            }
            set
            {
                this.isDirtyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public Language Language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    public partial class Parameter
    {
        private string labelField;
        private string nameField;
        private string valueField;
        private bool   isProjectNameField;
        private bool   isVisibleField = true; //default

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Name
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField= value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public bool IsProjectName
        {
            get
            {
                return this.isProjectNameField;
            }
            set
            {
                this.isProjectNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public bool IsVisible
        {
            get
            {
                return this.isVisibleField;
            }
            set
            {
                this.isVisibleField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(this.valueField);
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    public partial class Dependency
    {

        private string idField;

        private string locationField;
        private Component componentField;

        public Dependency()
        {
            this.componentField = new Component();
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Component Component
        {
            get { return this.componentField; }
            set
            {
                this.componentField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    public partial class Code
    {

        private string sourceFileField;

        private string valueField;
        private string composedValueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string SourceFile
        {
            get
            {
                return this.sourceFileField;
            }
            set
            {
                this.sourceFileField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlText()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        [JsonIgnore]
        public string ComposedValue
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.composedValueField) 
                    ? this.composedValueField
                    : this.valueField;
            }
            set
            {
                this.composedValueField = value;
            }
        }
    }
}
