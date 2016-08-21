using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Log.Common.Services.Common;

namespace Log.Common.Services.Tests
{
    [TestClass]
    public class MarkdownParserTests
    {
        [TestInitialize]
        public void Setup()
        {

        }

        [TestCleanup]
        public void Teardown()
        {

        }

        [TestMethod]
        public void Markdown_Parsed_WhenValidTextAndXmlCode()
        {
            var parser = ParserFactory.CreateParser("markdown");
            Assert.IsNotNull(parser);

            #region code string

            var code = @"# First Slide #
> This is a bolded **Quote** inside the slide

This is some sample text on *Italics*
Yet another text **Bolded** going through

And some Strikethrough ~~Text~~


... And what about this *Indented* text
```xml
<code>this is inline code</code>
```";
            #endregion

            var tokens = parser.Parse(code);
            Assert.IsNotNull(tokens);
            Assert.IsTrue(tokens.Any());
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.H1)));
            Assert.IsTrue(tokens.First(x => x.Type.Equals(TokenType.H1)).Value.Equals(" First Slide "));
            Assert.IsTrue(tokens.First(x => x.Type.Equals(TokenType.H1)).RawValue.Equals("# First Slide #"));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Blockquotes)));
            Assert.IsTrue(tokens.Where(x => x.Type.Equals(TokenType.Bold)).Count().Equals(2));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Italics)));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Strikethrough)));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Indented)));
            Assert.IsTrue(tokens.Where(x => x.Type.Equals(TokenType.InlineCode)).Count().Equals(2));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Xml)));
            var newLineCnt = tokens.Where(x => x.Type.Equals(TokenType.NewLine)).Count();
            Assert.IsTrue(newLineCnt.Equals(12));
            var codeStr = parser.Concat(tokens);
            Assert.AreEqual(code, codeStr);
        }

        [TestMethod]
        public void Markdown_Parsed_WhenValidTextAndJsonCode()
        {
            var parser = ParserFactory.CreateParser("markdown");
            Assert.IsNotNull(parser);

            #region code string

            var code = @"## Second Slide ##
> This is a bolded **Quote** inside the slide

This is some *Italics* text
Yet another text **Bolded** going through

~~Text~~ with Strikethrough apply


... And what about this *Indented* text
```json
{ 
    'component': {
        'title': 'In line Code',
        'isEnabled': true,
        'quantity': 10,
        'items': [1, 2, 3]
    }
}
```";
            #endregion

            var tokens = parser.Parse(code);
            Assert.IsNotNull(tokens);
            Assert.IsTrue(tokens.Any());
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.H2)));
            Assert.IsTrue(tokens.First(x => x.Type.Equals(TokenType.H2)).Value.Equals(" Second Slide "));
            Assert.IsTrue(tokens.First(x => x.Type.Equals(TokenType.H2)).RawValue.Equals("## Second Slide ##"));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Blockquotes)));
            Assert.IsTrue(tokens.Where(x => x.Type.Equals(TokenType.Bold)).Count().Equals(2));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Italics)));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Strikethrough)));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Indented)));
            Assert.IsTrue(tokens.Where(x => x.Type.Equals(TokenType.InlineCode)).Count().Equals(2));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Json)));
            var newLineCnt = tokens.Where(x => x.Type.Equals(TokenType.NewLine)).Count();
            Assert.IsTrue(newLineCnt.Equals(19));
            var codeStr = parser.Concat(tokens);
            Assert.AreEqual(code, codeStr);
        }

        [TestMethod]
        public void Markdown_Parsed_WhenValidTextAndCSharpCode()
        {
            var parser = ParserFactory.CreateParser("markdown");
            Assert.IsNotNull(parser);

            #region code string

            var code = @"### Third Slide ###
> This is a bolded **Quote** inside the slide

This is some *Italics* text
Yet another text **Bolded** going through

~~Text~~ with Strikethrough apply


... And what about this *Indented* text
```csharp
using System.Xml;

namespace Markdown.Parsers
{ 
    public class Parser: IParser<Markdown> {
        public int LInes {get; set; }
        public Parser() {
            Lines = 0;
        }    
    }
}
```";
            #endregion

            var tokens = parser.Parse(code);
            Assert.IsNotNull(tokens);
            Assert.IsTrue(tokens.Any());
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.H3)));
            Assert.IsTrue(tokens.First(x => x.Type.Equals(TokenType.H3)).Value.Equals(" Third Slide "));
            Assert.IsTrue(tokens.First(x => x.Type.Equals(TokenType.H3)).RawValue.Equals("### Third Slide ###"));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Blockquotes)));
            Assert.IsTrue(tokens.Where(x => x.Type.Equals(TokenType.Bold)).Count().Equals(2));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Italics)));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Strikethrough)));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Indented)));
            Assert.IsTrue(tokens.Where(x => x.Type.Equals(TokenType.InlineCode)).Count().Equals(2));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Csharp)));
            var newLineCnt = tokens.Where(x => x.Type.Equals(TokenType.NewLine)).Count();
            Assert.IsTrue(newLineCnt.Equals(22));
            var codeStr = parser.Concat(tokens);
            Assert.AreEqual(code, codeStr);
        }
    }
}
