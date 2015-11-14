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
            Assert.IsNotNull(MainViewModel.ViewModel.SelectedConfiguration);
            Assert.IsNotNull(MainViewModel.ViewModel.SelectedDataset);
            Assert.IsNotNull(MainViewModel.ViewModel.SelectedDatasource);
        }

        [TestMethod]
        public void Navigate_To_Root_Test()
        {
            MainViewModel.ViewModel.Command = "root";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
            //Assert.IsNotNull(MainViewModel.ViewModel.CurrentTable);
        }

        [TestMethod]
        public void Navigate_Back_Test()
        {
            MainViewModel.ViewModel.Command = "back";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
            //Assert.IsNotNull(MainViewModel.ViewModel.CurrentTable);
        }
    }
}
