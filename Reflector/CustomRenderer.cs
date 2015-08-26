using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Reflection;

namespace Reflector
{
    public class CustomRenderer : BaseRenderer, IRenderable
    {
        private Assembly _assemblySource;

        public CustomRenderer(string sourcePath = "", bool includeSystemObjects = false)
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

        public Assembly AssemblySource
        {
            set
            {
                _assemblySource = value;
            }
        }

        public string Render(Type type, Type[] onlyTypes, string[] onlyMethods)
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                type.FullName) + ".txt";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var methods = type.GetMethods();
            if (!_includeSystemObjects)
            {
                methods = methods.Where(m => !AppConfig.IncludeNetObjectMethods.Contains(m.Name))
                    .ToArray();
            }
            if (null != onlyMethods && onlyMethods.Any())
            {
                methods = methods.Where(m => onlyMethods.Contains(m.Name)).ToArray();
            }

            var propTpl = AppConfig.GetGroupTemplateItem("Property").Value;
            var methTpl = AppConfig.GetGroupTemplateItem("Method").Value;
            var attrTpl = AppConfig.GetGroupTemplateItem("Attribute").Value;
            var parTpl = AppConfig.GetGroupTemplateItem("Parameter").Value;

            using (var writer = new StreamWriter(fileName))
            {
                var root =
                    new StrBuilder("methods".Quote(": {"),
                        
                    from m in methods
                        select
                                new StrBuilder(
                                    string.Format(methTpl,
                                        m.Name,
                                        m.ReturnType.FullName,
                                        m.IsPublic,
                                        m.IsPrivate,
                                        m.IsStatic),
                        
                        new StrBuilder("parameters".Quote(": {"),
                        from p in m.GetParameters()
                        select
                                new StrBuilder(
                                    string.Format(parTpl,
                                        p.Name,
                                        p.ParameterType.FullName,
                                        p.Name
                                    )
                                )
                            )
                        )                    
                );

                writer.Write(root.ToString() + "}");
            }

            return fileName;
        }

        public object GetAttributeValues(object attribute)
        {
            var propTpl = AppConfig.GetGroupTemplateItem("Property").Value;                

            return
                new StrBuilder("properties".Quote(": {"),

                from prop in attribute.GetType().GetProperties()
                let value = prop.GetValue(attribute) ?? ""

                select
                    new StrBuilder(
                        string.Format(propTpl,
                            prop.Name,
                            prop.PropertyType.FullName,
                            value.ToString()
                        )
                    )
                ).ToString() + " }";                              
        }
    }
}
