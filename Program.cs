using System;
using System.IO;

/*
------------------------------------------------------------
Developer: Sandeep Hari
Project: Warp Development Password API Assessment
Language: C# (.NET 8)

Program Purpose:
This console application:

1. Generates password permutations
2. Saves them into dict.txt
3. Attempts authentication against the Warp API
4. Creates a ZIP package containing:
   - Source code
   - CV
   - Dictionary file
5. Encodes the ZIP into Base64
6. Uploads the final payload

The application also includes:
- SAFE MODE for local testing
- LIVE MODE for real submission
- Debug logging for easier troubleshooting
------------------------------------------------------------
*/

class Program
{
    static async System.Threading.Tasks.Task Main()
    {

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // SAFE MODE:
        // false = local testing only
        // true = real API authentication + upload
        bool isLive = false;

        // Finds the root folder of the project
        // This avoids issues with bin/Debug paths
        string projectRoot = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..")
        );

        string dataFolder = Path.Combine(projectRoot, "Data");
        string assetsFolder = Path.Combine(projectRoot, "Assets");

        Directory.CreateDirectory(dataFolder);

        string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appFolder = Path.Combine(appData, "PasswordAPI_SH");

        Directory.CreateDirectory(appFolder);

        string dictPath = Path.Combine(dataFolder, "dict.txt");

        // =========================================================
        // STEP 1 - PASSWORD GENERATION
        // =========================================================

        Console.WriteLine("\n==================================================");
        Console.WriteLine("STEP 1 - PASSWORD GENERATION");
        Console.WriteLine("==================================================");

        var passwords = PasswordGenerator.Generate("password");

        PasswordGenerator.SaveToFile(passwords, dictPath);

        Console.WriteLine("==================================================");
        Console.WriteLine("PASSWORD GENERATION COMPLETE ✅");
        Console.WriteLine($"TOTAL PASSWORDS: {passwords.Count}");
        Console.WriteLine("==================================================");

        string zipPath = Path.Combine(appFolder, "PasswordAPIAssessment_Submission.zip");

        // =========================================================
        // SAFE MODE
        // =========================================================

        if (!isLive)
        {
            Console.WriteLine("\n==================================================");
            Console.WriteLine("SAFE MODE ACTIVE");
            Console.WriteLine("NO API CALLS WILL BE MADE");
            Console.WriteLine("==================================================");

            Console.WriteLine("\nSTEP 2 - ZIP CREATION (LOCAL ONLY)");

            var zipBytes = ZipBuilder.CreateZip(projectRoot, appFolder);

            File.WriteAllBytes(zipPath, zipBytes);

            Console.WriteLine("\nZIP CREATED ✅");
            Console.WriteLine($"LOCATION: {zipPath}");

            return;
        }

        // =========================================================
        // LIVE MODE
        // =========================================================

        Console.WriteLine("\n==================================================");
        Console.WriteLine("LIVE MODE ACTIVE");
        Console.WriteLine("==================================================");

        Console.WriteLine("\nSTEP 2 - AUTHENTICATION STARTING");

        var authClient = new AuthClient();

        var uploadUrl = await authClient.TryPasswordsAsync(
            "John",
            passwords.ToArray()
        );

        if (uploadUrl == null)
        {
            Console.WriteLine("AUTH FAILED ❌");
            return;
        }

        Console.WriteLine("AUTH SUCCESS ✅");
        Console.WriteLine($"UPLOAD URL: {uploadUrl}");

        Console.WriteLine("\nSTEP 3 - ZIP CREATION");

        var zipBytesLive = ZipBuilder.CreateZip(projectRoot, appFolder);

        Console.WriteLine("ZIP CREATED ✅");

        Console.WriteLine("\nSTEP 4 - UPLOADING");

        var uploader = new UploadClient();

        await uploader.UploadAsync(uploadUrl, ZipBuilder.ToBase64(zipBytesLive));

        Console.WriteLine("UPLOAD COMPLETE ✅");

        Console.WriteLine("\n==================================================");
        Console.WriteLine("END LIVE RUN");
        Console.WriteLine("==================================================");
    }
}