using System;
using System.Threading.Tasks;

namespace Site_Manager
{
    class Deployer
    {
        public static async Task DeployAll() => await (new DeployDialog(WebPageManager.Pages)).ShowAsync();
    }
}