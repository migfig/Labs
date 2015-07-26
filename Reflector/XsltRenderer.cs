using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace Reflector
{
    public class XsltRenderer : BaseRenderer, IRenderable
    {
        private readonly string _xsltFile;
        public XsltRenderer(string xsltFile, string sourcePath = "", bool includeSystemObjects = false)
            : base(sourcePath, includeSystemObjects)
        {
            _xsltFile = xsltFile;
        }

        public bool IncludeSystemObjects
        {
            get { return _includeSystemObjects; }
        }

        public string SourcePath
        {
            get { return _sourcePath; }
        }

        public object GetAttributeValues(object attribute)
        {
            throw new NotImplementedException();
        }

        public string Render(Type type, Type[] onlyTypes, string[] onlyMethods)
        {
            var fileName = _xsltFile.ToLower().Replace(".xslt", ".json");
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            try
            {
                var xslt = new XslCompiledTransform(true);
                xslt.Load(_xsltFile);
                xslt.Transform(_sourcePath, fileName);
            }
            catch (Exception)
            {
                ;
            }

            return fileName;
        }
    }
}
