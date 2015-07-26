using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflector
{
    public static class AppConfig
    {
        private static Configuration _config;
        public static Configuration Config
        {
            get
            {
                if(_config == null)
                {
                    _config = XmlHelper<Configuration>.Load(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuration.xml")
                        );
                }

                return _config;
            }
        }

        public static Dictionary<string, string> CommandLineArguments
        {
            get
            {
                var results = new Dictionary<string, string>();
                var options = Config.Datasources
                    .Where(d => d.Type == enSourceType.GroupedTextList)
                    .First(f => f.Name == "CommandLineArguments")
                    .Items.Cast<GroupSource>()
                    .Where(g => g.Name == "RegularExpressions")
                    .First();
                    
                foreach(var o in options.Text)
                {
                    results.Add(o.Name, o.Value.Trim());
                }

                return results;
            }
        }

        public static TextItem GetGroupTemplateItem(string itemName)
        {
            return Config.Datasources
                    .Where(d => d.Type == enSourceType.GroupedTextList)
                    .First(g => g.Name == "IODocumentation")
                    .Items.Cast<GroupSource>()
                    .Where(g => g.Name == "Templates")
                    .First()
                    .Text.Where(t => t.Name == itemName)
                    .First();
        }

        public static IEnumerable<string> IncludeNetObjectMethods
        {
            get
            {
                return from t in Config.Datasources
                    .Where(d => d.Type == enSourceType.TextList)
                    .First(f => f.Name == "IgnoreObjectMethods")
                    .Items.Cast<TextSource>()
                    .First().Item
                       select t.Value.Trim();
            }
        }
    }
}
