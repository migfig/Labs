﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace Reflector
{
    public class XsltRenderer : BaseRenderer, IRenderable
    {
        private readonly string _xsltFile;
        private readonly string _outExt;
        private Assembly _assemblySource;

        public XsltRenderer(string xsltFile, string outExt = "json", string sourcePath = "", bool includeSystemObjects = false)
            : base(sourcePath, includeSystemObjects)
        {
            _xsltFile = xsltFile;
            _outExt = outExt;
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

        public object GetAttributeValues(object attribute)
        {
            throw new NotImplementedException();
        }

        public string Render(Type type, Type[] onlyTypes, string[] onlyMethods)
        {
            var fileName = Path.Combine(Path.GetDirectoryName(_sourcePath),
                Path.GetFileNameWithoutExtension(_sourcePath) + "-" +
                Path.GetFileName(_xsltFile).ToLower().Replace(".xslt", "." + _outExt));

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            try
            {
                var xslt = new XslCompiledTransform(true);
                var args = new XsltArgumentList();
                args.AddExtensionObject("urn:schemas-reflector-com:xslt", new XslUtils());

                xslt.Load(_xsltFile);

                using (var stream = new StreamWriter(fileName)) {
                    xslt.Transform(_sourcePath, args, stream);
                }
            }
            catch (Exception e)
            {
                Common.Extensions.ErrorLog.Error(e, "@ Render type {type}", type);
            }

            return fileName;
        }
    }
}
