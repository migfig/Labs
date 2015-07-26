using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Reflector
{
    public class Parser
    {
        private readonly bool _includeSystemObjects;
        private readonly string _sourcePath;

        public Parser()
            :this("", true)
        {
        }

        public Parser(string sourcePath = "", bool includeSystemObjects = false)
        {
            _sourcePath = sourcePath;
            _includeSystemObjects = includeSystemObjects;
        }

        [Description("Sample attribute")]
        public string Render(Type[] onlyTypes = null, string[] onlyMethods = null)
        {
            if(string.IsNullOrEmpty(_sourcePath))
            {
                return string.Empty;
            }

            if(IsFile(_sourcePath))
            {
                return renderFile(_sourcePath, onlyTypes, onlyMethods);
            }
            else if(IsPath(_sourcePath))
            {
                var files = new StringBuilder();

                foreach(var file in Directory.GetFiles(_sourcePath, "*.dll"))
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
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                type.FullName) + ".xml";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var methods = type.GetMethods();
            if(!_includeSystemObjects)
            {
                methods = methods.Where(m => !AppConfig.IncludeNetObjectMethods.Contains(m.Name))
                    .ToArray();
            }
            if(null != onlyMethods && onlyMethods.Any())
            {
                methods = methods.Where(m => onlyMethods.Contains(m.Name)).ToArray();
            }

            using (var writer = new StreamWriter(fileName))
            {
                var root =
                    new XElement("type",
                            new XAttribute("name", type.FullName),
                        
                        new XElement("attributes",
                        from ta in type.GetCustomAttributes(true)
                        select
                            new XElement("attribute",                                
                                new XAttribute("type", ta.GetType().FullName),
                                GetAttributeValues(ta)
                            )
                         ),

                        new XElement("properties",
                        from prop in type.GetProperties()
                        select
                            new XElement("property",
                                new XAttribute("name", prop.Name),
                                new XAttribute("type", prop.PropertyType.FullName)
                            )
                         ),

                        new XElement("methods",                        
                        from m in methods
                        select
                                new XElement("method",
                                    new XAttribute("name", m.Name),
                                    new XAttribute("type", m.ReturnType.FullName),
                                    new XAttribute("public", m.IsPublic),
                                    new XAttribute("private", m.IsPrivate),
                                    new XAttribute("static", m.IsStatic),

                        new XElement("attributes",
                        from ma in m.GetCustomAttributes(true)                      
                        select
                                new XElement("attribute",
                                    new XAttribute("type", ma.GetType().FullName),
                                    GetAttributeValues(ma)
                                )
                            ),
                            
                        new XElement("parameters",
                        from p in m.GetParameters()
                        select
                                new XElement("parameter",
                                    new XAttribute("name", p.Name),
                                    new XAttribute("type", p.ParameterType.FullName)
                                )
                            )
                        )
                    )
                );

                if (!string.IsNullOrEmpty(_sourcePath))
                {
                    root.Add(new XAttribute("source", _sourcePath));
                }

                root.Save(writer);
            }

            return fileName;
        }

        private XElement GetAttributeValues(object attribute)
        {
            return
                new XElement("properties",
                        from prop in attribute.GetType().GetProperties()
                        let value = prop.GetValue(attribute) ?? ""
                        select
                            new XElement("property",
                                new XAttribute("name", prop.Name),
                                new XAttribute("type", prop.PropertyType.FullName),
                                new XAttribute("value", value.ToString())
                            )
                         );
        }

        public static bool IsFile(string fileName)
        {
            return File.Exists(fileName)
                && Path.GetExtension(fileName).ToLower() == "dll";
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
