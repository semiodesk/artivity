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

using System;
using Nancy;

namespace Artivity.Apid
{
    public interface ILogger
    {
        void LogInfo(string msg, params object[] p);

        void LogDebug(string msg, params object[] p);

        void LogError(string msg, params object[] p);

        void LogError(Exception ex);

        void LogFatal(string msg, params object[] p);

        HttpStatusCode LogInfo(HttpStatusCode status, string msg, params object[] p);

        HttpStatusCode LogRequest(HttpStatusCode status, Request request);

        HttpStatusCode LogRequest(HttpStatusCode status, string route, string method, string content);

        HttpStatusCode LogError(HttpStatusCode status, Exception e);

        HttpStatusCode LogError(HttpStatusCode status, string msg, params object[] p);
    }
}

