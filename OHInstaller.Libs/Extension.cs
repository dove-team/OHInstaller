using PCLUntils.Plantform;
using PCLUntils;
using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace OHInstaller.Libs
{
    public static class Extension
    {
        public static bool ExecuteShell(this string arguments, out string result)
        {
            result = string.Empty;
            try
            {
                using (var process = new Process())
                {
                    process.StartInfo.FileName = Path.Combine(Hdc.Instance.Root, "hdc.exe");
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    process.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError("ExecuteShell", ex);
            }
            return false;
        }
        public static string GetRoot(bool isData)
        {
            if (isData)
            {
                switch (PlantformUntils.System)
                {
                    case Platforms.Windows:
                        {
                            var assemblyLocation = Assembly.GetEntryAssembly()?.Location ?? Assembly.GetExecutingAssembly()?.Location;
                            if (!string.IsNullOrEmpty(assemblyLocation))
                                return Path.GetDirectoryName(assemblyLocation);
                            return AppDomain.CurrentDomain.BaseDirectory;
                        }
                    default:
                        {
                            var root = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "bilibili_michael");
                            if (!Directory.Exists(root))
                                Directory.CreateDirectory(root);
                            return root;
                        }
                }
            }
            else
            {
                var assemblyLocation = Assembly.GetEntryAssembly()?.Location ?? Assembly.GetExecutingAssembly()?.Location;
                if (!string.IsNullOrEmpty(assemblyLocation))
                    return Path.GetDirectoryName(assemblyLocation);
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
    }
}