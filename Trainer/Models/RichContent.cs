using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Trainer.Models
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class Slides
    {
        [XmlElement("Slide")]
        public ObservableCollection<Slide> Slide { get; set; }
    }

    [XmlType(AnonymousType = true)]
    public partial class Slide
    {
        [XmlElement("RichTextBlock")]
        public ObservableCollection<RichTextBlock> Block { get; set; }

        [XmlAttribute]
        public string Title { get; set; }
    }

    [XmlType(AnonymousType = true)]
    public partial class RichTextBlock: Run
    {
        [XmlElement("Paragraph")]
        public ObservableCollection<Paragraph> Paragraph { get; set; }
        
        [XmlAttribute()]
        public string SelectionHighlightColor { get; set; }

        [XmlAttribute()]
        public bool IsTextSelectionEnabled { get; set; }

        [XmlAttribute()]
        public byte MaxLines { get; set; }

        [XmlAttribute()]
        public string TextWrapping { get; set; }

        [XmlAttribute("Typography.Capitals")]
        public string TypographyCapitals { get; set; }
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
        public string FontStyle { get; set; }
        
        [XmlAttribute()]
        public string FontWeight { get; set; }
        
        [XmlAttribute()]
        public byte CharacterSpacing { get; set; }
        
        [XmlAttribute()]
        public byte FontSize { get; set; }
        
        [XmlAttribute()]
        public string FontStretch { get; set; }
        
        [XmlAttribute()]
        public string Foreground { get; set; }
        
        [XmlAttribute("Typography.CapitalSpacing")]
        public string TypographyCapitalSpacing { get; set; }
        
        [XmlText()]
        public string Value { get; set; }
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
        public string Margin { get; set; }

        [XmlAttribute()]
        public decimal Opacity { get; set; }

        [XmlAttribute()]
        public string Padding { get; set; }

        [XmlAttribute()]
        public string TextAlignment { get; set; }

        [XmlAttribute()]
        public string HorizontalAlignment { get; set; }

        [XmlAttribute()]
        public string VerticalAlignment { get; set; }

        [XmlAttribute()]
        public string Visibility { get; set; }
    }

    public class Block
    {
        [XmlAttribute()]
        public byte LineHeight { get; set; }

        [XmlAttribute()]
        public string LineStackingStrategy { get; set; }

        [XmlAttribute()]
        public byte TextIndent { get; set; }

        [XmlIgnoreAttribute()]
        public bool TextIndentSpecified { get; set; }
    }
}
