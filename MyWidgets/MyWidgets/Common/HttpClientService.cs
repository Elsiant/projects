using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyWidgets.Common
{
    public class HttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientService(IHttpClientFactory httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetAsync(string url)
        {
            var client = _httpClientFactory.CreateClient(); // 새로운 HttpClient 인스턴스 생성

            try
            {
                return await client.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
