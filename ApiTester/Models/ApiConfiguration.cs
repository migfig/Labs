namespace ApiTester.Models
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
    using System.Text;
    using System.Reflection;
    using FluentTesting;
    using System.Windows;
    using Common.Commands;
    using System.Windows.Input;
    using System.Collections.Generic;
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class apiConfiguration: BaseModel
    {
        private Setup setupField;
        private ObservableCollection<Method> methodField;
        private ObservableCollection<assembly> assemblyField;
        private string documentationUrlField;

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

        [XmlAttributeAttribute()]
        public string documentationUrl
        {
            get
            {
                return this.documentationUrlField;
            }
            set
            {
                this.documentationUrlField = value;
            }
        }

        public override string ToString()
        {
            return setup.name.Split('.').Last() 
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
        private ObservableCollection<Host> hostField;
        private ObservableCollection<workflow> workflowField;

        private string commandLineField;
        private string sourceField;
        private string nameField;

        public Setup()
        {
            headerField = new ObservableCollection<Header>();
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
    public partial class Host : BaseModel
    {
        private string nameField;
        private string baseAddressField;
        private ObservableCollection<Header> headerField;

        public Host()
        {
            headerField = new ObservableCollection<Header>();
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
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public partial class Header: BaseModel
    {

        private string nameField;
        private string valueField;
        private string propertyNameField;
        private ObservableCollection<BuildHeader> buildHeaderField;

        public Header()
        {
            buildHeaderField = new ObservableCollection<BuildHeader>();
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

        [XmlAttributeAttribute()]
        public string propertyName
        {
            get
            {
                return this.propertyNameField;
            }
            set
            {
                this.propertyNameField = value;
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

        public override string ToString()
        {
            return this.name
                + Environment.NewLine
                + Environment.NewLine
                + taskField.Count.ToString()
                + " Tasks";
        }
    }

    public enum eValidTest
    {
        Undefined,
        Passed,
        Failed
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public partial class Task: BaseModel
    {
        private string nameField;
        private string patternField;
        private ObservableCollection<Parameter> parameterField;
        private ObservableCollection<Task> taskField;
        private ObservableCollection<ResultValue> resultValueField;
        private string resultsField;
        private object resultsObjectField;
        private Task parentTaskField;
        private bool isDisabledField;
        private bool hasPassedField;
        private bool isHeaderBuilderField;
        private string xmlField;

        public Task()
        {
            parameterField = new ObservableCollection<Parameter>();
            taskField = new ObservableCollection<Task>();
            resultValueField = new ObservableCollection<ResultValue>();
        }

        [XmlIgnore]
        [EditColumnIgnore]
        public bool Passed
        {
            get { return hasPassedField; }
            set
            {
                hasPassedField = value;
                OnPropertyChanged("IsVisible");
                OnPropertyChanged("bodyBackground");
                OnPropertyChanged("Results");
                OnPropertyChanged("ResultsTable");
                OnPropertyChanged("Properties");
                OnPropertyChanged("SelectedCondition");
                OnPropertyChanged("SelectedOperator");
                OnPropertyChanged("SelectedProperty");
            }
        }

        private Instance _instance;
        public Instance GetInstance()
        {
            return _instance;
        }

        [ColumnIgnore]
        [EditColumnIgnore]
        [XmlIgnore]
        public bool IsValidTest
        {
            get
            {
                hasPassedField = false;
                if (ResultsObject == null || ResultsObject is Exception) return false;

                if (resultValue.Any())
                {
                    _instance = new Instance("Verifying results for Task [" + this.name + "] ", ResultsObject);
                    foreach (var val in resultValue)
                    {
                        var item = _instance.VerifyProperty(val.propertyName);
                        switch (val.@operator)
                        {
                            case eOperator.isEqualTo:
                                item.IsEqualTo(val.value, val.condition);
                                break;
                            case eOperator.isNotEqualTo:
                                item.IsNotEqualTo(val.value, val.condition);
                                break;
                            case eOperator.isGreaterThan:
                                item.IsGreaterThan(val.value, val.condition);
                                break;
                            case eOperator.isLessThan:
                                item.IsLessThan(val.value, val.condition);
                                break;
                            case eOperator.isGreaterThanOrEqual:
                                item.IsGreaterThanOrEqual(val.value, val.condition);
                                break;
                            case eOperator.isLessThanOrEqual:
                                item.IsLessThanOrEqual(val.value, val.condition);
                                break;
                        }
                    }

                    Passed = _instance.GetResults().ResultsPassed;
                    return hasPassedField;
                }

                Passed = true;
                return hasPassedField;
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
        
        [ColumnIgnore]
        [EditColumnIgnore]
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
        [EditColumnIgnore]
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
        [EditColumnIgnore]
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

        [ColumnIgnore]
        [EditColumnIgnore]
        [XmlElementAttribute("resultValue", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ObservableCollection<ResultValue> resultValue
        {
            get
            {
                return this.resultValueField;
            }
            set
            {
                this.resultValueField = value;
            }
        }

        [XmlIgnore]
        [EditColumnIgnore]
        public string Results
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

        [ColumnIgnore]
        [EditColumnIgnore]
        [XmlIgnore]
        public object ResultsObject
        {
            get
            {
                return this.resultsObjectField;
            }
            set
            {
                this.resultsObjectField = value;
                this.Results = JsonConvert.SerializeObject(value, Formatting.Indented);
            }
        }

        [XmlIgnore]
        public DataTable ResultsTable
        {
            get
            {
                var table = new DataTable("Results");

                if(resultsObjectField != null)
                {
                    Type itemType = null;
                    MethodInfo getMethod = null;
                    var type = resultsObjectField.GetType();
                    var size = 0;
                    itemType = type;
                    if (type.IsArray)
                    {
                        foreach (var p in type.GetProperties().Where(x => x.Name.Equals("Length")))
                        {
                            if (!_properties.ContainsKey(p.Name)) _properties.Add(p.Name, p.PropertyType.ToString());
                        }
                        size = (int)type.GetProperty("Length").GetValue(resultsObjectField);
                        getMethod = type.GetMethod("Get");
                        itemType = getMethod.Invoke(resultsObjectField, new object[] {0}).GetType();
                    }
                    var props = from p in itemType.GetProperties()
                                select p;
                    foreach (var prop in props)
                    {
                        table.Columns.Add(prop.Name, prop.PropertyType);
                        if(!type.IsArray && !_properties.ContainsKey(prop.Name))
                        {
                            _properties.Add(prop.Name, prop.PropertyType.ToString());
                        }
                    }

                    if (size > 0)
                    {
                        for (var i = 0; i < size; i++)
                        {
                            var item = getMethod.Invoke(resultsObjectField, new object[] {i});

                            var row = table.NewRow();
                            foreach (var prop in props)
                            {
                                row[prop.Name] = prop.GetValue(item);
                            }
                            table.Rows.Add(row);
                        }
                    }
                    else //sigle item type
                    {
                        var row = table.NewRow();
                        foreach (var prop in props)
                        {
                            row[prop.Name] = prop.GetValue(resultsObjectField);
                        }
                        table.Rows.Add(row);
                    }
                }

                if(string.IsNullOrEmpty(SelectedProperty.Key) && _properties.Any())
                {
                    SelectedProperty = _properties.First();
                    ((RelayCommand)AddCondition).RaiseCanExecuteChanged();
                }

                return table;
            }
        }

        [ColumnIgnore]
        [EditColumnIgnore]
        [XmlIgnore]
        public Task ParentTask
        {
            get { return parentTaskField; }
            set
            {
                parentTaskField = value;
            }
        }

        [ColumnIgnore]
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
                OnPropertyChanged("bodyBackground");
                xml = string.Empty;
                OnPropertyChanged("xml");
            }
        }

        [ColumnIgnore]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isHeaderBuilder
        {
            get
            {
                return this.isHeaderBuilderField;
            }
            set
            {
                this.isHeaderBuilderField = value;
            }
        }

        [XmlIgnore]
        public Visibility IsVisible
        {
            get { return hasPassedField ? Visibility.Visible : Visibility.Collapsed; }
        }

        [ColumnIgnore]
        [XmlIgnore]
        public string xml
        {
            get
            {
                if(string.IsNullOrEmpty(xmlField))
                {
                    xmlField = XmlHelper<Task>.Save(this, omitXml: true)
                        .Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", string.Empty)
                        .Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
                }
                return xmlField;
            }
            set
            {
                xmlField = value;
            }
        }

        [XmlIgnore]
        public string bodyBackground
        {
            get
            {
                return isDisabledField 
                    ? "#FFCDD1CC" //gray tone
                    : ResultsObject == null
                        ? "#FFFCE48A" //yellow tone
                        : hasPassedField 
                            ? "#FFBCEDB9" //green tone
                            : "#FFECAA6A"; //orange tone
            }
        }

        #region commands and helpers

        private ObservableCollection<eCondition> _conditions = new ObservableCollection<eCondition>();
        [XmlIgnore]
        public ObservableCollection<eCondition> Conditions
        {
            get
            {
                if(!_conditions.Any())
                {
                    foreach(var c in Enum.GetValues(typeof(eCondition)))
                    {
                        _conditions.Add((eCondition)c);
                    }
                }

                return _conditions;
            }
        }

        private eCondition _selectedCondition = eCondition.And;
        [XmlIgnore]
        public eCondition SelectedCondition
        {
            get { return _selectedCondition; }
            set
            {
                _selectedCondition = (eCondition)Enum.Parse(typeof(eCondition), value.ToString());
            }
        }

        private ObservableCollection<eOperator> _operators = new ObservableCollection<eOperator>();
        [XmlIgnore]
        public ObservableCollection<eOperator> Operators
        {
            get
            {
                if (!_operators.Any())
                {
                    foreach (var c in Enum.GetValues(typeof(eOperator)))
                    {
                        _operators.Add((eOperator)c);
                    }
                }

                return _operators;
            }
        }

        private eOperator _selectedOperator = eOperator.isEqualTo;
        [XmlIgnore]
        public eOperator SelectedOperator
        {
            get { return _selectedOperator; }
            set
            {
                _selectedOperator = (eOperator)Enum.Parse(typeof(eOperator), value.ToString());
            }
        }

        private Dictionary<string, string> _properties = new Dictionary<string, string>();
        [XmlIgnore]
        public Dictionary<string, string> Properties
        {
            get { return _properties; } 
        }

        private KeyValuePair<string, string> _selectedProperty;
        [XmlIgnore]
        public KeyValuePair<string, string> SelectedProperty
        {
            get { return _selectedProperty; }
            set
            {
                _selectedProperty = value;
            }
        }

        RelayCommand _addCondition;
        [XmlIgnore]
        public ICommand AddCondition
        {
            get
            {
                _addCondition = _addCondition ?? new RelayCommand(
                    (parameter) => {
                        this.resultValue.Add(new ResultValue
                        {
                            condition = SelectedCondition,
                            propertyName = SelectedProperty.Key,
                            @operator = _selectedOperator,
                            value = SelectedProperty.Value.ToLower().Contains("int") ? "0" : ""
                        });
                        xml = string.Empty;
                        OnPropertyChanged("xml");
                    },
                    x => _properties.Any());
                return _addCondition;
            }
        }

        RelayCommand _removeCondition;
        [XmlIgnore]
        public ICommand RemoveCondition
        {
            get
            {
                _removeCondition = _removeCondition ?? new RelayCommand(
                    (parameter) => {
                        resultValue.RemoveAt(resultValue.Count-1);
                        xml = string.Empty;
                        OnPropertyChanged("xml");
                    },
                    x => resultValue.Any());
                return _removeCondition;
            }
        }

        RelayCommand _addSubtask;
        [XmlIgnore]
        public ICommand AddSubtask
        {
            get
            {
                _addSubtask = _addSubtask ?? new RelayCommand(
                    (parameter) => {
                        var SelectedMethod = parameter as Method;
                        taskField.Add(new Task
                        {
                            name = SelectedMethod.name,
                            parameter = new ObservableCollection<Parameter>(SelectedMethod.parameter.ToArray()),
                            resultValue = new ObservableCollection<ResultValue>
                            {
                               new ResultValue
                               {
                                   condition = eCondition.And,
                                   propertyName = "Length",
                                   @operator = eOperator.isGreaterThan,
                                   value = "0"
                               }
                            }
                        });
                        xml = string.Empty;
                        OnPropertyChanged("xml");
                        ((RelayCommand)RemoveSubtask).RaiseCanExecuteChanged();
                    },
                    x => (bool)x.Equals(true));
                return _addSubtask;
            }
        }

        RelayCommand _removeSubtask;
        [XmlIgnore]
        public ICommand RemoveSubtask
        {
            get
            {
                _removeSubtask = _removeSubtask ?? new RelayCommand(
                    (parameter) => {
                        task.RemoveAt(task.Count - 1);
                        xml = string.Empty;
                        OnPropertyChanged("xml");
                        ((RelayCommand)RemoveSubtask).RaiseCanExecuteChanged();
                    },
                    x => task.Any());
                return _removeSubtask;
            }
        }

        #endregion
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public partial class ResultValue: BaseModel
    {
        private string propertyNameField;
        private eCondition conditionField;
        private eOperator operatorField;
        private string valueField;

        [XmlAttributeAttribute()]
        public eCondition condition
        {
            get
            {
                return this.conditionField;
            }
            set
            {
                this.conditionField = value;
            }
        }

        [XmlAttributeAttribute()]
        public string propertyName
        {
            get
            {
                return this.propertyNameField;
            }
            set
            {
                this.propertyNameField = value;
            }
        }        

        [XmlAttributeAttribute()]
        public eOperator @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
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
        private eValidTest isValidTestField;

        public Method()
        {
            isSelectedField = false;
            isValidTestField = eValidTest.Undefined;
            parameterField = new ObservableCollection<Parameter>();
        }

        [XmlIgnore]
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

        [XmlIgnore]
        public eValidTest isValidTest
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

        public override string ToString()
        {
            return this.name
                + Environment.NewLine
                + Environment.NewLine
                + parameterField.Count.ToString()
                + " Parameters";
        }
    }

    public static class ConfigurationExtensions
    {
        public static DataTable ToTable(this apiConfiguration configuration)
        {
            var table = new DataTable("Configuration");
            if (configuration == null) return table;

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

        public static DataTable ToTable(this workflow workflow, bool isEdit = false)
        {
            var table = new DataTable(workflow.name);
            foreach (var p in workflow.task.First().GetType().GetProperties()
                .Where(x => x.GetCustomAttributes(
                    isEdit ? typeof(EditColumnIgnoreAttribute) : typeof(ColumnIgnoreAttribute), false)
                        .Count() == 0))
            {
                table.Columns.Add(new DataColumn(p.Name, p.PropertyType));
            }

            foreach (var t in workflow.task)
            {
                AddRow(t, table, isEdit);
            }
            return table;
        }

        private static void AddRow(Task task, DataTable table, bool isEdit = false)
        {
            table.Rows.Add(task.ToRow(table, isEdit));
            foreach (var t in task.task)
            {
                AddRow(t, table, isEdit);
            }
        }

        public static DataRow ToRow(this Task task, DataTable table, bool isEdit = false)
        {
            var row = table.NewRow();
            foreach (var p in task.GetType().GetProperties()
                .Where(x => x.GetCustomAttributes(
                    isEdit ? typeof(EditColumnIgnoreAttribute) : typeof(ColumnIgnoreAttribute), false).Count() == 0))
            {
                row[p.Name] = p.GetValue(task);
            }

            return row;
        }

        public static DataTable ToTable(this Task task, bool isParameter = true)
        {
            var table = new DataTable(task.name);
            if (isParameter)
            {
                foreach (var p in task.parameter.First().GetType().GetProperties())
                {
                    table.Columns.Add(new DataColumn(p.Name, p.PropertyType));
                }

                foreach (var p in task.parameter)
                {
                    table.Rows.Add(p.ToRow(table));
                }
            }
            else
            {
                foreach (var p in task.resultValue.First().GetType().GetProperties())
                {
                    table.Columns.Add(new DataColumn(p.Name, p.PropertyType));
                }

                foreach (var p in task.resultValue)
                {
                    table.Rows.Add(p.ToRow(table));
                }
            }
            return table;
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

        public static DataRow ToRow(this ResultValue result, DataTable table)
        {
            var row = table.NewRow();
            foreach (var p in result.GetType().GetProperties())
            {
                row[p.Name] = p.GetValue(result);
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
            return string.Format("-X {0} {1} -o output\\{2} {3} {4}",
                method.httpMethod.ToUpper(),
                string.Format("{0}{1}", "{0}", task.QueryUrl(method.url)), //for baseAddress
                method.name + ".json",
                "{1} {2}", //for headers
                method.httpMethod.ToUpper() != "GET" 
                    ? task.Json().Length > 0 ? string.Format("-d \"{0}\"", task.Json()) : string.Empty 
                    : string.Empty);
        }

        public static string Json(this Task task)
        {            
            var parameter = task.parameter.FirstOrDefault(p => p.location.ToLower() == "body");
            if (null != parameter && !string.IsNullOrEmpty(parameter.jsonObject))
            {
                var value = string.Empty;
                if (!string.IsNullOrEmpty(parameter.valueFromProperty) && task.ParentTask != null)
                {
                    value = getDefaultValue(parameter, task.ParentTask);
                    if (!string.IsNullOrEmpty(value))
                    {
                        parameter.jsonObject = parameter.jsonObject
                            .Replace("$"+parameter.valueFromProperty+"$", value);
                    }
                }

                return parameter.jsonObject
                    .Replace(Environment.NewLine, string.Empty)
                    .Replace("\n", string.Empty)
                    .Replace("{", "{{")
                    .Replace("}", "}}")
                    .Replace("\"", "\\\"")
                    .Trim();
            }

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
                            if (!string.IsNullOrEmpty(p.valueFromProperty) && task.ParentTask != null)
                            {
                                query = query.Replace(parameter, getDefaultValue(p, task.ParentTask));
                            }
                            else
                            {
                                query = query.Replace(parameter, p.defaultValue);
                            }
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

        public static string ToHeaders(this Setup setup)
        {
            var headers = new StringBuilder();
            foreach (var h in setup.header.Where(x => !x.buildHeader.Any() || !string.IsNullOrEmpty(x.value)))
            {
                headers.AppendFormat("-H \"{0}:{1}\" ", h.name, h.value);
            }

            return headers.ToString();
        }

        public static string ToHeaders(this Host host)
        {
            var headers = new StringBuilder();
            foreach (var h in host.header.Where(x => !x.buildHeader.Any() || !string.IsNullOrEmpty(x.value)))
            {
                headers.AppendFormat("-H \"{0}:{1}\" ", h.name, h.value);
            }

            return headers.ToString();
        }

        private static string getDefaultValue(Parameter p, Task parentTask)
        {
            if (parentTask == null || parentTask.ResultsObject is Exception) return p.defaultValue;

            var value = getPropertyValue(parentTask, p.valueFromProperty);
            if (null == value) return p.defaultValue;

            return value.ToString();
        }

        private static object getPropertyValue(Task parent, string propertyName)
        {
            if (null == parent) return null;

            var prop = parent.ResultsObject.GetType().GetProperties()
                .Where(x => x.Name == propertyName).FirstOrDefault();

            if (null != prop) return prop.GetValue(parent.ResultsObject);

            return getPropertyValue(parent.ParentTask, propertyName);
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
        private string valueFromObjectField;
        private string valueFromPropertyField;

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

        [XmlAttributeAttribute()]
        public string valueFromProperty
        {
            get
            {
                return this.valueFromPropertyField;
            }
            set
            {
                this.valueFromPropertyField = value;
            }
        }

        [XmlAttributeAttribute()]
        public string valueFromObject
        {
            get
            {
                return this.valueFromObjectField;
            }
            set
            {
                this.valueFromObjectField = value;
            }
        }

    }
}
