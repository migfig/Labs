using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Reflector
{
    public class Parser
    {
        private readonly IRenderable _renderer;

        public Parser(IRenderable renderer)
        {
            _renderer = renderer;
        }

        public string Render(string[] onlyTypes = null, string[] onlyMethods = null)
        {
            if(string.IsNullOrEmpty(_renderer.SourcePath))
            {
                return string.Empty;
            }

            if(IsFile(_renderer.SourcePath))
            {
                return renderFile(_renderer.SourcePath, onlyTypes, onlyMethods);
            }
            else if(IsPath(_renderer.SourcePath))
            {
                var files = new StringBuilder();

                foreach(var file in Directory.GetFiles(_renderer.SourcePath, "*.dll"))
                {
                    files.AppendFormat("{0}{1}", files.Length > 0 ? ";" : string.Empty,
                        renderFile(file, onlyTypes, onlyMethods));
                }

                return files.ToString();
            }

            return string.Empty;
        }

        private bool isAssignable(Assembly asm, Type t, string[] types)
        {
            foreach (var type in types)
            {
                if (t.Name.Contains(type))
                {
                    return true;
                }
            }

            return false;
        }
       
        private string renderFile(string fileName, string[] onlyTypes = null, string[] onlyMethods = null)
        {
            try
            {
                if (IsDllFile(fileName))
                {
                    var asm = Assembly.LoadFrom(fileName);
                    _renderer.AssemblySource = asm;
                    var types = asm.GetTypes();
                    if (onlyTypes != null && onlyTypes.Any())
                    {
                        types = types.Where(t => isAssignable(asm, t, onlyTypes)).ToArray();
                    }

                    var files = new StringBuilder();
                    foreach (var t in types)
                    {
                        files.AppendFormat("{0}{1}", files.Length > 0 ? ";" : string.Empty,
                            render(t, null, onlyMethods));
                    }

                    return files.ToString();
                }
                else if(IsXmlFile(fileName))
                {
                    return _renderer.Render(this.GetType(), null, null);
                }
                else if(IsTextFile(fileName))
                {
                    return _renderer.Render(this.GetType(), null, null);
                }
                else
                {
                    return "Unsupported filename " + fileName;
                }
            }
            catch (Exception e)
            {
                Common.Extensions.ErrorLog.Error(e, "@ render {fileName}", fileName);
                return e.Message + (e.InnerException != null ? e.InnerException.Message : string.Empty);
            }
        }

        public string Render(Type type)
        {
            return render(type, null, null);
        }

        public string Render(Type type, params string[] onlyMethods)
        {
            return render(type, null, onlyMethods);
        }

        public string Render(Type type, params Type[] onlyTypes)
        {
            return render(type, onlyTypes, null);
        }

        private string render(Type type, Type[] onlyTypes, string[] onlyMethods)
        {
            return _renderer.Render(type, onlyTypes, onlyMethods);
        }
        
        public static bool IsFile(string fileName)
        {
            return File.Exists(fileName)
                && ".dll,.xml,.xslt,.xsl,.yaml,.txt,.docx".Split(',')
                    .Contains(Path.GetExtension(fileName).ToLower());
        }
         
        public static bool IsXmlFile(string fileName)
        {
            return File.Exists(fileName)
                && Path.GetExtension(fileName).ToLower() == ".xml";
        }

        public static bool IsXslFile(string fileName)
        {
            return File.Exists(fileName)
                && (Path.GetExtension(fileName).ToLower() == ".xsl"
                    || Path.GetExtension(fileName).ToLower() == ".xslt");
        }

        public static bool IsDllFile(string fileName)
        {
            return File.Exists(fileName)
                && Path.GetExtension(fileName).ToLower() == ".dll";
        }

        public static bool IsYamlFile(string fileName)
        {
            return File.Exists(fileName)
                && Path.GetExtension(fileName).ToLower() == ".yaml";
        }

        public static bool IsTextFile(string fileName)
        {
            return File.Exists(fileName)
                && Path.GetExtension(fileName).ToLower() == ".txt";
        }

        public static bool IsWordFile(string fileName)
        {
            return File.Exists(fileName)
                && Path.GetExtension(fileName).ToLower() == ".docx";
        }

        public static bool IsPath(string path)
        {
            return Directory.Exists(BuildPath(path));
        }

        public static string BuildPath(string path)
        {
            if (path == ".")
            {
                return Environment.CurrentDirectory;
            }
            else if (Path.GetDirectoryName(path) == "")
            {
                return Path.Combine(Environment.CurrentDirectory, path);
            }

            return path;
        }

        public static Dictionary<string, string> ParseText(string fileName)
        {
            var dict = new Dictionary<string, string>();
            if (File.Exists(fileName))
            {
                var reg = new Regex(@"((?<key>\d{1,3}.\d{1,3}(.\d{1,3})?)\s+(?<text>[\sa-zA-Z0-9\.\-_\\/,;\?#\'\+\<\>\[\]\(\)\$’“”]*)");
                using (var stream = File.OpenText(fileName))
                {
                    string line = string.Empty;
                    while(!string.IsNullOrWhiteSpace((line = stream.ReadLine())))
                    {
                        var match = reg.Match(line);
                        if(match.Success)
                        {
                            var key = match.Groups["key"].Value;
                            var text = match.Groups["text"].Value;
                            if(!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(text)
                                && !dict.ContainsKey(key))
                            {
                                dict.Add(key, text);
                            }
                        }
                    }
                }
            }

            return dict;
        }
    }
}
