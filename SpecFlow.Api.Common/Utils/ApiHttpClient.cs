using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace SpecFlow.Api.Common.Utils
{
    public interface IGenericApiService<T>: IDisposable where T: class 
    {
        Task<object> Get();
        Task<object> Post(T item);
        Task<object> Put(T item);
        Task<object> Delete(T item = null, string propertyName = "");
    }    

    public class ApiServiceFactory
    {        
        public static IGenericApiService<T> CreateService<T>(string baseUrl, IList<KeyValuePair<string, object>> headers) where T: class
        {
            return new GenericApiHttpClient<T>(baseUrl, headers);
        }
    }

    public class GenericApiHttpClient<T> : IDisposable, IGenericApiService<T> where T : class 
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:3033/api/";
        private readonly string _contentType;
        private readonly IList<KeyValuePair<string, object>> _headers;

        public GenericApiHttpClient(string baseUrl, IList<KeyValuePair<string, object>> headers)
        {
            _client = new HttpClient();
            _headers = headers;

            _contentType = headers.First(x => x.Key.ToLower().Equals("content-type")).Value.ToString();
            _client.DefaultRequestHeaders.Add("Accept", _contentType);
            _baseUrl = baseUrl;
        }

        public async Task<object> Get()
        {
            try
            {
                var response = await _client.GetAsync(_baseUrl);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Activator.CreateInstance<T>();
        }

        public async Task<object> Post(T item)
        {
            try
            {
                string value = item.ToString();
                if (!item.GetType().Equals(typeof(string)))
                {
                    if (_contentType.EndsWith("json"))
                    {
                        value = JsonConvert.SerializeObject(item);
                    }
                    else
                    {
                        value = XmlHelper<T>.Save(item);
                    }
                }
                using (var content = new ByteArrayContent(Encoding.UTF8.GetBytes(value)))
                {
                    content.Headers.Add("Content-Type", _contentType);
                    content.Headers.Add("Content-Length", value.Length.ToString());
                    var response = await _client.PostAsync(_baseUrl, content);
                    if (response.IsSuccessStatusCode)
                        return await response.Content.ReadAsStringAsync();

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<object> Put(T item)
        {
            try
            {
                string value = item.ToString();
                if (!item.GetType().Equals(typeof(string)))
                {
                    if (_contentType.EndsWith("json"))
                    {
                        value = JsonConvert.SerializeObject(item);
                    }
                    else
                    {
                        value = XmlHelper<T>.Save(item);
                    }
                }
                using (var content = new ByteArrayContent(Encoding.UTF8.GetBytes(value)))
                {
                    content.Headers.Add("Content-Type", _contentType);
                    content.Headers.Add("Content-Length", value.Length.ToString());
                    var response = await _client.PutAsync(_baseUrl, content);
                    if (response.IsSuccessStatusCode)
                        return await response.Content.ReadAsStringAsync();

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<object> Delete(T item = null, string propertyName = "")
        {
            try
            {
                var prop = item == null ? "": item.ToString();
                if (!string.IsNullOrEmpty(propertyName))
                    prop = item.GetType().GetRuntimeProperty(propertyName).GetValue(item).ToString();

                var response = await _client.DeleteAsync(_baseUrl + prop);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }        
    }    
}
