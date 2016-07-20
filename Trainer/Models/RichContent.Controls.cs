using System;
using System.Linq;
using System.Globalization;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Media;
using Xaml = Windows.UI.Xaml.Documents;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Log.Common.Services.Common;

namespace Trainer.Models
{
    public static class ConverterHelpers
    {
        public static Xaml.Run Control(this Domain.Run item)
        {
            var control = new Xaml.Run
            {
                Text = item.Value
            };

            if (item.CharacterSpacing > 0) control.CharacterSpacing = item.CharacterSpacing;
            if (item.FontSize > 0) control.FontSize = item.FontSize;
            if (!string.IsNullOrEmpty(item.FontStretch)) control.FontStretch = Helpers.ParseEnum<FontStretch>(item.FontStretch);
            if (!string.IsNullOrEmpty(item.FontStyle)) control.FontStyle = Helpers.ParseEnum<FontStyle>(item.FontStyle);
            if (!string.IsNullOrEmpty(item.FontWeight)) control.FontWeight = Helpers.ParseEnum<FontWeight>(item.FontWeight);
            if (!string.IsNullOrEmpty(item.Foreground)) control.Foreground = new SolidColorBrush(item.Foreground.GetColor());

            return control;
        }

        public static Xaml.Hyperlink Control(this Domain.HyperLink item)
        {
            var control = new Xaml.Hyperlink
            {
                NavigateUri = new Uri(item.NavigateUri)
            };
            control.Inlines.Add(new Xaml.Run { Text = item.Value });

            return control;
        }

        public static Xaml.Bold Control(this Domain.Bold item)
        {
            var control = new Xaml.Bold();

            if (item.CharacterSpacing > 0) control.CharacterSpacing = item.CharacterSpacing;
            if (item.FontSize > 0) control.FontSize = item.FontSize;
            if (!string.IsNullOrEmpty(item.FontStretch)) control.FontStretch = Helpers.ParseEnum<FontStretch>(item.FontStretch);
            if (!string.IsNullOrEmpty(item.FontStyle)) control.FontStyle = Helpers.ParseEnum<FontStyle>(item.FontStyle);
            if (!string.IsNullOrEmpty(item.FontWeight)) control.FontWeight = Helpers.ParseEnum<FontWeight>(item.FontWeight);
            if (!string.IsNullOrEmpty(item.Foreground)) control.Foreground = new SolidColorBrush(item.Foreground.GetColor());
            control.Inlines.Add(new Xaml.Run { Text = item.Value });

            return control;
        }

        public static Windows.UI.Xaml.Controls.Image Control(this Domain.Image item)
        {
            var control = new Windows.UI.Xaml.Controls.Image
            {
                Width = item.Width,
                Height = item.Height
            };

            if (!string.IsNullOrEmpty(item.Stretch)) control.Stretch = Helpers.ParseEnum<Stretch>(item.Stretch);

            return control;
        }

        public static Xaml.Paragraph Control(this Domain.Paragraph item)
        {
            var control = new Xaml.Paragraph();

            if (item.CharacterSpacing > 0) control.CharacterSpacing = item.CharacterSpacing;
            if (item.FontSize > 0) control.FontSize = item.FontSize;
            if (!string.IsNullOrEmpty(item.FontStretch)) control.FontStretch = Helpers.ParseEnum<FontStretch>(item.FontStretch);
            if (!string.IsNullOrEmpty(item.FontStyle)) control.FontStyle = Helpers.ParseEnum<FontStyle>(item.FontStyle);
            if (!string.IsNullOrEmpty(item.FontWeight)) control.FontWeight = Helpers.ParseEnum<FontWeight>(item.FontWeight);
            if (!string.IsNullOrEmpty(item.Foreground)) control.Foreground = new SolidColorBrush(item.Foreground.GetColor());

            if (item.LineHeight > 0) control.LineHeight = item.LineHeight;
            if (!string.IsNullOrEmpty(item.LineStackingStrategy)) control.LineStackingStrategy = Helpers.ParseEnum<Windows.UI.Xaml.LineStackingStrategy>(item.LineStackingStrategy);
            if (!string.IsNullOrEmpty(item.Margin)) control.Margin = Helpers.GetThickness(item.Margin);
            if (!string.IsNullOrEmpty(item.TextAlignment)) control.TextAlignment = Helpers.ParseEnum<Windows.UI.Xaml.TextAlignment>(item.TextAlignment);
            if (item.TextIndent > 0) control.TextIndent = item.TextIndent;
            if (item.Text != null && item.Text.Any())
            {
                control.Inlines.Add(new Xaml.Run { Text = string.Join(Environment.NewLine, item.Text) });
            }

            if (item.Bold != null && item.Bold.Any())
            {
                foreach (var bold in item.Bold)
                {
                    control.Inlines.Add(bold.Control());
                }
            }

            if (item.Hyperlink != null)
            {
                control.Inlines.Add(item.Hyperlink.Control());
            }

            if (item.Run != null && item.Run.Any())
            {
                foreach (var run in item.Run)
                {
                    control.Inlines.Add(run.Control());
                }
            }

            if (item.InlineUIContainer != null)
            {
                control.Inlines.Add(item.InlineUIContainer.Control());
            }

            return control;
        }

