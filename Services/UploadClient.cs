using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

/*
------------------------------------------------------------
Upload Client

Purpose:
- Sends final Base64 ZIP to API endpoint
------------------------------------------------------------
*/

public class UploadClient
{
    private readonly HttpClient _client = new HttpClient();

    public async Task UploadAsync(string url, string base64Zip)
    {
        Console.WriteLine("\n==================================================");
        Console.WriteLine("UPLOADING PACKAGE");
        Console.WriteLine("==================================================");

        var payload = new
        {
            data = base64Zip,
            name = "Sandeep",
            surname = "Hari",
            email = "Sandeep.Hari19@gmail.com"
        };

        var json = JsonSerializer.Serialize(payload);

        var response = await _client.PostAsync(
            url,
            new StringContent(json, Encoding.UTF8, "application/json")
        );

        var result = await response.Content.ReadAsStringAsync();

        Console.WriteLine("SERVER RESPONSE:");
        Console.WriteLine(result);

        if (response.IsSuccessStatusCode)
            Console.WriteLine("UPLOAD SUCCESS ✅");
        else
            Console.WriteLine("UPLOAD FAILED ❌");
    }
}