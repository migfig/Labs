using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RelatedRecords.Data.ViewModels;

namespace RelatedRecords.Data.Tests
{
    [TestClass]
    public class DataTests
    {
        [TestInitialize]
        public void Initialize()
        {
            MainViewModel.ViewModel.LoadConfiguration();
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void Configuration_Is_Valid_Test()
        {
            Assert.IsNotNull(MainViewModel.ViewModel.SelectedConfiguration);
            Assert.IsTrue(MainViewModel.ViewModel.SelectedConfiguration.Dataset.Any());
        }
    }
}
