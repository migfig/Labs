using Log.Common.Services.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log.Common.Services.Tests
{
    [TestClass]
    public class PlainTextParserTests
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
            var parser = ParserFactory.CreateParser("plaintext");
            Assert.IsNotNull(parser);

            #region code string

            var code = @"title: Diagram Title
entity: dialog,Comments,Proxy,CVSServlet,CVSService,GimqRequestBatch,Proxy2,GimqService
message: validate to dialog
    validate to Comments
    return
message: validate from dialog to Comments
";
            
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
    }
}
