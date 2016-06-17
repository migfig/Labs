using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Trainer.Models
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class RichTextBlock: Run
    {
        [XmlElement("Paragraph")]
        public ObservableCollection<Paragraph> Paragraph { get; set; }
        
        [XmlAttribute()]
        public SolidColorBrush SelectionHighlightColor { get; set; }

        [XmlAttribute()]
        public bool IsTextSelectionEnabled { get; set; }

        [XmlAttribute()]
        public byte MaxLines { get; set; }

        [XmlAttribute()]
        public TextWrapping TextWrapping { get; set; }

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
        public Stretch Stretch { get; set; }
    }
    
    [XmlType(AnonymousType = true)]
    public partial class Bold: Run
    {
    }
    
    [XmlType(AnonymousType = true)]
    public partial class Run: UIElement
    {
        [XmlAttribute()]
        public FontStyle FontStyle { get; set; }
        
        [XmlAttribute()]
        public FontWeight FontWeight { get; set; }
        
        [XmlAttribute()]
        public byte CharacterSpacing { get; set; }
        
        [XmlAttribute()]
        public byte FontSize { get; set; }
        
        [XmlAttribute()]
        public FontStretch FontStretch { get; set; }
        
        [XmlAttribute()]
        public Brush Foreground { get; set; }
        
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
        public TextAlignment TextAlignment { get; set; }

        [XmlAttribute()]
        public HorizontalAlignment HorizontalAlignment { get; set; }

        [XmlAttribute()]
        public VerticalAlignment VerticalAlignment { get; set; }

        [XmlAttribute()]
        public Visibility Visibility { get; set; }
    }

    public class Block
    {
        [XmlAttribute()]
        public byte LineHeight { get; set; }

        [XmlAttribute()]
        public LineStackingStrategy LineStackingStrategy { get; set; }

        [XmlAttribute()]
        public byte TextIndent { get; set; }

        [XmlIgnoreAttribute()]
        public bool TextIndentSpecified { get; set; }
    }
}
