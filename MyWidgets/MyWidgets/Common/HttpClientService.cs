using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyWidgets.Common
{
    public class HttpClientService
    {
        private readonly HttpClient _client;

        public HttpClientService(HttpClient client) 
        {
            _client = client;
        }

        public async Task<string> GetAsync(string url)
        {
            try
            {
                return await _client.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
