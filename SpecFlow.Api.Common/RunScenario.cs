using System;
using System.Linq;
using System.Collections.Generic;

namespace SpecFlow.Api.Common
{
    public class RunSuite
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public IList<RunScenario> Scenarios { get; set; }

        public RunSuite()
        {
            Scenarios = new List<RunScenario>();
        }

        public IEnumerable<KeyValuePair<string, object>> GetSummary()
        {
            return new List<KeyValuePair<string, object>>{
                    new KeyValuePair<string, object>("Total Scenarios:", Scenarios.Count.ToString()),
                    new KeyValuePair<string, object>("Total Run time:", Scenarios.Sum(x => x.RunTime).ToString("###,##0.0#") + " Sec"),
                    new KeyValuePair<string, object>("Run date:", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                };
        }
    }

    public class RunScenario
    {
        public string Name { get; set; }
        public string Program { get; set; }
        public string Args { get; set; }
        public string Method { get; set; }
        public string Endpoint { get; set; }
        public DateTime StartTime { get; set; }
        public float RunTime { get; set; }
        public string Output { get; set; }
        public IList<KeyValuePair<string, object>> Headers { get; set; } 
        public string Payload { get; set; }
        public string Results { get; set; }

        public RunScenario()
        {
            Headers = new List<KeyValuePair<string, object>>();
            StartTime = DateTime.UtcNow;
        }

        public void SetRunTime()
        {
            RunTime = DateTime.UtcNow.Subtract(StartTime).Milliseconds / 60.0f;
        }

        public IEnumerable<KeyValuePair<string, object>> GetBodyProperties()
        {
            foreach (var p in GetType().GetProperties())
                yield return new KeyValuePair<string, object>(p.Name, p.GetValue(this));
        }
    }
}
