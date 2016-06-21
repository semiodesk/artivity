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

#ifndef _ART_FOLDER_H
#define _ART_FOLDER_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/rdfs.h"
#include "../../Ontologies/nfo.h"
#include "../Entity.h"

#include "InformationElement.h"

namespace artivity
{
    class Folder;

    typedef boost::shared_ptr<Folder> FolderRef;

    class Folder : public Resource
    {
    private:
        std::string _label;

        std::string _url;

        FolderRef _container;

    public:
        Folder() : Resource(UriGenerator::getUri())
        {
            setType(nfo::Folder);

            _url = "";
        }

        Folder(const char* uriref) : Resource(uriref)
        {
            setType(nfo::Folder);

            _url = "";
        }

        const char* getLabel()
        {
            return _label.c_str();
        }

        void setLabel(std::string label)
        {
            _label = label;

            setValue(rdfs::label, _label);
        }

        std::string getUrl()
        {
            return _url;
        }

        void setUrl(std::string url)
        {
            _url = url;

            setValue(nie::url, _url, typeid(Resource));
        }

        FolderRef getContainer()
        {
            return _container;
        }

        void setContainer(FolderRef folder)
        {
            _container = folder;

            setValue(nfo::belongsToContainer, folder);
        }
    };
}

#endif // _ART_FOLDER_H
