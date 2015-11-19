/*
 * Authors:
 * Moritz Eberl <moritz@semiodesk.com>
 * Sebastian Faubel <sebastian@semiodesk.com>
 *
 * Copyright (c) 2015 Semiodesk GmbH
 *
 * Released under GNU GPL, see the file 'COPYING' for more information
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation; version 2 of the License.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.
 */

#include "artivity.h"

#include <ctime>

#include <QDesktopServices>
#include <QApplication>
#include <QFileDialog>

#include <kactioncollection.h>
#include <kcomponentdata.h>
#include <kpluginfactory.h>
#include <klocale.h>
#include <kstandarddirs.h>

#include <KoIcon.h>
#include <KoUpdater.h>
#include <KoPattern.h>
#include <KoResourceServerProvider.h>
#include <KoFileDialogHelper.h>

#include <kis_action.h>
#include <kis_config.h>
#include <kis_cursor.h>
#include <kis_debug.h>
#include <kis_global.h>
#include <kis_image.h>
#include <kis_resource_server_provider.h>
#include <kis_types.h>
#include <kis_view2.h>
#include <recorder/kis_recorded_action_save_context.h> 
#include <commands/kis_image_layer_add_command.h>

using namespace artivity;

K_PLUGIN_FACTORY(ArtivityPluginFactory, registerPlugin<ArtivityPlugin>();)
K_EXPORT_PLUGIN(ArtivityPluginFactory("krita"))

class RecordedActionSaveContext : public KisRecordedActionSaveContext 
{                     
    public: 
        virtual void saveGradient(const KoAbstractGradient* ) {} 
        virtual void savePattern(const KoPattern* ) {}
}; 

ArtivityPlugin::ArtivityPlugin(QObject *parent, const QVariantList &)
    : KisViewPlugin(parent, "kritaplugins/artivity.rc")
{
    _currentIndex = 1;
    _lastStackSize = 0;
    _topCommand = NULL;

    // Test if plugin was loaded correctly.
    if (parent->inherits("KisView2")) 
    {
        _view = (KisView2*)parent;

        _document = _view->document();
        _canvas = _view->canvasBase();
        _undoStack = _canvas->shapeController()->resourceManager()->undoStack();

        _active = TRUE;

        // Initialize the Artivity log.
        _log = new ActivityLog();
        _log->addAgent(new SoftwareAgent("application://krita.desktop"));

        double width = _canvas->currentImage()->xRes();
        double height = _canvas->currentImage()->yRes();
        const Resource* unit = getLengthUnit();

        _filePath = string(_view->document()->localFilePath().toUtf8().constData()).c_str();

        if(_filePath == NULL || strlen(_filePath) == 0)
        {
           _activity = _log->createFile(width, height, unit);
        }
        else
        {
           _activity = _log->editFile(_filePath, width, height, unit);

           transmitQueue();
        }

        connect(_undoStack, SIGNAL(indexChanged(int)), this, SLOT(onUndoIndexChanged(int)));
        connect(_canvas->currentImage(), SIGNAL(sigAboutToBeDeleted()), this, SLOT(onClose()));
        connect(_view->zoomController(), SIGNAL(zoomChanged(KoZoomMode::Mode, qreal)), this, SLOT(onZoomChanged(KoZoomMode::Mode, qreal)));
        connect((const QObject*)_view->mainWindow(), SIGNAL(documentSaved()), this, SLOT(onDocumentSaved()));
        //connect(_recorder, SIGNAL(addedAction(const KisRecordedAction&)), this, SLOT(AddAction(const KisRecordedAction&)));
    }
}

ArtivityPlugin::~ArtivityPlugin()
{
    onClose();
 
    _view = NULL;
    _undoStack = NULL;

    if( _log != NULL ) delete _log;
}

