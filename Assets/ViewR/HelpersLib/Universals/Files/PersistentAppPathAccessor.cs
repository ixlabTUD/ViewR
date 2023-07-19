using System.IO;
using System.Text;
using UnityEngine;

namespace ViewR.HelpersLib.Universals.Files
{
    public class PersistentAppPathAccessor
    {
        private const string FallbackFileName = "unnamedCSVFile.csv";

        public static string GetPersistentFilePath(string filenameInPersistentPath, FileTypeCheckConfig fileTypeCheckConfig, string parentingFolder)
        {
            // Cache it
            var builder = new StringBuilder(Application.persistentDataPath);
            builder.Append(Path.DirectorySeparatorChar);

            // Parenting folder
            if (!string.IsNullOrEmpty(parentingFolder))
            {
                builder.Append(parentingFolder);
                builder.Append(Path.DirectorySeparatorChar);
                // Ensure folder exists
                Directory.CreateDirectory(builder.ToString());
            }
            // File name
            builder.Append(filenameInPersistentPath);
            // File name catch
            if (filenameInPersistentPath.Length == 0)
                builder.Append(FallbackFileName);

            // Check file end
            if (fileTypeCheckConfig.checkForFileType)
            {
                // Append if needed
                if (!builder.ToString().EndsWith(fileTypeCheckConfig.fileEnding))
                    builder.Append(fileTypeCheckConfig.fileEnding);
            }

            return builder.ToString();
        }
    }

    [System.Serializable]
    public class FileTypeCheckConfig
    {
        public bool checkForFileType = true;
        public string fileEnding = ".csv";
    }
}