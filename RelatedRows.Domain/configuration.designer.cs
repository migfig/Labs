using DynamicData.Binding;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Serialization;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;
using System.Windows.Controls;

namespace RelatedRows.Domain
{
    public enum eAutoFilter
    {
        Everything,
        TablesWithPrimaryKey,
        MatchingColumnNames
    }

    public enum eViewType
    {
        Datasets,
        Tables,
        Queries
    }

    public enum eViewMode
    {
        Data,
        Schema
    }

    [XmlRoot("Configuration", Namespace="", IsNullable=false)]
    public partial class CConfiguration: AbstractNotifyPropertyChanged {
        
        public CConfiguration() {
            Dataset = new ObservableCollection<CDataset>();
            Datasource = new ObservableCollection<CDatasource>();
        }
        
        [XmlElement("Datasource")]
        public ObservableCollection<CDatasource> Datasource { get; set; }
        
        [XmlElement("Dataset")]
        public ObservableCollection<CDataset> Dataset { get; set; }

        [XmlAttribute()]
        public string defaultDataset { get; set; }

        [XmlAttribute()]
        public string defaultDatasource { get; set; }

        public void Inflate()
        {
            foreach(var source in Datasource)
            {
                //source.ConnectionString = source.ConnectionString.Inflated();
            }
        }

        public void Deflate()
        {
            foreach (var source in Datasource)
            {
                //source.ConnectionString = source.ConnectionString.Deflated();
            }
        }
    }
    
    [XmlRoot(Namespace="", IsNullable=true)]
    public partial class CDatasource: AbstractNotifyPropertyChanged, IXmlSerializable
    {        
        public string ConnectionString
        {
            get {
                if(isTrustedConnection)
                    return string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True;Connect Timeout=120;MultipleActiveResultSets=True;Asynchronous Processing=True;Enlist=false;", _serverName, _databaseName);
                else
                    return string.Format(@"Data Source={0};Initial Catalog={1};User={2};Password={3};Connect Timeout=120;MultipleActiveResultSets=True;Asynchronous Processing=True;Enlist=false;", _serverName, _databaseName, _userName, _password);
            }
            set {
                serverName = GetString(value, "server");
                databaseName = GetString(value, "database");
                userName = GetString(value, "userid");
                password = GetString(value, "password");
                var trusted = GetString(value, "trusted");
                isTrustedConnection = bool.Parse(trusted.Length > 0 ? trusted : "false");
            }
        }

        public CDatasource()
        {
            ConnectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Chinook;Integrated Security=False;
User=sa;Password=1234;Connect Timeout=120;MultipleActiveResultSets=True;Asynchronous Processing=True;Enlist=false;";
        }
        
        [XmlAttribute()]
        public string name { get; set; }

        private string _serverName;
        [XmlIgnore]
        public string serverName
        {
            get { return _serverName; }
            set
            {
                SetAndRaise(ref _serverName, value);
                OnPropertyChanged("IsValid");
            }
        }

        private string _databaseName;
        [XmlIgnore]
        public string databaseName
        {
            get { return _databaseName; }
            set
            {
                SetAndRaise(ref _databaseName, value);
                OnPropertyChanged("IsValid");
            }
        }

        private string _userName;
        [XmlIgnore]
        public string userName
        {
            get { return _userName; }
            set
            {
                SetAndRaise(ref _userName, value);
                OnPropertyChanged("IsValid");
            }
        }

        private string _password;
        [XmlIgnore]
        public string password
        {
            get { return _password; }
            set
            {
                SetAndRaise(ref _password, value);
                OnPropertyChanged("IsValid");
            }
        }

        private bool _isTrustedConnection = false;
        [XmlIgnore]
        public bool isTrustedConnection
        {
            get { return _isTrustedConnection; }
            set
            {
                SetAndRaise(ref _isTrustedConnection,  value);
                OnPropertyChanged("notIsTrustedConnection");
                OnPropertyChanged("IsValid");
            }
        }

        [XmlIgnore]
        public bool notIsTrustedConnection
        {
            get { return !_isTrustedConnection; }
        }

