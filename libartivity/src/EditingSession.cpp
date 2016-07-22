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

#include <boost/filesystem/operations.hpp>

#include "Resource.h"
#include "Property.h"
#include "EditingSession.h"

#include "ObjectModel/SoftwareAgent.h"

using namespace std;

namespace artivity
{
    typedef vector<std::string>::reverse_iterator rchangeIt;
    typedef vector<std::string>::iterator changeIt;

    EditingSession::EditingSession()
    {
        pathChanged = true;
        contentChanged = false;
        _endTime = false;
        _initial = true;
        _initialized = false;
    }

    void EditingSession::initialize(string server, bool newDocument)
    {
		document = getDocument();

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
        _log->setDocument(document, _filePath, newDocument);

        if (_log->connect(server + "/artivity/api/1.0"))
        {
            _fileUri = document->uri;
            _imagePath = _log->getRenderOutputPath();
            _initialized = true;

            eventStart();
        }

        _currentChangeIndex = 0;
    }

    void EditingSession::finalize()
    {
        if (_initialized)
        {
            eventEnd();

            _initialized = false;
        }
    }

    EditingSession::~EditingSession()
    {
        _consumer->stop();
        _log->close();
        _log = ActivityLogRef();
    }

    void EditingSession::eventStart()
    {
        StartRef res = onEventStart();

        if (res != NULL)
        {
            _consumer->push(res);
        }
    }

    void EditingSession::eventEnd()
    {
        EndRef res = onEventEnd();

        if (res != NULL)
        {
            _consumer->push(res);
        }
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
        UndoRef undo = onEventUndo();

        int count = undo->getCount();

        changeIt current = _changes.begin() + _currentChangeIndex;

        _currentChangeIndex -= count;

        for (changeIt it = _changes.begin()+_currentChangeIndex;  it != current; it++)
        {
            undo->addRevision(*it);
        }

        _consumer->push(undo);
    }

    void EditingSession::eventRedo()
    {
        RedoRef res = onEventRedo();
        int count = res->getCount();


        if (_changes.size() > _currentChangeIndex + count) 
        {
            int beforeIndex = _currentChangeIndex;

            _currentChangeIndex += count;

            changeIt current = _changes.begin() + _currentChangeIndex;

            for (changeIt it = _changes.begin() + beforeIndex; it != current; it++)
            {
                res->addRevision(*it);
            }

            _consumer->push(res);
        }
    }

    void EditingSession::eventSave()
    {
        SaveRef save = onEventSave();

        // TODO: Check if file paths match if not empty -> derivation.
        if (_filePath.empty())
        {
            _filePath = getDocumentFilePath();

            if (!_filePath.empty())
            {
                _log->createDataObject(_filePath);
            }
        }
		else if (!_log->hasDataObject())
		{
			// Create a data object if we edit a non-indexed file.
			_log->createDataObject(_filePath);
		}

        time_t now;
        time(&now);

        document->setModified(now);

        _consumer->push(save);
    }

    void EditingSession::eventSaveAs()
    {
        SaveAsRef saveAs = onEventSaveAs();

        // TODO: Check if file paths match if not empty -> derivation.
        if (_filePath.empty())
        {
            _filePath = getDocumentFilePath();

            if (!_filePath.empty())
            {
                _log->createDataObject(_filePath);
            }
        }
		else if (!_log->hasDataObject())
		{
			// Create a data object if we edit a non-indexed file.
			_log->createDataObject(_filePath);
		}

        time_t now;
        time(&now);

        document->setModified(now);

        _consumer->push(saveAs);
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
        InfluenceRef influence = boost::dynamic_pointer_cast<Influence>(res);

        _log->addInfluence(influence);

        if (influence->getIsSave())
        {
            _log->transmit();
        }
    }

    string EditingSession::createImageFilePath(time_t time, int count)
    {
        string resPath = "";

        if (!_imagePath.empty())
        {
            #ifdef WIN32
            const char* sep = "\\\\";
            #else
            const char* sep = "//";
            #endif
            stringstream ss;
            do
            {
                ss.str(string()); // empty string
                ss << _imagePath << sep << time << "-" << count << ".png";
                count++;
            } while (boost::filesystem::exists(ss.str()));

            resPath = ss.str();
        }

        return resPath;
    }

    CanvasRef EditingSession::createCanvas(string uri, double x, double y, double width, double height)
    {
        CanvasRef canvas = CanvasRef(new Canvas(uri.c_str()));

        ChangeRef unit = ChangeRef(new Change(canvas, art::lengthUnit, ResourceRef(new Resource(art::px))));

        RectangleRef bounds = RectangleRef(new Rectangle());
        bounds->setWidth(width);
        bounds->setHeight(height);
        bounds->setX(x);
        bounds->setY(y);

        GenerationRef generation = GenerationRef(new Generation());
        generation->setBoundaries(bounds);
        generation->addEntity(canvas);
        generation->addChange(unit);

        _consumer->push(generation);

        return canvas;
    }

    CanvasRef EditingSession::updateCanvas(string uri, double x, double y, double width, double height)
    {
        CanvasRef canvas = CanvasRef(new Canvas(uri.c_str()));

        RectangleRef bounds = RectangleRef(new Rectangle());
        bounds->setWidth(width);
        bounds->setHeight(height);
        bounds->setX(x);
        bounds->setY(y);

        RevisionRef generation = RevisionRef(new Revision());
        generation->setBoundaries(bounds);
        generation->addEntity(canvas);
        generation->addChange(canvas, art::lengthUnit, ResourceRef(new Resource(art::px)));

        _consumer->push(generation);

        return canvas;
    }

    SaveRef EditingSession::onEventSave()
    {
        time_t now;
        time(&now);

        SaveRef save = createSave();
        save->setTime(now);

        return save;
    }

    SaveAsRef EditingSession::onEventSaveAs()
    {
        time_t now;
        time(&now);

        SaveAsRef saveAs = createSaveAs();
        saveAs->setTime(now);

        return saveAs;
    }
}
