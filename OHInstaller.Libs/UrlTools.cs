using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OHInstaller.Libs
{
    public class UrlTools
    {
        private readonly HttpClient httpClient;
        private UrlTools()
        {
            httpClient = new HttpClient();
        }
        private static UrlTools instance;
        public static UrlTools Instance
        {
            get
            {
                instance ??= new UrlTools();
                return instance;
            }
        }
        private string windowsUrl;
        public string WindowsUrl => windowsUrl;
        private string macUrl;
        public string MacUrl => macUrl;
        private string linuxUrl;
        public string LinuxUrl => linuxUrl;
        public async Task<bool> GetToolsUrl()
        {
            try
            {
                using var stream = await httpClient.GetStreamAsync("https://developer.harmonyos.com/cn/develop/deveco-studio#download");
                var doc = new HtmlDocument();
                doc.Load(stream);
                var root = doc.GetElementbyId("harmonyosDevelopers-page");
                foreach (var node in root.SelectSingleNode("//div[@id='download_cli']").SelectNodes("//a[@data='1']"))
                {
                    var script = node.Attributes["onclick"].Value;
                    if (script.Contains("command line tools", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var url = script.Substring(script.IndexOf("ListA1Download") + 15).Split(',').FirstOrDefault().Replace("'", "");
                        if (script.Contains("windows", StringComparison.CurrentCultureIgnoreCase))
                            windowsUrl = url;
                        else if (script.Contains("mac", StringComparison.CurrentCultureIgnoreCase))
                            macUrl = url;
                        else if (script.Contains("linux", StringComparison.CurrentCultureIgnoreCase))
                            linuxUrl = url;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError("GetToolsUrl", ex);
            }
            return false;
        }
    }
}