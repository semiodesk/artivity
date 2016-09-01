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

        consumer = ProducerConsumerRef(new ProducerConsumer<EditingSession, ResourceRef>());
        consumer->ConsumeHandler = &EditingSession::consume;
        consumer->setClassObject(this);
        consumer->start();

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
        consumer->stop();
        _log->close();
        _log = ActivityLogRef();
    }

    void EditingSession::eventStart()
    {
        StartRef res = onEventStart();

        if (res != NULL)
        {
            consumer->push(res);
        }
    }

    void EditingSession::eventEnd()
    {
        EndRef res = onEventEnd();

        if (res != NULL)
        {
            consumer->push(res);
        }
    }

    void EditingSession::eventAdd()
    {
        ResourceRef res = onEventAdd();
        handleChanges(res);
        
        consumer->push(res);
    }

    void EditingSession::eventDelete()
    {
        ResourceRef res = onEventDelete();
        handleChanges(res);

        consumer->push(res);
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

        consumer->push(res);
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

        consumer->push(undo);
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

            consumer->push(res);
        }
    }

    void EditingSession::eventSave()
    {
        string filePath = getDocumentFilePath();

        if (_filePath.empty())
        {
            if (!filePath.empty())
            {
                // A new document was saved.
                _log->createDataObject(filePath);
                
                _filePath = filePath;
            }
            else
            {
                // We got a save without a new file name?
                // Something's definitly wrong on this planet..

                return;
            }
        }
        else if (_filePath.compare(filePath) == 0)
        {
            if (!_log->hasDataObject())
            {
                // Create a data object if we edit a non-indexed file.
                _log->createDataObject(filePath);
            }
        }
        else
        {
            // We're saving an existing file under a new name.
            eventSaveAs(filePath);
            
            return;
        }

        SaveRef save = onEventSave();

        consumer->push(save);
    }

    void EditingSession::eventSaveAs(string targetPath)
    {
        if (!_filePath.empty() && !targetPath.empty() && _filePath.compare(targetPath) != 0)
        {
            consumer->stop();
            
            SaveAsRef saveAs = onEventSaveAs();

            ImageRef targetImage = ImageRef(new Image());
            targetImage->setType(document->getType());

            // Replace the current activity with a new one.
            _log->createDerivation(saveAs, targetImage, targetPath);

            // Update the new document handle.
            document = targetImage;
            
            // Udpate the new file path.
            _filePath = targetPath;
            
            consumer->start();
		}
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

    CanvasRef EditingSession::createCanvas(string uri, time_t time, double x, double y, double width, double height)
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
        generation->setTime(time);
        generation->addChange(unit);

        consumer->push(generation);

        return canvas;
    }

    CanvasRef EditingSession::updateCanvas(string uri, time_t time, double x, double y, double width, double height)
    {
        CanvasRef canvas = CanvasRef(new Canvas(uri.c_str()));

        RectangleRef bounds = RectangleRef(new Rectangle());
        bounds->setWidth(width);
        bounds->setHeight(height);
        bounds->setX(x);
        bounds->setY(y);

        
        RevisionRef revision = RevisionRef(new Revision());
        revision->setBoundaries(bounds);
        revision->setTime(time);
        revision->addEntity(canvas);
        revision->addChange(canvas, art::lengthUnit, ResourceRef(new Resource(art::px)));

        consumer->push(revision);

        return canvas;
    }

    SaveRef EditingSession::onEventSave()
    {
        time_t now;
        time(&now);

        SaveRef save = createSave();
        save->addEntity(document);
        save->setTime(now);

        return save;
    }

    SaveAsRef EditingSession::onEventSaveAs()
    {
        time_t now;
        time(&now);

        SaveAsRef saveAs = createSaveAs();
        saveAs->addEntity(document);
        saveAs->setTime(now);

        return saveAs;
    }
}
