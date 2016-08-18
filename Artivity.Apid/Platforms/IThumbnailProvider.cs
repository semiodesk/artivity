using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Apid.Platforms
{
    public interface IThumbnailProvider
    {
    }

    public class ThumbnailProviderFactory
    {
        public static IThumbnailProvider CreateThumbnailProvider()
        {
            #if WIN
            return new Artivity.Apid.Platforms.Win.ThumbnailProvider();
            #elif OSX
            return new Artivity.Apid.Platforms.OSX.ThumbnailProvider();
            #endif
        }
    }
}
