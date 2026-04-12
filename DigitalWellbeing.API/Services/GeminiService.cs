using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace DigitalWellbeing.API.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
    
        public GeminiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["Gemini:ApiKey"]; // 🔥 read from config
        }

        public async Task<string> GenerateInsights(string prompt)
        {
            var url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-flash-latest:generateContent";

            var requestBody = new
            {
                contents = new[]
                {
            new {
                parts = new[] {
                    new { text = prompt }
                }
            }
        }
            };

            var json = JsonConvert.SerializeObject(requestBody);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("X-goog-api-key", _apiKey);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("RAW GEMINI RESPONSE:");
            Console.WriteLine(result);

            dynamic parsed = JsonConvert.DeserializeObject(result);

            try
            {
                return parsed.candidates[0].content.parts[0].text.ToString();
            }
            catch
            {
                return "Gemini response parsing failed.";
            }
        }
    }
}