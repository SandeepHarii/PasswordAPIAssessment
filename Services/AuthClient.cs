using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

/*
------------------------------------------------------------
Developer: Sandeep Hari
File: AuthClient.cs

Purpose:
Handles authentication against Warp API using Basic Auth.

Core flow:
- Iterate through generated password list
- Encode username + password into Base64
- Send request to authentication endpoint
- Detect successful login
- Extract and return temporary upload URL
------------------------------------------------------------
*/

public class AuthClient
{
    private readonly HttpClient _client = new HttpClient();

    // Authentication endpoint provided by assessment
    private const string Url =
        "https://recruitment.warpdevelopment.co.za/v2/api/authenticate";

    /*
    Attempts each password until correct one is found.
    Returns upload URL on success, null on failure.
    */
    public async Task<string?> TryPasswordsAsync(string username, string[] passwords)
    {
        Console.WriteLine("\n[AUTH] Starting authentication attempts...");

        foreach (var password in passwords)
        {
            Console.WriteLine($"[AUTH] Trying: {password}");

            // Convert username:password -> Base64 (Basic Auth format)
            var token = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{username}:{password}")
            );

            var request = new HttpRequestMessage(HttpMethod.Get, Url);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", token);

            var response = await _client.SendAsync(request);

            // SUCCESS CONDITION
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("[AUTH] SUCCESS ✔");

                var content = await response.Content.ReadAsStringAsync();

                return ExtractUrl(content);
            }

            // Prevent hitting API rate limits (429 errors)
            await Task.Delay(120);
        }

        Console.WriteLine("[AUTH] FAILED ❌");
        return null;
    }

    /*
    Extracts upload URL from API response.

    API may return:
    - JSON with "url"
    - or plain text fallback
    */
    private string ExtractUrl(string content)
    {
        try
        {
            using var doc = JsonDocument.Parse(content);

            if (doc.RootElement.TryGetProperty("url", out var url))
                return url.GetString() ?? content;
        }
        catch
        {
            // fallback if response is not JSON
        }

        return content.Trim();
    }
}