using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using FluentFTP;

namespace Site_Manager
{
    class FTPManager
    {
        public static string Server, Password, Username;
        private static bool ConfigurationLoaded = false;
        public static bool Connected = false;
        public static FtpClient Client;

        /// <summary>
        /// Load configuartion, such as username and password (does nothing if already called)
        /// </summary>
        public static void LoadConfiguration()
        {
            if (ConfigurationLoaded)
            {
                Debug.Out("FTP configuration already loaded!", "OKAY");
                return;
            }
            Debug.Out("Loading FTP configuration...", "FTP MANAGER");
            try
            {
                ApplicationDataCompositeValue composite = SettingsManager.GetComposite(GlobalString.COMPOSITE_KEY_FTPCONFIG);

                if (composite == null)
                {
                    composite = new ApplicationDataCompositeValue() { [GlobalString.COMPOSITE_KEY_FTPCONFIG_USERNAME] = "", [GlobalString.COMPOSITE_KEY_FTPCONFIG_PASSWORD] = "", [GlobalString.COMPOSITE_KEY_FTPCONFIG_SERVER] = "" };
                }

                Username = (string)composite[GlobalString.COMPOSITE_KEY_FTPCONFIG_USERNAME];
                Password = (string)composite[GlobalString.COMPOSITE_KEY_FTPCONFIG_PASSWORD];
                Server = (string)composite[GlobalString.COMPOSITE_KEY_FTPCONFIG_SERVER];
                ConfigurationLoaded = true;

                Client = new FtpClient(Server)
                {
                    Credentials = new System.Net.NetworkCredential(Username, Password),
                    RetryAttempts = 3,
                    TransferChunkSize = 1024 * 6
                };
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\n" + e.StackTrace);
            }
        }

        /// <summary>
        /// Ensures that the FTP configuration (username, password and server) has been loaded
        /// </summary>
        private static void CheckConfiguration()
        {
            if (!ConfigurationLoaded)
            {
                LoadConfiguration();
            }
        }

        /// <summary>
        /// Connects to the server
        /// </summary>
        public static async Task Connect()
        {
            CheckConfiguration();
            Debug.Out("Connected", "FTP MANAGER");
            await Client.ConnectAsync();
            Connected = true;
        }

        /// <summary>
        /// Disconnects from the server
        /// </summary>
        public static async Task Disconnect()
        {
            CheckConfiguration();
            Debug.Out("Disconnected", "FTP MANAGER");
            await Client.DisconnectAsync();
            Connected = false;
        }

        /// <summary>
        /// Downloads the file to LocalFolder and returns a StorageFile representation of it
        /// </summary>
        public static async Task<StorageFile> Download(string dir, string name)
        {
            CheckConfiguration();
            FileStream stream = File.Create(ApplicationData.Current.LocalFolder.Path + "/" + name);
            await Client.DownloadAsync(stream, dir + name);
            stream.Dispose();
            return await ApplicationData.Current.LocalFolder.GetFileAsync(name);
        }

        /// <summary>
        /// Downloads the file to LocalFolder and returns a StorageFile representation of it
        /// </summary>
        public static async Task<StorageFile> Download(string dir, string name, bool needsConnect)
        {
            CheckConfiguration();
            if (needsConnect)
            {
                await Connect();
            }
            StorageFile r = await Download(dir, name);
            if (needsConnect)
            {
                await Disconnect();
            }
            return r;
        }

        /// <summary>
        /// Deletes the file (path must include the filename and all parent directories)
        /// </summary>
        public static async Task DeleteFile(string path)
        {
            CheckConfiguration();
            await Client.DeleteFileAsync(path);
        }

        /// <summary>
        /// Deletes the file (path must include the filename and all parent directories)
        /// </summary>
        public static async Task DeleteFile(string path, bool needsConnect)
        {
            if (needsConnect)
            {
                await Connect();
            }
            await DeleteFile(path);
        }

