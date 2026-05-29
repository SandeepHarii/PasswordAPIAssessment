using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

/*
------------------------------------------------------------
Developer: Sandeep Hari
File: UploadClient.cs

Purpose:
Handles final submission of the assessment.

This class:
1. Takes the generated ZIP file (Base64 encoded)
2. Builds JSON payload
3. Sends POST request to upload endpoint
4. Prints server response for validation
------------------------------------------------------------
*/

public class UploadClient
{
    private readonly HttpClient _client = new HttpClient();

    /*
    Sends final submission payload to Warp API upload endpoint.

    Inputs:
    - url: temporary upload URL from authentication step
    - base64Zip: encoded ZIP file containing project submission
    */
    public async Task UploadAsync(string url, string base64Zip)
    {
        Console.WriteLine("\n[UPLOAD] Preparing submission...");

        // Build payload required by API
        var payload = new
        {
            data = base64Zip,
            name = "Sandeep",
            surname = "Hari",
            email = "Sandeep.Hari19@gmail.com"
        };

        // Convert payload into JSON string
        var json = JsonSerializer.Serialize(payload);

        Console.WriteLine("[UPLOAD] Sending request...");

        // Send POST request to upload endpoint
        var response = await _client.PostAsync(
            url,
            new StringContent(json, Encoding.UTF8, "application/json")
        );

        // Read server response
        var result = await response.Content.ReadAsStringAsync();

        Console.WriteLine("\n[UPLOAD] SERVER RESPONSE:");
        Console.WriteLine(result);

        // Success / failure feedback
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("[UPLOAD] SUCCESS ✔");
        }
        else
        {
            Console.WriteLine("[UPLOAD] FAILED ❌");
        }
    }
}