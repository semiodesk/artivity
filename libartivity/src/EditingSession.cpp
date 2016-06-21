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
		_log->addAssociation(art::SOFTWARE, this->getSoftwareAgent(), this->getSoftwareVersion());
		_log->setDocument(document, _filePath->c_str());

		if (_log->connect(server.c_str()))
		{
			_fileUri = stringRef(new string(document->uri));

			_initialized = true;
		}
    }

    EditingSession::~EditingSession()
    {
        time_t now;
        time(&now);

        _log->close(now);
    }

    void EditingSession::eventAdd()
    {
        ResourceRef res = onEventAdd();
        _consumer->push(res);
    }

    void EditingSession::eventDelete()
    {
        ResourceRef res = onEventDelete();
        _consumer->push(res);
    }

    void EditingSession::eventEdit()
    {
        RevisionRef res = onEventEdit();
        _consumer->push(res);
    }

    void EditingSession::eventUndo()
    {
        ResourceRef res = onEventUndo();
        _consumer->push(res);
    }

    void EditingSession::eventRedo()
    {
        ResourceRef res = onEventRedo();
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
            //_log->addInfluence(boost::dynamic_pointer_cast<ActivityInfluence>(res));
        }
        else if (strcmp(type, art::Redo) == 0)
        {
            //_log->addInfluence(boost::dynamic_pointer_cast<ActivityInfluence>(res));
        }
    }

}