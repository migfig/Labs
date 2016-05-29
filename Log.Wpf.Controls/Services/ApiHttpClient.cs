using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Log.Common;
using Newtonsoft.Json;

namespace Log.Wpf.Controls
{
    public interface IApiService: IDisposable
    {
        Task<IEnumerable<LogEntry>> GetEntries(eEventLevel level = eEventLevel.All);
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

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
