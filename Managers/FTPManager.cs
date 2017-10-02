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
                return;
            try
            {
                ApplicationDataCompositeValue composite = SettingsManager.GetComposite(GlobalString.COMPOSITE_KEY_FTPCONFIG);

                if (composite == null)
                    composite = new ApplicationDataCompositeValue() { [GlobalString.COMPOSITE_KEY_FTPCONFIG_USERNAME] = "", [GlobalString.COMPOSITE_KEY_FTPCONFIG_PASSWORD] = "", [GlobalString.COMPOSITE_KEY_FTPCONFIG_SERVER] = "" };

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
        /// Connects to the server
        /// </summary>
        public static async Task ConnectAsync()
        {
            if (!ConfigurationLoaded)
                LoadConfiguration();

            await Client.ConnectAsync();

            Connected = true;
        }

        /// <summary>
        /// Disconnects from the server
        /// </summary>
        public static async Task DisconnectAsync()
        {
            if (!ConfigurationLoaded)
                LoadConfiguration();

            await Client.DisconnectAsync();

            Connected = false;
        }

        /// <summary>
        /// Downloads the file to LocalFolder and returns a StorageFile representation of it
        /// </summary>
        /// <param name="dir">Relative path of file (excluding its name)</param>
        /// <param name="name">Name of file, including extension</param>
        /// <returns>StorageFile representation of the download file</returns>
        public static async Task<StorageFile> DownloadAsync(string dir, string name)
        {
            if (!ConfigurationLoaded)
                LoadConfiguration();

            await Client.GetFileAsync(dir + name, ApplicationData.Current.LocalFolder.Path + "/" + name);

            return await ApplicationData.Current.LocalFolder.GetFileAsync(name);
        }

        /// <summary>
        /// Downloads the file to LocalFolder and returns a StorageFile representation of it
        /// </summary>
        /// <param name="dir">Relative path of file (excluding its name)</param>
        /// <param name="name">Name of file, including extension</param>
        /// <param name="needsConnect">Connect before downloading, then disconnect after</param>
        /// <returns>StorageFile representation of the download file</returns>
        public static async Task<StorageFile> DownloadAsync(string dir, string name, bool needsConnect)
        {
            if (!ConfigurationLoaded)
                LoadConfiguration();

            if (needsConnect)
                await ConnectAsync();

            StorageFile r = await DownloadAsync(dir, name);

            if (needsConnect)
                await DisconnectAsync();

            return r;
        }

        public static async Task<bool> DeleteFileAsync(string path)
        {
            if (!ConfigurationLoaded)
                LoadConfiguration();

            return await Client.DeleteRemoteFileAsync(path);
        }

        public static async Task UploadAsync(StorageFile file, string to)
        {
            if (!ConfigurationLoaded)
                LoadConfiguration();

            if (to.EndsWith("/"))
                to = to.Substring(0, to.Length);
            string r = await GetWorkingDirectoryAsync();
            try
            {
                // System.Diagnostics.Debug.WriteLine("Changing remote dir to: " + to);
                await Client.ChangeRemoteDirAsync(to);
                // System.Diagnostics.Debug.WriteLine("Uploading file \"" + file.Name + "\"...");
                // System.Diagnostics.Debug.WriteLine("Successful: " + await Client.PutFileAsync(file.Path, file.Name));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            finally
            {
                // System.Diagnostics.Debug.WriteLine("Changing remote dir back to: " + r);
                await SetWorkingDirectoryAsync(r);
            }
        }

        public static async Task CreateDirectoryAsync(string name)
        {
            if (!ConfigurationLoaded)
                LoadConfiguration();

            await Client.CreateRemoteDirAsync(name);
        }

        public static async Task CreateDirectoryAsync(string name, bool needsConnect)
        {
            if (!ConfigurationLoaded)
                LoadConfiguration();
            if (needsConnect)
                await ConnectAsync();

            await CreateDirectoryAsync(name);

            if (needsConnect)
                await DisconnectAsync();
        }

        public static async Task DeleteDirectoryAsync(string name)
        {
            if (!ConfigurationLoaded)
                LoadConfiguration();

            await Client.RemoveRemoteDirAsync(name);
        }

        public static async Task DeleteDirectoryAsync(string name, bool needsConnect)
        {
            if (!ConfigurationLoaded)
                LoadConfiguration();
            if (needsConnect)
                await ConnectAsync();

            await DeleteDirectoryAsync(name);

            if (needsConnect)
                await DisconnectAsync();
        }

        public static async Task<bool> GetDirectoryExistsAsync(string dir)
        {
            if (!ConfigurationLoaded)
                LoadConfiguration();
            string r = await GetWorkingDirectoryAsync();
            bool exists = await SetWorkingDirectoryAsync(dir);
            // go back to previous working directory
            await Client.ChangeRemoteDirAsync(r);
            return exists;
        }

        public static async Task<bool> GetDirectoryExistsAsync(string dir, bool needsConnect)
        {
            if (!ConfigurationLoaded)
                LoadConfiguration();
            if (needsConnect)
                await ConnectAsync();

            bool exists = await GetDirectoryExistsAsync(dir);

            if (needsConnect)
                await DisconnectAsync();

            return exists;
        }

        public static async Task<string> GetWorkingDirectoryAsync()
        {
            if (!ConfigurationLoaded)
                LoadConfiguration();

            return await Client.GetCurrentRemoteDirAsync();
        }

        public static async Task<bool> SetWorkingDirectoryAsync(string dir)
        {
            if (!ConfigurationLoaded)
                LoadConfiguration();

            return await Client.ChangeRemoteDirAsync(dir);
        }

        public static async Task<List<FTPItem>> GetDirectoryContentsAsync(string dir)
        {
            if (!ConfigurationLoaded)
                LoadConfiguration();

            string r = await GetWorkingDirectoryAsync();
            await SetWorkingDirectoryAsync(dir);
            List<FTPItem> list = new List<FTPItem>();

            for (int i = 0; i < await Client.GetDirCountAsync() - 1; i++)
                list.Add(new FTPItem() { IsDirectory = await Client.GetIsDirectoryAsync(i), Name = await Client.GetFilenameAsync(i), Size = await Client.GetSizeAsync(i) });

            await SetWorkingDirectoryAsync(r);
            return list;
        }

        public static async Task<List<FTPItem>> GetDirectoryContentsAsync(string dir, bool needsConnect)
        {
            if (!ConfigurationLoaded)
                LoadConfiguration();
            if (needsConnect)
                await ConnectAsync();

            List<FTPItem> list = await GetDirectoryContentsAsync(dir);

            if (needsConnect)
                await DisconnectAsync();

            return list;
        }

    }
}