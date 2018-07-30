using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace Site_Manager
{
    class FileManager
    {

        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        private static StorageFolder temporaryFolder = ApplicationData.Current.TemporaryFolder;

        /// <summary>
        /// Returns the StorageFile object with the given name, or null if it doesn't exist
        /// </summary>
        public static async Task<StorageFile> GetStorageFile(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (!await GetExists(name))
            {
                return null;
            }
            Debug.Out($"Returning \"{name}\" from LocalFolder", "FILE MANAGER");
            return await localFolder.GetFileAsync(name);
        }

        /// <summary>
        /// Creates a file with specified content and name in TemporaryFolder (replaces existing)
        /// </summary>
        public static async Task<StorageFile> CreateTemporaryFile(string content, string name)
        {
            if (content == null || name == null)
            {
                throw new ArgumentNullException(nameof(content) + " or " + nameof(name));
            }
            StorageFile file = await temporaryFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
            Debug.Out($"Creating temporary file, \"{name}\", with {content.Length + 1} bytes of content", "FILE MANAGER");
            await FileIO.WriteTextAsync(file, content);
            return file;
        }

        /// <summary>
        /// Creates an "index.html" file with specified content in TemporaryFolder
        /// </summary>
        public static async Task<StorageFile> CreateTemporaryFile(string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            return await CreateTemporaryFile(content, "index.html");
        }

        /// <summary>
        /// Creates an empty file in LocalFolder with specified name (replaces existing)
        /// </summary>
        public static async Task<StorageFile> CreateStorageFile(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            Debug.Out($"Creating \"{name}\" in LocalState", "FILE MANAGER");
            return await localFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
        }

        /// <summary>
        /// Returns the contents of the file as a string
        /// </summary>
        public static async Task<string> GetFileContents(StorageFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            Debug.Out($"Returning contents of \"{file.Name}\"", "FILE MANAGER");
            return await FileIO.ReadTextAsync(file);
        }

        /// <summary>
        /// Writes specified content to the file
        /// </summary>
        public static async Task WriteToFile(StorageFile file, string content)
        {
            if (file == null || content == null)
            {
                throw new ArgumentNullException(nameof(file) + " or " + nameof(content));
            }
            Debug.Out($"Writing {content.Length + 1} bytes of content to \"{file.Name}\"", "FILE MANAGER");
            await FileIO.WriteTextAsync(file, content);
        }

        /// <summary>
        /// Writes specified lines to the file
        /// </summary>
        public static async Task WriteToFile(StorageFile file, IEnumerable<String> lines)
        {
            if (file == null || lines == null)
            {
                throw new ArgumentNullException(nameof(file) + " or " + nameof(lines));
            }
            Debug.Out($"Writing specified lines to \"{file.Name}\"", "FILE MANAGER");
            await FileIO.WriteLinesAsync(file, lines);
        }

        /// <summary>
        /// Returns file or folder's existence in the given folder (relative name or path)
        /// </summary>
        public static async Task<bool> GetExists(StorageFolder folder, string name)
        {
            if (folder == null || name == null)
            {
                throw new ArgumentNullException(nameof(folder) + " or " + nameof(name));
            }
            Debug.Out($"Returning if whether or not \"{name}\" exists in {folder.Name}", "FILE MANAGER");
            return await folder.TryGetItemAsync(name) != null;
        }

        /// <summary>
        /// Returns the file or folder's existence within the LocalState folder (relative name or path)
        /// </summary>
        public static async Task<bool> GetExists(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            return await GetExists(localFolder, name);
        }
    }
}