void ArtivityPlugin::logEvent(const Resource& type, const KUndo2Command* command)
{
    if(type.Uri != art::Edit.Uri && type.Uri != art::Undo.Uri && type.Uri == art::Redo.Uri)
    {
        return;
    }

    time_t now;
    time(&now);

    QPoint origin = _canvas->documentOffset();

    Point* p = _log->createResource<Point>();
    p->setX(origin.x() / _zoomFactor);
    p->setY(origin.y() / _zoomFactor);
   
    QWidget* widget = _canvas->canvasWidget();
    
    Viewport* viewport = _log->createResource<Viewport>();
    viewport->setPosition(p);
    viewport->setWidth(widget->width() / _zoomFactor);
    viewport->setHeight(widget->height() / _zoomFactor);
    viewport->setZoomFactor(_zoomFactor);

    string title = string(command->actionText().toUtf8().constData());
    string layer = string(_view->activeLayer()->name().toUtf8().constData());
 
    Generation* generation = _log->createEntityInfluence<Generation>(now, type, viewport);
    generation->setValue(art::selectedLayer, layer.c_str());
    generation->setDescription(title);

    Invalidation* invalidation = _log->createEntityInfluence<Invalidation>(now, type, viewport);
    generation->setValue(art::selectedLayer, layer.c_str());

    Canvas* canvas = getCanvas();

    FileDataObject* file = _log->getFile();

    FileDataObject* newver = _log->createEntityVersion<FileDataObject>(file, canvas);
    newver->setGeneration(generation);

    FileDataObject* oldver = _log->createEntityVersion<FileDataObject>(file, canvas);
    oldver->setInvalidation(invalidation);

    _activity->addGeneratedEntity(newver);
    _activity->addInvalidatedEntity(oldver);

    /* TODO: Add support for more specialized entity influences to art Ontology.
    if( dynamic_cast<const KisImageLayerAddCommand*>(command) != NULL )
    {
        activity->setValue(rdf::_type, art::Add);

    }else if( dynamic_cast<const KisImageLayerRemoveCommand*>(command) != NULL )
    {
        activity->setValue(rdf::_type, art::Delete);
    }
    else if( dynamic_cast<const KisImageLayerMoveCommand*>(command) != NULL )
    {
        std::cout << "Layer move" << std::endl;
    }
    */
}

void ArtivityPlugin::transmitQueue()
{
    // If the document has not been saved yet, we keep all 
    // pending changes in the event queue.
    if(_log->empty() || _document->localFilePath() == NULL)
    {
        return;
    }

    FileDataObject* file = _log->getFile();

    if(file == NULL)
    {
        return;
    }

    if(_filePath == NULL || strlen(_filePath) == 0)
    {
        _filePath = string(_document->localFilePath().toUtf8().constData()).c_str();
        
        // Add the newly created file to the file system monitor.
        _log.enableMonitoring(_filePath);
    }
    
    string fileUrl = string("file://") + _filePath;
    
    file->setUrl(fileUrl.c_str());

    _activity->addUsedEntity(file);
    
    if(!_activity->getGeneratedEntities().empty())
    {
        Entity* e = *(_activity->getGeneratedEntities().begin());
    
        FileDataObject* newver = dynamic_cast<FileDataObject*>(e);
            
        if(newver != NULL)
        {
            stringstream uri;
            uri << fileUrl << "#" << UriGenerator::getRandomId(10);
            
            newver->setUri(uri.str());
            newver->addGenericEntity(file);
        }
    }
        
    if(!_activity->getInvalidatedEntities().empty())
    {
        Entity* e = *(_activity->getInvalidatedEntities().begin());
    
        FileDataObject* oldver = dynamic_cast<FileDataObject*>(e);
            
        if(oldver != NULL)
        {
            stringstream uri;
            uri << fileUrl << "#" << UriGenerator::getRandomId(10);
            
            oldver->setUri(uri.str());
            oldver->addGenericEntity(file);
        }
    }
   
    _log->transmit();
}

