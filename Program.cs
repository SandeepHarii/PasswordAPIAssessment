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
        Console.WriteLine("=== START RUN ===");

        // SAFE MODE:
        // false = local testing only
        // true = real API authentication + upload
        bool isLive = false;

        // Finds the root folder of the project
        // This avoids issues with bin/Debug paths
        string projectRoot = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..")
        );

        // Project folders
        string dataFolder = Path.Combine(projectRoot, "Data");
        string assetsFolder = Path.Combine(projectRoot, "Assets");

        // Creates Data folder if it doesn't exist
        Directory.CreateDirectory(dataFolder);

        // AppData folder used for safe local output
        // Prevents recursive zip problems
        string appData = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData
        );

        string appFolder = Path.Combine(appData, "PasswordAPI_SH");

        Directory.CreateDirectory(appFolder);

        // Final dictionary output path
        string dictPath = Path.Combine(dataFolder, "dict.txt");

        // =========================================================
        // STEP 1 - PASSWORD GENERATION
        // =========================================================

        Console.WriteLine("\n[STEP 1] Generating passwords...");

        // Generate all password permutations
        var passwords = PasswordGenerator.Generate("password");

        // Save generated passwords into dict.txt
        PasswordGenerator.SaveToFile(passwords, dictPath);

        Console.WriteLine("[STEP 1] Password generation complete ✔");
        Console.WriteLine($"Generated passwords: {passwords.Count}");

        // Local zip output location
        string zipPath = Path.Combine(appFolder, "LOCAL_TEST.zip");

        // =========================================================
        // SAFE MODE
        // =========================================================

        if (!isLive)
        {
            Console.WriteLine("\nSAFE MODE ACTIVE");
            Console.WriteLine("No real API calls will be made.");

            Console.WriteLine("\n[STEP 2] Creating zip (local only)...");

            // Create zip locally for testing
            var zipBytes = ZipBuilder.CreateZip(projectRoot, appFolder);

            // Save zip locally
            File.WriteAllBytes(zipPath, zipBytes);

            Console.WriteLine("[STEP 2] Zip created ✔");

            Console.WriteLine("\nZIP LOCATION:");
            Console.WriteLine(zipPath);

            Console.WriteLine("\nNEXT STEP:");
            Console.WriteLine("Open LOCAL_TEST.zip and verify:");
            Console.WriteLine("- CV exists");
            Console.WriteLine("- Source code exists");
            Console.WriteLine("- dict.txt exists");
            Console.WriteLine("- No duplicate files");
            Console.WriteLine("- No nested zip files");

            Console.WriteLine("\n=== END SAFE RUN ===");

            return;
        }

        // =========================================================
        // LIVE MODE
        // =========================================================

        Console.WriteLine("\nLIVE MODE ACTIVE");

        // STEP 2 - AUTHENTICATION
        Console.WriteLine("\n[STEP 2] Starting authentication...");

        var authClient = new AuthClient();

        // Attempts all generated passwords
        var uploadUrl = await authClient.TryPasswordsAsync(
            "John",
            passwords.ToArray()
        );

        // Stops if password was not found
        if (uploadUrl == null)
        {
            Console.WriteLine("AUTH FAILED ❌");
            return;
        }

        Console.WriteLine("AUTH SUCCESS ✔");

        Console.WriteLine("\nUPLOAD URL:");
        Console.WriteLine(uploadUrl);

        // STEP 3 - ZIP CREATION
        Console.WriteLine("\n[STEP 3] Creating zip...");

        var zipBytesLive = ZipBuilder.CreateZip(
            projectRoot,
            appFolder
        );

        Console.WriteLine("[STEP 3] Zip created ✔");

        // Basic size validation
        Console.WriteLine("\nZIP SIZE:");
        Console.WriteLine($"{zipBytesLive.Length} bytes");

        // STEP 4 - UPLOAD
        Console.WriteLine("\n[STEP 4] Uploading...");

        var uploader = new UploadClient();

        // Convert zip bytes into Base64 before upload
        await uploader.UploadAsync(
            uploadUrl,
            ZipBuilder.ToBase64(zipBytesLive)
        );

        Console.WriteLine("[STEP 4] Upload complete ✔");

        Console.WriteLine("\n=== END LIVE RUN ===");
    }
}