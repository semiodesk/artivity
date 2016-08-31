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
using log4net;

namespace Artivity.Apid
{
    public class Logger
    {
        public static readonly ILog Log = LogManager.GetLogger("HttpService");

        public static void LogInfo(string msg, params object[] p)
        {
            if (Log.IsInfoEnabled)
            {
                Log.InfoFormat(msg, p);
            }
        }

        public static void LogDebug(string msg, params object[] p)
        {
            if (Log.IsDebugEnabled)
            {
                Log.DebugFormat(msg, p);
            }
        }

        public static void LogError(string msg, params object[] p)
        {
            if (Log.IsErrorEnabled)
            {
                Log.ErrorFormat(msg, p);
            }
        }

        public static void LogError(Exception ex)
        {
            if (Log.IsErrorEnabled)
            {
                Log.ErrorFormat("{0}, {1}\n\n{2}", ex.GetType(), ex.Message, ex.StackTrace);
            }
        }

        public static void LogFatal(string msg, params object[] p)
        {
            if (Log.IsFatalEnabled)
            {
                Log.FatalFormat(msg, p);
            }
        }

        public static HttpStatusCode LogInfo(HttpStatusCode status, string msg, params object[] p)
        {
            if (Log.IsInfoEnabled)
            {
                Log.InfoFormat("{0} {1}", status, msg, p);
            }

            return status;
        }

        public static HttpStatusCode LogRequest(HttpStatusCode status, Request request)
        {
            if (Log.IsInfoEnabled)
            {
                Log.InfoFormat("{0}, {2} {1}", status, request.Path, request.Method);
            }

            return status;
        }

        public static HttpStatusCode LogRequest(HttpStatusCode status, string route, string method, string content)
        {
            if(Log.IsInfoEnabled)
            { 
                Log.InfoFormat("{0}, {2} {1}", status, route, method);

                if (!string.IsNullOrEmpty(content))
                {
                    Log.Info(content);
                }
            }

            return status;
        }

        public static HttpStatusCode LogError(HttpStatusCode status, Exception e)
        {
            if (Log.IsErrorEnabled)
            {
                Log.ErrorFormat("{0}, {1} {2}\n\n{3}", status, e.GetType(), e.Message, e.StackTrace);
            }

            return status;
        }

        public static HttpStatusCode LogError(HttpStatusCode status, string msg, params object[] p)
        {
            if (Log.IsErrorEnabled)
            {
                Log.ErrorFormat("{0}, {1}", status, string.Format(msg, p));
            }

            return status;
        }
    }
}

