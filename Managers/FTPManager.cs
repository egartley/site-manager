using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Site_Manager
{
    class FTPManager
    {
        public static string Server, Password, Username;
        public static bool ConfigurationLoaded = false;
        public static bool Connected = false;
        public static SftpClient Client;

        /// <summary>
        /// Load configuartion, such as username and password (does nothing if already called)
        /// </summary>
        public static void LoadConfiguration()
        {

            if (ConfigurationLoaded)
            {
                Debug.Out("FTP configuration already loaded!", "WARNING");
                return;
            }
            Debug.Out("Loading FTP configuration", "FTP MANAGER");
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

                Client = new SftpClient(Server, 22, Username, Password);
            }
            catch (Exception e)
            {
                Debug.Out(e);
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
            try
            {
                Client.Connect();
            }
            catch (Exception e)
            {
                Debug.Out("Could not connect! (" + e.Message + ")", "FTP MANAGER");
                Debug.Out(e.StackTrace);
                Connected = false;
                return;
            }
            Debug.Out("Connected", "FTP MANAGER");
            Connected = true;
        }

        /// <summary>
        /// Disconnects from the server
        /// </summary>
        public static async Task Disconnect()
        {
            CheckConfiguration();
            Debug.Out("Disconnected", "FTP MANAGER");
            // KJDFIOSAHFUISHIGGGGGGUIFHEDUFUIEHUHUIIHUIUIUHFEHUFEFEHFEWEFHEF
            Connected = false;
        }

        /// <summary>
        /// Downloads the file to LocalFolder and returns a StorageFile representation of it
        /// </summary>
        public static async Task<StorageFile> Download(string dir, string name)
        {
            CheckConfiguration();
            FileStream stream = File.Create(ApplicationData.Current.LocalFolder.Path + "/" + name);
            // await Client.DownloadAsync(stream, dir + name);
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
            // await Client.DeleteFileAsync(path);
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
        /// Uploads a file (path must not include the filename, but must end with a "/")
        /// </summary>
        public static async Task UploadFile(StorageFile file, string path)
        {
            CheckConfiguration();
            if (path.EndsWith("/") == false)
            {
                Debug.Out("Fixed path", "FTP MANAGER");
                path += "/";
            }
            string filepath = path + file.Name;
            Debug.Out($"Starting upload of \"{file.Name}\" to \"{path}\"", "FTP MANAGER");
            try
            {
                /*if (await Client.DirectoryExistsAsync(path) == false)
                {
                    await Client.CreateDirectoryAsync(path);
                }
                else
                {
                    try
                    {
                        await Client.DeleteFileAsync(filepath);
                    }
                    catch (FtpException e)
                    {
                        Debug.Out(e);
                        Debug.Out("Could not delete \"" + filepath + "\", assuming that it wasn't there, will continue upload", "FTP MANAGER");
                    }
                }
                FileStream stream = File.OpenRead(file.Path);
                // FtpExists.NoCheck because it was already checked
                await Client.UploadAsync(stream, filepath, FtpExists.NoCheck, true);
                await stream.FlushAsync();
                stream.Dispose();
                stream.Close();*/
            }
            catch (Exception e)
            {
                Debug.Out("Upload failed!", "FTP MANAGER");
                Debug.Out(e);
            }
        }

        /// <summary>
        /// Creates the directory (path must start from root and include the directory name)
        /// </summary>
        public static async Task CreateDirectory(string path)
        {
            CheckConfiguration();
            // await Client.CreateDirectoryAsync(path);
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
            // await Client.DeleteDirectoryAsync(path);
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
            // return await Client.DirectoryExistsAsync(path);
            return false;
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
            // return await Client.GetWorkingDirectoryAsync();
            return null;
        }

        /// <summary>
        /// Sets the current "working" directory (path must start from root and include the directory name)
        /// </summary>
        public static async Task SetWorkingDirectory(string path)
        {
            CheckConfiguration();
            // await Client.SetWorkingDirectoryAsync(path);
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
            /*foreach (FtpListItem item in await Client.GetListingAsync(path))
            {
                list.Add(new FTPItem() { IsDirectory = item.Type == FtpFileSystemObjectType.Directory, Name = item.Name, Size = (int)item.Size });
            }
            await SetWorkingDirectory(r);*/
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