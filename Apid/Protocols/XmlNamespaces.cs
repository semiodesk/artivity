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

namespace Artivity.Apid.Protocols
{
    public static class app
    {
        public const string NS = "http://www.w3.org/2007/app";

        public static readonly XNamespace XNS = NS;

        public static readonly XName service = XNS + "service";
        public static readonly XName workspace = XNS + "workspace";
        public static readonly XName title = XNS + "title";
        public static readonly XName accept = XNS + "accept";
        public static readonly XName acceptPackaging = XNS + "acceptPackaging";
        public static readonly XName categories = XNS + "categories";
        public static readonly XName collection = XNS + "collection";
    }

    public static class atom
    {
        public const string NS = "http://www.w3.org/2005/Atom";

        public static readonly XNamespace XNS = NS;

        public static readonly XName feed = XNS + "feed";
        public static readonly XName id = XNS + "id";
        public static readonly XName title = XNS + "title";
        public static readonly XName published = XNS + "published";
        public static readonly XName updated = XNS + "updated";
        public static readonly XName entry = XNS + "entry";
        public static readonly XName category = XNS + "category";
        public static readonly XName summary = XNS + "summary";
        public static readonly XName author = XNS + "author";
        public static readonly XName name = XNS + "name";
        public static readonly XName email = XNS + "email";
        public static readonly XName link = XNS + "link";
        public static readonly XName rel = XNS + "rel";
        public static readonly XName type = XNS + "type";
        public static readonly XName href = XNS + "href";
    }

    public static class dcterms
    {
        public const string NS = "http://purl.org/dc/terms/";

        public static readonly XNamespace XNS = NS;

        public static readonly XName title = XNS + "title";
        public static readonly XName type = XNS + "type";
        public static readonly XName subject = XNS + "subject";
    }

    public static class sword
    {
        public const string NS = "http://purl.org/net/sword/";

        public static readonly XNamespace XNS = NS;

        public static readonly XName mediation = XNS + "mediation";
        public static readonly XName acceptPackaging = XNS + "acceptPackaging";
        public static readonly XName version = XNS + "version";
        public static readonly XName state = XNS + "state";
    }
}
