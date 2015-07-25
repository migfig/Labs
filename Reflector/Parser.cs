using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Reflector
{
    public class Parser
    {
        public string Render(Type type, string sourceFile = "")
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                type.FullName) + ".xml";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
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
                                new XAttribute("name", ta.ToString()),
                                new XAttribute("type", ta.GetType().FullName)
                            )
                         ),

                        from m in type.GetMethods()
                        select
                            new XElement("methods",
                                new XElement("method",
                                    new XAttribute("name", m.Name),
                                    new XAttribute("type", m.ReturnType.FullName),

                        from ma in m.GetCustomAttributes(true)
                        select
                            new XElement("attributes",
                                new XElement("attribute",
                                    new XAttribute("name", ma.ToString()),
                                    new XAttribute("type", ma.GetType().FullName)
                                )
                            ),

                        from p in m.GetParameters()
                        select
                            new XElement("parameters",
                                new XElement("parameter",
                                    new XAttribute("name", p.Name),
                                    new XAttribute("type", p.ParameterType.FullName)
                                )
                            )
                        )
                    )
                );

                if (!string.IsNullOrEmpty(sourceFile))
                {
                    root.Add(new XAttribute("source", sourceFile));
                }

                root.Save(writer);
            }

            return fileName;
        }
    }
}
