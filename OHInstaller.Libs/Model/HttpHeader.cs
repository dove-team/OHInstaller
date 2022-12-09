using System.Collections.Generic;

namespace OHInstaller.Libs.Model
{
    public sealed class HttpHeader
    {
        public string Referer { get; set; }
        public List<string> Headers { get; set; }
    }
}