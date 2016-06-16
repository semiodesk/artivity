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

#define NFO(label) "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#" label;

namespace artivity
{
    namespace nfo
    {

        static const char* Media = NFO("Media");
        static const char* Image = NFO("Image");
        static const char* RasterImage = NFO("RasterImage");
        static const char* VectorImage = NFO("VectorImage");
        
		static const char* FileDataObject = NFO("FileDataObject");
        static const char* WebDataObject = NFO("WebDataObject");

        static const char* fileLastAccessed = NFO("fileLastAccessed");
        static const char* fileLastModified = NFO("fileLastModified");
        static const char* fileCreated = NFO("fileCreated");
        static const char* fileSize = NFO("fileSize");
        static const char* fileUrl = NFO("fileUrl");
    }
}

#endif // NFO_H
