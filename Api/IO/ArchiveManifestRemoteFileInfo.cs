using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Api.IO
{
    public class ArchiveManifestRemoteFileInfo
    {
        #region Members

        /// <summary>
        /// File path relative to the archive root directory (including subdirectories).
        /// </summary>
        public readonly string LocalName;

        /// <summary>
        /// URL from which the file can be downloaded.
        /// </summary>
        public readonly Uri RemoteUrl;

        #endregion

        #region Constructors

        public ArchiveManifestRemoteFileInfo(string localName, Uri remoteUrl)
        {
            LocalName = localName;
            RemoteUrl = remoteUrl;
        }

        #endregion
    }
}
