using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
                options.ContainsKey("includeflag"));
            System.Console.WriteLine(parser.Render(typeof(Parser)));
        }

        private static Dictionary<string, string> ParseCommandLine(string args)
        {
            var options = AppConfig.CommandLineArguments;
            var dict = new Dictionary<string, string>();

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
