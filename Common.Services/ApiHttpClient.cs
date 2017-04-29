using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Reflection;
using System.Xml.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Common.Services
{
    public interface IGenericApiService<T>: IDisposable where T: class 
    {
        Task<IEnumerable<T>> GetItems(string url);
        Task<T> GetItem(string url);
        Task<bool> AddItems(IEnumerable<T> items);
        Task<bool> AddItem(T item);
        Task<bool> RemoveItem(T item, string propertyName);
        Task<string> TransformXml(XElement xml, string styleSheet);
    }    

    public class ApiServiceFactory
    {
        public static IGenericApiService<T> CreateService<T>(bool useJson = true) where T : class
        {
            const string baseUrl = "http://localhost:3033/api/";
            return new GenericApiHttpClient<T>(baseUrl, null, useJson);
        }

        public static IGenericApiService<T> CreateService<T>(string baseUrl, IList<KeyValuePair<string, object>> headers, bool useJson = true) where T: class
        {
            return new GenericApiHttpClient<T>(baseUrl, headers, useJson);
        }
    }

    public class GenericApiHttpClient<T> : IDisposable, IGenericApiService<T> where T : class 
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:3033/api/";
        private readonly string _contentType;

        public GenericApiHttpClient(string baseUrl, IList<KeyValuePair<string, object>> headers, bool useJson)
        {
            _client = new HttpClient();
            _contentType = useJson ? "application/json" : "application/xml";
            _client.DefaultRequestHeaders.Add("Accept", _contentType);
            _baseUrl = baseUrl;
            foreach (var header in headers)
                _client.DefaultRequestHeaders.Add(header.Key, header.Value.ToString());

            //security settings
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

            ServicePointManager.CheckCertificateRevocationList = false;
            ServicePointManager.ServerCertificateValidationCallback += (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) =>
            {
                return true;
            };
        }

        public async Task<bool> AddItem(T item)
        {
            var className = item.GetType().FullName.Split('.').Last().ToLower();
            string value = string.Empty;
            if (_contentType.EndsWith("json"))
            {
                value = JsonConvert.SerializeObject(item);
            }
            else
            {
                value = XmlHelper<T>.Save(item);
            }
            using (var content = new ByteArrayContent(Encoding.UTF8.GetBytes(value)))
            {
                content.Headers.Add("Content-Type", _contentType);
                content.Headers.Add("Content-Length", value.Length.ToString());
                var response = await _client.PostAsync(_baseUrl + className + "/add", content);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> RemoveItem(T item, string propertyName)
        {
            var className = item.GetType().FullName.Split('.').Last().ToLower();
            var response = await _client.DeleteAsync(_baseUrl + className + "s/remove/" + item.GetType().GetRuntimeProperty(propertyName).GetValue(item).ToString());

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddItems(IEnumerable<T> items)
        {
            var className = items.First().GetType().FullName.Split('.').Last().ToLower();
            string value = string.Empty;
            if (_contentType.EndsWith("json"))
            {
                value = JsonConvert.SerializeObject(items);
            }
            else
            {
                //value = XmlHelper<T>.Save(items);
            }
            using (var content = new ByteArrayContent(Encoding.UTF8.GetBytes(value)))
            {
                content.Headers.Add("Content-Type", _contentType);
                content.Headers.Add("Content-Length", value.Length.ToString());
                var response = await _client.PostAsync(_baseUrl + className + "/additems", content);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<IEnumerable<T>> GetItems(string url)
        {
            try
            {
                var response = await _client.GetAsync(_baseUrl + url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
                }
            }
            catch (Exception) {; }

            return new List<T>();
        }

        public async Task<T> GetItem(string url)
        {
            try
            {
                var response = await _client.GetAsync(_baseUrl + url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception) {; }

            return Activator.CreateInstance<T>();
        }

        public async Task<string> TransformXml(XElement xml, string styleSheet)
        {
            try
            {
                string value = xml.ToString();
                using (var content = new ByteArrayContent(Encoding.UTF8.GetBytes(value)))
                {
                    content.Headers.Add("Content-Type", _contentType);
                    content.Headers.Add("Content-Length", value.Length.ToString());
                    var response = await _client.PostAsync(_baseUrl + "xslt/transform?stylesheet=" + styleSheet, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception) {; }

            return string.Empty;
        }

        public void Dispose()
        {
            _client.Dispose();
        }        
    }
}
