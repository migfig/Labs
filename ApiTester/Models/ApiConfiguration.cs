﻿namespace ApiTester.Models
{
    using Common;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.IO;
    using System.Xml.Serialization;
    using System.Linq;
    using System;
    using System.Text.RegularExpressions;
    using System.Configuration;
    using Newtonsoft.Json;
    using ApiTester.Attributes;

    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class apiConfiguration: BaseModel
    {
        private Setup setupField;
        private ObservableCollection<Method> methodField;
        private ObservableCollection<assembly> assemblyField;

        public apiConfiguration()
        {
            methodField = new ObservableCollection<Method>();
            assemblyField = new ObservableCollection<assembly>();
        }

        [XmlElementAttribute("setup", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Setup setup
        {
            get
            {
                return this.setupField;
            }
            set
            {
                this.setupField = value;
            }
        }

        [XmlElementAttribute("method", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ObservableCollection<Method> method
        {
            get
            {
                return this.methodField;
            }
            set
            {
                this.methodField = value;
            }
        }

        [XmlElementAttribute("assembly", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ObservableCollection<assembly> assembly
        {
            get
            {
                return this.assemblyField;
            }
            set
            {
                this.assemblyField = value;
            }
        }

        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(setup.source) 
                + Environment.NewLine
                + Environment.NewLine
                + methodField.Count.ToString()
                + " Methods";
        }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public partial class Setup: BaseModel
    {
        private ObservableCollection<Header> headerField;
        private ObservableCollection<BuildHeader> buildHeaderField;
        private ObservableCollection<Host> hostField;
        private ObservableCollection<workflow> workflowField;

        private string commandLineField;

        private string sourceField;

        public Setup()
        {
            headerField = new ObservableCollection<Header>();
            buildHeaderField = new ObservableCollection<BuildHeader>();
            hostField = new ObservableCollection<Host>();
            workflowField = new ObservableCollection<workflow>();
        }
        
        [XmlElementAttribute("header", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ObservableCollection<Header> header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }
        
        [XmlElementAttribute("buildHeader", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ObservableCollection<BuildHeader> buildHeader
        {
            get
            {
                return this.buildHeaderField;
            }
            set
            {
                this.buildHeaderField = value;
            }
        }

        [XmlElementAttribute("host", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ObservableCollection<Host> host
        {
            get
            {
                return this.hostField;
            }
            set
            {
                this.hostField = value;
            }
        }

        [XmlElementAttribute("workflow", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ObservableCollection<workflow> workflow
        {
            get
            {
                return this.workflowField;
            }
            set
            {
                this.workflowField = value;
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

        [XmlAttributeAttribute()]
        public string source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public partial class Host : BaseModel
    {
        private string nameField;
        private string baseAddressField;

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

        [XmlTextAttribute()]
        public string baseAddress
        {
            get
            {
                return this.baseAddressField;
            }
            set
            {
                this.baseAddressField = value;
            }
        }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public partial class Header: BaseModel
    {

        private string nameField;

        private string valueField;

        
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
        public string value
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
    }
    
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class BuildHeader: BaseModel
    {

        private ObservableCollection<Task> workflowField;

        private string nameField;

        public BuildHeader()
        {
            workflowField = new ObservableCollection<Task>();
        }
        
        [XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [XmlArrayItemAttribute("task", typeof(Task), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ObservableCollection<Task> workflow
        {
            get
            {
                return this.workflowField;
            }
            set
            {
                this.workflowField = value;
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
    }

    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class workflow
    {

        private ObservableCollection<Task> taskField;

        private string nameField;

        public workflow()
        {
            this.taskField = new ObservableCollection<Task>();
        }

        [XmlElementAttribute("task", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ObservableCollection<Task> task
        {
            get
            {
                return this.taskField;
            }
            set
            {
                this.taskField = value;
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
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public partial class Task: BaseModel
    {
        private string nameField;
        private string patternField;
        private ObservableCollection<Parameter> parameterField;
        private ObservableCollection<Task> taskField;
        private object resultsField;

        public Task()
        {
            parameterField = new ObservableCollection<Parameter>();
            taskField = new ObservableCollection<Task>();
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
        
        [ColumnIgnore]
        [XmlAttributeAttribute()]
        public string pattern
        {
            get
            {
                return this.patternField;
            }
            set
            {
                this.patternField = value;
            }
        }

        [ColumnIgnore]
        [XmlElementAttribute("parameter", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ObservableCollection<Parameter> parameter
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

        [ColumnIgnore]
        [XmlElementAttribute("task", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ObservableCollection<Task> task
        {
            get
            {
                return this.taskField;
            }
            set
            {
                this.taskField = value;
            }
        }

        [XmlIgnore]
        public object Results
        {
            get
            {
                return this.resultsField;
            }
            set
            {
                this.resultsField = value;
            }
        }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public partial class assembly : BaseModel
    {
        private string nameField;

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
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public partial class Method: BaseModel
    {
        private ObservableCollection<Parameter> parameterField;

        private string nameField;

        private string httpMethodField;

        private string urlField;

        private string typeField;

        private string descriptionField;

        private bool isSelectedField;
        private bool isValidTestField;

        public Method()
        {
            isSelectedField = true;
            isValidTestField = false;
            parameterField = new ObservableCollection<Parameter>();
        }

        [XmlAttributeAttribute()]
        public bool isSelected
        {
            get
            {
                return this.isSelectedField;
            }
            set
            {
                this.isSelectedField = value;
                OnPropertyChanged();
            }
        }

        [XmlAttributeAttribute()]
        public bool isValidTest
        {
            get
            {
                return this.isValidTestField;
            }
            set
            {
                this.isValidTestField = value;
                OnPropertyChanged();
            }
        }

        [ColumnIgnore]
        [XmlElementAttribute("parameter", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ObservableCollection<Parameter> parameter
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
        public string httpMethod
        {
            get
            {
                return this.httpMethodField;
            }
            set
            {
                this.httpMethodField = value;
            }
        }

        
        [XmlAttributeAttribute()]
        public string url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }

        
        [XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        
        [XmlAttributeAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    public static class ConfigurationExtensions
    {
        public static DataTable ToTable(this apiConfiguration configuration)
        {
            var table = new DataTable("Configuration");
            foreach (var p in configuration.method.First().GetType().GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(ColumnIgnoreAttribute), false).Count() == 0))
            {
                table.Columns.Add(new DataColumn(p.Name, p.PropertyType));
            }

            foreach (var m in configuration.method)
            {
                table.Rows.Add(m.ToRow(table));
            }
            return table;
        }

        public static DataTable ToTable(this Method method)
        {
            var table = new DataTable(method.name);
            foreach (var p in method.parameter.First().GetType().GetProperties())
            {
                table.Columns.Add(new DataColumn(p.Name, p.PropertyType));
            }

            foreach (var p in method.parameter)
            {
                table.Rows.Add(p.ToRow(table));
            }
            return table;
        }

        public static DataTable ToTable(this workflow workflow)
        {
            var table = new DataTable(workflow.name);
            foreach (var p in workflow.task.First().GetType().GetProperties()
                .Where(x => x.GetCustomAttributes(
                    typeof(ColumnIgnoreAttribute), false).Count() == 0))
            {
                table.Columns.Add(new DataColumn(p.Name, p.PropertyType));
            }

            foreach (var p in workflow.task)
            {
                table.Rows.Add(p.ToRow(table));
            }
            return table;
        }

        public static DataRow ToRow(this Task task, DataTable table)
        {
            var row = table.NewRow();
            foreach (var p in task.GetType().GetProperties()
                .Where(x => x.GetCustomAttributes(
                    typeof(ColumnIgnoreAttribute), false).Count() == 0))
            {
                row[p.Name] = p.GetValue(task);
            }

            return row;
        }

        public static DataRow ToRow(this Parameter parameter, DataTable table)
        {
            var row = table.NewRow();
            foreach (var p in parameter.GetType().GetProperties())
            {
                row[p.Name] = p.GetValue(parameter);
            }

            return row;
        }

        public static DataRow ToRow(this Method method, DataTable table)
        {
            var row = table.NewRow();
            foreach (var p in method.GetType().GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(ColumnIgnoreAttribute), false).Count() == 0))
            {
                row[p.Name] = p.GetValue(method);
            }

            return row;
        }

        public static string ToArgs(this Method method, Task task)
        {
            return string.Format("-X {0} {1} -o {2} {3}",
                method.httpMethod.ToUpper(),
                string.Format("{0}{1}", "{0}", task.QueryUrl(method.url)),
                method.name + ".json",
                method.httpMethod.ToUpper() != "GET" 
                    ? task.Json().Length > 0 ? string.Format("-d {0}", task.Json()) : string.Empty 
                    : string.Empty);
        }

        public static string Json(this Task task)
        {
            var parameter = task.parameter.FirstOrDefault(p => p.location.ToLower() == "body");
            if(null != parameter && !string.IsNullOrEmpty(parameter.jsonObject))
                    return parameter.jsonObject
                        .Replace(Environment.NewLine, string.Empty)
                        .Replace("\n", string.Empty)
                        .Replace("{", "{{")
                        .Replace("}", "}}")
                        .Trim();

            return string.Empty;
        }

        public static string QueryUrl(this Task task, string url)
        {
            var query = url;
            foreach(var p in task.parameter.Where(p => p.location.ToLower() == "query"))
            {
                var regEx = new Regex(ConfigurationManager.AppSettings["ParameterRegEx"]
                    .Replace("&lt;", "<")
                    .Replace("&gt;", ">"));
                var match = regEx.Match(url);
                if (match != null && match.Success)
                {
                    var parameter = match.Groups["parameter"].Value;
                    var name = match.Groups["name"].Value;
                    if(!string.IsNullOrEmpty(parameter) && !string.IsNullOrEmpty(name))
                    {
                        if(p.name == name)
                        {
                            query = query.Replace(parameter, p.defaultValue);
                        }
                    }
                }
                else
                {
                    query += query.Contains("?") ? "&" : "?" + p.name + "=" + p.defaultValue;
                }
            }

            return query;
        }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public partial class Parameter: BaseModel
    {
        private string nameField;
        private string typeField;
        private string locationField;
        private string jsonObjectField;
        private string defaultValueField;

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
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        
        [XmlAttributeAttribute()]
        public string location
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

        [XmlElementAttribute()]
        public string jsonObject
        {
            get
            {
                return this.jsonObjectField;
            }
            set
            {
                this.jsonObjectField = value;
            }
        }

        [XmlAttributeAttribute()]
        public string defaultValue
        {
            get
            {
                return this.defaultValueField;
            }
            set
            {
                this.defaultValueField = value;
            }
        }

    }
}