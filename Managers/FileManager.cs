using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace Site_Manager
{
    class FileManager
    {

        /// <summary>
        /// Creates a file with specified content and name in TemporaryFolder (replaces existing)
        /// </summary>
        public static async Task<StorageFile> CreateTemporaryFile(string content, string name)
        {
            StorageFile file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, content);
            return file;
        }

        /// <summary>
        /// Creates an "index.html" file with specified content in TemporaryFolder
        /// </summary>
        public static async Task<StorageFile> CreateTemporaryFile(string content)
        {
            return await CreateTemporaryFile(content, "index.html");
        }

        /// <summary>
        /// Creates an empty file in LocalFolder with specified name (replaces existing)
        /// </summary>
        public static async Task<StorageFile> CreateStorageFile(string name)
        {
            return await ApplicationData.Current.LocalFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
        }

        public static async Task<StorageFile> GetStorageFile(string name)
        {
            if (!await GetExists(name))
                return null;
            return await ApplicationData.Current.LocalFolder.GetFileAsync(name);
        }

        /// <summary>
        /// Returns the contents of the file as a string
        /// </summary>
        public static async Task<string> GetFileContents(StorageFile file)
        {
            return await FileIO.ReadTextAsync(file);
        }

        /// <summary>
        /// Writes specified content to the file
        /// </summary>
        public static async Task WriteToFile(StorageFile file, string content)
        {
            await FileIO.WriteTextAsync(file, content);
        }

        /// <summary>
        /// Writes specified lines to the file
        /// </summary>
        public static async Task WriteToFile(StorageFile file, IEnumerable<String> content)
        {
            await FileIO.WriteLinesAsync(file, content);
        }

        /// <summary>
        /// Returns file or folder's existence in the StorageFolder (relative name or path)
        /// </summary>
        public static async Task<bool> GetExists(StorageFolder folder, string name)
        {
            return await folder.TryGetItemAsync(name) != null;
        }

        /// <summary>
        /// Returns the file or folder's existence within LocalFolder (relative name or path)
        /// </summary>
        public static async Task<bool> GetExists(string name)
        {
            return await GetExists(ApplicationData.Current.LocalFolder, name);
        }
    }
}