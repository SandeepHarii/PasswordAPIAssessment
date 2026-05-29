using System;
using System.IO;
using System.IO.Compression;

/*
------------------------------------------------------------
ZIP Builder

Purpose:
- Packages:
  - Source code
  - Assets
  - Dictionary file
- Excludes build artifacts
------------------------------------------------------------
*/

public static class ZipBuilder
{
    public static byte[] CreateZip(string projectRoot, string excludeFolder)
    {
        Console.WriteLine("\n==================================================");
        Console.WriteLine("BUILDING ZIP PACKAGE");
        Console.WriteLine("==================================================");

        using var ms = new MemoryStream();

        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
        {
            AddDirectory(zip, projectRoot, excludeFolder);
        }

        Console.WriteLine("ZIP BUILD COMPLETE ✅");

        return ms.ToArray();
    }

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

    private static void AddFile(ZipArchive zip, string filePath, string entryName)
    {
        if (!File.Exists(filePath))
            return;

        Console.WriteLine($"ADDING: {entryName}");

        var entry = zip.CreateEntry(entryName, CompressionLevel.Optimal);

        using var entryStream = entry.Open();
        using var fileStream = File.OpenRead(filePath);

        fileStream.CopyTo(entryStream);
    }

    public static string ToBase64(byte[] zipBytes)
    {
        return Convert.ToBase64String(zipBytes);
    }
}