void ArtivityPlugin::onDocumentSaved()
{
    time_t now;
    time(&now);

    QPoint origin = _canvas->documentOffset();
    
    Point* p = _log->createResource<Point>();
    p->setX(origin.x() / _zoomFactor);
    p->setY(origin.y() / _zoomFactor);
    
    QWidget* widget = _canvas->canvasWidget();
    
    Viewport* viewport = _log->createResource<Viewport>();
    viewport->setPosition(p);
    viewport->setWidth(widget->width() / _zoomFactor);
    viewport->setHeight(widget->height() / _zoomFactor);
    viewport->setZoomFactor(_zoomFactor);
    
    Generation* generation = _log->createEntityInfluence<Generation>(now, art::Save, viewport);
    
    FileDataObject* file = _log->getFile();
    file->setGeneration(generation);
    file->setLastModificationTime(now);
    
    _activity->addGeneratedEntity(file);
    
    transmitQueue();
}

void ArtivityPlugin::onZoomChanged(KoZoomMode::Mode mode, qreal zoomFactor)
{
    _zoomFactor = zoomFactor;
}

void ArtivityPlugin::onUndoIndexChanged(int newIndex)
{
    if(_undoStack == NULL || _active == FALSE || !_undoStack->isActive())
    {
        return; 
    }

    const KUndo2Command* command = _undoStack->command(newIndex - 1);

    if(command == NULL)
    {
        return;
    }

    if(_lastStackSize > 0 && _undoStack->count() == 0)
    {
        // Stack was emptied, do not log undos...
        return;
    }

    if(newIndex < _currentIndex)
    {
        int i = 0;
        int commandCount =  _currentIndex - newIndex;
        
        while(i < commandCount)
        {
            command = _undoStack->command(_currentIndex - i - 1);

	    logEvent(art::Undo, command);

            i += 1; 
        }
    }
    else if((newIndex < _undoStack->count() && _lastStackSize == _undoStack->count())
        || (newIndex == _undoStack->count() && _topCommand == command))
    {
	logEvent(art::Redo, command);
    }
    else
    {
	logEvent(art::Edit, command);

        _topCommand = command;
    }

    _currentIndex = newIndex;
    _lastStackSize = _undoStack->count();

    transmitQueue();
}

void ArtivityPlugin::onClose()
{
    const char* filePath = _document->localFilePath().toUtf8().constData();

    // Mark the end time of the activity if the file has been saved.
    if(filePath != NULL && strlen(filePath) > 0)
    {
        time_t now;
        time(&now);

        _activity->setEndTime(now);

        transmitQueue();
    }
    
    _active = FALSE;
}

const Resource* ArtivityPlugin::getLengthUnit()
{
    // TODO: Implement.
    //std::cout << "Unit -> " << _canvas->unit().symbol().toUtf8().constData() << std::endl;
    return &art::px;
}

Canvas* ArtivityPlugin::getCanvas()
{
    double width = _canvas->currentImage()->xRes();
    double height = _canvas->currentImage()->yRes();
    const Resource* lengthUnit = getLengthUnit();

    _log->updateCanvas(width, height, lengthUnit);

    return _log->getCanvas();
}

void ArtivityPlugin::getImageInformation()
{
    //KisDoc2* doc = _view->document();
    //std::cout << "File : " << doc->localFilePath().toUtf8().constData() << std::endl;

    //KisImageSP image = _canvas->currentImage();
    //std::cout << "Image info -> w: " << image->width() << " h: " << image->height() << std::endl;
    //std::cout << "Res -> x: " << image->xRes() << " y: " << image->yRes() << std::endl;
    //std::cout << "Color -> " << image->colorSpace()->name().toUtf8().constData() << std::endl;
    //std::cout << "Unit -> " << _canvas->unit().symbol().toUtf8().constData() << std::endl;
}

void ArtivityPlugin::getSelection()
{
    //KisSelectionSP sel = _view->selection();
}

#include "artivity.moc"
