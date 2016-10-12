using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow.Api.Context
{
    public class RunSuite
    {
        public string Name { get; set; }
        public string Program { get; set; }
        public string FileName { get; set; }
        public IList<RunScenario> Scenarios { get; set; }

        public RunSuite()
        {
            Scenarios = new List<RunScenario>();
        }
    }
    public class RunScenario
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public int RunTime { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string Output { get; set; }
        public IList<KeyValuePair<string, string>> Headers { get; set; } 
        public string Payload { get; set; }
        public string Results { get; set; }

        public RunScenario()
        {
            Headers = new List<KeyValuePair<string, string>>();
            StartTime = DateTime.UtcNow;
        }

        public void SetRunTime()
        {
            RunTime = DateTime.UtcNow.Subtract(StartTime).Seconds;
        }
    }
}
