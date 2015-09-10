using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Reflector
{
    public class XmlRenderer : BaseRenderer, IRenderable
    {
        private Assembly _assemblySource;
        private List<Assembly> _assemblyTypes;

        public XmlRenderer(string sourcePath = "", bool includeSystemObjects = false)
            : base(sourcePath, includeSystemObjects)
        {
            _assemblyTypes = new List<Assembly>();
        }

        public bool IncludeSystemObjects
        {
            get { return _includeSystemObjects; }
        }

        public string SourcePath
        {
            get { return _sourcePath; }
        }

        public Assembly AssemblySource
        {
            set
            {
                _assemblySource = value;
            }
        }

        public string Render(Type type, Type[] onlyTypes, string[] onlyMethods)
        {
            var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output");
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            var fileName = Path.Combine(basePath, type.FullName.Split('.').Last().Replace("Controller",string.Empty)) + ".xml";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var methods = type.GetMethods().Where(x => x.CustomAttributes.Count() > 0);
            if (!_includeSystemObjects)
            {
                methods = methods.Where(m => !AppConfig.IncludeNetObjectMethods.Contains(m.Name))
                    .ToArray();
            }
            if (null != onlyMethods && onlyMethods.Any())
            {
                methods = methods.Where(m => onlyMethods.Contains(m.Name)).ToArray();
            }

            Common.Extensions.TraceLog.Information("Rendering type {type} for file {fileName}", type, fileName);
            
            using (var writer = new StreamWriter(fileName))
            {
                var root =
                    new XElement("type",
                            new XAttribute("name", type.FullName),

                        new XElement("attributes",
                        from ta in type.GetCustomAttributes(true)
                        where ta != null
                        select
                            new XElement("attribute",
                                new XAttribute("type", ta.GetType().FullName),
                                GetAttributeValues(ta)
                            )
                         ),

                        new XElement("properties",
                        from prop in type.GetProperties()
                            .Where(x => !AppConfig.IncludeNetObjectProperties.Contains(x.Name))
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

                                    m.ReturnType.FullName.Contains("System.Collections")
                                        ? new XAttribute("itemType", m.ReturnType.GenericTypeArguments.First().FullName)
                                        : null,

                        new XElement("attributes",
                        from ma in m.GetCustomAttributes(true)
                        where ma != null
                        select
                                new XElement("attribute",
                                    new XAttribute("type", ma.GetType().FullName),
                                    GetAttributeValues(ma)
                                )
                            ),

                        new XElement("parameters",
                        from p in m.GetParameters()
                        where p != null
                        select
                                new XElement("parameter",
                                    new XAttribute("name", p.Name),
                                    new XAttribute("type", p.ParameterType.FullName),
                                    new XAttribute("defaultValue", GetInstanceValue(p.ParameterType)),

                                    GetTypeProperties(p.ParameterType),

                                    new XElement("attributes",
                                    from pa in p.GetCustomAttributes(true)
                                    where pa != null
                                    select
                                        new XElement("attribute",
                                            new XAttribute("type", pa.GetType().FullName),
                                            GetAttributeValues(pa)
                                        )
                                    )
                                )
                            )
                        )
                    )
                );

                if (!string.IsNullOrEmpty(_sourcePath))
                {
                    root.Add(new XAttribute("source", _sourcePath));
                    root.Add(new XElement("assemblies",
                        from asm in _assemblyTypes
                            .Where(x => x.FullName != type.Assembly.FullName)
                            .Distinct()
                        select new XElement("assembly",
                            new XAttribute("name", asm.Location)
                        )
                    ));
                }

                root.Save(writer);
            }

            return fileName;
        }

        public object GetAttributeValues(object attribute)
        {
            return
                new XElement("properties",
                        from prop in attribute.GetType().GetProperties()
                            .Where(x => !AppConfig.IncludeNetObjectProperties.Contains(x.Name))
                        let value = prop.GetValue(attribute) ?? string.Empty
                        select
                            new XElement("property",
                                new XAttribute("name", prop.Name),
                                new XAttribute("type", prop.PropertyType.FullName),
                                new XAttribute("value", value.ToString()),

                                GetAttributeValueProperties(prop.Name, value)
                            )
                         );
        }

        public object GetAttributeValueProperties(string properyName, object value)
        {
            if (properyName != "ResponseType"
                || null == value 
                || string.IsNullOrEmpty(value.ToString()) 
                || value.ToString().Split('.').Length <= 1) return null;

            return GetTypeProperties((Type)value, true);
        }

        public XElement GetTypeProperties(Type type, bool ignoreInnerProps = false)
        {
            if (type == null || type.IsPrimitive || type == typeof(string)) return null;

            if (!_assemblyTypes.Contains(type.Assembly))
            {
                _assemblyTypes.Add(type.Assembly);
            }

            return new XElement("properties",

                    from prop in type.GetProperties()
                    let ptype = prop.PropertyType
                    select
                        new XElement("property",
                            new XAttribute("name", prop.Name),
                            new XAttribute("isArray", type.IsArray 
                                || prop.PropertyType.FullName.Contains("System.Collections")
                            ),
                            new XAttribute("type", prop.PropertyType.FullName),
                            new XAttribute("defaultValue", GetInstanceValue(prop.PropertyType)),
                            
                            ignoreInnerProps 
                                ? null
                                :
                            prop.PropertyType.IsClass 
                                    && prop.PropertyType.FullName != "System.String"
                                    && !prop.PropertyType.FullName.Contains("System.Collections")
                                ? GetTypeProperties(prop.PropertyType)
                                : GetTypeProperties(prop.PropertyType.GenericTypeArguments.FirstOrDefault())
                        )
                     );
        }

        public string GetInstanceValue(Type type)
        {
            try
            {
                if (type == typeof(DateTime))
                    return DateTime.Now.Ticks.ToString();

                return Activator.CreateInstance(type).ToString();
            }
            catch(Exception)
            {
                return string.Empty;
            }
        }
    }
}
