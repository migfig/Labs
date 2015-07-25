using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Reflector.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if(null == args && !args.Any())
            {
                DisplayUsage();
                return;
            }

            var options = ParseCommandLine(string.Join(" ", args));

            if(!options.Any() && (!Parser.IsFile(options["file"]) || !Parser.IsPath(options["path"])))
            {
                DisplayUsage();
                return;
            }

            var parser = new Parser(
                options.ContainsKey("file") ? options["file"] : options["path"], 
                options.ContainsKey("include"));
            System.Console.WriteLine(parser.Render(typeof(Parser)));
        }

        private static Dictionary<string, string> ParseCommandLine(string args)
        {
            var options = new Dictionary<string, string> { 
                { "file",    @"(?<file>[a-zA-Z0-9:\\\-\.]*[\.]dll)" },
                { "path",    @"((?<pathOption>[\-]p)[\s]*(?<path>[a-zA-Z0-9:\\\-\.]*))" },
                { "types",   @"((?<typeOption>[\-]t)[\s]*(?<types>[a-zA-Z0-9,\.]*))" },
                { "methods", @"((?<methodOption>[\-]m)[\s]*(?<methods>[a-zA-Z0-9,]*))" }
            };
            var dict = new Dictionary<string, string>();

            if (args.Contains("-i"))
            {
                dict.Add("include", "true");
            }

            foreach(var key in options.Keys)
            {
                var value = Regex.Match(args, options[key]).Groups[key].Value;
                if(!string.IsNullOrEmpty(value))
                {
                    if(key == "file" || key == "path")
                    {
                        value = Parser.BuildPath(value);
                    }
                    dict.Add(key, value);
                }
            }

            return dict;
        }
                
        private static void DisplayUsage()
        {
            System.Console.WriteLine("Builder usage:");
            System.Console.WriteLine("{0}builder [<file>.dll] [options]", "\t");
            System.Console.WriteLine("{0}builder [options]{1}", "\t", Environment.NewLine);
            System.Console.WriteLine("{0}where options are:", "\t");
            System.Console.WriteLine("{0}-p <path>", "\t\t");
            System.Console.WriteLine("{0}specifies the path to process. example: is c:\\mybin\\ or .", "\t\t\t");
            System.Console.WriteLine("{0}-i", "\t\t");
            System.Console.WriteLine("{0}include .NET System.Object methods", "\t\t\t");
            System.Console.WriteLine("{0}-t <type list>", "\t\t");
            System.Console.WriteLine("{0}list of only types (comma separated) to process", "\t\t\t");
            System.Console.WriteLine("{0}-m <type list>", "\t\t");
            System.Console.WriteLine("{0}list of only methods (comma separated) to process", "\t\t\t");
        }
    }
}
