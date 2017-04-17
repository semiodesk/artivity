// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using Nancy;
using Semiodesk.Trinity;
using System;

namespace Artivity.Api.Platform
{
    public interface IPlatformProvider
    {
        #region Logging

        ILogger Logger { get; }

        #endregion

        #region Deployment

        UserConfig Config { get; }

        string ConfigFile { get; }

        string AppDataFolder { get; }

        string ArtivityDataFolder { get; }

        string AvatarsFolder { get; }

        string UserFolder { get; }

        string RenderingsFolder { get; }

        string ExportFolder { get; }

        string ImportFolder { get; }

        string TempFolder { get; }

        string DatabaseFolder { get; }

        string DatabaseName { get; }

        string DeploymentDir { get; }

        string PluginDir { get; }

        string OntologyDir { get; }

        bool RequiresAuthentication { get; }

        #endregion

        #region Platform

        bool IsLinux { get; }

        bool IsMac { get; }

        bool IsWindows { get; }

        #endregion

        #region Settings

        bool CheckForNewSoftwareAgents { get; set; }

        bool AutomaticallyInstallSoftwareAgentPlugins { get; set; }

        bool DidSetupRun { get; set; }

        #endregion

        #region Methods

        void WriteConfig(UserConfig config);

        void AddFile(UriRef uri, Uri url);

        string EncodeFileName(string str);

        string GetRenderOutputPath(UriRef entityUri);

        Response GetPersonPhoto(UriRef agentUri, string owner);

        #endregion
    }
}
