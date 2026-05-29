using System;
using System.Collections.Generic;
using System.IO;

/*
------------------------------------------------------------
Developer: Sandeep Hari
File: PasswordGenerator.cs

Purpose:
This class is responsible for:

1. Generating all password permutations
2. Applying uppercase/lowercase variations
3. Replacing:
   - a -> @
   - s -> 5
   - o -> 0
4. Saving the generated passwords into dict.txt

The generated dictionary is later used during
authentication attempts against the API.
------------------------------------------------------------
*/

public static class PasswordGenerator
{
    // Main method used to generate all password combinations
    public static List<string> Generate(string input)
    {
        // HashSet prevents duplicate passwords
        var results = new HashSet<string>();

        Console.WriteLine("Generating permutations...");

        // Starts recursive generation process
        Backtrack(
            input,
            0,
            new char[input.Length],
            results
        );

        Console.WriteLine("Permutation generation complete ✔");

        return new List<string>(results);
    }

    /*
    ------------------------------------------------------------
    Recursive backtracking function

    This builds password combinations character by character.

    Example:
    password
    Password
    p@ssword
    P@55w0rd

    The function:
    1. Chooses a possible character
    2. Moves to next position
    3. Repeats until full password is built
    ------------------------------------------------------------
    */
    private static void Backtrack(
        string input,
        int index,
        char[] current,
        HashSet<string> results)
    {
        // Base case:
        // Full password has been built
        if (index == input.Length)
        {
            results.Add(new string(current));
            return;
        }

        // Gets all possible variations for current character
        foreach (var option in GetOptions(input[index]))
        {
            current[index] = option;

            // Recursive call for next character
            Backtrack(
                input,
                index + 1,
                current,
                results
            );
        }
    }

    /*
    ------------------------------------------------------------
    Generates possible character substitutions

    Rules:
    - lowercase allowed
    - uppercase allowed
    - a -> @
    - s -> 5
    - o -> 0
    ------------------------------------------------------------
    */
    private static HashSet<char> GetOptions(char c)
    {
        var options = new HashSet<char>();

        char lower = char.ToLower(c);

        // Adds lowercase + uppercase versions
        if (char.IsLetter(c))
        {
            options.Add(char.ToLower(c));
            options.Add(char.ToUpper(c));
        }
        else
        {
            options.Add(c);
        }

        // Adds special replacement characters
        if (lower == 'a')
            options.Add('@');

        if (lower == 's')
            options.Add('5');

        if (lower == 'o')
            options.Add('0');

        return options;
    }

    /*
    ------------------------------------------------------------
    Saves generated passwords into dict.txt

    This dictionary file is later included inside
    the final submission ZIP file.
    ------------------------------------------------------------
    */
    public static void SaveToFile(
        List<string> passwords,
        string path)
    {
        Console.WriteLine("Saving dict.txt...");

        // Converts relative path into full absolute path
        var fullPath = Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "..",
            "..",
            path
        );

        fullPath = Path.GetFullPath(fullPath);

        // Ensures directory exists before saving
        var directory = Path.GetDirectoryName(fullPath);

        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        // Writes all passwords into dict.txt
        File.WriteAllLines(fullPath, passwords);

        Console.WriteLine("dict.txt saved ✔");
        Console.WriteLine(fullPath);
    }
}