using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Reflector
{
    public class XslUtils
    {
        public string Iif(bool expression, string trueValue, string falseValue)
        {
            return expression ? trueValue : falseValue;
        }

        public string HttpMethod(bool get, bool post, bool put, bool patch, bool delete, bool options)
        {
            if (get)
                return "GET";
            else if (post)
                return "POST";
            else if (put)
                return "PUT";
            else if (patch)
                return "PATCH";
            else if (delete)
                return "DELETE";

            return "OPTIONS";
        }

        public string CapitalizeWords(string value)
        {
            var capitalized = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                for(var i=0; i<value.Length; i++)
                {
                    if (i == 0)
                        capitalized = value[i].ToString().ToUpper();
                    else if (Char.IsUpper(value[i]))
                        capitalized += " " + value[i];
                    else
                        capitalized += value[i];
                }
            }

            return capitalized;
        }

        public string ToLower(string value)
        {
            return value.ToLower();
        }

        public string Guid()
        {
            return System.Guid.NewGuid().ToString();
        }

        private Dictionary<string, string> _guids = new Dictionary<string, string>();
        public string Guid(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            if(!_guids.ContainsKey(key))
            {
                _guids.Add(key, Guid());
            }

            return _guids[key];
        }

        public string TimeStamp()
        {
            return DateTime.Now.ToFileTime().ToString();
        }

        public string FileName(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }

        public string TypeName(string value)
        {
            return value.Split('.').Last().Replace("Controller", string.Empty);
        }

        public string CompleteTypeName(string value)
        {
            return value.Split('.').Last();
        }

        public string FixType(string value)
        {
            var reg = new Regex(@"[a-zA-Z0-9.`]*[0-9\[]*(?<typename>[a-zA-Z0-9.]*)[,\s\]]?");
            var match = reg.Match(value);
            if(value.StartsWith("System.Collections") && null != match && match.Success)
            {
                return match.Groups["typename"].Value + "[]";
            }

            return value;
        }

        public string NewLine()
        {
            return Environment.NewLine;
        }
        public string Default(string type)
        {
            try
            {
                var t = Type.GetType(type);
                var value = t == typeof(DateTime) || t == typeof(DateTime?)
                            ? "DateTime.Now.AddHours(-8)"
                            : Activator.CreateInstance(t).ToString();

                return QuoteValue(value, "\"");
            }
            catch (Exception)
            {
            }

            return "\"\"";
        }

        private string QuoteValue(object value, string quoteChar = "'")
        {
            if ((value is string || value is Guid) && value.ToString().Trim().ToLower() != "null")
            {
                return quoteChar + value.ToString() + quoteChar;
            }

            return value.ToString();
        }
    }
}
