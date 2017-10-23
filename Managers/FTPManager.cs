using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace Site_Manager
{
    class FTPManager
    {
        public static string Server, Password, Username;
        private static bool ConfigurationLoaded = false;
        public static bool Connected = false;
        private static Chilkat.Ftp2 Client;

        /// <summary>
        /// Loads config values, such as username and password (does nothing if already called)
        /// </summary>
        public static void LoadConfiguration()
        {
            if (ConfigurationLoaded)
            {
                Debug.Out("FTP configuration already loaded!", "WARNING");
                return;
            }
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

                Client = CreateFtpClient();
                Client.UnlockComponent("mysupersecretunlockcode");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\n" + e.StackTrace);
            }
        }

        private static Chilkat.Ftp2 CreateFtpClient() => new Chilkat.Ftp2() { Port = 21, Username = Username, Password = Password, Hostname = Server };

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
            await Client.ConnectAsync();
            Connected = true;
        }

        /// <summary>
        /// Disconnects from the server
        /// </summary>
        public static async Task Disconnect()
        {
            CheckConfiguration();
            await Client.DisconnectAsync();
            Connected = false;
        }

        /// <summary>
        /// Downloads the file to LocalFolder and returns a StorageFile representation of it
        /// </summary>
        public static async Task<StorageFile> Download(string dir, string name)
        {
            CheckConfiguration();
            await Client.GetFileAsync(dir + name, ApplicationData.Current.LocalFolder.Path + "/" + name);
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
        public static async Task<bool> DeleteFile(string path)
        {
            CheckConfiguration();
            return await Client.DeleteRemoteFileAsync(path);
        }

        /// <summary>
        /// Deletes the file (path must include the filename and all parent directories)
        /// </summary>
        public static async Task<bool> DeleteFile(string path, bool needsConnect)
        {
            if (needsConnect)
            {
                await Connect();
            }
            return await DeleteFile(path);
        }

        /// <summary>
        /// Uploades the file (path must not include the filename or a trailing "/")
        /// </summary>
        public static async Task UploadFile(StorageFile file, string path)
        {
            CheckConfiguration();
            if (path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length);
            }
            string r = await GetWorkingDirectory();
            try
            {
                if (!await GetDirectoryExists(path))
                {
                    await CreateDirectory(path);
                }
                await Client.ChangeRemoteDirAsync(path);
                await Client.PutFileAsync(file.Path, path + "/" + file.Name);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            finally
            {
                await SetWorkingDirectory(r);
            }
        }

        /// <summary>
        /// Creates the directory (path must start from root and include the directory name)
        /// </summary>
        public static async Task CreateDirectory(string path)
        {
            CheckConfiguration();
            await Client.CreateRemoteDirAsync(path);
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
            await Client.RemoveRemoteDirAsync(path);
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
            string r = await GetWorkingDirectory();
            bool exists = await SetWorkingDirectory(path);
            await Client.ChangeRemoteDirAsync(r);
            return exists;
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
            return await Client.GetCurrentRemoteDirAsync();
        }

        /// <summary>
        /// Sets the current "working" directory (path must start from root and include the directory name)
        /// </summary>
        public static async Task<bool> SetWorkingDirectory(string path)
        {
            CheckConfiguration();
            return await Client.ChangeRemoteDirAsync(path);
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
            for (int i = 0; i < await Client.GetDirCountAsync() - 1; i++)
            {
                list.Add(new FTPItem() { IsDirectory = await Client.GetIsDirectoryAsync(i), Name = await Client.GetFilenameAsync(i), Size = await Client.GetSizeAsync(i) });
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