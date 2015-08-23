using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Reflector
{
    public class XmlRenderer : BaseRenderer, IRenderable
    {
        public XmlRenderer(string sourcePath = "", bool includeSystemObjects = false)
            : base(sourcePath, includeSystemObjects)
        {
        }

        public bool IncludeSystemObjects
        {
            get { return _includeSystemObjects; }
        }

        public string SourcePath
        {
            get { return _sourcePath; }
        }

        public string Render(Type type, Type[] onlyTypes, string[] onlyMethods)
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                type.FullName) + ".xml";
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
                        let value = prop.GetValue(attribute) ?? ""
                        select
                            new XElement("property",
                                new XAttribute("name", prop.Name),
                                new XAttribute("type", prop.PropertyType.FullName),
                                new XAttribute("value", value.ToString())
                            )
                         );
        }
    }
}
