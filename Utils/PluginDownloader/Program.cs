using Mono.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PluginDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri host = null;
            DirectoryInfo dir = null;
            bool help = false;


            OptionSet options = new OptionSet()
            {
                { "h|host=", "Host location.", v => host = new Uri(v) },
                { "t|target=", "Target location where to create the plugin folders.", v => dir = new DirectoryInfo(v) },
                { "?|help",  "Show this message and exit.", v => help = v != null }
            };

            try
            {
                options.Parse(args);
                if (help || host == null || dir == null)
                    return;

                Downloader.DownloadPlugins(host, dir);
            }catch(Exception e)
            {

            }
        }
    }
}
