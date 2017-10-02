using System;
using System.Threading.Tasks;

namespace Site_Manager
{
    class Deployer
    {
        public static async Task DeployAll()
        {
            await (new DeployDialog(WebPageManager.Pages)).ShowAsync();
            /*try
            {
                System.Diagnostics.Debug.WriteLine("Connecting...");
                await FTPManager.ConnectAsync();
                System.Diagnostics.Debug.WriteLine("Upload 1...");
                await FTPManager.UploadAsync(await FileManager.CreateTemporaryFile("test1", "test1.txt"), "/testupload");
                System.Diagnostics.Debug.WriteLine("Upload 2...");
                await FTPManager.UploadAsync(await FileManager.CreateTemporaryFile("test2", "test2.txt"), "/testupload");
                System.Diagnostics.Debug.WriteLine("Disconnecting...");
                await FTPManager.DisconnectAsync();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            finally
            {
                if (FTPManager.Connected)
                    await FTPManager.DisconnectAsync();
            }*/
        }
    }
}