using Interviewer.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace InterviewerHubApp.Services
{
    public class ApiHttpClient: IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:52485/api/";

        public ApiHttpClient(string baseUrl)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _baseUrl = baseUrl;
        }

        public async Task<configuration> GetConfiguration()
        {
            var response = await _client.GetAsync(_baseUrl + "configuration");
            if (response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                var ser = new DataContractJsonSerializer(typeof(configuration));
                var stream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(text));
                var config = (configuration)ser.ReadObject(stream);

                return config;
            }

            return new configuration();
        }

        public async Task<int> AddItem<T>(T item)
        {
            _client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            var className = item.GetType().FullName.Split('.').Last().ToLower();
            var ser = new DataContractJsonSerializer(item.GetType());
            var stream = new MemoryStream();
            ser.WriteObject(stream, item);
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, (int)stream.Length);
            var content = new ByteArrayContent(buffer);
            var response = await _client.PostAsync(_baseUrl + "add/" + className, content);
            return response.IsSuccessStatusCode ? 1 : 0;
        }

        public async Task<int> UpdateItem<T>(T item)
        {
            _client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            var className = item.GetType().FullName.Split('.').Last().ToLower();
            var ser = new DataContractJsonSerializer(item.GetType());
            var stream = new MemoryStream();
            ser.WriteObject(stream, item);
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, (int)stream.Length);
            var content = new ByteArrayContent(buffer);
            var response = await _client.PutAsync(_baseUrl + "update/" + className, content);
            return response.IsSuccessStatusCode ? 1 : 0;
        }

        public async Task<int> DeleteItem<T>(T item, int id)
        {
            _client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            var className = item.GetType().FullName.Split('.').Last().ToLower();
            var response = await _client.DeleteAsync(_baseUrl + "delete/" + className + "/" + id.ToString());
            return response.IsSuccessStatusCode ? 1 : 0;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