        public static Xaml.InlineUIContainer Control(this Domain.InlineUIContainer item)
        {
            return new Xaml.InlineUIContainer
            {
                Child = item.Image.Control()
            };
        }

        public static Windows.UI.Xaml.Controls.RichTextBlock Control(this Domain.RichTextBlock item)
        {
            var control = new Windows.UI.Xaml.Controls.RichTextBlock
            {
                IsTextSelectionEnabled = true,
                MinHeight = 40,
                MinWidth = 400
            };

            if (item.CharacterSpacing > 0) control.CharacterSpacing = item.CharacterSpacing;
            if (item.FontSize > 0) control.FontSize = item.FontSize;
            if (!string.IsNullOrEmpty(item.FontStretch)) control.FontStretch = Helpers.ParseEnum<FontStretch>(item.FontStretch);
            if (!string.IsNullOrEmpty(item.FontStyle)) control.FontStyle = Helpers.ParseEnum<FontStyle>(item.FontStyle);
            if (!string.IsNullOrEmpty(item.FontWeight)) control.FontWeight = Helpers.ParseEnum<FontWeight>(item.FontWeight);
            if (!string.IsNullOrEmpty(item.Foreground)) control.Foreground = new SolidColorBrush(item.Foreground.GetColor());

            if (item.LineHeight > 0) control.LineHeight = item.LineHeight;
            if (!string.IsNullOrEmpty(item.LineStackingStrategy)) control.LineStackingStrategy = Helpers.ParseEnum<Windows.UI.Xaml.LineStackingStrategy>(item.LineStackingStrategy);
            if (!string.IsNullOrEmpty(item.Margin)) control.Margin = Helpers.GetThickness(item.Margin);
            if (!string.IsNullOrEmpty(item.TextAlignment)) control.TextAlignment = Helpers.ParseEnum<Windows.UI.Xaml.TextAlignment>(item.TextAlignment);
            if (item.TextIndent > 0) control.TextIndent = item.TextIndent;

            if (!string.IsNullOrEmpty(item.HorizontalAlignment)) control.HorizontalAlignment = Helpers.ParseEnum<Windows.UI.Xaml.HorizontalAlignment>(item.HorizontalAlignment);
            if (!string.IsNullOrEmpty(item.VerticalAlignment)) control.VerticalAlignment = Helpers.ParseEnum<Windows.UI.Xaml.VerticalAlignment>(item.VerticalAlignment);
            if (item.MaxLines > 0) control.MaxLines = item.MaxLines;
            if (item.Opacity > 0.0M) control.Opacity = (double)item.Opacity;
            if (!string.IsNullOrEmpty(item.LineStackingStrategy)) control.TextWrapping = Helpers.ParseEnum<Windows.UI.Xaml.TextWrapping>(item.TextWrapping);

            foreach (var p in item.Paragraph)
            {
                control.Blocks.Add(p.Control());
            }

            return control;
        }

        private static List<Xaml.Run> GetInlines(string line)
        {
            var tokens = CodeParser.Parse(line);
            var dict = new Dictionary<TokenType, string>
            {
                {TokenType.Keyword, "#FF569CD6"},
                {TokenType.Comment, "#FF82C65B"},
                {TokenType.String , "#FFD69D85"},
                {TokenType.Number , "#FF00C68B"},
                {TokenType.Operator,"#FF000000"},
                {TokenType.Symbol , "#FF1E1E1E"},
                {TokenType.Regular, "#FF1E1E1E"}
            };
            var list = new List<Xaml.Run>();
            foreach(var token in tokens)
            {

                list.Add(new Xaml.Run
                {
                    Text = token.Value,
                    Foreground = new SolidColorBrush(Helpers.GetColor(dict[token.Type]))
                });
            }
            return list;
        }

        public static Windows.UI.Xaml.Controls.RichTextBlock Control(this Domain.Component item)
        {
            var control = new Windows.UI.Xaml.Controls.RichTextBlock
            {
                IsTextSelectionEnabled = true,
                MinHeight = 40,
                MinWidth = 400
            };

            foreach(var line in item.Code.Value.Split(Environment.NewLine.ToArray()))
            {
                var par = new Xaml.Paragraph
                {
                    FontStyle = FontStyle.Italic,
                    TextIndent = 4
                };
                foreach (var inline in GetInlines(line)) {
                    par.Inlines.Add(inline);
                }
                control.Blocks.Add(par);
            }

            return control;
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
                        return new Windows.UI.Xaml.Thickness(double.Parse(value));
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
