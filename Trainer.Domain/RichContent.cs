using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Trainer.Domain
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class Presentation
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public string Image { get; set; }

        [XmlElement("Slide")]
        public ObservableCollection<Slide> Slide { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public IEnumerable<Slide> FewSlide { get { return Slide.Take(6); } }

        public Presentation()
        {
            Slide = new ObservableCollection<Slide>();
        }

        [JsonIgnore]
        [XmlIgnore]
        public string Xml
        {
            get
            {
                if(Slide.Any())
                {
                    var ser = new XmlSerializer(GetType());
                    var builder = new StringBuilder();
                    using(var stream = new StringWriter(builder))
                    {
                        ser.Serialize(stream, this);
                        return builder.ToString();
                    }
                }

                return string.Empty;
            }
        }

        #region should serialize

        public bool ShouldSerializeId()
        {
            return !string.IsNullOrEmpty(Id); 
        }

        public bool ShouldSerializeImage()
        {
            return !string.IsNullOrEmpty(Image); 
        }

        #endregion
    }

    [XmlType(AnonymousType = true)]
    public partial class Slide: BaseModel
    {
        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public string Image { get; set; }

        [XmlAttribute]
        public string Margin { get; set; }

        [XmlAttribute]
        public string Padding { get; set; }

        [XmlElement("RichTextBlock", Order = 0)]
        public ObservableCollection<RichTextBlock> Block { get; set; }

        [XmlElement("Component", Order = 1)]
        public ObservableCollection<Component> Component { get; set; }

        private string _markDown;
        [JsonIgnore]
        [XmlIgnore]
        public string Markdown {
            get { return _markDown; }
            set {
                _markDown = value;
                OnPropertyChanged();
            }
        }

        public Slide()
        {
            Block = new ObservableCollection<RichTextBlock>();
            Component = new ObservableCollection<Domain.Component>();
        }

        #region serialization control

        public bool ShouldSerializeImage()
        {
            return !string.IsNullOrEmpty(Image);
        }

        public bool ShouldSerializeMargin()
        {
            return !string.IsNullOrEmpty(Margin);
        }

        public bool ShouldSerializePadding()
        {
            return !string.IsNullOrEmpty(Padding);
        }

        public bool ShouldSerializeComponent()
        {
            return Component != null && Component.Any();
        }

        #endregion
    }

    [XmlType(AnonymousType = true)]
    public partial class RichTextBlock: Run
    {
        [XmlElement("Paragraph")]
        public ObservableCollection<Paragraph> Paragraph { get; set; }
        
        [XmlAttribute()]
        [DefaultValue("#FAFBFCFF")]
        public string SelectionHighlightColor { get; set; }

        [XmlAttribute()]
        [DefaultValue(true)]
        public bool IsTextSelectionEnabled { get; set; }

        [XmlAttribute()]
        [DefaultValue(typeof(byte), "10")]
        public byte MaxLines { get; set; }

        [XmlAttribute()]
        [DefaultValue("Wrap")]
        public string TextWrapping { get; set; }

        [XmlAttribute("Typography.Capitals")]
        [DefaultValue("True")]
        public string TypographyCapitals { get; set; }

        public RichTextBlock()
        {
            Paragraph = new ObservableCollection<Paragraph>();
        }

        #region should serialize

        public bool ShouldSerializeSelectionHighlightColor()
        {
            return !string.IsNullOrEmpty(SelectionHighlightColor);
        }

        public bool ShouldSerializeMaxLines()
        {
            return MaxLines > 0; 
        }

        public bool ShouldSerializeTextWrapping()
        {
            return !string.IsNullOrEmpty(TextWrapping); 
        }

        public bool ShouldSerializeTypographyCapitals()
        {
            return !string.IsNullOrEmpty(TypographyCapitals); 
        }

        #endregion
    }

    [XmlType(AnonymousType = true)]
    public partial class Paragraph: Run
    {
        [XmlElement("InlineUIContainer")]
        public InlineUIContainer InlineUIContainer { get; set; }

        [XmlElement("Bold")]
        public ObservableCollection<Bold> Bold { get; set; }

        [XmlElement("Run")]
        public ObservableCollection<Run> Run { get; set; }

        [XmlElement("LineBreak")]
        public ObservableCollection<LineBreak> LineBreak { get; set; }

        [XmlElement("Hyperlink")]
        public HyperLink Hyperlink { get; set; }

        [XmlText()]
        public string[] Text { get; set; }

        public Paragraph()
        {
            Bold = new ObservableCollection<Domain.Bold>();
            Run = new ObservableCollection<Domain.Run>();
            LineBreak = new ObservableCollection<Domain.LineBreak>();
        }

        #region should serialize

        public bool ShouldSerializeInlineUIContainer()
        {
            return InlineUIContainer != null; 
        }

        public bool ShouldSerializeBold()
        {
            return Bold != null && Bold.Any(); 
        }

        public bool ShouldSerializeRun()
        {
            return Run != null && Run.Any(); 
        }

        public bool ShouldSerializeLineBreak()
        {
            return LineBreak != null && LineBreak.Any(); 
        }

        public bool ShouldSerializeHyperlink()
        {
            return Hyperlink != null; 
        }

        public bool ShouldSerializeText()
        {
            return Text != null && Text.Any(); 
        }
        
        #endregion
    }

    [XmlType(AnonymousType = true)]
    public partial class InlineUIContainer
    {
        [XmlElement("Image")]
        public Image Image { get; set; }
    }
    
    [XmlType(AnonymousType = true)]
    public partial class Image
    {
        [XmlAttribute()]
        public string Source { get; set; }
        
        [XmlAttribute()]
        public byte Width { get; set; }

        
        [XmlAttribute()]
        public byte Height { get; set; }
        
        [XmlAttribute()]
        [DefaultValue("Fill")]
        public string Stretch { get; set; }

        public Image()
        {
        }
    }

    [XmlType(AnonymousType = true)]
    public partial class LineBreak : Run
    {
        public LineBreak() : base()
        {
        }
    }

    [XmlType(AnonymousType = true)]
    public partial class Bold: Run
    {
        public Bold(): base()
        {
        }
    }
    
    [XmlType(AnonymousType = true)]
    public partial class Run: UIElement
    {
        [XmlAttribute()]
        [DefaultValue("Normal")]
        public string FontStyle { get; set; }
        
        [XmlAttribute()]
        [DefaultValue("Normal")]
        public string FontWeight { get; set; }
        
        [XmlAttribute()]
        [DefaultValue(typeof(byte) ,"0")]
        public byte CharacterSpacing { get; set; }
        
        [XmlAttribute()]
        [DefaultValue(typeof(byte), "34")]
        public byte FontSize { get; set; }
        
        [XmlAttribute()]
        [DefaultValue("Normal")]
        public string FontStretch { get; set; }
        
        [XmlAttribute()]
        [DefaultValue("#FF000000")]
        public string Foreground { get; set; }
        
        [XmlAttribute("Typography.CapitalSpacing")]
        [DefaultValue("True")]
        public string TypographyCapitalSpacing { get; set; }
        
        [XmlText()]
        public string Value { get; set; }

        public Run(): base()
        {
        }

        #region should serialize

        public bool ShouldSerializeFontStyle()
        {
            return !string.IsNullOrEmpty(FontStyle); 
        }

        public bool ShouldSerializeFontWeight()
        {
            return !string.IsNullOrEmpty(FontWeight); 
        }

        public bool ShouldSerializeCharacterSpacing()
        {
            return CharacterSpacing > 0; 
        }

        public bool ShouldSerializeFontSize()
        {
            return FontSize > 0;
        }

        public bool ShouldSerializeFontStretch()
        {
            return !string.IsNullOrEmpty(FontStretch); 
        }

        public bool ShouldSerializeForeground()
        {
            return !string.IsNullOrEmpty(Foreground); 
        }

        public bool ShouldSerializeTypographyCapitalSpacing()
        {
            return !string.IsNullOrEmpty(TypographyCapitalSpacing); 
        }

        public bool ShouldSerializeValue()
        {
            return !string.IsNullOrEmpty(Value); 
        }

        #endregion
    }
    
    [XmlType(AnonymousType = true)]
    public partial class HyperLink
    {
        [XmlAttribute()]
        public string NavigateUri { get; set; }
        
        [XmlText()]
        public string Value { get; set; }
    }

    public class UIElement: Block
    {
        [XmlAttribute()]
        [DefaultValue("0")]
        public string Margin { get; set; }

        [XmlAttribute()]
        [DefaultValue(typeof(decimal), "1.0")]
        public decimal Opacity { get; set; }

        [XmlAttribute()]
        [DefaultValue("0")]
        public string Padding { get; set; }

        [XmlAttribute()]
        [DefaultValue("Justify")]
        public string TextAlignment { get; set; }

        [XmlAttribute()]
        [DefaultValue("Stretch")]
        public string HorizontalAlignment { get; set; }

        [XmlAttribute()]
        [DefaultValue("Stretch")]
        public string VerticalAlignment { get; set; }

        [XmlAttribute()]
        [DefaultValue("Visible")]
        public string Visibility { get; set; }

        public UIElement(): base()
        {
        }

        #region should serialize

        public bool ShouldSerializeMargin()
        {
            return !string.IsNullOrEmpty(Margin); 
        }

        public bool ShouldSerializeOpacity()
        {
            return Opacity > 0.0M; 
        }

        public bool ShouldSerializePadding()
        {
            return !string.IsNullOrEmpty(Padding); 
        }

        public bool ShouldSerializeTextAlignment()
        {
            return !string.IsNullOrEmpty(TextAlignment); 
        }

        public bool ShouldSerializeHorizontalAlignment()
        {
            return !string.IsNullOrEmpty(HorizontalAlignment); 
        }

        public bool ShouldSerializeVerticalAlignment()
        {
            return !string.IsNullOrEmpty(VerticalAlignment); 
        }

        public bool ShouldSerializeVisibility()
        {
            return !string.IsNullOrEmpty(Visibility); 
        }

        #endregion
    }

    public class Block
    {
        [XmlAttribute()]
        [DefaultValue(typeof(byte), "20")]
        public byte LineHeight { get; set; }

        [XmlAttribute()]
        [DefaultValue("BlockLineHeight")]
        public string LineStackingStrategy { get; set; }

        [XmlAttribute()]
        [DefaultValue(typeof(byte), "0")]
        public byte TextIndent { get; set; }

        [JsonIgnore]
        [XmlIgnore()]
        [DefaultValue(false)]
        public bool TextIndentSpecified { get; set; }

        #region should serialize

        public bool ShouldSerializeLineHeight()
        {
            return LineHeight > 0; 
        }

        public bool ShouldSerializeLineStackingStrategy()
        {
            return !string.IsNullOrEmpty(LineStackingStrategy); 
        }

        public bool ShouldSerializeTextIndent()
        {
            return TextIndent > 0; 
        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
    {
        public NotifyPropertyChangedInvocatorAttribute() { }
        public NotifyPropertyChangedInvocatorAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        public string ParameterName { get; private set; }
    }

    public abstract class BaseModel : INotifyPropertyChanged
    {
        #region property changed handler

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion property changed handler    
    }
}
