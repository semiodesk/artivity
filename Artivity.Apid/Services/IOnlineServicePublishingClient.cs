using Artivity.DataModel;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid
{
    public interface IOnlineServicePublishingClient
    {
        #region Methods

        void UploadArchive(Request request, Uri serviceUrl, string filePath);

        #endregion
    }
}
