using System.Linq;

namespace OHInstaller.Libs.Models
{
    public sealed class Device
    {
        public Device(string device)
        {
            var ds = device.Split("\t");
            if (ds != null && ds.Length == 2)
            {
                Id = ds.FirstOrDefault();
                Name = $"{ds.LastOrDefault()}({Id})";
            }
        }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}