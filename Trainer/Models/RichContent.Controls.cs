using System;
using System.Globalization;
using System.Xml.Serialization;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Media;
using Xaml = Windows.UI.Xaml.Documents;

namespace Trainer.Models
{
    public partial class Run
    {
        [XmlIgnore]
        public Xaml.Run Control =>
             new Xaml.Run
             {
                 CharacterSpacing = this.CharacterSpacing,
                 FontSize = this.FontSize,
                 FontStretch = Helpers.ParseEnum<FontStretch>(this.FontStretch),
                 FontStyle = Helpers.ParseEnum<FontStyle>(this.FontStyle),
                 FontWeight = Helpers.ParseEnum<FontWeight>(this.FontWeight),
                 Foreground = new SolidColorBrush(this.Foreground.GetColor()),
                 Text = this.Value
             };
    }

    public partial class HyperLink
    {
        [XmlIgnore]
        public Xaml.Hyperlink Control =>
            new Xaml.Hyperlink
            {
                NavigateUri = new Uri(this.NavigateUri)
            };
    }

    public partial class Bold
    {
        [XmlIgnore]
        public new Xaml.Bold Control
        {
            get
            {
                var bold = new Xaml.Bold
                {
                    CharacterSpacing = this.CharacterSpacing,
                    FontSize = this.FontSize,
                    FontStretch = Helpers.ParseEnum<FontStretch>(this.FontStretch),
                    FontStyle = Helpers.ParseEnum<FontStyle>(this.FontStyle),
                    FontWeight = Helpers.ParseEnum<FontWeight>(this.FontWeight),
                    Foreground = new SolidColorBrush(this.Foreground.GetColor())
                };
                bold.Inlines.Add(new Xaml.Run { Text = this.Value });
                return bold;
            }
        }
    }

    public partial class Image
    {
        [XmlIgnore]
        public Windows.UI.Xaml.Controls.Image Control =>
            new Windows.UI.Xaml.Controls.Image
            {
                Width = this.Width,
                Height = this.Height,
                Stretch = Helpers.ParseEnum<Stretch>(this.Stretch)
            };
    }

    public partial class Paragraph
    {
        [XmlIgnore]
        public new Xaml.Paragraph Control
        {
            get
            {
                var paragraph = new Xaml.Paragraph
                {
                    CharacterSpacing = this.CharacterSpacing,
                    FontSize = this.FontSize,
                    FontStretch = Helpers.ParseEnum<FontStretch>(this.FontStretch),
                    FontStyle = Helpers.ParseEnum<FontStyle>(this.FontStyle),
                    FontWeight = Helpers.ParseEnum<FontWeight>(this.FontWeight),
                    Foreground = new SolidColorBrush(this.Foreground.GetColor()),
                    LineHeight = this.LineHeight,
                    LineStackingStrategy = Helpers.ParseEnum<Windows.UI.Xaml.LineStackingStrategy>(this.LineStackingStrategy),
                    Margin = Helpers.GetThickness(this.Margin),
                    TextAlignment = Helpers.ParseEnum<Windows.UI.Xaml.TextAlignment>(this.TextAlignment),
                    TextIndent = this.TextIndent 
                };

                if (this.Bold != null)
                {
                    paragraph.Inlines.Add(this.Bold.Control);
                }

                if (this.Hyperlink != null)
                {
                    paragraph.Inlines.Add(this.Hyperlink.Control);
                }

                if (this.Run != null)
                {
                    paragraph.Inlines.Add(this.Run.Control);
                }

                if (this.InlineUIContainer != null)
                {
                    paragraph.Inlines.Add(this.InlineUIContainer.Control);
                }

                return paragraph;
            }
        }
    }

    public partial class InlineUIContainer
    {
        [XmlIgnore]
        public Xaml.InlineUIContainer Control =>
            new Xaml.InlineUIContainer
            {
                Child = this.Image.Control
            };
    }

    public partial class RichTextBlock
    {
        [XmlIgnore]
        public new Windows.UI.Xaml.Controls.RichTextBlock Control
        {
            get
            {
                var control = new Windows.UI.Xaml.Controls.RichTextBlock
                {
                    CharacterSpacing = this.CharacterSpacing,
                    FontSize = this.FontSize,
                    FontStretch = Helpers.ParseEnum<FontStretch>(this.FontStretch),
                    FontStyle = Helpers.ParseEnum<FontStyle>(this.FontStyle),
                    FontWeight = Helpers.ParseEnum<FontWeight>(this.FontWeight),
                    Foreground = new SolidColorBrush(this.Foreground.GetColor()),
                    LineHeight = this.LineHeight,
                    LineStackingStrategy = Helpers.ParseEnum<Windows.UI.Xaml.LineStackingStrategy>(this.LineStackingStrategy),
                    Margin = Helpers.GetThickness(this.Margin),
                    TextAlignment = Helpers.ParseEnum<Windows.UI.Xaml.TextAlignment>(this.TextAlignment),
                    TextIndent = this.TextIndent,
                    HorizontalAlignment = Helpers.ParseEnum<Windows.UI.Xaml.HorizontalAlignment>(this.HorizontalAlignment),
                    VerticalAlignment = Helpers.ParseEnum<Windows.UI.Xaml.VerticalAlignment>(this.VerticalAlignment),
                    IsTextSelectionEnabled = this.IsTextSelectionEnabled,
                    MaxLines = this.MaxLines,
                    Opacity = (double)this.Opacity,
                    TextWrapping = Helpers.ParseEnum<Windows.UI.Xaml.TextWrapping>(this.TextWrapping),
                    MinHeight = 40,
                    MinWidth = 400
                };

                foreach (var p in this.Paragraph)
                {
                    control.Blocks.Add(p.Control);
                }

                return control;
            }
        }
    }

    public static class Helpers
    {
        public static T ParseEnum<T>(string value)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value);
            } catch(Exception)
            {
                return default(T);
            }
        }

        public static Windows.UI.Xaml.Thickness GetThickness(this string value)
        {
            if(!string.IsNullOrWhiteSpace(value))
            {
                var parts = value.Split(',');
                switch (parts.Length)
                {
                    case 1:
                        return new Windows.UI.Xaml.Thickness(Double.Parse(value));
                    case 2:
                        return new Windows.UI.Xaml.Thickness(double.Parse(parts[0]),
                            double.Parse(parts[1]),
                            double.Parse(parts[0]),
                            double.Parse(parts[1]));
                    case 4:
                        return new Windows.UI.Xaml.Thickness(double.Parse(parts[0]), 
                            double.Parse(parts[1]),
                            double.Parse(parts[2]), 
                            double.Parse(parts[3]));
                }
            }

            return default(Windows.UI.Xaml.Thickness);
        }

        public static Color GetColor(this string color)
        {
            return Color.FromArgb(
                     Byte.Parse(color.Substring(1, 2), NumberStyles.HexNumber),
                     Byte.Parse(color.Substring(3, 2), NumberStyles.HexNumber),
                     Byte.Parse(color.Substring(5, 2), NumberStyles.HexNumber),
                     Byte.Parse(color.Substring(7, 2), NumberStyles.HexNumber));
        }
    }
}
