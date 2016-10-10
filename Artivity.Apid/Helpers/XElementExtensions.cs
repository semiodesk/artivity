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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Artivity.Apid.Helpers
{
    public static class XElementExtensions
    {
        public static IEnumerable<string> GetElementValues(this XElement e, params XName[] elementNames)
        {
            foreach(XName name in elementNames)
            {
                foreach (string v in e.Elements(name).Select(x => x.Value))
                {
                    yield return v;
                }
            }
        }

        public static string GetElementValue(this XElement e, XName elementName, string defaultValue)
        {
            if (e.Elements(elementName).Any())
            {
                return e.Elements(elementName).First().Value;
            }

            return defaultValue;
        }

        public static bool GetElementValue(this XElement e, XName elementName, bool defaultValue)
        {
            if (e.Elements(elementName).Any())
            {
                return Convert.ToBoolean(e.Elements(elementName).First().Value);
            }

            return defaultValue;
        }

        public static DateTime GetElementValueUtc(this XElement e, XName elementName, DateTime defaultValue)
        {
            DateTime result = defaultValue;

            if (e.Elements(elementName).Any())
            {
                if(DateTime.TryParse(e.Elements(elementName).First().Value, out result))
                {
                    return result.ToUniversalTime();
                }
            }

            return result;
        }

        public static Version GetElementValue(this XElement e, XName elementName, Version defaultValue)
        {
            if (e.Elements(elementName).Any())
            {
                return new Version(e.Elements(elementName).First().Value);
            }

            return defaultValue;
        }

        public static string GetAttributeValue(this XElement e, string attributeName, string defaultValue)
        {
            return e.GetAttributeValue(XName.Get(attributeName), defaultValue);
        }

        public static string GetAttributeValue(this XElement e, XName attributeName, string defaultValue)
        {
            if (e.Attributes(attributeName).Any())
            {
                return e.Attribute(attributeName).Value;
            }

            return defaultValue;
        }

        public static bool GetAttributeValue(this XElement e, string attributeName, bool defaultValue)
        {
            return e.GetAttributeValue(XName.Get(attributeName), defaultValue);
        }

        public static bool GetAttributeValue(this XElement e, XName attributeName, bool defaultValue)
        {
            if (e.Attributes(attributeName).Any())
            {
                return Convert.ToBoolean(e.Attribute(attributeName).Value);
            }

            return defaultValue;
        }

        public static Uri GetAttributeValueUri(this XElement e, XName attributeName, Uri defaultValue = null)
        {
            if (e.Attributes(attributeName).Any())
            {
                string uri = e.Attribute(attributeName).Value;

                if (Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute))
                {
                    return new Uri(uri);
                }
            }

            return defaultValue;
        }
    }
}
