using System;
using System.Linq;
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
        public Xaml.Run Control
        {
            get
            {
                var control = new Xaml.Run
                {
                  Text = this.Value
                };

                if (this.CharacterSpacing > 0) control.CharacterSpacing = this.CharacterSpacing;
                if (this.FontSize > 0) control.FontSize = this.FontSize;
                if (!string.IsNullOrEmpty(this.FontStretch)) control.FontStretch = Helpers.ParseEnum<FontStretch>(this.FontStretch);
                if (!string.IsNullOrEmpty(this.FontStyle)) control.FontStyle = Helpers.ParseEnum<FontStyle>(this.FontStyle);
                if (!string.IsNullOrEmpty(this.FontWeight)) control.FontWeight = Helpers.ParseEnum<FontWeight>(this.FontWeight);
                if (!string.IsNullOrEmpty(this.Foreground)) control.Foreground = new SolidColorBrush(this.Foreground.GetColor());

                return control;
            }
        }
    }

    public partial class HyperLink
    {
        [XmlIgnore]
        public Xaml.Hyperlink Control
        {
            get
            {
                var control = new Xaml.Hyperlink
                {
                    NavigateUri = new Uri(this.NavigateUri)
                };
                control.Inlines.Add(new Xaml.Run { Text = this.Value });

                return control;
            }
        }
    }

    public partial class Bold
    {
        [XmlIgnore]
        public new Xaml.Bold Control
        {
            get
            {
                var control = new Xaml.Bold();

                if (this.CharacterSpacing > 0) control.CharacterSpacing = this.CharacterSpacing;
                if (this.FontSize > 0) control.FontSize = this.FontSize;
                if (!string.IsNullOrEmpty(this.FontStretch)) control.FontStretch = Helpers.ParseEnum<FontStretch>(this.FontStretch);
                if (!string.IsNullOrEmpty(this.FontStyle)) control.FontStyle = Helpers.ParseEnum<FontStyle>(this.FontStyle);
                if (!string.IsNullOrEmpty(this.FontWeight)) control.FontWeight = Helpers.ParseEnum<FontWeight>(this.FontWeight);
                if (!string.IsNullOrEmpty(this.Foreground)) control.Foreground = new SolidColorBrush(this.Foreground.GetColor());
                control.Inlines.Add(new Xaml.Run { Text = this.Value });

                return control;
            }
        }
    }

    public partial class Image
    {
        [XmlIgnore]
        public Windows.UI.Xaml.Controls.Image Control
        {
            get
            {
                var control = new Windows.UI.Xaml.Controls.Image
                {
                    Width = this.Width,
                    Height = this.Height
                };

                if (!string.IsNullOrEmpty(this.Stretch)) control.Stretch = Helpers.ParseEnum<Stretch>(this.Stretch);

                return control;
            }
        }
    }

    public partial class Paragraph
    {
        [XmlIgnore]
        public new Xaml.Paragraph Control
        {
            get
            {
                var control = new Xaml.Paragraph();

                if (this.CharacterSpacing > 0) control.CharacterSpacing = this.CharacterSpacing;
                if (this.FontSize > 0) control.FontSize = this.FontSize;
                if (!string.IsNullOrEmpty(this.FontStretch)) control.FontStretch = Helpers.ParseEnum<FontStretch>(this.FontStretch);
                if (!string.IsNullOrEmpty(this.FontStyle)) control.FontStyle = Helpers.ParseEnum<FontStyle>(this.FontStyle);
                if (!string.IsNullOrEmpty(this.FontWeight)) control.FontWeight = Helpers.ParseEnum<FontWeight>(this.FontWeight);
                if (!string.IsNullOrEmpty(this.Foreground)) control.Foreground = new SolidColorBrush(this.Foreground.GetColor());

                if (this.LineHeight > 0) control.LineHeight = this.LineHeight;
                if (!string.IsNullOrEmpty(this.LineStackingStrategy)) control.LineStackingStrategy = Helpers.ParseEnum<Windows.UI.Xaml.LineStackingStrategy>(this.LineStackingStrategy);
                if (!string.IsNullOrEmpty(this.Margin)) control.Margin = Helpers.GetThickness(this.Margin);
                if (!string.IsNullOrEmpty(this.TextAlignment)) control.TextAlignment = Helpers.ParseEnum<Windows.UI.Xaml.TextAlignment>(this.TextAlignment);
                if (this.TextIndent > 0) control.TextIndent = this.TextIndent;
                if(this.Text != null && this.Text.Any())
                {
                    control.Inlines.Add(new Xaml.Run { Text = string.Join(Environment.NewLine, this.Text) });
                }

                if (this.Bold != null)
                {
                    control.Inlines.Add(this.Bold.Control);
                }

                if (this.Hyperlink != null)
                {
                    control.Inlines.Add(this.Hyperlink.Control);
                }

                if (this.Run != null)
                {
                    control.Inlines.Add(this.Run.Control);
                }

                if (this.InlineUIContainer != null)
                {
                    control.Inlines.Add(this.InlineUIContainer.Control);
                }

                return control;
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
        public Windows.UI.Xaml.Controls.RichTextBlock RichControl
        {
            get
            {
                var control = new Windows.UI.Xaml.Controls.RichTextBlock
                {
                    IsTextSelectionEnabled = true,
                    MinHeight = 40,
                    MinWidth = 400
                };

                if (this.CharacterSpacing > 0) control.CharacterSpacing = this.CharacterSpacing;
                if (this.FontSize > 0) control.FontSize = this.FontSize;
                if (!string.IsNullOrEmpty(this.FontStretch)) control.FontStretch = Helpers.ParseEnum<FontStretch>(this.FontStretch);
                if (!string.IsNullOrEmpty(this.FontStyle)) control.FontStyle = Helpers.ParseEnum<FontStyle>(this.FontStyle);
                if (!string.IsNullOrEmpty(this.FontWeight)) control.FontWeight = Helpers.ParseEnum<FontWeight>(this.FontWeight);
                if (!string.IsNullOrEmpty(this.Foreground)) control.Foreground = new SolidColorBrush(this.Foreground.GetColor());

                if (this.LineHeight > 0) control.LineHeight = this.LineHeight;
                if (!string.IsNullOrEmpty(this.LineStackingStrategy)) control.LineStackingStrategy = Helpers.ParseEnum<Windows.UI.Xaml.LineStackingStrategy>(this.LineStackingStrategy);
                if (!string.IsNullOrEmpty(this.Margin)) control.Margin = Helpers.GetThickness(this.Margin);
                if (!string.IsNullOrEmpty(this.TextAlignment)) control.TextAlignment = Helpers.ParseEnum<Windows.UI.Xaml.TextAlignment>(this.TextAlignment);
                if (this.TextIndent > 0) control.TextIndent = this.TextIndent;

                if (!string.IsNullOrEmpty(this.HorizontalAlignment)) control.HorizontalAlignment = Helpers.ParseEnum<Windows.UI.Xaml.HorizontalAlignment>(this.HorizontalAlignment);
                if (!string.IsNullOrEmpty(this.VerticalAlignment)) control.VerticalAlignment = Helpers.ParseEnum<Windows.UI.Xaml.VerticalAlignment>(this.VerticalAlignment);
                if (this.MaxLines > 0) control.MaxLines = this.MaxLines;
                if (this.Opacity > 0.0M) control.Opacity = (double)this.Opacity;
                if (!string.IsNullOrEmpty(this.LineStackingStrategy)) control.TextWrapping = Helpers.ParseEnum<Windows.UI.Xaml.TextWrapping>(this.TextWrapping);

                foreach (var p in this.Paragraph)
                {
                    control.Blocks.Add(p.Control);
                }

                return control;
            }
        }
    }

    public partial class Component: Trainer.Domain.Component
    {
        [XmlIgnore]
        public Windows.UI.Xaml.Controls.RichTextBlock RichControl
        {
            get
            {
                var control = new Windows.UI.Xaml.Controls.RichTextBlock
                {
                    IsTextSelectionEnabled = true,
                    MinHeight = 40,
                    MinWidth = 400,
                };
                
                control.Blocks.Add(new Paragraph
                {
                    FontStyle = "Italic",
                    Padding = "4",
                    TextIndent = 4,
                    Text = Code.Value.Split(Environment.NewLine.ToArray())
                }.Control);

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

        //public static string[] GetText(this Component component)
        //{
        //    return component.c
        //}
    }
}
