using System;
using System.IO;
using System.Linq;
using Xunit;
using ConsoleAppGit;

namespace ConsoleAppGit.Tests
{
    public class CopilotGitPlayTests
    {
        [Fact]
        public void CreateFoldersWithRandomFiles_CreatesExpectedFoldersAndFiles()
        {
            // Arrange
            string tempRoot = Path.Combine(Path.GetTempPath(), "CopilotGitPlayTests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempRoot);

            try
            {
                int expectedFolders = 4;
                int expectedFilesPerFolder = 4;
                string[] expectedExtensions = new[] { ".txt", ".json", ".xml", ".md" };

                // Act
                CopilotGitPlay.CreateFoldersWithRandomFiles(tempRoot, expectedFolders, expectedFilesPerFolder);

                // Assert - folders created
                var folders = Directory.GetDirectories(tempRoot);
                Assert.Equal(expectedFolders, folders.Length);

                foreach (var folder in folders)
                {
                    // Assert - files per folder
                    var files = Directory.GetFiles(folder);
                    Assert.Equal(expectedFilesPerFolder, files.Length);

                    foreach (var file in files)
                    {
                        // Assert - file is not empty
                        var info = new FileInfo(file);
                        Assert.True(info.Length > 0, $"File {info.FullName} should not be empty.");

                        // Assert - extension is one of expected types
                        string ext = info.Extension.ToLowerInvariant();
                        Assert.Contains(ext, expectedExtensions);
                    }
                }
            }
            finally
            {
                // Cleanup
                try
                {
                    if (Directory.Exists(tempRoot))
                        Directory.Delete(tempRoot, recursive: true);
                }
                catch
                {
                    // swallow cleanup errors to not mask test results
                }
            }
        }
    }
}