        [XmlIgnore]
        public bool IsValid
        {
            get { return !string.IsNullOrEmpty(serverName)
                    && !string.IsNullOrEmpty(databaseName)
                    && (isTrustedConnection ? true : !string.IsNullOrEmpty(userName)); }
        }

        private string GetString(string value, string group)
        {
            var reg = new Regex(@"(?<source>data\ssource=(?<server>.*?));(?<catalog>initial\scatalog=(?<database>.*?));(?<security>(integrated\ssecurity=(?<trusted>true|false))|(?<user>(user\sid|user)=(?<userid>.*?))?;(?<pwd>password=(?<password>.*?))?);.*?", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var match = reg.Match(value);
            if(match != null && match.Success)
            {
                return match.Groups[group].Value;
            }

            return string.Empty;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            var elem = XElement.ReadFrom(reader) as XElement;
            name = elem.Attribute("name").Value;
            ConnectionString = elem.Element("ConnectionString").Value.Deflated();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("name", name);
            writer.WriteElementString("ConnectionString", ConnectionString.Inflated());
        }
    }
    
    [XmlRoot(Namespace="", IsNullable=true)]
    public partial class CRelationship: AbstractNotifyPropertyChanged {
        [XmlAttribute()]
        public string name { get; set; }

        [XmlAttribute()]
        public string fromTable { get; set; }

        [XmlAttribute()]
        public string toTable { get; set; }

        [XmlElement("ColumnRelationship")]
        public ObservableCollection<CColumnRelationShip> ColumnRelationship { get; set; }

        public CRelationship()
        {
            ColumnRelationship = new ObservableCollection<CColumnRelationShip>();
        }

        public string GetName()
        {
            return string.Format("{0}->{1}{2}", fromTable, toTable,
                string.Join(",", ColumnRelationship.Select(cr => $"{ cr.fromColumn}:{cr.toColumn}")));
        }
    }

    [XmlType(AnonymousType = true)]
    public partial class CColumnRelationShip
    {
        [XmlAttribute()]
        public string fromColumn { get; set; }

        [XmlAttribute()]
        public string toColumn { get; set; }

    }

    [XmlRoot(Namespace="", IsNullable=true)]
    public partial class CColumn : AbstractNotifyPropertyChanged
    {
        [XmlAttribute()]
        public int ordinal { get; set; }

        [XmlAttribute()]
        public string name { get; set; }

        [XmlAttribute()]
        public eDbType DbType { get; set; }

        [XmlAttribute()]
        public int maxLen { get; set; }

        [XmlAttribute()]
        public int precision { get; set; }

        [XmlAttribute()]
        public bool isPrimaryKey { get; set; }

        [XmlAttribute()]
        public bool isForeignKey { get; set; }

        [XmlAttribute()]
        public bool isIdentity { get; set; }

        [XmlAttribute()]
        public bool isNullable { get; set; }

        [XmlAttribute()]
        public string defaultValue { get; set; }

        //[XmlIgnore()]
        //public bool isSelected { get; set; }
    }

    [XmlRoot(Namespace = "", IsNullable = false)]
    public enum eDbType
    {
        bigint,
        binary,
        bit,
        @bool,
        @char,
        date,
        datetime,
        datetime2,
        datetimeoffset,
        @decimal,
        @float,
        geography,
        geometry,
        guid,
        hierarchyid,
        image,
        @int,
        @long,
        money,
        nchar,
        ntext,
        numeric,
        nvarchar,
        real,
        smalldatetime,
        smallint,
        smallmoney,
        sql_variant,
        @string,
        text,
        time,
        timestamp,
        tinyint,
        uniqueidentifier,
        varbinary,
        varchar,
        xml,
        custom
    }
    
    [XmlRoot(Namespace="", IsNullable=true)]
    public partial class CTable: AbstractNotifyPropertyChanged {
        public CTable() {
            Column = new ObservableCollection<CColumn>();
            Children = new ObservableCollection<CTable>();
            Relationship = new ObservableCollection<CRelationship>();
            Pager = new CPager(1, 1);
        }
        
