using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace RelatedRecords
{
    public class XmlHelper<T> where T: class
    {
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
                LogError(e);
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
                LogError(e);
            }

            return false;
        }

        private static void LogError(Exception e)
        {
            System.Diagnostics.Debug.Write(e.Message);
        }
    }
}
