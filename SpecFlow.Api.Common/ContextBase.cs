using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecFlow.Api.Common.Utils;
using System.Linq;
using TechTalk.SpecFlow;

namespace SpecFlow.Api.Common
{
    public class ContextBase
    {
        protected static EndpointContext _context;
        protected static RunSuite Suite { get { return FeatureContext.Current.Get<RunSuite>("suite"); } }

        public ContextBase(EndpointContext context)
        {
            _context = context;
        }

        protected bool IsAuthenticated()
        {
            var suite = Suite;
            if (suite == null || !suite.Scenarios.Any()) return false;

            var scenario = suite.Scenarios.FirstOrDefault(x => x.Results.Contains("Token"));
            if (scenario == null) return false;

            return _context.HasAuthenticationToken("Token",
                scenario.Results,
                "using System; public class Results {public string Token {get;set;} public DateTime ExpirationDate {get;set;} public string TargetUrl {get;set;}}");
        }

        protected static void RegisterSuite()
        {
            FeatureContext.Current.Add("suite", new RunSuite());
        }

        protected static void SaveSuiteResults()
        {
            var suite = Suite;
            Assert.IsTrue(_context.SaveResults(suite));
            
            _context.RunProcess(@"C:\Windows\explorer.exe", PdfReport.CreateReport(suite));
        }

        protected static void RegisterScenario()
        {
            var suite = Suite;
            suite.Name = FeatureContext.Current.FeatureInfo.Title;
            suite.Scenarios.Add(_context.Scenario);
        }
    }
}
