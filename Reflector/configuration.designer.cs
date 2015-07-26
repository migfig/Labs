using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Reflector {
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.38968")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.megapanezote.com")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.megapanezote.com", IsNullable=false)]
    public partial class Configuration : NotificationBase {
        private ObservableCollection<Datasource> datasourcesField;

        public Configuration() {
            this.datasourcesField = new ObservableCollection<Datasource>();
        }
       
        [System.Xml.Serialization.XmlArrayAttribute(Order=1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Source", IsNullable=false)]
        public ObservableCollection<Datasource> Datasources {
            get {
                return this.datasourcesField;
            }
            set {
                if ((this.datasourcesField != null)) {
                    if ((datasourcesField.Equals(value) != true)) {
                        this.datasourcesField = value;
                        this.OnPropertyChanged("Datasources");
                    }
                }
                else {
                    this.datasourcesField = value;
                    this.OnPropertyChanged("Datasources");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.38968")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.megapanezote.com")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.megapanezote.com", IsNullable=true)]
    public partial class SettingSource : NotificationBase
    {
        
        private string nameField;
        
        private string valueField;
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                if ((this.nameField != null)) {
                    if ((nameField.Equals(value) != true)) {
                        this.nameField = value;
                        this.OnPropertyChanged("Name");
                    }
                }
                else {
                    this.nameField = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                if ((this.valueField != null)) {
                    if ((valueField.Equals(value) != true)) {
                        this.valueField = value;
                        this.OnPropertyChanged("Value");
                    }
                }
                else {
                    this.valueField = value;
                    this.OnPropertyChanged("Value");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.38968")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.megapanezote.com")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.megapanezote.com", IsNullable=true)]
    public partial class TextSource : NotificationBase
    {
        
        private ObservableCollection<TextItem> itemField;
        
        public TextSource() {
            this.itemField = new ObservableCollection<TextItem>();
        }
        
        [System.Xml.Serialization.XmlElementAttribute("Item", Order=0)]
        public ObservableCollection<TextItem> Item {
            get {
                return this.itemField;
            }
            set {
                if ((this.itemField != null)) {
                    if ((itemField.Equals(value) != true)) {
                        this.itemField = value;
                        this.OnPropertyChanged("Item");
                    }
                }
                else {
                    this.itemField = value;
                    this.OnPropertyChanged("Item");
                }
            }
        }
    }    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.38968")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.megapanezote.com")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.megapanezote.com", IsNullable=true)]
    public partial class TextItem : NotificationBase
    {
        
        private string valueField;
        private string nameField;
        private string tagField;
        private bool enabledField = true;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                if ((this.nameField != null))
                {
                    if ((nameField.Equals(value) != true))
                    {
                        this.nameField = value;
                        this.OnPropertyChanged("Name");
                    }
                }
                else
                {
                    this.nameField = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }
        
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                if ((this.valueField != null)) {
                    if ((valueField.Equals(value) != true)) {
                        this.valueField = value;
                        this.OnPropertyChanged("Value");
                    }
                }
                else {
                    this.valueField = value;
                    this.OnPropertyChanged("Value");
                }
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Tag
        {
            get
            {
                return this.tagField;
            }
            set
            {
                if ((this.tagField != null))
                {
                    if ((tagField.Equals(value) != true))
                    {
                        this.tagField = value;
                        this.OnPropertyChanged("Tag");
                    }
                }
                else
                {
                    this.tagField = value;
                    this.OnPropertyChanged("Tag");
                }
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool Enabled
        {
            get
            {
                return this.enabledField;
            }
            set
            {
                if ((enabledField.Equals(value) != true))
                {
                    this.enabledField = value;
                    this.OnPropertyChanged("Enabled");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.38968")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.megapanezote.com")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.megapanezote.com", IsNullable = true)]
    public partial class TupleSource : NotificationBase
    {
        private string keyField;

        private string valueField;

        private string item1Field;
        private string item2Field;
        private string item3Field;
        private string item4Field;
        private string item5Field;
        private string item6Field;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                if ((this.keyField != null))
                {
                    if ((keyField.Equals(value) != true))
                    {
                        this.keyField = value;
                        this.OnPropertyChanged("Key");
                    }
                }
                else
                {
                    this.keyField = value;
                    this.OnPropertyChanged("Key");
                }
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                if ((this.valueField != null))
                {
                    if ((valueField.Equals(value) != true))
                    {
                        this.valueField = value;
                        this.OnPropertyChanged("Value");
                    }
                }
                else
                {
                    this.valueField = value;
                    this.OnPropertyChanged("Value");
                }
            }
        }        

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Item1
        {
            get
            {
                return this.item1Field;
            }
            set
            {
                if ((this.item1Field != null))
                {
                    if ((item1Field.Equals(value) != true))
                    {
                        this.item1Field = value;
                        this.OnPropertyChanged("Item1");
                    }
                }
                else
                {
                    this.item1Field = value;
                    this.OnPropertyChanged("Item1");
                }
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Item2
        {
            get
            {
                return this.item2Field;
            }
            set
            {
                if ((this.item2Field != null))
                {
                    if ((item2Field.Equals(value) != true))
                    {
                        this.item2Field = value;
                        this.OnPropertyChanged("Item2");
                    }
                }
                else
                {
                    this.item2Field = value;
                    this.OnPropertyChanged("Item2");
                }
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Item3
        {
            get
            {
                return this.item3Field;
            }
            set
            {
                if ((this.item3Field != null))
                {
                    if ((item3Field.Equals(value) != true))
                    {
                        this.item3Field = value;
                        this.OnPropertyChanged("Item3");
                    }
                }
                else
                {
                    this.item3Field = value;
                    this.OnPropertyChanged("Item3");
                }
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Item4
        {
            get
            {
                return this.item4Field;
            }
            set
            {
                if ((this.item4Field != null))
                {
                    if ((item4Field.Equals(value) != true))
                    {
                        this.item4Field = value;
                        this.OnPropertyChanged("Item4");
                    }
                }
                else
                {
                    this.item4Field = value;
                    this.OnPropertyChanged("Item4");
                }
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Item5
        {
            get
            {
                return this.item5Field;
            }
            set
            {
                if ((this.item5Field != null))
                {
                    if ((item5Field.Equals(value) != true))
                    {
                        this.item5Field = value;
                        this.OnPropertyChanged("Item5");
                    }
                }
                else
                {
                    this.item5Field = value;
                    this.OnPropertyChanged("Item5");
                }
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Item6
        {
            get
            {
                return this.item6Field;
            }
            set
            {
                if ((this.item6Field != null))
                {
                    if ((item6Field.Equals(value) != true))
                    {
                        this.item6Field = value;
                        this.OnPropertyChanged("Item6");
                    }
                }
                else
                {
                    this.item6Field = value;
                    this.OnPropertyChanged("Item6");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.38968")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.megapanezote.com")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.megapanezote.com", IsNullable=true)]
    public partial class Datasource : NotificationBase
    {
        
        private ObservableCollection<object> itemsField;
        
        private string nameField;
        
        private enSourceType typeField;
        
        private bool isReservedField;
        private string basedOnField;
        
        public Datasource() {
            this.itemsField = new ObservableCollection<object>();
            this.isReservedField = false;
        }
        
        [System.Xml.Serialization.XmlElementAttribute("Setting", typeof(SettingSource), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("Text", typeof(TextSource), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("Dictionary", typeof(TupleSource), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("GroupedText", typeof(GroupSource), Order = 0)]
        public ObservableCollection<object> Items {
            get {
                return this.itemsField;
            }
            set {
                if ((this.itemsField != null)) {
                    if ((itemsField.Equals(value) != true)) {
                        this.itemsField = value;
                        this.OnPropertyChanged("Items");
                    }
                }
                else {
                    this.itemsField = value;
                    this.OnPropertyChanged("Items");
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                if ((this.nameField != null)) {
                    if ((nameField.Equals(value) != true)) {
                        this.nameField = value;
                        this.OnPropertyChanged("Name");
                    }
                }
                else {
                    this.nameField = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public enSourceType Type {
            get {
                return this.typeField;
            }
            set {
                if ((typeField.Equals(value) != true)) {
                    this.typeField = value;
                    this.OnPropertyChanged("Type");
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsReserved {
            get {
                return this.isReservedField;
            }
            set {
                if ((isReservedField.Equals(value) != true)) {
                    this.isReservedField = value;
                    this.OnPropertyChanged("IsReserved");
                }
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string BasedOn
        {
            get
            {
                return this.basedOnField;
            }
            set
            {
                if ((this.basedOnField != null))
                {
                    if ((basedOnField.Equals(value) != true))
                    {
                        this.basedOnField = value;
                        this.OnPropertyChanged("BasedOn");
                    }
                }
                else
                {
                    this.basedOnField = value;
                    this.OnPropertyChanged("BasedOn");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.38968")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.megapanezote.com")]
    public enum enSourceType {
        
        /// <remarks/>
        TextList,
        
        /// <remarks/>
        Settings,

        /// <remarks/>
        DictionaryTuple,

        /// <remarks/>
        GroupedTextList,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.38968")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.megapanezote.com")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.megapanezote.com", IsNullable = true)]
    public partial class GroupSource : NotificationBase
    {

        private ObservableCollection<TextItem> textField;

        private ObservableCollection<GroupExpression> textExpressionField;

        private string nameField;

        public GroupSource()
        {
            this.textExpressionField = new ObservableCollection<GroupExpression>();
            this.textField = new ObservableCollection<TextItem>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Text", Order = 0)]
        public ObservableCollection<TextItem> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                if ((this.textField != null))
                {
                    if ((textField.Equals(value) != true))
                    {
                        this.textField = value;
                        this.OnPropertyChanged("Text");
                    }
                }
                else
                {
                    this.textField = value;
                    this.OnPropertyChanged("Text");
                }
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("TextExpression", Order = 1)]
        public ObservableCollection<GroupExpression> TextExpression
        {
            get
            {
                return this.textExpressionField;
            }
            set
            {
                if ((this.textExpressionField != null))
                {
                    if ((textExpressionField.Equals(value) != true))
                    {
                        this.textExpressionField = value;
                        this.OnPropertyChanged("TextExpression");
                    }
                }
                else
                {
                    this.textExpressionField = value;
                    this.OnPropertyChanged("TextExpression");
                }
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                if ((this.nameField != null))
                {
                    if ((nameField.Equals(value) != true))
                    {
                        this.nameField = value;
                        this.OnPropertyChanged("Name");
                    }
                }
                else
                {
                    this.nameField = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.38968")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.megapanezote.com")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.megapanezote.com", IsNullable = true)]
    public partial class GroupExpression : NotificationBase
    {

        private TextItem expressionField;

        private ObservableCollection<TextItem> textField;

        private string nameField;

        public GroupExpression()
        {
            this.textField = new ObservableCollection<TextItem>();
            this.expressionField = new TextItem();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public TextItem Expression
        {
            get
            {
                return this.expressionField;
            }
            set
            {
                if ((this.expressionField != null))
                {
                    if ((expressionField.Equals(value) != true))
                    {
                        this.expressionField = value;
                        this.OnPropertyChanged("Expression");
                    }
                }
                else
                {
                    this.expressionField = value;
                    this.OnPropertyChanged("Expression");
                }
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Text", Order = 1)]
        public ObservableCollection<TextItem> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                if ((this.textField != null))
                {
                    if ((textField.Equals(value) != true))
                    {
                        this.textField = value;
                        this.OnPropertyChanged("Text");
                    }
                }
                else
                {
                    this.textField = value;
                    this.OnPropertyChanged("Text");
                }
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                if ((this.nameField != null))
                {
                    if ((nameField.Equals(value) != true))
                    {
                        this.nameField = value;
                        this.OnPropertyChanged("Name");
                    }
                }
                else
                {
                    this.nameField = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }
    }

    public class NotificationBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if ((handler != null))
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged implementation
    }

}
