using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

/*
------------------------------------------------------------
Authentication Client

Purpose:
- Attempts Basic Auth against API
- Finds correct password
------------------------------------------------------------
*/

public class AuthClient
{
    private readonly HttpClient _client = new HttpClient();

    private const string Url =
        "https://recruitment.warpdevelopment.co.za/v2/api/authenticate";

    public async Task<string?> TryPasswordsAsync(string username, string[] passwords)
    {
        Console.WriteLine("\n==================================================");
        Console.WriteLine("STARTING AUTH ATTEMPTS");
        Console.WriteLine("==================================================");

        foreach (var password in passwords)
        {
            Console.WriteLine($"TRYING: {password}");

            var token = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{username}:{password}")
            );

            var request = new HttpRequestMessage(HttpMethod.Get, Url);
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", token);

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("AUTH SUCCESS ✅");
                var content = await response.Content.ReadAsStringAsync();
                return ExtractUrl(content);
            }

            await Task.Delay(120);
        }

        Console.WriteLine("AUTH FAILED ❌");
        return null;
    }

    private string ExtractUrl(string content)
    {
        try
        {
            using var doc = JsonDocument.Parse(content);

            if (doc.RootElement.TryGetProperty("url", out var url))
                return url.GetString() ?? content;
        }
        catch { }

        return content.Trim();
    }
}