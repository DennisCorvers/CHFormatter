using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CHFormatter
{
    internal class NameFormatter
    {
        /// <summary>
        /// Regex to remove the trailing charter name portion.
        /// </summary>
        private Regex m_regex = new(@" \([^)]+\)$");
        private readonly string m_folder;
        private readonly FormatOptions m_options;

        public NameFormatter(string? parentFolder, FormatOptions options)
        {
            if (!Directory.Exists(parentFolder))
                throw new Exception("Directory doesn't exist.");

            m_folder = parentFolder;
            m_options = options;
        }

        public int Format(Action<string>? progress)
        {
            progress?.Invoke(string.Empty);
            progress?.Invoke($"Scanning directory: {m_folder}");

            int renameCount = 0;
            foreach (var songDir in Directory.EnumerateDirectories(m_folder))
            {
                var currentName = Path.GetFileName(songDir);
                var newName = currentName;

                // Remove artist prefix.
                if (m_options.RemoveArtist)
                {
                    int prefixIndex = currentName.IndexOf(" - ");
                    if (prefixIndex > 0)
                        newName = currentName[(prefixIndex + 3)..];
                }

                // Remove charter suffix.
                newName = m_regex.Replace(newName, "");

                if (string.CompareOrdinal(currentName, newName) != 0)
                {
                    RenameFolder(currentName, newName);
                    progress?.Invoke($"Renamed \"{currentName}\" to \"{newName}\"");
                    renameCount++;
                }
            }

            progress?.Invoke(string.Empty);
            progress?.Invoke($"Found and renamed {renameCount} folders.");
            progress?.Invoke(string.Empty);

            return renameCount;
        }

        private void RenameFolder(string oldName, string newName)
        {
            var oldPath = Path.Combine(m_folder, oldName);
            var newPath = Path.Combine(m_folder, newName);

            Directory.Move(oldPath, newPath);
        }
    }
}
