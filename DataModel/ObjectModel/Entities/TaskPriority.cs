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
    /// An item's relative importance.
    /// <see href="http://oscaf.sourceforge.net/tmo.html#tmo:Priority"/>
    /// </summary>
    [RdfClass(TMO.Priority)]
    public class TaskPriority : Resource
    {
        #region Constructors

        public TaskPriority(Uri uri) : base(uri) { }

        public TaskPriority(string uriString) : base(uriString) { }

        #endregion
    }

    /// <summary>
    /// Defines priority types used in the NEPOMUK Task Ontology.
    /// <see href="http://oscaf.sourceforge.net/tmo.html#tmo:Priority"/>
    /// </summary>
    public static class TaskPriorityTypes
    {
        /// <summary>
        /// An item has a relatively high importance.
        /// </summary>
        public static readonly TaskPriority High = new TaskPriority(TMO.TMO_Instance_Priority_High);

        /// <summary>
        /// An item is of normal importance.
        /// </summary>
        public static readonly TaskPriority Medium = new TaskPriority(TMO.TMO_Instance_Priority_Medium);

        /// <summary>
        /// An item has a relatively low importance.
        /// </summary>
        public static readonly TaskPriority Low = new TaskPriority(TMO.TMO_Instance_Priority_Low);
    }
}
