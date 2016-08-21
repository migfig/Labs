using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Log.Common.Services.Common
{
    #region parser interface & factory

    public interface IParser
    {
        IEnumerable<Token> Parse(string code);
        string[] Tokenize(string code);
        bool IsDelimiter(char c);
        bool IsComment(char c);
        bool IsString(char c);
        string Language { get; }
        string Concat(IEnumerable<Token> tokens);
    }

    public class ParserFactory
    {
        public static IParser CreateParser(string language = "csharp")
        {
            switch(language.Trim().ToLower())
            {
                case "markdown":
                    return new MarkdownParser(language);
                default:
                    return new CSharpParser(language);
            }
        }
    }

    #endregion

    #region default methods for language parser

    public abstract class LanguageParser: IParser
    {
        protected string[] _regExps;

        public string Language { get; private set; }

        public virtual bool IsComment(char c) { return false; }
        public virtual bool IsDelimiter(char c) { return false; }
        public virtual bool IsString(char c) { return false; }

        public LanguageParser(string language)
        {
            Language = language;
            _regExps = new string[] { };
        }

        public virtual IEnumerable<Token> Parse(string code)
        {
            var items = new List<Token>();            

            var tokens = this.Tokenize(code.Trim());
            var exps = _regExps.Where(x => !x.Contains("Xml") && !x.Contains("Json") && !x.Contains("Csharp"));
            foreach (var token in tokens)
            {
                foreach (var exp in exps)
                {
                    var regEx = new Regex(exp);
                    var match = regEx.Match(token);
                    if (match != null && match.Success && match.Value.Length > 0)
                    {
                        var key = exp.Substring(3, exp.IndexOf('>') - 3);
                        items.Add(new Token(token, (TokenType)Enum.Parse(typeof(TokenType), key)));
                        if (exp.Contains("InlineCode") && token.Length > 3)
                        {
                            var tok = token.Replace("```", string.Empty);
                            tok = tok.Substring(0, 1).ToUpper() + tok.Substring(1);
                            exps = _regExps.Where(x => x.Contains("<" + tok + ">") || x.Contains("InlineCode") || x.Contains("NewLine")).ToArray();
                        }

                        break;
                    }
                }
            }

            return items;
        }

        public string Concat(IEnumerable<Token> tokens)
        {
            return tokens.Aggregate("", (aggregated, token) =>
            {
                aggregated += token.RawValue; return aggregated;
            });
        }

        public abstract string[] Tokenize(string code);
    }

    #endregion

    #region C# parser

    public sealed class CSharpParser: LanguageParser
    {
        public CSharpParser(string language)
            :base(language)
        {
            _regExps = new string[] {
                @"(?<Keyword>var|using|public|private|protected|partial|class|new|void|bool|string|int|static|typeof|get|set|return|this|params|if|while|for|do)",
                @"(?<Comment>//[\w\s]*)",
                @"(?<String>""[\w\s/:,]*"")",
                @"(?<Symbol>[;,\.\(\)\[\]\<\>=\+\-\*&\|{}]*)",
                @"(?<Regular>[a-zA-Z0-9_\s:]*)",
                @"(?<Number>[\d]*)"
            };
        }

        public override string[] Tokenize(string code)
        {
            var tokens = new List<string>();
            var word = string.Empty;
            bool isQuotedString = false;
            bool isInlineComment = false;

            for(var i=0; i<code.Length; i++)
            {
                var c = code[i];
                
                if (isQuotedString || IsString(c))
                {
                    isQuotedString = IsString(c) && isQuotedString ? false : true;
                    word += c;
                }
                else if(isInlineComment || IsComment(c))
                {
                    isInlineComment = true;
                    word += c;
                }
                else if (IsDelimiter(c))
                {
                    if (word.Length > 0)
                    {
                        tokens.Add(word);
                        tokens.Add(c.ToString());
                    }
                    else
                    {
                        tokens.Add(c.ToString());
                    }
                    word = string.Empty;
                }
                else
                {
                    word += c;
                }
            }

            if(word.Length > 0) tokens.Add(word);

            return tokens.ToArray();
        }

        public override bool IsDelimiter(char c)
        {
            return " ;,.()[]<>{}=+-*&|\t\n\r\f".ToCharArray().Contains(c);
        }

        public override bool IsComment(char c)
        {
            return "/".ToCharArray().Contains(c);
        }

        public override bool IsString(char c)
        {
            return c == '"' || c == '\'';
        }
    }

    #endregion

    #region Markdown parser

    public sealed class MarkdownParser : LanguageParser
    {
        public MarkdownParser(string language)
            :base(language)
        {
            var openText = @"([\s]*\w+[\s]*)+";
            var headerFmt = @"(?<H{0}>{1}" + openText + "{1})";
            var charReplFmt = @"(?<{0}>{1}" + openText + "{1})";

            _regExps = new string[] {
                @"(?<Indented>\.\.\." + openText + ")",
                @"(?<Xml>(\<\w+(\s+\w+='\w+')*[/]?\>(\s*\w+\s*)*)(\</\w+\>)?|(\</\w+\>))",
                @"(?<NewLine>[\r])",
                @"(?<Json>([\{\}':\[\],\w\s\-])*)",
                @"(?<Csharp>([\{\}':\[\]\<\>,;\w\s\-\.\(\)!@#\$%\^\*_\+=\|\\\?])*)",
                @"(?<Blockquotes>\>" + openText + ")",
                @"(?<InlineCode>```(csharp|javascript|xml|html|java|sql|json|))",
                Normalize(headerFmt, '#', 6),
                Normalize(headerFmt, '#', 5),
                Normalize(headerFmt, '#', 4),
                Normalize(headerFmt, '#', 3),
                Normalize(headerFmt, '#', 2),
                Normalize(headerFmt, '#'),
                @"(?<Regular>[\sa-zA-Z_\-]*)",
                NormalizeChars(charReplFmt, "Bold", @"\*", 2),
                NormalizeChars(charReplFmt, "Italics", @"\*"),
                NormalizeChars(charReplFmt, "Strikethrough", "~", 2)
            };
        }

        #region text helpers 

        private string Normalize(string fmt, char c, int times = 1)
        {
            return string.Format(fmt, times, new string(c, times));
        }

        private string NormalizeChars(string fmt, string groupName, string chars, int times = 1)
        {
            var repl = string.Empty;
            for (var i = 1; i <= times; i++) repl += chars;

            return string.Format(fmt, groupName, repl);
        }

        #endregion

        public override string[] Tokenize(string code)
        {
            var tokens = new List<string>();
            var inOnlineCode = false;
            var exps = _regExps.Where(x => !x.Contains("Xml") && !x.Contains("Json") && !x.Contains("Csharp"));

            var lines = code.Split(new char[] { '\n' }, StringSplitOptions.None);
            for (var i=0;i<lines.Length; i++)
            {
                var line = lines[i];

                if(line.Length == 1 && line[0] == '\r')
                {
                    tokens.Add(Environment.NewLine);
                    continue;
                }

                var retRemoved = false;
                if (line[line.Length - 1] == '\r')
                {
                    line = line.Substring(0, line.Length - 1);
                    retRemoved = true;
                }

                while (line.Length > 0)
                {
                    foreach (var exp in exps)
                    {
                        var regEx = new Regex(exp);
                        var match = regEx.Match(line);
                        if (match != null && match.Success && match.Value.Length > 0)
                        {
                            var token = match.Value;
                            if (exp.Contains("InlineCode") && token.Length > 3)
                            {
                                var tok = token.Replace("```", string.Empty);
                                tok = tok.Substring(0, 1).ToUpper() + tok.Substring(1);
                                exps = _regExps.Where(x => x.Contains("<" + tok + ">") || x.Contains("InlineCode") || x.Contains("NewLine")).ToArray();
                            }

                            tokens.Add(match.Value);
                            if (match.Value.Length.Equals(line.Length))
                            {
                                line = string.Empty;
                                if (retRemoved) tokens.Add(Environment.NewLine);

                                break;
                            }

                            line = line.Substring(match.Value.Length);
                        }
                    }
                }
            }

            return tokens.ToArray();
        }
    }

    #endregion

    #region Token defs

    public class Token
    {
        public Token(string value, TokenType type, int start = 0, int end = 0)
        {
            Value = value;
            Type = type;
            Start = start;
            End = end;
        }

        private string _value;
        public string Value
        {
            get
            {
                switch(Type)
                {
                    case TokenType.Bold:
                    case TokenType.Italics:
                        return _value.Replace("*", string.Empty);
                    case TokenType.Strikethrough:
                        return _value.Replace("~", string.Empty);
                    case TokenType.H1:
                    case TokenType.H2:
                    case TokenType.H3:
                    case TokenType.H4:
                    case TokenType.H5:
                    case TokenType.H6:
                        return _value.Replace("#", string.Empty);
                    default:
                        return _value;
                }
            }
            private set { _value = value; }
        }
        public string RawValue { get { return _value; } }

        public TokenType Type { get; private set; }
        public int Start { get; private set; }
        public int End { get; private set; }
    }

    public enum TokenType
    {
        //csharp
        Keyword,
        Symbol,
        Delimeter,
        Operator,
        Comment,
        String,
        Number,
        Regular,

        //markdown
        H1, 
        H2,
        H3,
        H4,
        H5,
        H6,
        Italics,
        Bold,
        Strikethrough,
        Indented,
        Blockquotes,
        InlineCode,
        NewLine,
        Xml,
        Json,
        Csharp
    }

    #endregion
}
