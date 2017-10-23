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
            Debug.Out($"Returning the StorageFile \"{name}\" from LocalFolder (\"{localFolder.Path}\")", "FILE MANAGER");
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
            Debug.Out($"Creating and returning temporary file \"index.html\" with content of size {content.Length}", "FILE MANAGER");
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
            Debug.Out($"Creating and returnign storage file \"{name}\"", "FILE MANAGER");
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
            Debug.Out($"Returning contents of file \"{file.Name}\"", "FILE MANAGER");
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
            Debug.Out($"Writing content with size of {content.Length} to \"{file.Name}\"", "FILE MANAGER");
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
            Debug.Out($"Writing content (as lines) to \"{file.Name}\"", "FILE MANAGER");
            await FileIO.WriteLinesAsync(file, lines);
        }

        /// <summary>
        /// Returns file or folder's existence in the StorageFolder (relative name or path)
        /// </summary>
        public static async Task<bool> GetExists(StorageFolder folder, string name)
        {
            if (folder == null || name == null)
            {
                throw new ArgumentNullException(nameof(folder) + " or " + nameof(name));
            }
            Debug.Out($"Returning if {name} exists or not in {folder.Name} (\"{folder.Path}\")", "FILE MANAGER");
            return await folder.TryGetItemAsync(name) != null;
        }

        /// <summary>
        /// Returns the file or folder's existence within LocalFolder (relative name or path)
        /// </summary>
        public static async Task<bool> GetExists(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            Debug.Out($"Returning if {name} exists or not in LocalFolder (\"{localFolder.Path}\")", "FILE MANAGER");
            return await GetExists(localFolder, name);
        }
    }
}