// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.4.0.32989
//    <NameSpace>RelatedRecords</NameSpace><Collection>ObservableCollection</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><EnableLazyLoading>False</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>False</HidePrivateFieldInIDE><EnableSummaryComment>False</EnableSummaryComment><VirtualProp>False</VirtualProp><IncludeSerializeMethod>False</IncludeSerializeMethod><UseBaseClass>False</UseBaseClass><GenBaseClass>False</GenBaseClass><GenerateCloneMethod>False</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><CodeBaseTag>Net35</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>True</GenerateXMLAttributes><OrderXMLAttrib>False</OrderXMLAttrib><EnableEncoding>False</EnableEncoding><AutomaticProperties>False</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>False</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>UTF8</Encoder><CustomUsings></CustomUsings><ExcludeIncludedTypes>False</ExcludeIncludedTypes><EnableInitializeFields>True</EnableInitializeFields>
//  </auto-generated>
// ------------------------------------------------------------------------------
namespace RelatedRecords
{
    using System.ComponentModel;
    using System.Collections.ObjectModel;
    using System;
    using System.Runtime.CompilerServices;
    using Common;
    using System.Linq;

    public enum eAutoFilter
    {
        Everything,
        TablesWithPrimaryKey,
        MatchingColumnNames
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.32990")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Configuration", Namespace="", IsNullable=false)]
    public partial class CConfiguration: BaseModel {
        
        private ObservableCollection<CDatasource> datasourceField;
        private ObservableCollection<CDataset> datasetField;
        private string defaultDatasetField;
        private string defaultDatasourceField;

        public CConfiguration() {
            this.datasetField = new ObservableCollection<CDataset>();
            this.datasourceField = new ObservableCollection<CDatasource>();
        }
        
        [System.Xml.Serialization.XmlElementAttribute("Datasource")]
        public ObservableCollection<CDatasource> Datasource {
            get {
                return this.datasourceField;
            }
            set {
                this.datasourceField = value;
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute("Dataset")]
        public ObservableCollection<CDataset> Dataset {
            get {
                return this.datasetField;
            }
            set {
                this.datasetField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string defaultDataset
        {
            get
            {
                return this.defaultDatasetField;
            }
            set
            {
                this.defaultDatasetField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string defaultDatasource
        {
            get
            {
                return this.defaultDatasourceField;
            }
            set
            {
                this.defaultDatasourceField = value;
            }
        }

        public void Inflate()
        {
            foreach(var source in Datasource)
            {
                source.ConnectionString = source.ConnectionString.Inflated();
            }
        }

        public void Deflate()
        {
            foreach (var source in Datasource)
            {
                source.ConnectionString = source.ConnectionString.Deflated();
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.32990")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    public partial class CDatasource: BaseModel {
        
        private string connectionStringField;
        
        private string nameField;
        
        public string ConnectionString {
            get {
                return this.connectionStringField;
            }
            set {
                this.connectionStringField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.32990")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    public partial class CRelationship: BaseModel {
        
        private string nameField;
        
        private string fromTableField;
        
        private string toTableField;
        
        private string fromColumnField;
        
        private string toColumnField;
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string fromTable {
            get {
                return this.fromTableField;
            }
            set {
                this.fromTableField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string toTable {
            get {
                return this.toTableField;
            }
            set {
                this.toTableField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string fromColumn {
            get {
                return this.fromColumnField;
            }
            set {
                this.fromColumnField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string toColumn {
            get {
                return this.toColumnField;
            }
            set {
                this.toColumnField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.32990")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    public partial class CColumn : BaseModel
    {
        
        private string nameField;
        
        private eDbType dbTypeField;
        
        private bool isPrimaryKeyField;
        
        private bool isForeignKeyField;
        
        private bool isNullableField;
        
        private string defaultValueField;
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public eDbType DbType {
            get {
                return this.dbTypeField;
            }
            set {
                this.dbTypeField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isPrimaryKey {
            get {
                return this.isPrimaryKeyField;
            }
            set {
                this.isPrimaryKeyField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isForeignKey {
            get {
                return this.isForeignKeyField;
            }
            set {
                this.isForeignKeyField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isNullable {
            get {
                return this.isNullableField;
            }
            set {
                this.isNullableField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string defaultValue {
            get {
                return this.defaultValueField;
            }
            set {
                this.defaultValueField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.32990")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public enum eDbType
    {
        /// <remarks/>
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
        xml
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.32990")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    public partial class CTable: BaseModel {
        
        private ObservableCollection<CColumn> columnField;
        private ObservableCollection<CTable> childrenField;
        private string nameField;
        
        public CTable() {
            this.columnField = new ObservableCollection<CColumn>();
            this.childrenField = new ObservableCollection<CTable>();
        }
        
        [System.Xml.Serialization.XmlIgnore]
        public ObservableCollection<CTable> Children
        {
            get { return this.childrenField; }
            set { this.childrenField = value; }
        }

        [System.Xml.Serialization.XmlElementAttribute("Column")]
        public ObservableCollection<CColumn> Column {
            get {
                return this.columnField;
            }
            set {
                this.columnField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }

        public override string ToString()
        {
            return this.name.ToUpper() + Environment.NewLine + Environment.NewLine + "Columns: " + Column.Count.ToString();
        }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class CQuery
    {
        private ObservableCollection<CParameter> parameterField;
        private string textField;
        private string nameField;
        private bool isStoreProcedureField;

        public CQuery()
        {
            this.parameterField = new ObservableCollection<CParameter>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Parameter")]
        public ObservableCollection<CParameter> Parameter
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
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isStoreProcedure
        {
            get
            {
                return this.isStoreProcedureField;
            }
            set
            {
                this.isStoreProcedureField = value;
            }
        }

        public override string ToString()
        {
            return this.name.ToUpper() + Environment.NewLine + Environment.NewLine + "Parameters: " + Parameter.Count.ToString();
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class CParameter
    {
        private string nameField;
        private string typeField;
        private string defaultValueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.32990")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    public partial class CDataset : BaseModel
    {
        
        private ObservableCollection<CTable> tableField;
        private ObservableCollection<CRelationship> relationshipField;
        private ObservableCollection<CQuery> queryField;

        private string nameField;
        private string dataSourceNameField;
        private string defaultTableField;
        private bool isDisabledField;

        public CDataset() {
            this.relationshipField = new ObservableCollection<CRelationship>();
            this.tableField = new ObservableCollection<CTable>();
            this.queryField = new ObservableCollection<CQuery>();
        }
        
        [System.Xml.Serialization.XmlElementAttribute("Table")]
        public ObservableCollection<CTable> Table {
            get {
                return this.tableField;
            }
            set {
                this.tableField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Query")]
        public ObservableCollection<CQuery> Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }


        [System.Xml.Serialization.XmlElementAttribute("Relationship")]
        public ObservableCollection<CRelationship> Relationship {
            get {
                return this.relationshipField;
            }
            set {
                this.relationshipField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
                OnPropertyChanged();
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataSourceName {
            get {
                return this.dataSourceNameField;
            }
            set {
                this.dataSourceNameField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string defaultTable
        {
            get
            {
                return this.defaultTableField;
            }
            set
            {
                this.defaultTableField = value;
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

        public override string ToString()
        {
            return this.name.ToUpper() + Environment.NewLine + Environment.NewLine + "Tables: " + Table.Count.ToString();
        }
    }
}
