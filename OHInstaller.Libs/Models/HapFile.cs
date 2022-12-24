namespace OHInstaller.Libs.Models
{
    public sealed class HapFile
    {
        public HapFile(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileName(path);
        }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}