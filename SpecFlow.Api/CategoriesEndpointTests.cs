using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecFlow.Api.Context;
using System.IO;
using System.Reflection;
using TechTalk.SpecFlow;

namespace SpecFlow.Api
{
    [Binding]
    public class CategoriesEndpointTests
    {
        private readonly EndpointContext _context;
        public CategoriesEndpointTests(EndpointContext context)
        {
            _context = context;
        }

        [Given(@"I provide run settings as table")]
        public void GivenIProvideRunSettingsAsTable(Table table)
        {
            _context.SuiteTitle = FeatureContext.Current.FeatureInfo.Title;
            _context.Settings = table;
        }

        [Given(@"I call the authentication endpoint with values")]
        public void GivenICallTheAuthenticationEndpointWithValues(Table table)
        {
            var result = _context.Call(table, ScenarioContext.Current.StepContext.StepInfo);
            Assert.IsTrue(result);
        }

        [Given(@"then call the token endpoint with values")]
        public void GivenThenCallTheTokenEndpointWithValues(Table table)
        {
            var result = _context.Call(table, ScenarioContext.Current.StepContext.StepInfo);
            Assert.IsTrue(result);
        }

        [Given(@"I have been granted with a valid access token '(.*)'")]
        public void GivenIHaveBeenGrantedWithAValidAccessToken(string token)
        {
            Assert.IsTrue(_context.IsTokenValid(token));
        }


        [When(@"I call the endpoint with values")]
        public void WhenICallTheEndpointWithValues(Table table)
        {            
            var result = _context.Call(table, ScenarioContext.Current.StepContext.StepInfo);
            Assert.IsTrue(result);
        }

        [Then(@"result items count should be (.*) and values match the table for property '(.*)'")]
        public void ThenResultItemsCountShouldBeAndValuesMatchTheTableForProperty(int count, string property, Table table)
        {
            //Following code should be encapsulated in some sort of object helper, so code gets simplified into just a few lines (result object => ResultsHelper object)
            //also, create comparer for ResultsHelper to Table
            var result = _context.GetResult(property);
            Assert.IsNotNull(result);
            var type = result.GetType();
            var countMethod = type.GetMethod("get_Count");
            var isArray = countMethod != null;
            if (isArray)
            {
                Assert.IsNotNull(countMethod);
                var objCount = (int)countMethod.Invoke(result, null);
                Assert.AreEqual(count, objCount);
                Assert.AreEqual(table.RowCount, objCount);
                for (var i = 0; i < objCount; i++)
                {
                    var itemMethod = type.GetMethod("get_Item");
                    Assert.IsNotNull(itemMethod);
                    var item = itemMethod.Invoke(result, new object[] { i });
                    Assert.IsNotNull(item);
                    var row = table.Rows[i];
                    Assert.IsNotNull(row);
                    foreach (var h in table.Header)
                    {
                        var value = row[h];
                        var prop = item.GetType().GetProperty(h);
                        Assert.IsNotNull(prop);
                        var itemPropertyValue = prop.GetValue(item);
                        Assert.IsNotNull(itemPropertyValue);
                        Assert.AreEqual(value, itemPropertyValue.ToString());
                    }
                }
            }
            else
            {
                Assert.AreEqual(table.RowCount, 1);
                var row = table.Rows[0];
                Assert.IsNotNull(row);
                foreach (var h in table.Header)
                {
                    var value = row[h];
                    var prop = type.GetProperty(h);
                    Assert.IsNotNull(prop);
                    var itemPropertyValue = prop.GetValue(result);
                    Assert.IsNotNull(itemPropertyValue);
                    Assert.AreEqual(value, itemPropertyValue.ToString());
                }
            }
        }

        [Given(@"all tests have successfuly run and the results file '(.*)' is generated")]
        public void GivenAllTestsHaveSuccessfulyRunThenTheResultsFileIsGenerated(string file)
        {
            Assert.IsTrue(_context.SaveResults(file));
        }

        [Then(@"I can take a look at the results file")]
        public void ThenICanTakeALookAtTheResultsFile()
        {
            _context.RunProcess(@"C:\Windows\explorer.exe", _context.ResultsFile);
        }
    }
}
