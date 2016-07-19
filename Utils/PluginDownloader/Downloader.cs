using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Utilities;

namespace PluginDownloader
{
    class Downloader
    {
        public static void DownloadPlugins(Uri host, DirectoryInfo dir, TaskLoggingHelper log = null)
        {
            WebClient client = new WebClient();
            string content = client.DownloadString(host);
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            foreach (var v in values)
            {
                FileInfo temp = null;
                try
                {
                    temp = new FileInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()) );
                    client.DownloadFile(v.Value, temp.FullName);

                }
                catch (Exception)
                {
                    if (log != null)
                    {
                        log.LogError("Could not load from url {0}", v.Value);

                        continue;
                    }
                }

                try
                {
                    using (Stream zipStream = temp.OpenRead())
                    {
                        using (ZipArchive arch = new ZipArchive(zipStream, ZipArchiveMode.Read))
                        {
                            foreach (var entry in arch.Entries)
                            {
                                string name = entry.FullName;
                                FileInfo target = new FileInfo(Path.Combine(dir.FullName, v.Key, name));
                                try
                                {
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
                                catch (Exception)
                                {
                                    if (log != null)
                                    {
                                        log.LogError("Error while writing file {0} from archive {1}.", name, v.Value);
                                    }
                                }
                            }
                        }
                    }
                    temp.Delete();
                }
                catch (Exception)
                {
                    if (log != null)
                    {
                        log.LogError("Could not open temp zip file {0} downloaded from {1}", temp.FullName, v.Value);
                    }
                }
            }
        }
    }
}
