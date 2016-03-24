using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Api.Platforms
{
    public interface IThumbnailProvider
    {
    }

    public class ThumbnailProviderFactory
    {
        public static IThumbnailProvider CreateThumbnailProvider()
        {
            #if WIN
            return new Artivity.Api.Platforms.Win.ThumbnailProvider();
            #elif OSX
            return new Artivity.Api.Platforms.OSX.ThumbnailProvider();
            #endif
        }
    }
}
