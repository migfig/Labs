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
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "relatedrecords.egt"));
            parser.Parse("table Test21 where col1 between 1.34 and 245.234");
        }

        [TestMethod]
        public void MorozovParserTests()
        {
            ParserFactory.InitializeFactoryFromFile(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "relatedrecords.egt"));
            var parser = new MorozovParser().Parse("table Test21 where col1 between 1.34 and 245.234");
        }
    }
}
