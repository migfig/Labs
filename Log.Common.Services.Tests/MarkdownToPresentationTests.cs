using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Log.Common.Services.Common;
using System.Collections.Generic;
using Trainer.Domain;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml;
using System.Text;
using Common;

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
            var parser = TokenParserFactory<Presentation>.CreateParser(new MockApiService());
            Assert.IsNotNull(parser);
            var presentation = parser.Parse(_tokens);
            Assert.IsNotNull(presentation);
        }        
    }

    internal class MockApiService : IGenericApiService<Slide>
    {
        #region unused methods

        public Task<bool> AddItem(Slide item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddItems(IEnumerable<Slide> items)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Slide>> GetItems(string url)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveItem(Slide item, string propertyName)
        {
            throw new NotImplementedException();
        }

        #endregion

        public Task<Slide> TransformXml(XElement xml)
        {
            var xslt = new XslCompiledTransform(true);
            xslt.Load(@"C:\Code\RelatedRecords.Tests\Log.Common.Services\Common\token2slide.xslt");

            var builder = new StringBuilder();
            using (var stream = XmlWriter.Create(builder))
            {
                xslt.Transform(xml.CreateReader(), stream);
            }
            var slideXml = XElement.Parse(builder.ToString());

            return Task.FromResult(XmlHelper2<Slide>.Load(slideXml));
        }
    }
}
