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

#include "Image.h"

namespace artivity
{
    using namespace std;
    using namespace boost::filesystem;

    void Image::setPath(path p)
    {
        // Create the folder object.
        path folder = p.parent_path();

        if (_folder == NULL)
        {
            _folder = FolderRef(new Folder());
        }

        _folder->setLabel(folder.filename().string());
        _folder->setUrl(UriGenerator::getUrl(folder.string()));

        // Create the file data object object.
        if (_file == NULL)
        {
            _file = FileDataObjectRef(new FileDataObject());

            setDataObject(_file);
        }

        time_t now;
        time(&now);

        _file->setLabel(p.filename().string());
        _file->setContainer(_folder);
        _file->setCreated(now);
        _file->setModified(now);
    }
}

