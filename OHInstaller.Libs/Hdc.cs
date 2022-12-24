using OHInstaller.Libs.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace OHInstaller.Libs
{
    public sealed class Hdc
    {
        private static Hdc instance;
        public static Hdc Instance
        {
            get
            {
                instance ??= new Hdc();
                return instance;
            }
        }
        public string Root => root;
        private readonly string root;
        private const string DEVICE_LIST = "list targets";
        private const string INSTALL_HAP = "app install {0}";
        private const string INSTALL_REPLACE_HAP = "app install -r {0}";
        private Hdc()
        {
            root = Path.Combine(Extension.GetRoot(true), "sdk");
        }
        public bool HasHdc
        {
            get
            {
                var file = Path.Combine(root, "hdc.exe");
                return Directory.Exists(root) && File.Exists(file);
            }
        }
        public List<Device> GetDevices()
        {
            List<Device> list = new List<Device>();
            if (DEVICE_LIST.ExecuteShell(out string result))
            {
                var array = result.Replace("List of targets attached:", "").Split("\r\n");
                if (array != null && array.Length > 0)
                {
                    foreach (var device in array)
                    {
                        if (!string.IsNullOrEmpty(device))
                        {
                            var model = new Device(device);
                            list.Add(model);
                        }
                    }
                }
            }
            return list;
        }
        public bool InstallHap(string path)
        {
            if (string.Format(INSTALL_HAP, path).ExecuteShell(out string result))
                return result.Contains("success", StringComparison.CurrentCultureIgnoreCase);
            return false;
        }
        public bool ReplaceHap(string path)
        {
            if (string.Format(INSTALL_REPLACE_HAP, path).ExecuteShell(out string result))
                return result.Contains("success", StringComparison.CurrentCultureIgnoreCase);
            return false;
        }
    }
}