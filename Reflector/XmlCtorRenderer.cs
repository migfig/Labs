using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Reflector
{
    public class XmlCtorRenderer : BaseRenderer, IRenderable
    {
        private Assembly _assemblySource;

        public Assembly AssemblySource
        {
            set { _assemblySource = value; }
        }

        public bool IncludeSystemObjects
        {
            get { return _includeSystemObjects; }
        }

        public string SourcePath
        {
            get { return _sourcePath; }
        }

        public XmlCtorRenderer(string sourcePath = "", bool includeSystemObjects = false)
            : base(sourcePath, includeSystemObjects)
        {
        }

        public string Render(Type type, Type[] onlyTypes, string[] onlyMethods)
        {
            var basePath = Path.Combine(Path.GetDirectoryName(_sourcePath), "output");
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            var fileName = Path.Combine(basePath, type.FullName.Split('.').Last().Replace("Controller", string.Empty)) + "-ctor.xml";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var methods = type.GetMethods().Where(x => x.CustomAttributes.Any());
            if (!_includeSystemObjects)
            {
                methods = methods.Where(m => !AppConfig.IncludeNetObjectMethods.Contains(m.Name))
                         .ToArray();
            }
            if (null != onlyMethods && onlyMethods.Any())
            {
                methods = methods.Where(m => onlyMethods.Contains(m.Name)).ToArray();
            }

            var ctors = type.GetConstructors();

            using (var writer = new StreamWriter(fileName))
            {
                var root =
                    new XElement("type",
                            new XAttribute("name", type.FullName),

                        //GetTypeConstructors(type),
                        new XElement("constructors",
                        from ctor in ctors
                        select
                             new XElement("constructor",
                                 new XAttribute("name", type.FullName.Split('.').Last()),

                                 new XElement("parameters",
                                     from par in ctor.GetParameters()
                                     where par != null
                                     select
                                         new XElement("parameter",
                                             new XAttribute("name", par.Name),
                                             new XAttribute("type", par.ParameterType.FullName),
                                             GetTypeConstructors(par.ParameterType),
                                             GetTypeMethods(par.ParameterType)
                                         )
                                 )
                             )
                        ),

                        GetTypeMethods(type, methods)
                );

                if (!string.IsNullOrEmpty(_sourcePath))
                {
                    root.Add(new XAttribute("source", _sourcePath));
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
            if (type == null || type.IsPrimitive || type == typeof(string) || type.FullName == "System.Type")
                return null;

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
                                : prop.PropertyType.GenericTypeArguments != null
                                    ? GetTypeProperties(prop.PropertyType.GenericTypeArguments.FirstOrDefault())
                                    : null
                        )
                     );
        }

        public string GetInstanceValue(Type type)
        {
            try
            {
                return type == typeof(DateTime)
                    ? DateTime.Now.Ticks.ToString(CultureInfo.CurrentCulture)
                    : Activator.CreateInstance(type).ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public XElement GetTypeConstructors(Type type)
        {
            var ctors = type.GetConstructors();
            return new XElement("constructors",
                       from ctor in ctors
                       select
                            new XElement("constructor",
                                new XAttribute("name", type.FullName.Split('.').Last()),

                                new XElement("parameters",
                                    from par in ctor.GetParameters()
                                    where par != null
                                    select
                                        new XElement("parameter",
                                            new XAttribute("name", par.Name),
                                            new XAttribute("type", par.ParameterType.FullName)
                                            //, GetTypeConstructors(par.ParameterType)
                                            , GetTypeMethods(par.ParameterType)
                                        )
                                )
                            )
                       );
        }

        public XElement GetTypeMethods(Type type, IEnumerable<MethodInfo> metds = null)
        {
            var methods = metds ?? type.GetMethods().Where(m => !AppConfig.IncludeNetObjectMethods.Contains(m.Name));
            return new XElement("methods",
                       from m in methods
                       select
                               new XElement("method",
                                   new XAttribute("name", m.Name),
                                   new XAttribute("type", m.ReturnType.FullName),

                                   m.ReturnType.FullName.Contains("System.Collections")
                                       ? m.ReturnType.GenericTypeArguments != null && m.ReturnType.GenericTypeArguments.First() != null
                                        ? new XAttribute("itemType", m.ReturnType.GenericTypeArguments.First().FullName)
                                        : null
                                       : null,

                       new XElement("parameters",
                       from p in m.GetParameters()
                       where p != null
                       select
                               new XElement("parameter",
                                   new XAttribute("name", p.Name),
                                   new XAttribute("type", p.ParameterType.FullName),
                                   new XAttribute("defaultValue", GetInstanceValue(p.ParameterType)),

                                   GetTypeProperties(p.ParameterType)
                               )
                           )
                       )
                   );
        }

    }
}
