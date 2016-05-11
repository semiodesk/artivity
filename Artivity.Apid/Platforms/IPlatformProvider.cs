using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Apid.Platforms
{
    public interface IPlatformProvider
    {
        string AppDataFolder { get; }
        string ArtivityUserDataFolder { get; }
        string UserFolder { get; }
        string ThumbnailFolder { get; }
        string DatabaseFolder { get; }
        string DatabaseName { get; }
        bool IsLinux { get; }
        bool IsMac { get; }
        bool IsWindows { get; }
    }
}
