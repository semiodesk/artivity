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

using System;
using System.Collections.Generic;
using Semiodesk.Trinity;

namespace Artivity.DataModel.Tasks
{
    /// <summary>
    /// The task state property allows tracking a task during its lifecycle. Initially 
    /// the state is just "created". The TaskState class was modeled so that for each 
    /// state can be set which the typical prior and posterior states are. This has 
    /// the advantage that e.g. a UI can retrieve the allowed states at runtime from 
    /// the ontology; rather can having this potentially changing knowledge hard coded. 
    /// But the prior and posterior states are only defaults; the human user is always 
    /// free to change the state.
    /// <see href="http://oscaf.sourceforge.net/tmo.html#tmo:TaskState"/>
    /// </summary>
    [RdfClass(TMO.TaskState)]
    public class TaskState : Resource
    {
        #region Constructors

        public TaskState(Uri uri) : base(uri) { }

        public TaskState(string uriString) : base(uriString) { }

        #endregion
    }

    /// <summary>
    /// Task states as defined in the NEPOMUK Task Ontology.
    /// <see href="http://oscaf.sourceforge.net/tmo.html#tmo:TaskState"/>
    /// </summary>
    public static class TaskStates
    {
        public static readonly TaskState New = new TaskState(TMO.TMO_Instance_TaskState_New);

        public static readonly TaskState Running = new TaskState(TMO.TMO_Instance_TaskState_Running);

        public static readonly TaskState Suspended =new TaskState(TMO.TMO_Instance_TaskState_Suspended);

        public static readonly TaskState Completed =new TaskState(TMO.TMO_Instance_TaskState_Completed);

        public static readonly TaskState Archived =new TaskState(TMO.TMO_Instance_TaskState_Archived);

        public static readonly TaskState Terminated =new TaskState(TMO.TMO_Instance_TaskState_Terminated);

        public static readonly TaskState Deleted =new TaskState(TMO.TMO_Instance_TaskState_Deleted);
    }
}