        [XmlIgnore]
        public ObservableCollection<CTable> Children { get; set; }

        [XmlIgnore]
        public ObservableCollection<CRelationship> Relationship { get; set; }

        [XmlElement("Column")]
        public ObservableCollection<CColumn> Column { get; set; }

        private ITabularSource _data;
        [XmlIgnore]
        public ITabularSource Data {
            get
            {
                return _data;
            }
            set
            {
                SetAndRaise(ref _data, value);
            }
        }

        private DataTable _dataTable;
        [XmlIgnore]
        public DataTable DataTable
        {
            get
            {
                return _dataTable;
            }
            set
            {
                SetAndRaise(ref _dataTable, value);
            }
        }

        private CPager _pager;
        [XmlIgnore]
        public CPager Pager
        {
            get { return _pager; }
            set
            {
                SetAndRaise(ref _pager, value);
            }
        }

        [XmlAttribute()]
        public string name { get; set; }

        [XmlAttribute()]
        public string catalog { get; set; }

        [XmlAttribute()]
        public string schemaName { get; set; }

        [XmlIgnore()]
        public string iconName
        {
            get
            {
                return name.Replace("[","").Replace("]", "").Substring(0, 1);
            }            
        }

        [XmlIgnore]
        public bool isDefault { get; set; }

        [XmlIgnore]
        public Visibility DefaultVisibility
        {
            get { return isDefault ? Visibility.Visible : Visibility.Collapsed; }
        }

        [XmlIgnore]
        public Visibility RelationshipVisibility
        {
            get { return Relationship.Any() ? Visibility.Visible : Visibility.Collapsed; }
        }

        public override string ToString()
        {
            return name.Replace("[", "").Replace("]", "");
        }

        #region clipboard handling

        private DataRowView _selectedRow;
        [XmlIgnore]
        public DataRowView SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                SetAndRaise(ref _selectedRow, value);                
            }
        }

        private DataGridCellInfo _selectedViewCell;
        [XmlIgnore]
        public DataGridCellInfo SelectedViewCell
        {
            get { return _selectedViewCell; }
            set
            {
                SetAndRaise(ref _selectedViewCell, value);

                if (SelectedRow != null
                    && SelectedViewCell != null
                    && SelectedViewCell.Column != null
                    && SelectedViewCell.Column.Header != null)
                {                    
                    var column = SelectedViewCell.Column.Header.ToString();
                    SelectedRow = SelectedViewCell.Item as DataRowView;
                    CopyTooltip = this.GetQueryTooltip(column, SelectedRow.Row);
                }
            }
        }

        private string _copyTooltip = string.Empty;
        [XmlIgnore]
        public string CopyTooltip
        {
            get { return _copyTooltip; }
            set
            {
                SetAndRaise(ref _copyTooltip, value);
                OnPropertyChanged("CopyTooltipShort");
            }
        }

        [XmlIgnore]
        public string CopyTooltipShort
        {
            get
            {
                return string.Join(Environment.NewLine,
                    _copyTooltip.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                        .Select(s => s.Substring(0, Math.Min(50, s.Length)) 
                            + (s.Length > 50 ? "..." : "")));
            }           
        }

