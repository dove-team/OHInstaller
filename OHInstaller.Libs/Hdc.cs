using PCLUntils.Plantform;
using PCLUntils;
using PCLUntils.Untils;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace OHInstaller.Libs
{
    public sealed class Hdc
    {
        //https://developer.harmonyos.com/cn/docs/documentation/doc-guides-V3/ide-command-line-sdkmgr-0000001110390078-V3
        private static Hdc instance;
        public static Hdc Instance
        {
            get
            {
                instance ??= new Hdc();
                return instance;
            }
        }
        private readonly string root;
        private readonly string manager;
        private const string DEVICE_LIST = "hdc list targets";
        private const string INSTALL_HAP = "hdc app install {0}";
        private const string INSTALL_TOOL = "sdkmgr install toolchains";
        private Hdc()
        {
            root = Path.Combine(Extension.GetRoot(true), "sdk");
            manager = Path.Combine(Extension.GetRoot(true), "sdk", "manager");
            DownloadManager.Instance.ProgressComplete += DownloadManager_ProgressComplete;
        }
        private async void DownloadManager_ProgressComplete(object sender, bool hasError, Uri address, string filePath)
        {
            if (!hasError)
            {
                if (address.AbsoluteUri == UrlTools.Instance.WindowsUrl || address.AbsoluteUri == UrlTools.Instance.LinuxUrl || address.AbsoluteUri == UrlTools.Instance.MacUrl)
                {
                    await InstallHdc();
                }
                else
                {

                }
            }
        }
        public bool HasHdc
        {
            get
            {
                var file = Path.Combine(root, "hdc.exe");
                return Directory.Exists(root) && File.Exists(file);
            }
        }
        public bool HasManager
        {
            get
            {
                var file = Path.Combine(manager, "bin", "sdkmgr.bat");
                return Directory.Exists(manager) && File.Exists(file);
            }
        }
        private async Task InstallManager()
        {
            var url = string.Empty;
            switch (PlantformUntils.System)
            {
                case Platforms.Windows:
                    {
                        url = UrlTools.Instance.WindowsUrl;
                        break;
                    }
                case Platforms.Linux:
                    {
                        url = UrlTools.Instance.LinuxUrl;
                        break;
                    }
                case Platforms.MacOS:
                    {
                        url = UrlTools.Instance.MacUrl;
                        break;
                    }
            }
            await DownloadManager.Instance.Create(url).ConfigureAwait(false);
        }
        public async Task InstallHdc()
        {
            if (HasManager)
            {
                switch (PlantformUntils.System)
                {
                    case Platforms.Windows:
                        {
                            if (INSTALL_TOOL.ExecuteShell(out string result))
                            {

                            }
                            break;
                        }
                }
            }
            else
                await InstallManager();
        }
        public void InstallHap(string path)
        {
            if (string.Format(INSTALL_HAP, path).ExecuteShell(out string result))
            {

            }
        }
    }
}