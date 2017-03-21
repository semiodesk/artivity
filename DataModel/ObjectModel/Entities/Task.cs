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
// Copyright (c) Semiodesk GmbH 2017

using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.DataModel.Tasks
{
    [RdfClass(TMO.Task)]
    public class Task : Entity
    {
        #region Members

        public bool IsCompleted
        {
            get { return State.Equals(TaskStates.Completed); }
            set
            {
                if (value)
                {
                    State = TaskStates.Completed;
                }
                else
                {
                    State = TaskStates.Running;
                }
            }
        }

        /// <summary>
        /// Name of the task.
        /// <seealso href="http://oscaf.sourceforge.net/tmo.html#tmo:taskName"/>
        /// </summary>
        [RdfProperty(TMO.taskName)]
        public string Name { get; set; }

        /// <summary>
        /// Importance of the task.
        /// <seealso href="http://oscaf.sourceforge.net/tmo.html#tmo:importance"/>
        /// </summary>
        [RdfProperty(TMO.importance), DefaultValue(5)]
        public int Importance { get; set; }

        /// <summary>
        /// Urgency of the task.
        /// <seealso href="http://oscaf.sourceforge.net/tmo.html#tmo:urgency"/>
        /// </summary>
        [RdfProperty(TMO.urgency)]
        public int Urgency { get; set; }

        /// <summary>
        /// Current state of the task.
        /// <seealso href="http://oscaf.sourceforge.net/tmo.html#tmo:taskState"/>
        /// </summary>
        [RdfProperty(TMO.taskState)]
        public TaskState State { get; set; }

        /// <summary>
        /// Priority of the task.
        /// <seealso href="http://oscaf.sourceforge.net/tmo.html#tmo:priority"/>
        /// </summary>
        [RdfProperty(TMO.priority)]
        public TaskPriority Priority { get; set; }

        /// <summary>
        /// The time the user plans to start doing a task.
        /// <seealso href="http://oscaf.sourceforge.net/tmo.html#tmo:targetStartTime"/>
        /// </summary>
        [RdfProperty(TMO.targetStartTime)]
        public DateTime TargetStartTime { get; set; }

        /// <summary>
        /// The time the user plans to end doing a task.
        /// <seealso href="http://oscaf.sourceforge.net/tmo.html#tmo:targetEndTime"/>
        /// </summary>
        [RdfProperty(TMO.targetEndTime)]
        public DateTime TargetEndTime { get; set; }

        /// <summary>
        /// The time the task has to be done.
        /// <seealso href="http://oscaf.sourceforge.net/tmo.html#tmo:dueDate"/>
        /// </summary>
        [RdfProperty(TMO.dueDate)]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// The task this task is subordinated.
        /// <seealso href="http://oscaf.sourceforge.net/tmo.html#tmo:superTask"/>
        /// </summary>
        [RdfProperty(TMO.superTask)]
        public Task SuperTask { get; set; }

        /// <summary>
        /// Subordinated tasks.
        /// <seealso href="http://oscaf.sourceforge.net/tmo.html#tmo:subTask"/>
        /// </summary>
        [RdfProperty(TMO.subTask)]
        public List<Task> SubTasks { get; private set; }

        #endregion

        #region Constructors

        public Task(Uri uri) : base(uri) { }

        #endregion
    }
}
