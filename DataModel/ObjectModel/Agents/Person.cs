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

using Artivity.DataModel.ObjectModel;
using Artivity.DataModel.Extensions;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Artivity.DataModel
{
    [RdfClass(PROV.Person)]
    public class Person : Agent, IUser
    {
		#region Members

        [RdfProperty(ART.guid), JsonIgnore]
        private string _guid { get; set; }

        [JsonIgnore]
        public Guid? Guid
        {
            get
            {
                if (!string.IsNullOrEmpty(_guid))
                {
                    return new Guid(_guid);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value.HasValue)
                {
                    _guid = value.Value.ToString();
                }
                else
                {
                    _guid = null;
                }
            }
        }

        [RdfProperty(DCES.identifier), JsonIgnore]
        private string _id { get; set; }

        public string Id
        {
            get { return _id; }
        }

        [RdfProperty(FOAF.mbox), NotifyPropertyChanged]
        public string EmailAddress { get; set; }

		#endregion

        #region Constructors

        public Person(Uri uri) : base(uri)
        {
            IsSynchronizable = true;
        }

        #endregion

        #region Methods

        private bool TryGetGuid(out Guid? guid)
        {
            Regex guidExpression = new Regex(@"(?i)\b[\dA-F]{8}-[\dA-F]{4}-[\dA-F]{4}-[\dA-F]{4}-[\dA-F]{12}");

            MatchCollection matches = guidExpression.Matches(Uri.AbsoluteUri);

            Guid g;

            if(matches.Count == 1 && System.Guid.TryParse(matches[0].Value, out g))
            {
                guid = g;

                return true;
            }
            else
            {
                guid = null;

                return false;
            }
        }

        public override bool Validate()
        {
            return base.Validate() && !string.IsNullOrEmpty(EmailAddress);
        }

        public override void Commit()
        {
            if(string.IsNullOrEmpty(_id) && !string.IsNullOrEmpty(EmailAddress))
            {
                _id = EmailAddress.GetHashString();
            }

            base.Commit();
        }

        #endregion
    }
}
