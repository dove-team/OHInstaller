using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Downloader;
using OHInstaller.Libs.Model;
using DownloadProgressChangedEventArgs = Downloader.DownloadProgressChangedEventArgs;

namespace OHInstaller.Libs
{
    public sealed class DownloadManager
    {
        private static DownloadManager instance;
        public static DownloadManager Instance
        {
            get
            {
                instance ??= new DownloadManager();
                return instance;
            }
        }
        private readonly List<string> array;
        private DownloadService Service { get; set; }
        private DirectoryInfo SaveDirectory { get; set; }
        private readonly DownloadConfiguration configuration;
        public event ProgressHandler ProcessChange;
        public event ProgressCompleteHandler ProgressComplete;
        private DownloadManager()
        {
            configuration = new DownloadConfiguration()
            {
                ChunkCount = 8,
                Timeout = int.MaxValue,
                BufferBlockSize = 10240,
                ParallelDownload = true,
                MaxTryAgainOnFailover = int.MaxValue,
                MaximumBytesPerSecond = long.MaxValue,
                RequestConfiguration =
                {
                    Accept = "*/*",
                    KeepAlive = false,
                    UseDefaultCredentials = false,
                    Headers = new WebHeaderCollection(),
                    ProtocolVersion = HttpVersion.Version11,
                    CookieContainer =  new CookieContainer(),
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36 Edg/95.0.1020.44"
                }
            };
            array = new List<string>();
        }
        private void Build(HttpHeader headers)
        {
            if (Service != null)
            {
                Service.Clear();
                Service.DownloadFileCompleted -= OnDownloadFileCompleted;
                Service.DownloadProgressChanged -= DownloadProgressChanged;
            }
            if (headers != null)
            {
                configuration.RequestConfiguration.Referer = headers.Referer;
                configuration.RequestConfiguration.Headers.Clear();
                if (headers.Headers != null && headers.Headers.Count > 0)
                {
                    foreach (var header in headers.Headers)
                        configuration.RequestConfiguration.Headers.Add(header);
                }
            }
            Service = new DownloadService(configuration);
            Service.DownloadFileCompleted += OnDownloadFileCompleted;
            Service.DownloadProgressChanged += DownloadProgressChanged;
        }
        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var hasError = e.Cancelled || e.Error != null;
            string address = string.Empty, path = string.Empty;
            if (e.UserState is DownloadPackage package)
            {
                address = package.Address;
                path = package.FileName;
                array.Add(path);
            }
            if (e.Error is Exception ex)
                LogManager.Instance.LogError("OnDownloadFileCompleted", ex);
            ProgressComplete?.Invoke(sender, hasError, new Uri(address), path);
        }
        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var progressPercentage = e.ProgressPercentage.ToString("0.00");
            ProcessChange?.Invoke(progressPercentage);
        }
        public void Init(string root)
        {
            SaveDirectory = new DirectoryInfo(root);
        }
        public async Task<bool> Create(string url, HttpHeader headers)
        {
            try
            {
                Build(headers);
                await Service.DownloadFileTaskAsync(url, SaveDirectory).ConfigureAwait(false);
                return true;
            }
            catch { }
            return false;
        }
        public async Task Create(params Uri[] urls)
        {
            foreach (var url in urls)
                await Create(url.AbsoluteUri, headers: null);
        }
        public async Task Create(params string[] urls)
        {
            foreach (var url in urls)
                await Create(url, headers: null);
        }
        public bool HasClear => array.Count > 0;
        public void Clear()
        {
            Service.Clear();
            foreach (var path in array)
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }
    }
}