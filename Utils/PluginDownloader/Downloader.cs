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
    class Downloader
    {
        public static void DownloadPlugins(Uri host, DirectoryInfo dir)
        {
            WebClient client = new WebClient();
            string content = client.DownloadString(host);
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            foreach (var v in values)
            {
                Uri u = new Uri(v.Value);

                using (Stream stream = client.OpenRead(v.Value))
                {
                    using (ZipArchive arch = new ZipArchive(stream, ZipArchiveMode.Read))
                    {
                        foreach (var entry in arch.Entries)
                        {
                            string name = entry.FullName;
                            FileInfo target = new FileInfo(Path.Combine(dir.FullName, v.Key, name));
                            target.Directory.Create();
                            if (string.IsNullOrEmpty(entry.Name))
                                continue;
                            using (Stream data = entry.Open())
                            {
                                using (var fileStream = File.Create(target.FullName))
                                {
                                    data.CopyTo(fileStream);
                                }
                            }
                        }
                    }
                }

            }
        }
    }
}
