using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace Trainer.Domain
{
    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    [System.Xml.Serialization.XmlRoot(Namespace = "", IsNullable = false)]
    public partial class Components: IDisposable
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

        public void Dispose()
        {
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    public partial class Component: IDisposable
    {
        private ObservableCollection<Dependency> dependencyField;

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

        public Component()
        {
            codeField = new Code();
            dependencyField = new ObservableCollection<Dependency>();
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("Dependency")]
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

        public void Dispose()
        {
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
