using System;
using System.IO;
using System.IO.Compression;

/*
------------------------------------------------------------
Developer: Sandeep Hari
File: ZipBuilder.cs

Purpose:
Creates the final submission ZIP file containing:

- Source code (entire project folder)
- Assets (CV, HTG, etc.)
- Generated dictionary (dict.txt)

Important rules:
- Excludes build/cache folders
- Prevents recursive ZIP inclusion
- Avoids duplicate file issues
------------------------------------------------------------
*/

public static class ZipBuilder
{
    /*
    Creates ZIP archive in memory
    */
    public static byte[] CreateZip(string projectRoot, string excludeFolder)
    {
        Console.WriteLine("\n[ZIP] Building zip...");

        using var ms = new MemoryStream();

        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
        {
            AddDirectory(zip, projectRoot, excludeFolder);
        }

        Console.WriteLine("[ZIP] Build complete ✔");

        return ms.ToArray();
    }

    /*
    Recursively adds all project files into ZIP
    */
    private static void AddDirectory(
        ZipArchive zip,
        string folder,
        string excludeFolder)
    {
        foreach (var file in Directory.GetFiles(folder, "*", SearchOption.AllDirectories))
        {
            if (IsIgnored(file, excludeFolder))
                continue;

            var entryName = Path.GetRelativePath(folder, file);

            AddFile(zip, file, entryName);
        }
    }

    /*
    Filters out unwanted files
    */
    private static bool IsIgnored(string file, string excludeFolder)
    {
        file = file.Replace("/", "\\");
        excludeFolder = excludeFolder.Replace("/", "\\");

        return file.Contains("\\bin\\") ||
               file.Contains("\\obj\\") ||
               file.Contains("\\.vs\\") ||
               file.Contains("\\.git\\") ||
               file.Contains(excludeFolder) ||
               file.EndsWith(".zip");
    }

    /*
    Adds a single file into ZIP archive
    */
    private static void AddFile(ZipArchive zip, string filePath, string entryName)
    {
        if (!File.Exists(filePath))
            return;

        Console.WriteLine($"[ZIP] Adding: {entryName}");

        var entry = zip.CreateEntry(entryName, CompressionLevel.Optimal);

        using var entryStream = entry.Open();
        using var fileStream = File.OpenRead(filePath);

        fileStream.CopyTo(entryStream);
    }

    /*
    Converts ZIP byte array into Base64 string
    */
    public static string ToBase64(byte[] zipBytes)
    {
        return Convert.ToBase64String(zipBytes);
    }
}