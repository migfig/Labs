using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Linq;
using System.IO;
using System.Text;

namespace Trainer.Domain
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class Presentation
    {
        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public string Image { get; set; }

        [XmlElement("Slide")]
        public ObservableCollection<Slide> Slide { get; set; }

        public Presentation()
        {
            Slide = new ObservableCollection<Slide>();
        }

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
    }

    [XmlType(AnonymousType = true)]
    public partial class Slide
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
            //SelectionHighlightColor = "#FAFBFCFF";
            //IsTextSelectionEnabled = true;
            //MaxLines = ((byte)(10));
            //TextWrapping = "Wrap";
            //TypographyCapitals = "True";
            Paragraph = new ObservableCollection<Paragraph>();
        }
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

        [XmlElement("Hyperlink")]
        public HyperLink Hyperlink { get; set; }

        [XmlText()]
        public string[] Text { get; set; }

        public Paragraph()
        {
            Bold = new ObservableCollection<Domain.Bold>();
            Run = new ObservableCollection<Domain.Run>();
        }
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
            //Stretch = "Fill";
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
            //FontSize = (byte)34;
            //FontStretch = "Normal";
            //FontStyle = "Normal";
            //FontWeight = "Normal";
            //Foreground = "#FF000000";
        }
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
            //HorizontalAlignment = "Stretch";
            //VerticalAlignment = "Stretch";
            //Margin = "0";
            //Opacity = (decimal)1.0m;
            //Padding = "0";
            //TextAlignment = "Justify";
            //Visibility = "Visible";
        }
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

        [XmlIgnore()]
        [DefaultValue(false)]
        public bool TextIndentSpecified { get; set; }

        public Block()
        {
            //LineHeight = (byte)40;
            //LineStackingStrategy = "BlockLineHeight";
            //TextIndent = (byte)0;
        }
    }
}
