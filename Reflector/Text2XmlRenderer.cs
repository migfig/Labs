using Common;
using Reflector.Draw.io;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Reflector
{
    public class Text2XmlRenderer : BaseRenderer, IRenderable
    {
        public string SourcePath
        {
            get
            {
                return _sourcePath;
            }
        }

        #region non used properties/methods

        public Assembly AssemblySource
        {
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IncludeSystemObjects
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object GetAttributeValues(object attribute)
        {
            throw new NotImplementedException();
        }

        #endregion

        public Text2XmlRenderer(string sourcePath = "")
            :base(sourcePath)
        {
        }

        public string Render(Type type, Type[] onlyTypes, string[] onlyMethods)
        {
            if (File.Exists(_sourcePath))
            {
                var fileName = _sourcePath.Replace(".txt", ".xml");
                if (File.Exists(fileName))
                    File.Delete(fileName);

                var dict = Parser.ParseText(_sourcePath);
                if(dict.Any())
                {
                    var model = new mxGraphModel
                    {
                        dx = ushort.Parse(AppConfig.DrawIOSettings["dx"])
                        ,dy = ushort.Parse(AppConfig.DrawIOSettings["dy"])
                        ,grid = byte.Parse(AppConfig.DrawIOSettings["grid"])
                    };

                    if (XmlHelper<mxGraphModel>.Save(fileName, model))
                        return fileName;
                }
            }
            return string.Empty;
        }
    }
}
