using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class LLMService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/";
    private const string EmbeddingModel = "models/gemini-embedding-001";

    public LLMService(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };
    }

    public async Task<float[]> CreateEmbeddingAsync(string text)
    {
        var requestBody = new
        {
            content = new
            {
                parts = new[]
                {
                    new { text }
                }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{EmbeddingModel}:embedContent?key={_apiKey}"
        )
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var values = document.RootElement
            .GetProperty("embedding")
            .GetProperty("values");

        var embedding = new float[values.GetArrayLength()];
        int i = 0;

        foreach (var value in values.EnumerateArray())
        {
            embedding[i++] = value.GetSingle();
        }

        return embedding;
    }
}
