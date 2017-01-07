using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecFlow.Api.Common.Utils;
using System.Linq;
using TechTalk.SpecFlow;

namespace SpecFlow.Api.Common
{
    public class ContextBase
    {
        protected static EndpointContext _context;
        protected static RunSuite Suite(string keySuffix) { return FeatureContext.Current.Get<RunSuite>("suite" + keySuffix); }

        public ContextBase(EndpointContext context)
        {
            _context = context;
        }

        protected bool IsAuthenticated(string keySuffix)
        {
            var suite = Suite(keySuffix);
            if (suite == null || !suite.Scenarios.Any()) return false;

            var scenario = suite.Scenarios.FirstOrDefault(x => x.Results.Contains("Token"));
            if (scenario == null) return false;

            return _context.HasAuthenticationToken("Token",
                scenario.Results,
                "using System; public class Results {public string Token {get;set;} public DateTime ExpirationDate {get;set;} public string TargetUrl {get;set;}}");
        }

        protected static void RegisterSuite(string keySuffix)
        {
            FeatureContext.Current.Add("suite" + keySuffix, new RunSuite());
        }

        protected static void SaveSuiteResults(string keySuffix)
        {
            var suite = Suite(keySuffix);
            Assert.IsTrue(_context.SaveResults(suite));
            
            _context.RunProcess(@"C:\Windows\explorer.exe", PdfReport.CreateReport(suite));
        }

        protected static void RegisterScenario(string keySuffix)
        {
            var suite = Suite(keySuffix);
            suite.Name = FeatureContext.Current.FeatureInfo.Title;
            suite.Scenarios.Add(_context.Scenario);
        }
    }
}
