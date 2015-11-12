using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace RelatedRecords.Parser.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void KalithaParserTests()
        {
            var parser = new KalithaParser(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "relatedrecords.cgt"));
            var results = parser.Parse("table Test21 where col1 between 1.34 and 245.234");
            Assert.IsTrue(results.isAccepted);
        }

        [TestMethod]
        public void MorozovParserTests()
        {
            //ParserFactory.InitializeFactoryFromResource("relatedrecords.cgt");
            ParserFactory.InitializeFactoryFromFile(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "relatedrecords.cgt"));
            var parser = new MorozovParser().Parse("table Test21 where col1 between 1.34 and 245.234");
        }
    }
}
