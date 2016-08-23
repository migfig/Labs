using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Log.Common.Services.Common;
using System.Collections.Generic;
using Trainer.Domain;

namespace Log.Common.Services.Tests
{
    [TestClass]
    public class MarkdownToPresentationTests
    {
        private IEnumerable<Token> _tokens;

        [TestInitialize]
        public void Setup()
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
```

# Second Slide #
> This is a bolded **Quote** inside the slide

This is some sample text on *Italics*
Yet another text **Bolded** going through

And some Strikethrough ~~Text~~


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

            _tokens = parser.Parse(code);
            Assert.IsNotNull(_tokens);
            Assert.IsTrue(_tokens.Any());
        }

        [TestCleanup]
        public void Teardown()
        { 
        }

        [TestMethod]
        public void Markdown_Translated_ToPresentation_WhenValidXmlAndCSharpCode()
        {
            var parser = TokenParserFactory<Presentation>.CreateParser();
            Assert.IsNotNull(parser);
            var presentation = parser.Parse(_tokens);
            Assert.IsNotNull(presentation);
        }
    }
}
