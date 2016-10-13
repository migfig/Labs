using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecFlow.Api.Context;
using TechTalk.SpecFlow;
using System.Linq;

namespace SpecFlow.Api
{
    [Binding]
    public class CategoriesEndpointTests
    {
        private static EndpointContext _context;
        public CategoriesEndpointTests(EndpointContext context)
        {
            _context = context;
        }

        [Given(@"I provide run settings as table")]
        public void GivenIProvideRunSettingsAsTable(Table table)
        {
            _context.Settings = table;
        }

        private bool IsAuthenticated()
        {
            var suite = FeatureContext.Current.Get<RunSuite>("suite");
            if (suite == null || !suite.Scenarios.Any()) return false;

            var scenario = suite.Scenarios.FirstOrDefault(x => x.Results.Contains("Token"));
            if (scenario == null) return false;

            return  _context.HasAuthenticationToken("Token", 
                scenario.Results,
                "using System; public class Results {public string Token {get;set;} public DateTime ExpirationDate {get;set;} public string TargetUrl {get;set;}}");
        }

        [Given(@"I call the authentication endpoint with values")]
        public void GivenICallTheAuthenticationEndpointWithValues(Table table)
        {
            if (!IsAuthenticated())
            {
                var result = _context.Call(table, ScenarioContext.Current.StepContext.StepInfo);
                Assert.IsTrue(result);
                UpdateSuiteAfterScenario();
            }
        }

        [Given(@"then call the token endpoint with values")]
        public void GivenThenCallTheTokenEndpointWithValues(Table table)
        {
            if (!IsAuthenticated())
            {
                var result = _context.Call(table, ScenarioContext.Current.StepContext.StepInfo);
                Assert.IsTrue(result);
                UpdateSuiteAfterScenario();
            }
        }

        [Given(@"I have been granted with a valid access token '(.*)'")]
        public void GivenIHaveBeenGrantedWithAValidAccessToken(string token)
        {
            Assert.IsTrue(_context.IsTokenValid(token) || IsAuthenticated());
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

        [BeforeFeature]
        public static void InitializeResultsFile()
        {
            var suite = new RunSuite();
            FeatureContext.Current.Add("suite", new RunSuite());
        }

        [AfterFeature]
        public static void SaveSuiteResultsAfterFeature()
        {
            var suite = FeatureContext.Current.Get<RunSuite>("suite");
            Assert.IsTrue(_context.SaveResults(suite));
            _context.RunProcess(@"C:\Windows\explorer.exe", suite.FileName);
        }

        [AfterScenario]
        public static void UpdateSuiteAfterScenario()
        {
            var suite = FeatureContext.Current.Get<RunSuite>("suite");
            suite.Name = FeatureContext.Current.FeatureInfo.Title;
            suite.Scenarios.Add(_context.Scenario);
        }
    }
}
