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
#if DEBUG
            //args = "WebApi.Example.dll -t Controller".Split(' ');
            //args = "WebApi.Example.Controllers.ProductsController.xml -r xslt -x iodocuments.xslt -o json".Split(' ');
            //args = "WebApi.Example.Controllers.ProductsController.xml -r xslt -x apitester.xslt -o xml".Split(' ');
            args = "WebApi.Example.dll -r ctorxml -t Controller".Split(' ');
#endif
            if (null == args || !args.Any())
            {
                DisplayUsage();
                return;
            }

            var options = ParseCommandLine(string.Join(" ", args));

            if(!options.Any() || !(options.ContainsKey("file") || options.ContainsKey("path")) 
                || (options.ContainsKey("file") && !Parser.IsFile(options["file"]))
                || (options.ContainsKey("path") && !Parser.IsPath(options["path"]))
                || (options["render"] == "xslt" 
                    && options.ContainsKey("xsltfile") && !Parser.IsFile(options["xsltfile"]))
                )
            {
                DisplayUsage();
                return;
            }

            IRenderable renderer = null;
            switch(options["render"])
            {
                case "text":
                    renderer = new CustomRenderer(
                        options.ContainsKey("file") ? options["file"] : options["path"],
                        options.ContainsKey("includeflag")
                    );
                    break;
                case "xslt":
                    renderer = new XsltRenderer(
                        options["xsltfile"],
                        options.ContainsKey("outext") ? options["outext"] : "json",
                        options.ContainsKey("file") ? options["file"] : options["path"],
                        options.ContainsKey("includeflag")
                    );
                    break;
                case "ctorxml":
                    renderer = new XmlCtorRenderer(
                         options.ContainsKey("file") ? options["file"] : options["path"],
                         options.ContainsKey("includeflag")
                     );
                    break;
                case "text2xml":
                    renderer = new Text2XmlRenderer(options["file"]);
                    break;

                default:
                    renderer = new XmlRenderer(
                        options.ContainsKey("file") ? options["file"] : options["path"],
                        options.ContainsKey("includeflag")
                    );
                    break;
            }

            var parser = new Parser(renderer);
            System.Console.WriteLine("Rendering as " + options["render"] + "..." + Environment.NewLine);
            System.Console.WriteLine(parser.Render(
                options.ContainsKey("types") ? options["types"].Split(',') : null,
                options.ContainsKey("methods") ? options["methods"].Split(','): null
                ));
        }

        private static Dictionary<string, string> ParseCommandLine(string args)
        {
            var options = AppConfig.CommandLineArguments;
            var dict = new Dictionary<string, string>();

            if(!args.Contains("-r"))
            {
                args += " -r xml"; //default renderer
            }

            foreach(var key in options.Keys)
            {
                var value = Regex.Match(args, options[key]).Groups[key].Value;
                if(!string.IsNullOrEmpty(value))
                {
                    if(key == "file" || key == "path" || key == "xsltfile")
                    {
                        value = Parser.BuildPath(value);
                    }
                    dict.Add(key, value);
                }
            }

            if(dict.Count == 1)
            {
                dict.Clear();
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
            System.Console.WriteLine("{0}-x <path>.xsl[t]", "\t\t");
            System.Console.WriteLine("{0}specifies the xsl file path to process.", "\t\t\t");
            System.Console.WriteLine("{0}-o ext", "\t\t");
            System.Console.WriteLine("{0}specifies the file extension for the xslt transformation.", "\t\t\t");
            System.Console.WriteLine("{0}-r <option>", "\t\t");
            System.Console.WriteLine("{0}where option can be xml, xslt, text, ctorxml, or text2xml. Default xml", "\t\t\t");
            System.Console.WriteLine("{0}-i", "\t\t");
            System.Console.WriteLine("{0}include .NET System.Object methods", "\t\t\t");
            System.Console.WriteLine("{0}-t <type list>", "\t\t");
            System.Console.WriteLine("{0}list of only types (comma separated) to process", "\t\t\t");
            System.Console.WriteLine("{0}-m <type list>", "\t\t");
            System.Console.WriteLine("{0}list of only methods (comma separated) to process", "\t\t\t");
        }
    }
}
