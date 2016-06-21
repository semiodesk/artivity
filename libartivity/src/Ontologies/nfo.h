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
// Copyright (c) Semiodesk GmbH 2016

#ifndef NFO_H
#define NFO_H

#define nfo(label) "nfo:"label;
#define NFO(label) "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#"label;

namespace artivity
{
    namespace nfo
    {
		static const char* NS_PREFIX = nfo("");
		static const char* NS_URI = NFO("");

        static const char* Media = nfo("Media");
        static const char* Image = nfo("Image");
        static const char* RasterImage = nfo("RasterImage");
        static const char* VectorImage = nfo("VectorImage");
        static const char* Folder = nfo("Folder");
		static const char* FileDataObject = nfo("FileDataObject");
        static const char* WebDataObject = nfo("WebDataObject");

        static const char* belongsToContainer = nfo("belongsToContainer");
        static const char* fileLastAccessed = nfo("fileLastAccessed");
        static const char* fileLastModified = nfo("fileLastModified");
        static const char* fileCreated = nfo("fileCreated");
        static const char* fileSize = nfo("fileSize");
        static const char* fileUrl = nfo("fileUrl");
    }
}

#endif // NFO_H
