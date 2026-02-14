using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using PD.DAL.Entitites.AppEntitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PD.BL.Helpers.GeminiHelper
{
    public class GeminiHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;

        public GeminiHelper(HttpClient httpClient, IConfiguration configuration, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _configuration = configuration;
            _cache = cache;
        }

        public async Task<List<int>> CallGeminiAsync(string userQuery, IEnumerable<MenuItem> menuItems)
        {
            System.Diagnostics.Debug.WriteLine(" GEMINI ÇAĞRILIYOR");

            // Cache kontrolü
            var cacheKey = $"gemini_search_{userQuery.ToLowerInvariant().Trim()}";
            if (_cache.TryGetValue(cacheKey, out List<int> cachedResult))
            {
                System.Diagnostics.Debug.WriteLine($"CACHE HIT: {cacheKey}");
                return cachedResult;
            }

            System.Diagnostics.Debug.WriteLine($" CACHE MISS: {cacheKey}");

            try
            {

                var apiKey = ApiKey.Gemini;
                if (string.IsNullOrEmpty(apiKey))
                {
                    System.Diagnostics.Debug.WriteLine(" HATA: API Key bulunamadı!");
                    return new List<int>();
                }

                var url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-lite:generateContent";

                // Menü hazırla
                var menuSummary = menuItems.Select(x => new
                {
                    x.Id,
                    x.Name,
                    Desc = x.Description ?? "",
                    Cat = x.Category ?? "",
                    Ingredients = x.Ingeredents ?? "",
                    Price = x.Price
                }).ToList();

                var options = new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var menuJson = JsonSerializer.Serialize(menuSummary, options);

                var prompt = $@"Sen profesyonel bir garsonsun ve musteriye ürün satman gerekiyor. Müşteri türkçe alfabesini tam yazamayabilir bunu tolere ederek senden bir ricada bulunacak.
Sen müşteri yerine karar verip müşteriye bir ürün önereceksin. Özellikle içeriklere bakarak müşterinin isteklerine göre müşteriye kesinlikle en az 2 adet ürün önereceksin. Sen nam salmış ünlü bir restoranın garsonusun acemi bir garson değilsin.
Müşteri isteği: '{userQuery}'

Aşağıdaki menüden bu isteğe en uygun yemek ID'lerini seç.

SADECE bir JSON array döndür, başka hiçbir şey yazma. Örnek: [29, 31, 34]
Eğer uygun ürün bulamazsan boş array döndür: []

Menü:
{menuJson}";

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        temperature = 0.1,
                        maxOutputTokens = 200
                    }
                };

                var jsonBody = JsonSerializer.Serialize(requestBody, options);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // Header ekle
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", apiKey);

                System.Diagnostics.Debug.WriteLine($" İSTEK ATILIYOR: {DateTime.Now:HH:mm:ss.fff}");

                // API çağrısı
                var response = await _httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($" STATUS: {(int)response.StatusCode} ({response.StatusCode})");

                // Hata kontrolü
                if (!response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($" HTTP HATA: {response.StatusCode}");
                    System.Diagnostics.Debug.WriteLine($" Response: {responseString.Substring(0, Math.Min(500, responseString.Length))}");
                    return new List<int>();
                }

                System.Diagnostics.Debug.WriteLine($" Full Response: {responseString}");

                // Response parse
                using var doc = JsonDocument.Parse(responseString);

                if (!doc.RootElement.TryGetProperty("candidates", out var candidates) ||
                    candidates.GetArrayLength() == 0)
                {
                    System.Diagnostics.Debug.WriteLine(" Candidates bulunamadı");
                    return new List<int>();
                }

                var text = candidates[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                if (string.IsNullOrWhiteSpace(text))
                {
                    System.Diagnostics.Debug.WriteLine("⚠️ Text boş");
                    return new List<int>();
                }

                System.Diagnostics.Debug.WriteLine($"📝 Gemini cevabı: {text}");

                // JSON parse
                text = text.Replace("```json", "").Replace("```", "").Trim();
                var result = JsonSerializer.Deserialize<List<int>>(text) ?? new List<int>();

                // Cache'e kaydet
                if (result.Any())
                {
                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
                    System.Diagnostics.Debug.WriteLine($"✅✅✅ {result.Count} ÜRÜN BULUNDU VE CACHE'LENDİ!");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ Gemini boş array döndü");
                }

                return result;
            }
            catch (JsonException jsonEx)
            {
                System.Diagnostics.Debug.WriteLine($"❌ JSON Parse Hatası: {jsonEx.Message}");
                return new List<int>();
            }
            catch (HttpRequestException httpEx)
            {
                System.Diagnostics.Debug.WriteLine($"❌ HTTP Hatası: {httpEx.Message}");
                return new List<int>();
            }
            catch (TaskCanceledException timeoutEx)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Timeout: {timeoutEx.Message}");
                return new List<int>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Genel Hata: {ex.GetType().Name} - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"   Stack: {ex.StackTrace}");
                return new List<int>();
            }
        }
    }
}