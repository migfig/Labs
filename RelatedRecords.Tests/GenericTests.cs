using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelatedRecords.Tests
{
    [TestClass]
    public class GenericTests
    {
        [TestMethod]
        public void RemoveLastNumbersTests()
        {
            Assert.AreEqual("localhost-Core", "localhost-Core".RemoveLastNumbers());
            Assert.AreEqual("localhost-Core", "localhost-Core0".RemoveLastNumbers());
            Assert.AreEqual("localhost-Core", "localhost-Core1".RemoveLastNumbers());
            Assert.AreEqual("localhost-Core", "localhost-Core123".RemoveLastNumbers());
            Assert.AreEqual("c", "c".RemoveLastNumbers());
            Assert.AreEqual("", "".RemoveLastNumbers());
            Assert.AreEqual("c", "c1".RemoveLastNumbers());

        }
    }
}
