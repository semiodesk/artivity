﻿// LICENSE:
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

using Artivity.DataModel.ObjectModel;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Artivity.DataModel
{
    [RdfClass(PROV.Person)]
    public class Person : Agent, IPerson
    {
		#region Members

        // Caches the parsed GUID from the URI of the resource.
        private string _guid = null;

        public string Guid
        {
            get
            {
                if (_guid == null)
                {
                    TryGetGuid(out _guid);
                }

                return _guid;
            }
        }

        [RdfProperty(FOAF.mbox)]
        public string EmailAddress { get; set; }

        [RdfProperty(FOAF.img)]
        public string Photo { get; set; }

		#endregion

        #region Constructors

        public Person(Uri uri) : base(uri)
        {
            IsSynchronizable = true;
        }

        #endregion

        #region Methods

        private bool TryGetGuid(out string guid)
        {
            Regex guidExpression = new Regex(@"(?i)\b[\dA-F]{8}-[\dA-F]{4}-[\dA-F]{4}-[\dA-F]{4}-[\dA-F]{12}");

            MatchCollection matches = guidExpression.Matches(Uri.AbsoluteUri);

            if(matches.Count == 1)
            {
                guid = matches[0].Value;

                return true;
            }
            else
            {
                guid = "";

                return false;
            }
        }

        public override bool Validate()
        {
            return base.Validate() && !string.IsNullOrEmpty(EmailAddress);
        }

        #endregion
    }
}
