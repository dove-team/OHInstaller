using PCLUntils.Plantform;
using PCLUntils;
using System;
using System.IO;
using System.Reflection;

namespace OHInstaller.Libs
{
    public static class Extension
    {
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