        #endregion

    }

    public partial class CPager: AbstractNotifyPropertyChanged
    {
        public long RowsCount { get; set; }
        public long Page { get; set; }
        public long Pages { get; set; }
        public long RowsPerPage { get; set; }

        public long Skip { get { return (Page-1) * RowsPerPage; } }

        public bool CanGoFirst { get { return Page > 2; } }
        public bool CanGoPrev { get { return Page > 1; } }
        public bool CanGoNext { get { return Page < Pages; } }
        public bool CanGoLast { get { return Page < Pages-1; } }

        public CPager(long rows, long rowsPerPage)
        {
            Reset(rows, rowsPerPage);
        }

        public void Reset(long rows, long rowsPerPage)
        {
            RowsCount = rows;
            RowsPerPage = rowsPerPage;
            Page = 1;
            Pages = (RowsCount / RowsPerPage) + (RowsCount % RowsPerPage > 0 ? 1 : 0);

            OnPropertyChanged("NextTooltip");
            OnPropertyChanged("PrevTooltip");
            OnPropertyChanged("CanGoFirst");
            OnPropertyChanged("CanGoNext");
            OnPropertyChanged("CanGoPrev");
            OnPropertyChanged("CanGoLast");
            OnPropertyChanged("FirstTooltip");
            OnPropertyChanged("LastTooltip");
            OnPropertyChanged("Visible");
            OnPropertyChanged("EdgesVisible");
        }

        public void Navigate(string direction)
        {
            switch (direction)
            {
                case "first":
                    First();
                    break;
                case "next":
                    Next();
                    break;
                case "prev":
                    Prev();
                    break;
                case "last":
                    Last();
                    break;
            }
            Logger.Log.Verbose("Navigated to page {@page} of {@pages}", Page, Pages);
        }

        public void First()
        {
            Page = 1;
            OnPropertyChanged("NextTooltip");
            OnPropertyChanged("PrevTooltip");
            OnPropertyChanged("CanGoFirst");
            OnPropertyChanged("CanGoNext");
            OnPropertyChanged("CanGoPrev");
            OnPropertyChanged("CanGoLast");
            OnPropertyChanged("Page");
        }

        public void Next()
        {
            if (++Page > Pages) Last();
            OnPropertyChanged("NextTooltip");
            OnPropertyChanged("PrevTooltip");
            OnPropertyChanged("CanGoFirst");
            OnPropertyChanged("CanGoNext");
            OnPropertyChanged("CanGoPrev");
            OnPropertyChanged("CanGoLast");
            OnPropertyChanged("Page");
        }

        public void Prev()
        {
            if (--Page == 0) First();
            OnPropertyChanged("NextTooltip");
            OnPropertyChanged("PrevTooltip");
            OnPropertyChanged("CanGoFirst");
            OnPropertyChanged("CanGoNext");
            OnPropertyChanged("CanGoPrev");
            OnPropertyChanged("CanGoLast");
            OnPropertyChanged("Page");
        }

        public void Last()
        {
            Page = Pages;
            OnPropertyChanged("NextTooltip");
            OnPropertyChanged("PrevTooltip");
            OnPropertyChanged("CanGoFirst");
            OnPropertyChanged("CanGoNext");
            OnPropertyChanged("CanGoPrev");
            OnPropertyChanged("CanGoLast");
            OnPropertyChanged("Page");
        }

        public string FirstTooltip
        {
            get { return $"Page 1 of {Pages}"; }
        }

        public string NextTooltip
        {
            get { return $"Page {Page+1} of {Pages}"; }
        }

        public string PrevTooltip
        {
            get { return $"Page {Page-1} of {Pages}"; }
        }

        public string LastTooltip
        {
            get { return $"Page {Pages} of {Pages}"; }
        }

        public Visibility Visible
        {
            get { return Pages > 1 ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility EdgesVisible
        {
            get { return Pages > 2 ? Visibility.Visible : Visibility.Collapsed; }
        }
    }

    [XmlType(AnonymousType = true)]
    public partial class CQuery: AbstractNotifyPropertyChanged
    {
        public CQuery()
        {
            Parameter = new ObservableCollection<CParameter>();
        }

        [XmlElement("Parameter")]
        public ObservableCollection<CParameter> Parameter { get; set; }

        [XmlText()]
        public string Text { get; set; }

        [XmlIgnore]
        public string FriendlyText {
            get { return Text.Replace("<![CDATA[", string.Empty).Replace("]]>", string.Empty); }
            set { Text = value; }
        }

        [XmlAttribute()]
        public string name { get; set; }

        [XmlAttribute()]
        public string catalog { get; set; }

        [XmlAttribute()]
        public string schemaName { get; set; }

        [XmlAttribute()]
        public bool isStoreProcedure { get; set; }

        [XmlAttribute]
        public string type { get; set; }

        [XmlAttribute]
        public string defaultValue { get; set; }

        [XmlIgnore()]
        public string iconName
        {
            get
            {
                return name.Replace("[", "").Replace("]", "").Substring(0, 1);
            }
        }

        private DataTable _dataTable;
        [XmlIgnore]
        public DataTable DataTable
        {
            get
            {
                return _dataTable;
            }
            set
            {
                SetAndRaise(ref _dataTable, value);
            }
        }

        public IDbDataParameter[] GetParameters()
        {
            return Parameter.Select(p => new SqlParameter(p.name, GetParameterValue(p))).ToArray();
        }

        private object GetParameterValue(CParameter p)
        {
            p.defaultValue = CheckDefaultValue(p.type, p.defaultValue);
            switch (p.type)
            {
                case eDbType.bigint:
                case eDbType.@long:
                    return Convert.ToInt64(p.defaultValue);
                case eDbType.binary:
                case eDbType.image:
                case eDbType.varbinary:
                    var i = 0;
                    var bytes = new byte[p.defaultValue.Length];
                    foreach (var c in p.defaultValue)
                        bytes[i++] = Convert.ToByte(c);
                    return bytes;
                case eDbType.bit:
                case eDbType.@bool:
                    return Convert.ToBoolean(p.defaultValue);
                case eDbType.@char:
                case eDbType.nchar:
                    return Convert.ToChar(p.defaultValue);
                case eDbType.date:
                case eDbType.datetime:
                case eDbType.datetime2:
                case eDbType.datetimeoffset:
                case eDbType.smalldatetime:
                case eDbType.time:
                case eDbType.timestamp:
                    return Convert.ToDateTime(p.defaultValue);
                case eDbType.@decimal:
                case eDbType.@float:
                case eDbType.money:
                case eDbType.numeric:
                case eDbType.real:
                case eDbType.smallmoney:
                    return Convert.ToDecimal(p.defaultValue);
                case eDbType.geography:
                case eDbType.geometry:
                case eDbType.hierarchyid:
                    return p.defaultValue;
                case eDbType.@int:
                    return Convert.ToInt32(p.defaultValue);
                case eDbType.guid:
                case eDbType.uniqueidentifier:
                case eDbType.ntext:
                case eDbType.nvarchar:
                case eDbType.@string:
                case eDbType.text:
                case eDbType.varchar:
                case eDbType.sql_variant:
                case eDbType.xml:
                    return Convert.ToString(p.defaultValue);
                case eDbType.smallint:
                case eDbType.tinyint:
                    return Convert.ToInt16(p.defaultValue);
            }

            return p.defaultValue;
        }

        private string CheckDefaultValue(eDbType type, string value)
        {
            if (!string.IsNullOrEmpty(value)) return value;
            switch (type)
            {
                case eDbType.binary:
                case eDbType.image:
                case eDbType.varbinary:
                    return default(byte).ToString();
                case eDbType.bit:
                case eDbType.@bool:
                    return default(bool).ToString();
                case eDbType.@char:
                case eDbType.nchar:
                case eDbType.ntext:
                case eDbType.nvarchar:
                case eDbType.@string:
                case eDbType.text:
                case eDbType.varchar:
                case eDbType.sql_variant:
                case eDbType.xml:
                case eDbType.geography:
                case eDbType.geometry:
                case eDbType.hierarchyid:
                    return default(string);
                case eDbType.date:
                case eDbType.datetime:
                case eDbType.datetime2:
                case eDbType.datetimeoffset:
                case eDbType.smalldatetime:
                case eDbType.time:
                case eDbType.timestamp:
                    return default(DateTime).ToString();
                case eDbType.@decimal:
                case eDbType.@float:
                case eDbType.money:
                case eDbType.numeric:
                case eDbType.real:
                case eDbType.smallmoney:
                    return default(float).ToString();
                case eDbType.guid:
                case eDbType.uniqueidentifier:
                    return default(Guid).ToString();                
                case eDbType.smallint:
                case eDbType.tinyint:
                    return default(Int16).ToString();
                case eDbType.@int:
                    return default(int).ToString();
                case eDbType.bigint:
                case eDbType.@long:
                    return default(long).ToString();
            }

            return default(string);
        }

        [XmlIgnore]
        public string key
        {
            get
            {
                return string.Format("{0}-{1}", name,
                    string.Join(",", Parameter.Select(p => $"{p.name}:{p.defaultValue}")));
            }
        }

        public override string ToString()
        {
            return name.Replace("[", "").Replace("]", "");
        }

        public static CQuery Clone(CQuery other)
        {
            return new CQuery
            {
                name = other.name,
                catalog = other.catalog,
                schemaName = other.schemaName,
                type = other.type,
                Parameter = new ObservableCollection<CParameter>(
                    other.Parameter.Select(p => new CParameter
                    {
                        name = p.name,
                        type = p.type,
                        length = p.length,
                        defaultValue = p.defaultValue
                    }))
            };
        }
    }

    [XmlRoot()]
    public partial class CParameter: AbstractNotifyPropertyChanged, IXmlSerializable
    {
        [XmlAttribute()]
        public string name { get; set; }

        [XmlAttribute()]
        public eDbType type { get; set; }

        [XmlAttribute()]
        public string customType { get; set; }

        [XmlAttribute()]
        public int length { get; set; }

        private string _defaultValue;
        [XmlAttribute()]
        public string defaultValue {
            get { return _defaultValue; }
            set { SetAndRaise(ref _defaultValue, value); }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            var elem = XElement.ReadFrom(reader) as XElement;
            name = elem.Attribute("name").Value;
            length = int.Parse(elem.Attribute("length").Value);

            if (elem.Attribute("defaultValue") != null)
                defaultValue = elem.Attribute("defaultValue").Value;
            var attType = elem.Attribute("type").Value;
            try
            {
                type = (eDbType)Enum.Parse(typeof(eDbType), attType);
            }
            catch(Exception)
            {
                type = eDbType.custom;
                customType = attType;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("name", name);
            writer.WriteAttributeString("type", type.ToString());
            writer.WriteAttributeString("length", length.ToString());
            writer.WriteAttributeString("defaultValue", defaultValue);
            writer.WriteAttributeString("customType", customType);
        }
    }

    [XmlRoot(Namespace = "", IsNullable = true)]
    public partial class CDataset : AbstractNotifyPropertyChanged
    {
        public CDataset() {
            Relationship = new ObservableCollection<CRelationship>();
            Table = new ObservableCollection<CTable>();
            Query = new ObservableCollection<CQuery>();
            QueryHistory = new ObservableCollection<CQuery>();
        }

        [XmlElement("Table")]
        public ObservableCollection<CTable> Table { get; set; }

        [XmlElement("Query")]
        public ObservableCollection<CQuery> Query { get; set; }

        [XmlIgnore]
        public ObservableCollection<CQuery> QueryHistory { get; set; }

        [XmlElement("Relationship")]
        public ObservableCollection<CRelationship> Relationship { get; set; }

        [XmlAttribute()]
        public string name { get; set; }

        [XmlAttribute()]
        public string dataSourceName { get; set; }

        [XmlAttribute()]
        public string defaultTable { get; set; }

        [XmlAttribute()]
        public bool isDisabled { get; set; }

        private bool _isSelected;
        [XmlIgnore]
        public bool isSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
                OnPropertyChanged("SelectedVisibility");
                OnPropertyChanged("ColorZone");
            }
        }

        [XmlIgnore]
        public ColorZoneMode ColorZone
        {
            get { return isSelected ? ColorZoneMode.Accent : ColorZoneMode.Standard; }
        }

        [XmlIgnore]
        public Visibility SelectedVisibility
        {
            get { return isSelected ? Visibility.Visible : Visibility.Collapsed; }
        }

        private bool _isDefault;
        [XmlIgnore]
        public bool isDefault
        {
            get { return _isDefault; }
            set
            {
                _isDefault = value;
                OnPropertyChanged();
                OnPropertyChanged("DefaultVisibility");
            }
        }

        [XmlIgnore]
        public Visibility DefaultVisibility
        {
            get { return isDefault ? Visibility.Visible : Visibility.Collapsed; }
        }
    }
}
