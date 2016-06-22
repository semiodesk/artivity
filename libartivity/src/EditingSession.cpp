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

    void EditingSession::initialize(string server, bool newDocument)
    {
		_document = getDocument();

        if (!newDocument)
        {
            _filePath = getDocumentFilePath();
        }

        _consumer = ProducerConsumerRef(new ProducerConsumer<EditingSession, ResourceRef>());
        _consumer->ConsumeHandler = &EditingSession::consume;
        _consumer->setClassObject(this);
        _consumer->start();

        _log = ActivityLogRef(new ActivityLog());
        _log->addAssociation(art::USER);
        _log->addAssociation(art::SOFTWARE, getSoftwareAgent(), getSoftwareAgentVersion());
        _log->setDocument(_document, _filePath, newDocument);

        if (_log->connect(server + "/artivity/api/1.0"))
        {
            _fileUri = _document->uri;
            _imagePath = _log->getRenderOutputPath();
            _initialized = true;
        }
        _currentChangeIndex = 0;

    }

    EditingSession::~EditingSession()
    {
        _consumer->stop();
        _log->close();
        _log = NULL;

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
        RevisionRef res = onEventSave();
        res->addProperty(prov::entity, _fileUri, typeid(Resource));
        res->setIsSave(true);

        // TODO: Check if file paths match if not empty -> derivation.
        if (_filePath.empty())
        {
            _filePath = getDocumentFilePath();

            if (!_filePath.empty())
            {
                _document->setPath(_filePath);
            }
        }

        time_t now;
        time(&now);

        _document->setModified(now);

        _consumer->push(res);
    }

    void EditingSession::eventSaveAs()
    {
        DerivationRef res = onEventSaveAs();
        res->setIsSave(true);

        time_t now;
        time(&now);

        _document->setModified(now);

        _consumer->push(res);
    }

    bool EditingSession::fileExists(const std::string& name)
    {
        return boost::filesystem::exists(name);
    }

    void EditingSession::transmitQueue()
    {
        _log->transmit();
    }

    void EditingSession::consume(ResourceRef res)
    {
        const char* type = res->getType();

        if (strcmp(type, prov::Revision) == 0)
        {
            EntityInfluenceRef influence = boost::dynamic_pointer_cast<EntityInfluence>(res);
            _log->addInfluence(influence);
            if (influence->getIsSave())
                _log->transmit();
        }
        else if (strcmp(type, prov::Derivation) == 0)
        {
            EntityInfluenceRef influence = boost::dynamic_pointer_cast<EntityInfluence>(res);
            _log->addInfluence(influence);
            if (influence->getIsSave())
                _log->transmit();
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

    string EditingSession::createImageFilePath(time_t time)
    {
        string resPath = "";
        if (!_imagePath.empty())
        {


            #ifdef WIN32
            const char* sep = "\\\\";
            #else
            const char* sep = "//";
            #endif
            int count = 0;
            stringstream ss;
            do
            {
                ss << _imagePath << sep << time << "-" << count << ".png";
                count++;
            } while (boost::filesystem::exists(ss.str()));

            resPath = ss.str();
        }
        return resPath;
    }

    RevisionRef EditingSession::onEventSave()
    {
        time_t now;
        time(&now);

        auto rev = createRevision();
        rev->setTime(now);

        return rev;
    }

    DerivationRef EditingSession::onEventSaveAs()
    {
        time_t now;
        time(&now);

        auto der = createDerivation();
        der->setTime(now);

        return der;
    }

}