using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RelatedRows.Domain
{
    public class XmlHelper<T> where T: class
    {
        public static object ErrorLog { get; private set; }

        public static T Load(string fileName)
        {
            Logger.Log.Verbose("Loading file {@fileName}", fileName);

            try
            {
                using(var stream = File.OpenText(fileName))
                {
                    var ser = new XmlSerializer(typeof(T));
                    return (T)ser.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(e, "Exception while loading {@fileName}", fileName);                
            }

            return Activator.CreateInstance<T>();
        }

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
            catch (Exception e)
            {
                Logger.Log.Error(e, "Exception while loading {0}", source.ToString().Substring(0, 50));
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
            catch (Exception e)
            {
                Logger.Log.Error(e, "Exception while loading {0}", source.Substring(0, 50));
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
            catch (Exception e)
            {
                Logger.Log.Error(e, "Exception while saving item {0}", item.ToString());
            }

            return string.Empty;
        }

        public static bool Save(string fileName, T item)
        {
            Logger.Log.Verbose("Saving file {@fileName}", fileName);

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            try
            {
                using (var stream = File.CreateText(fileName))
                {
                    var ser = new XmlSerializer(typeof(T));
                    ser.Serialize(stream, item);
                }

                return true;
            }
            catch (Exception e)
            {
                Logger.Log.Error(e, "Exception while saving to {@fileName}", fileName);
            }

            return false;
        }        
    }
}
