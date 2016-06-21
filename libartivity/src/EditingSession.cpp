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

#include "EditingSession.h"

#include <boost/filesystem/operations.hpp>
#include "ObjectModel/SoftwareAgent.h"

using namespace std;

namespace artivity
{
    typedef vector<string>::reverse_iterator rchangeIt;
    typedef vector<string>::iterator changeIt;
    EditingSession::EditingSession()
    {
    }

    void EditingSession::initialize(string server, bool isNewDocument)
    {
		ImageRef document = this->getDocument();

        if (!isNewDocument)
        {
            _filePath = getDocumentFilePath();
        }

        _consumer = ProducerConsumerRef(new ProducerConsumer<EditingSession, ResourceRef>());
        _consumer->ConsumeHandler = &EditingSession::consume;
        _consumer->setClassObject(this);
        _consumer->start();

        _log = ActivityLogRef(new ActivityLog(server));
        _log->addAssociation(art::USER);
        _log->addAssociation(art::SOFTWARE, this->getSoftwareAgent(), this->getSoftwareAgentVersion());
        _log->setDocument(document, _filePath.c_str());

        if (_log->connect(server.c_str()))
        {
            _fileUri = document->uri;

            _initialized = true;
        }
        _currentChangeIndex = 0;
    }

    EditingSession::~EditingSession()
    {
        _log->close();
    }

    void EditingSession::eventAdd()
    {
        ResourceRef res = onEventAdd();
        handleChanges(res);
        
        _consumer->push(res);
    }

    void EditingSession::eventDelete()
    {
        ResourceRef res = onEventDelete();
        handleChanges(res);
        _consumer->push(res);
    }

    void EditingSession::handleChanges(ResourceRef res)
    {
        if (_currentChangeIndex < _changes.size())
        {
            changeIt it = _changes.begin() + _currentChangeIndex;
            _changes.erase(it);
        }
        _currentChangeIndex++;
        _changes.push_back(res->uri);
    }

    void EditingSession::eventEdit()
    {
        RevisionRef res = onEventEdit();
        handleChanges(res);
        _consumer->push(res);
    }

    void EditingSession::eventUndo()
    {
        UndoRef res = onEventUndo();
        int count = res->getCount();

        changeIt current = _changes.begin() + _currentChangeIndex;
        _currentChangeIndex -= count;
        for (changeIt it = _changes.begin()+_currentChangeIndex;  it != current; it++)
        {
            res->addRevision(*it);
        }

        _consumer->push(res);
    }

    void EditingSession::eventRedo()
    {
        RedoRef res = onEventRedo();
        int count = res->getCount();

         int beforeIndex = _currentChangeIndex;
        _currentChangeIndex += count;
        changeIt current = _changes.begin() + _currentChangeIndex;
        for (changeIt it = _changes.begin() + beforeIndex; it != current; it++)
        {
            res->addRevision(*it);
        }

        _consumer->push(res);
    }

    void EditingSession::eventSave()
    {
        ResourceRef res = onEventSave();
        _consumer->push(res);
    }

    void EditingSession::eventSaveAs()
    {
        ResourceRef res = onEventSaveAs();
        _consumer->push(res);
    }

    bool EditingSession::fileExists(const std::string& name)
    {
        return boost::filesystem::exists(name);
    }

    void EditingSession::consume(ResourceRef res)
    {
        const char* type = res->getType();

        if (strcmp(type, prov::Revision) == 0)
        {
            _log->addInfluence(boost::dynamic_pointer_cast<EntityInfluence>(res));
        }
        else if (strcmp(type, prov::Derivation) == 0)
        {
            _log->addInfluence(boost::dynamic_pointer_cast<EntityInfluence>(res));
        }
        else if (strcmp(type, prov::Generation) == 0)
        {
            _log->addInfluence(boost::dynamic_pointer_cast<Generation>(res));
        }
        else if (strcmp(type, prov::Invalidation) == 0)
        {
            _log->addInfluence(boost::dynamic_pointer_cast<Invalidation>(res));
        }
        else if (strcmp(type, art::Undo) == 0)
        {
            _log->addInfluence(boost::dynamic_pointer_cast<EntityInfluence>(res));
        }
        else if (strcmp(type, art::Redo) == 0)
        {
            _log->addInfluence(boost::dynamic_pointer_cast<EntityInfluence>(res));
        }
    }

}