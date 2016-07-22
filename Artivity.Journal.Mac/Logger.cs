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
using log4net;

namespace Artivity.Journal.Mac
{
    public class Logger
    {
        public static readonly ILog Log = LogManager.GetLogger("Journal");

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
    }
}
