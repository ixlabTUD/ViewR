using System.IO;
using UnityEngine;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.HelpersLib.Extensions.JSON
{
    /// <summary>
    /// Largely inspired by
    ///     https://github.com/MichelleFuchs/StoringDataJsonUnity/blob/master/SaveJSON%20-%20FinalProject/Assets/Scripts/FileHandler.cs
    /// </summary>
    public static class FileHandler
    {
        #region Public Methods

        /// <summary>
        /// Saves an array of data.
        /// </summary>
        public static void SaveToJSON<T>(T[] objToSave, string fileNameAndPath, bool usePersistentPath = true, bool debugging = false)
        {
            if (debugging)
                Debug.Log(GetPath(fileNameAndPath, usePersistentPath).LightBlue());
            var content = JsonHelper.ToJson<T>(objToSave);
            WriteFile(GetPath(fileNameAndPath, usePersistentPath), content);
        }

        /// <summary>
        /// Saves a single object.
        /// </summary>
        public static void SaveToJSON<T>(T objToSave, string fileNameAndPath, bool usePersistentPath = true, bool debugging = false)
        {
            if (debugging)
                Debug.Log(GetPath(fileNameAndPath, usePersistentPath).LightBlue());
            var content = JsonUtility.ToJson(objToSave);
            WriteFile(GetPath(fileNameAndPath, usePersistentPath), content);
        }

        
        /// <summary>
        /// Assumes knowledge, that there is an array of objects to return. 
        /// </summary>
        public static T[] ReadArrayFromJSON<T>(string fileNameAndPath, bool usePersistentPath = true)
        {
            var content = ReadFile(GetPath(fileNameAndPath, usePersistentPath));

            if (string.IsNullOrEmpty(content) || content == "{}")
                return new T[]{};

            var res = JsonHelper.FromJson<T>(content);

            return res;
        }
        
        /// <summary>
        /// Assumes knowledge, that there is only one object to return. 
        /// </summary>
        public static T ReadObjectFromJSON<T> (string filename, bool usePersistentPath = true) {
            var content = ReadFile (GetPath (filename, usePersistentPath));

            if (string.IsNullOrEmpty (content) || content == "{}") {
                return default (T);
            }

            T res = JsonUtility.FromJson<T> (content);

            return res;

        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets the current path, usually in the persistent folder, otherwise the entire path (for now).
        /// </summary>
        private static string GetPath(string fileNameAndPath, bool usePersistentPath) =>
            usePersistentPath ? Application.persistentDataPath + "/" + fileNameAndPath : fileNameAndPath;

        private static void WriteFile(string path, string content)
        {
            var fileStream = new FileStream(path, FileMode.Create);

            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write(content);
            }
        }

        private static string ReadFile(string path)
        {
            if (!File.Exists(path)) return "";

            using (var reader = new StreamReader(path))
            {
                var content = reader.ReadToEnd();
                return content;
            }
        }

        #endregion
    }
}