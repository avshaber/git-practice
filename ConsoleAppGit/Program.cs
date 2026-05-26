using ConsoleAppGit;
using System;
using System.IO;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

try
{
    // Use a folder named "GeneratedFiles" inside the solution repository to avoid cluttering system folders.
    // Adjust this path if you want files created elsewhere.
    string repoRoot = @"C:\Users\avsha\Source\Repos\git-practice\PLAY";
    string target = Path.Combine(repoRoot, "GeneratedFiles");

    Console.WriteLine($"Creating folders and files under: {target}");
    // Execute the method
    CopilotGitPlay.CreateFoldersWithRandomFiles(target);
    Console.WriteLine("Creation complete.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}


