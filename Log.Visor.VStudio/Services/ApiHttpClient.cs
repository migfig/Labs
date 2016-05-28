using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Log.Common;
using Newtonsoft.Json;

namespace Log.Visor.VStudio
{
    public interface IApiService: IDisposable
    {
        Task<IEnumerable<LogEntry>> GetEntries();
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
    }

    public class ApiHttpClient: IApiService, IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:52485/api/";

        public ApiHttpClient(string baseUrl)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _baseUrl = baseUrl;
        }

        public async Task<IEnumerable<LogEntry>> GetEntries()
        {
            var response = await _client.GetAsync(_baseUrl + "top");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<LogEntry>>(json);
            }

            return new List<LogEntry>();
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
