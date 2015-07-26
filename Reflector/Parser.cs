using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Reflector
{
    public class Parser
    {
        private readonly IRenderable _renderer;

        public Parser(IRenderable renderer)
        {
            _renderer = renderer;
        }

        [Description("Sample attribute")]
        public string Render(Type[] onlyTypes = null, string[] onlyMethods = null)
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
       
        private string renderFile(string fileName, Type[] onlyTypes = null, string[] onlyMethods = null)
        {
            try
            {
                var asm = Assembly.LoadFrom(fileName);
                var types = asm.GetTypes();
                if (onlyTypes != null && onlyTypes.Any())
                {
                    types = types.Where(t => onlyTypes.Contains(t)).ToArray();
                }

                var files = new StringBuilder();
                foreach (var t in types)
                {
                    files.AppendFormat("{0}{1}", files.Length > 0 ? ";" : string.Empty,
                        render(t, onlyTypes, onlyMethods));
                }

                return files.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
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
                && Path.GetExtension(fileName).ToLower() == ".dll";
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
    }
}
