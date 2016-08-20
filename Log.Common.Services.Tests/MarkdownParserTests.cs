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
        public void Markdown_IsProperlyParsed_WhenValidTextIsProvided()
        {
            var parser = ParserFactory.CreateParser("markdown");
            Assert.IsNotNull(parser);
            var tokens = parser.Parse(@"
# First Slide #
> This is a bolded **Quote** inside the slide

This is some sample text on *Italics*
Yet another text **Bolded** going through

And some Strikethrough ~~Text~~


... And what about this *Indented* text
```xml
<code>this is inline code</code>
```
");
            Assert.IsNotNull(tokens);
            Assert.IsTrue(tokens.Any());
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.H1)));
            Assert.IsTrue(tokens.First(x => x.Type.Equals(TokenType.H1)).Value.Equals("# First Slide #"));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Blockquotes)));
            Assert.IsTrue(tokens.Where(x => x.Type.Equals(TokenType.Bold)).Count().Equals(2));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Italics)));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Strikethrough)));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Indented)));
            Assert.IsTrue(tokens.Where(x => x.Type.Equals(TokenType.InlineCode)).Count().Equals(2));
            Assert.IsTrue(tokens.Any(x => x.Type.Equals(TokenType.Xml)));
            Assert.IsTrue(tokens.Where(x => x.Type.Equals(TokenType.NewLine)).Count().Equals(4));
        }
    }
}
