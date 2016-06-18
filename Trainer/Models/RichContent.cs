using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using Template10.Mvvm;

namespace Trainer.Models
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class Presentation: BindableBase
    {
        [XmlAttribute]
        public string Title { get; set; }

        [XmlElement("Slide")]
        public ObservableCollection<Slide> Slide { get; set; }

        public Presentation()
        {
            Slide = new ObservableCollection<Slide>();
        }
    }

    [XmlType(AnonymousType = true)]
    public partial class Slide: BindableBase
    {
        [XmlAttribute]
        public string Title { get; set; }

        [XmlElement("RichTextBlock")]
        public ObservableCollection<RichTextBlock> Block { get; set; }

        public Slide()
        {
            Block = new ObservableCollection<RichTextBlock>();
        }
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
            Paragraph = new ObservableCollection<Models.Paragraph>();
        }
    }
    
    [XmlType(AnonymousType = true)]
    public partial class Paragraph: Run
    {
        [XmlElement("InlineUIContainer")]
        public InlineUIContainer InlineUIContainer { get; set; }

        [XmlElement("Bold")]
        public Bold Bold { get; set; }

        [XmlElement("Run")]
        public Run Run { get; set; }

        [XmlElement("Hyperlink")]
        public HyperLink Hyperlink { get; set; }

        [XmlText()]
        public string[] Text { get; set; }
    }

    [XmlType(AnonymousType = true)]
    public partial class InlineUIContainer: BindableBase
    {
        [XmlElement("Image")]
        public Image Image { get; set; }
    }
    
    [XmlType(AnonymousType = true)]
    public partial class Image: BindableBase
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
    }
    
    [XmlType(AnonymousType = true)]
    public partial class Bold: Run
    {
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
        [DefaultValue(typeof(byte), "20")]
        public byte FontSize { get; set; }
        
        [XmlAttribute()]
        [DefaultValue("Normal")]
        public string FontStretch { get; set; }
        
        [XmlAttribute()]
        [DefaultValue("#FFFFFFFF")]
        public string Foreground { get; set; }
        
        [XmlAttribute("Typography.CapitalSpacing")]
        [DefaultValue("True")]
        public string TypographyCapitalSpacing { get; set; }
        
        [XmlText()]
        public string Value { get; set; }
    }
    
    [XmlType(AnonymousType = true)]
    public partial class HyperLink: BindableBase
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
        [DefaultValue("Left")]
        public string HorizontalAlignment { get; set; }

        [XmlAttribute()]
        [DefaultValue("Center")]
        public string VerticalAlignment { get; set; }

        [XmlAttribute()]
        [DefaultValue("Visible")]
        public string Visibility { get; set; }
    }

    public class Block: BindableBase
    {
        [XmlAttribute()]
        [DefaultValue(typeof(byte), "10")]
        public byte LineHeight { get; set; }

        [XmlAttribute()]
        [DefaultValue("BlockLineHeight")]
        public string LineStackingStrategy { get; set; }

        [XmlAttribute()]
        [DefaultValue(typeof(byte), "0")]
        public byte TextIndent { get; set; }

        [XmlIgnore()]
        [DefaultValue(false)]
        public bool TextIndentSpecified { get; set; }
    }
}
