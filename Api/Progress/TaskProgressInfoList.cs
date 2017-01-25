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
using System.Collections.Generic;

namespace Artivity.Api.Helpers
{
    public class TaskProgressInfoList
    {
        #region Members

        private readonly List<TaskProgressInfo> _tasks = new List<TaskProgressInfo>();

        public IList<TaskProgressInfo> Tasks
        {
            get { return _tasks; }
        }

        public TaskProgressInfo CurrentTask { get; set; }

        public float PercentComplete
        {
            get
            {
                float result = 0;

                foreach (TaskProgressInfo progress in _tasks)
                {
                    result += progress.PercentComplete;
                }

                return Math.Min(100, (result / _tasks.Count));
            }
        }

        #endregion

        #region Methods

        public void Reset()
        {
            foreach(TaskProgressInfo task in _tasks)
            {
                task.Reset();
            }
        }

        #endregion
    }
}
