using System;
using System.Collections.Generic;
using System.IO;

/*
------------------------------------------------------------
Password Generator Module

Purpose:
Generates password permutations using:
- case variations
- character substitutions
------------------------------------------------------------
*/

public static class PasswordGenerator
{
    public static List<string> Generate(string input)
    {
        var results = new HashSet<string>();

        Console.WriteLine("\n==================================================");
        Console.WriteLine("GENERATING PASSWORD PERMUTATIONS");
        Console.WriteLine("==================================================");

        Backtrack(input, 0, new char[input.Length], results);

        Console.WriteLine("PERMUTATION GENERATION COMPLETE ✅");

        return new List<string>(results);
    }

    private static void Backtrack(
        string input,
        int index,
        char[] current,
        HashSet<string> results)
    {
        if (index == input.Length)
        {
            results.Add(new string(current));
            return;
        }

        foreach (var option in GetOptions(input[index]))
        {
            current[index] = option;
            Backtrack(input, index + 1, current, results);
        }
    }

    private static HashSet<char> GetOptions(char c)
    {
        var options = new HashSet<char>();

        char lower = char.ToLower(c);

        if (char.IsLetter(c))
        {
            options.Add(char.ToLower(c));
            options.Add(char.ToUpper(c));
        }
        else
        {
            options.Add(c);
        }

        if (lower == 'a') options.Add('@');
        if (lower == 's') options.Add('5');
        if (lower == 'o') options.Add('0');

        return options;
    }

    public static void SaveToFile(List<string> passwords, string path)
    {
        Console.WriteLine("\n==================================================");
        Console.WriteLine("SAVING DICTIONARY FILE");
        Console.WriteLine("==================================================");

        var fullPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", path);
        fullPath = Path.GetFullPath(fullPath);

        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        File.WriteAllLines(fullPath, passwords);

        Console.WriteLine("DICT SAVED ✅");
        Console.WriteLine($"PATH: {fullPath}");
    }
}