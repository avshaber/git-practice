using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleAppGit
{
    internal class CopilotGitPlay
    {
        /// <summary>
        /// Create folders and files under the provided root path.
        /// Creates 4 folders by default and in each folder creates 4 files of different types
        /// filled with random text.
        /// </summary>
        /// <param name="rootPath">The directory under which folders/files will be created. The directory is created if missing.</param>
        /// <param name="folderCount">Number of folders to create (default 4).</param>
        /// <param name="filesPerFolder">Number of files created inside each folder (default 4).</param>
        public static void CreateFoldersWithRandomFiles(string rootPath, int folderCount = 4, int filesPerFolder = 4)
        {
            if (string.IsNullOrWhiteSpace(rootPath))
                throw new ArgumentException("rootPath must be provided", nameof(rootPath));

            // Ensure root exists
            Directory.CreateDirectory(rootPath);

            // Example file extensions (4 different types)
            string[] extensions = new[] { ".txt", ".json", ".xml", ".md" };

            for (int i = 0; i < folderCount; i++)
            {
                // Create a folder with a predictable name; include index for clarity
                string folderName = Path.Combine(rootPath, $"Folder_{i + 1}");
                Directory.CreateDirectory(folderName);

                for (int j = 0; j < filesPerFolder; j++)
                {
                    string ext = extensions[j % extensions.Length];
                    string fileName = Path.Combine(folderName, $"file_{j + 1}{ext}");

                    string content = GenerateContentForExtension(ext, GetRandomNumber(80, 300));
                    File.WriteAllText(fileName, content, Encoding.UTF8);
                }
            }
        }

        private static string GenerateContentForExtension(string ext, int approxLength)
        {
            switch (ext.ToLowerInvariant())
            {
                case ".json":
                    // Simple JSON object with random strings
                    return $"{{\n  \"id\": \"{Guid.NewGuid()}\",\n  \"text\": \"{EscapeForJson(GenerateRandomString(approxLength))}\"\n}}";
                case ".xml":
                    // Simple XML payload - use local XML escaping helper
                    return $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<root>\n  <id>{Guid.NewGuid()}</id>\n  <text>{EscapeForXml(GenerateRandomString(approxLength))}</text>\n</root>";
                case ".md":
                    // Markdown sample
                    return $"# Sample Markdown\n\n{GenerateRandomParagraphs(3, approxLength)}\n\n- Generated: {DateTime.UtcNow:o}";
                default:
                    // Plain text for .txt and any other extension
                    return GenerateRandomParagraphs(2, approxLength);
            }
        }

        private static string EscapeForJson(string s)
        {
            // Minimal escaping for JSON string values
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "");
        }

        private static string EscapeForXml(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            // Replace ampersand first to avoid double-escaping other entities
            return s
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }

        private static string GenerateRandomParagraphs(int paragraphCount, int approxLengthPerParagraph)
        {
            var sb = new StringBuilder();
            for (int p = 0; p < paragraphCount; p++)
            {
                if (p > 0) sb.AppendLine().AppendLine();
                sb.Append(GenerateRandomString(approxLengthPerParagraph));
            }
            return sb.ToString();
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 .,;:!?'\"()-\n";
            var result = new char[length];
            byte[] buffer = new byte[length];

            // Use cryptographically strong RNG for varied content
            RandomNumberGenerator.Fill(buffer);

            for (int i = 0; i < length; i++)
            {
                // map random byte to range of chars
                result[i] = chars[buffer[i] % chars.Length];
            }

            return new string(result);
        }

        private static int GetRandomNumber(int minInclusive, int maxInclusive)
        {
            if (minInclusive > maxInclusive) (minInclusive, maxInclusive) = (maxInclusive, minInclusive);
            int range = maxInclusive - minInclusive + 1;
            byte[] fourBytes = new byte[4];
            RandomNumberGenerator.Fill(fourBytes);
            uint scale = BitConverter.ToUInt32(fourBytes, 0);
            return (int)(minInclusive + (scale % (uint)range));
        }
    }
}