        /// <summary>
        /// Uploades the file (path must not include the filename or a trailing "/")
        /// </summary>
        public static async Task UploadFile(StorageFile file, string path)
        {
            CheckConfiguration();
            if (path.Length == 1)
            {
                Debug.Out("Fixed path", "FTP MANAGER");
                path = "";
            }
            string filepath = path + "/" + file.Name;
            try
            {
                if (path.Length != 0)
                {
                    // path was not ""
                    Debug.Out("checking for dir", "FTP MANAGER");
                    bool e = await Client.DirectoryExistsAsync(path);
                    if (e == true)
                    {
                        Debug.Out("dir exists", "FTP MANAGER");
                    }
                    else
                    {
                        Debug.Out("dir DOES NOT exist, creating", "FTP MANAGER");
                        await Client.CreateDirectoryAsync(path);
                        Debug.Out("created \"" + path + "\"", "FTP MANAGER");
                    }
                }

                /*Debug.Out("checking for index.html (" + filepath + ")", "FTP MANAGER");
                bool fileExists = await Client.FileExistsAsync(filepath);
                if (fileExists)
                {
                    Debug.Out("file DOES exist, deleting", "FTP MANAGER");
                    await Client.DeleteFileAsync(filepath);
                }
                else
                {
                    Debug.Out("file does not exist, continuing", "FTP MANAGER");
                }*/

                FileStream stream = File.OpenRead(file.Path);
                Debug.Out("uploading file", "FTP MANAGER");
                bool upload = await Client.UploadAsync(stream, filepath, FtpExists.Overwrite, true);
                Debug.Out("success: " + upload, "FTP MANAGER");
                stream.Dispose();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\n" + e.StackTrace);
            }
        }

        /// <summary>
        /// Creates the directory (path must start from root and include the directory name)
        /// </summary>
        public static async Task CreateDirectory(string path)
        {
            CheckConfiguration();
            await Client.CreateDirectoryAsync(path);
        }

        /// <summary>
        /// Creates the directory (path must start from root and include the directory name)
        /// </summary>
        public static async Task CreateDirectory(string path, bool needsConnect)
        {
            CheckConfiguration();
            if (needsConnect)
            {
                await Connect();
            }
            await CreateDirectory(path);
            if (needsConnect)
            {
                await Disconnect();
            }
        }

        /// <summary>
        /// Deletes the directory, if empty (path must start from root and include the directory name)
        /// </summary>
        public static async Task DeleteDirectory(string path)
        {
            CheckConfiguration();
            await Client.DeleteDirectoryAsync(path);
        }

        /// <summary>
        /// Deletes the directory, if empty (path must start from root and include the directory name)
        /// </summary>
        public static async Task DeleteDirectory(string path, bool needsConnect)
        {
            CheckConfiguration();
            if (needsConnect)
            {
                await Connect();
            }
            await DeleteDirectory(path);
            if (needsConnect)
            {
                await Disconnect();
            }
        }

        /// <summary>
        /// Returns whether or not the directory exists (path must start from root and include the directory name)
        /// </summary>
        public static async Task<bool> GetDirectoryExists(string path)
        {
            CheckConfiguration();
            return await Client.DirectoryExistsAsync(path);
        }

        /// <summary>
        /// Returns whether or not the directory exists (path must start from root and include the directory name)
        /// </summary>
        public static async Task<bool> GetDirectoryExists(string path, bool needsConnect)
        {
            CheckConfiguration();
            if (needsConnect)
            {
                await Connect();
            }
            bool exists = await GetDirectoryExists(path);
            if (needsConnect)
            {
                await Disconnect();
            }
            return exists;
        }

        /// <summary>
        /// Returns the current "working" directory
        /// </summary>
        public static async Task<string> GetWorkingDirectory()
        {
            CheckConfiguration();
            return await Client.GetWorkingDirectoryAsync();
        }

        /// <summary>
        /// Sets the current "working" directory (path must start from root and include the directory name)
        /// </summary>
        public static async Task SetWorkingDirectory(string path)
        {
            CheckConfiguration();
            await Client.SetWorkingDirectoryAsync(path);
        }

        /// <summary>
        /// Returns the contents of the directory (path must start from root and include the directory name)
        /// </summary>
        public static async Task<List<FTPItem>> GetDirectoryContents(string path)
        {
            CheckConfiguration();
            string r = await GetWorkingDirectory();
            await SetWorkingDirectory(path);
            List<FTPItem> list = new List<FTPItem>();
            foreach (FtpListItem item in await Client.GetListingAsync(path))
            {
                list.Add(new FTPItem() { IsDirectory = item.Type == FtpFileSystemObjectType.Directory, Name = item.Name, Size = (int)item.Size });
            }
            await SetWorkingDirectory(r);
            return list;
        }

        /// <summary>
        /// Returns the contents of the directory (path must start from root and include the directory name)
        /// </summary>
        public static async Task<List<FTPItem>> GetDirectoryContents(string path, bool needsConnect)
        {
            CheckConfiguration();
            if (needsConnect)
            {
                await Connect();
            }
            List<FTPItem> list = await GetDirectoryContents(path);
            if (needsConnect)
            {
                await Disconnect();
            }
            return list;
        }

    }
}