using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Common;
using System.Reflection;
using System.Xml.Linq;

namespace Log.Common.Services
{
    public interface IGenericApiService<T>: IDisposable where T: class 
    {
        Task<IEnumerable<T>> GetItems(string url);
        Task<bool> AddItems(IEnumerable<T> items);
        Task<bool> AddItem(T item);
        Task<bool> RemoveItem(T item, string propertyName);
        Task<T> TransformXml(XElement xml);
    }

    public interface IApiService: IDisposable
    {
        Task<IEnumerable<LogEntry>> GetEntries(eEventLevel level = eEventLevel.All);
        Task<IEnumerable<LogSummary>> GetSummaryEntries();
        Task<IEnumerable<LogSummaryByLevel>> GetSummaryEntriesByLevel();
    }

    public class ApiServiceFactory
    {
        public static IApiService CreateService()
        {
            const string baseUrl = "http://localhost:3030/api/";
            return new ApiHttpClient(baseUrl);
        }

        public static IApiService CreateService(string baseUrl)
        {
            return new ApiHttpClient(baseUrl);
        }

        public static IGenericApiService<T> CreateService<T>(bool useJson = true) where T : class
        {
            const string baseUrl = "http://localhost:3033/api/";
            return new GenericApiHttpClient<T>(baseUrl, useJson);
        }

        public static IGenericApiService<T> CreateService<T>(string baseUrl, bool useJson = true) where T: class
        {
            return new GenericApiHttpClient<T>(baseUrl, useJson);
        }
    }

    public class GenericApiHttpClient<T> : IDisposable, IGenericApiService<T> where T : class 
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:3033/api/";
        private readonly string _contentType;

        public GenericApiHttpClient(string baseUrl, bool useJson)
        {
            _client = new HttpClient();
            _contentType = useJson ? "application/json" : "application/xml";
            _client.DefaultRequestHeaders.Add("Accept", _contentType);
            _baseUrl = baseUrl;
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
                value = XmlHelper2<T>.Save(item);
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

        public Task<T> TransformXml(XElement xml)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _client.Dispose();
        }        
    }

    public class ApiHttpClient: IApiService, IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:3030/api/";

        public ApiHttpClient(string baseUrl)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _baseUrl = baseUrl;
        }

        public async Task<IEnumerable<LogEntry>> GetEntries(eEventLevel level = eEventLevel.All)
        {
            var urls = new Dictionary<eEventLevel, string>
            {
                { eEventLevel.All, "items" },
                { eEventLevel.Error, "top/100/Error" },
                { eEventLevel.Warning, "top/100/Warning" },
                { eEventLevel.Information, "top/100/Information" },
                { eEventLevel.Critical, "top/100/Critical" }
            };

            try
            {
                var response = await _client.GetAsync(_baseUrl + urls[level]);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<LogEntry>>(json);
                }
            } catch(Exception) {;}

            return new List<LogEntry>();
        }

        public async Task<IEnumerable<LogSummary>> GetSummaryEntries()
        {          
            try
            {
                var response = await _client.GetAsync(_baseUrl + "summary");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<LogSummary>>(json);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return new List<LogSummary>();
        }

        public async Task<IEnumerable<LogSummaryByLevel>> GetSummaryEntriesByLevel()
        {
            try
            {
                var response = await _client.GetAsync(_baseUrl + "summarybylevel");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<LogSummaryByLevel>>(json);
                }
            }
            catch (Exception) {; }

            return new List<LogSummaryByLevel>();
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
