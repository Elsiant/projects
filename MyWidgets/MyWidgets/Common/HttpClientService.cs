using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
            return await _client.GetStringAsync(url);
        }
    }
}
