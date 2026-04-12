using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace DigitalWellbeing.API.Services
{
    public class NvidiaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public NvidiaService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["Nvidia:ApiKey"];
        }

        public async Task<string> GenerateInsights(string prompt)
        {
            var url = "https://integrate.api.nvidia.com/v1/chat/completions";

            var requestBody = new
            {
                model = "google/gemma-4-31b-it",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                max_tokens = 512,
                temperature = 0.7,
                top_p = 0.9,
                stream = false
            };

            var json = JsonConvert.SerializeObject(requestBody);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("RAW NVIDIA RESPONSE:");
            Console.WriteLine(result);

            dynamic parsed = JsonConvert.DeserializeObject(result);

            try
            {
                return parsed.choices[0].message.content.ToString();
            }
            catch
            {
                return "NVIDIA response parsing failed: " + result;
            }
        }
    }
}