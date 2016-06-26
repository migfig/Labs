using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Common
{
    public class XmlHelper2<T> where T: class
    {
        public static T Load(XElement source)
        {
            try
            {
                using (var stream = XmlReader.Create(new StringReader(source.ToString())))
                {
                    var ser = new XmlSerializer(typeof(T));
                    return (T)ser.Deserialize(stream);
                }
            }
            catch (Exception)
            {
            }

            return Activator.CreateInstance<T>();
        }

        public static T LoadFromString(string source)
        {
            try
            {
                using (var stream = XmlReader.Create(new StringReader(source)))
                {
                    var ser = new XmlSerializer(typeof(T));
                    return (T)ser.Deserialize(stream);
                }
            }
            catch (Exception)
            {
            }

            return Activator.CreateInstance<T>();
        }

        public static string Save(T item, bool omitXml = false)
        {
            try
            {
                var builder = new StringBuilder();
                using (var stream = XmlWriter.Create(builder, 
                    new XmlWriterSettings {
                        Indent = true,
                        OmitXmlDeclaration = omitXml                        
                    }))
                {
                    var ser = new XmlSerializer(typeof(T));
                    ser.Serialize(stream, item);
                }

                return builder.ToString();
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }
    }
}
