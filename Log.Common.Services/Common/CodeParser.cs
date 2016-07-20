using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Log.Common.Services.Common
{
    public static class CodeParser
    {
        public static IEnumerable<Token> Parse(string code)
        {
            var items = new List<Token>();

            var regExps = new string[] {
                @"(?<Keyword>var|using|public|private|protected|partial|class|new|void|bool|string|int|static|typeof|get|set|return|this|params|if|while|for|do)",
                @"(?<Comment>//[\w\s]*)",
                @"(?<String>""[\w\s/:,]*"")",
                @"(?<Symbol>[;,\.\(\)\[\]\<\>=\+\-\*&\|{}]*)",
                @"(?<Regular>[a-zA-Z0-9_\s:]*)",
                @"(?<Number>[\d]*)"
            };

            var tokens = Tokenize(code);
            foreach(var token in tokens)
            {
                foreach (var exp in regExps)
                {
                    var regEx = new Regex(exp);
                    var match = regEx.Match(token);
                    if (match != null && match.Success && match.Value.Length > 0)
                    {
                        var key = exp.Substring(3, exp.IndexOf('>') - 3);
                        items.Add(new Token(token, (TokenType)Enum.Parse(typeof(TokenType), key)));
                        break;
                    }
                }
            }

            return items;
        }

        private static string[] Tokenize(string code)
        {
            var tokens = new List<string>();
            var word = string.Empty;
            bool isQuotedString = false;
            bool isInlineComment = false;

            for(var i=0; i<code.Length; i++)
            {
                var c = code[i];
                
                if (isQuotedString || c.IsString())
                {
                    isQuotedString = c.IsString() && isQuotedString ? false : true;
                    word += c;
                }
                else if(isInlineComment || c.IsComment())
                {
                    isInlineComment = true;
                    word += c;
                }
                else if (c.IsDelimiter())
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

        internal static bool IsDelimiter(this char c)
        {
            return " ;,.()[]<>{}=+-*&|\t\n\r\f".ToCharArray().Contains(c);
        }

        internal static bool IsComment(this char c)
        {
            return "/".ToCharArray().Contains(c);
        }

        internal static bool IsString(this char c)
        {
            return c == '"' || c == '\'';
        }
    }

    public class Token
    {
        public Token(string value, TokenType type, int start = 0, int end = 0)
        {
            Value = value;
            Type = type;
            Start = start;
            End = end;
        }
        public string Value { get; private set; }
        public TokenType Type { get; private set; }
        public int Start { get; private set; }
        public int End { get; private set; }
    }

    public enum TokenType
    {
        Keyword,
        Symbol,
        Delimeter,
        Operator,
        Comment,
        String,
        Number,
        Regular
    }
}
