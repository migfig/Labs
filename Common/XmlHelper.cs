using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Common
{
    public class XmlHelper<T> where T: class
    {
        public static object ErrorLog { get; private set; }

        public static T Load(string fileName)
        {
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
                Extensions.ErrorLog.Error(e, "@ XmlHelper<T>.Load xml");
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
                Extensions.ErrorLog.Error(e, "@ XmlHelper<T>.Load xml");
                Extensions.TraceLog.Information("Tried to load {source}", source);
            }

            return Activator.CreateInstance<T>();
        }

        public static bool Save(string fileName, T item)
        {
            if(File.Exists(fileName))
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
                Extensions.ErrorLog.Error(e, "@ XmlHelper<T>.Save xml");
            }

            return false;
        }        
    }
}
