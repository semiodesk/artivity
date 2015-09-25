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
#include <cstdlib>
#include <unistd.h>

#include <kis_action.h>
#include <kactioncollection.h>
#include <kcomponentdata.h>
#include <kpluginfactory.h>
#include <klocale.h>
#include <kstandarddirs.h>

#include <KoIcon.h>
#include <KoUpdater.h>
#include <KoResourceServerProvider.h>
#include <KoFileDialogHelper.h>

#include <kis_config.h>
#include <kis_cursor.h>
#include <kis_debug.h>
#include <kis_global.h>
#include <kis_image.h>
#include <kis_resource_server_provider.h>
#include <kis_types.h>
#include <kis_view2.h>
#include <KoPattern.h>
#include <recorder/kis_recorded_action_save_context.h> 

#include <commands/kis_image_layer_add_command.h>


#include <QDesktopServices>
#include <QApplication>
#include <QFileDialog>

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
    _currentIdx = 1;
    _lastStackSize = 0;
    _agent = NULL;
    _entity = NULL;

    // test if plugin was loaded correctly
    if (parent->inherits("KisView2")) 
    {
        _active = TRUE;
        m_view = (KisView2*) parent;

        _doc = m_view->document();
        _canvas = m_view->canvasBase();
        _undoStack = _canvas->shapeController()->resourceManager()->undoStack();
        ImageInformation();

        connect(_undoStack, SIGNAL(indexChanged(int)), this, SLOT(indexChanged(int)));
        connect(_canvas->currentImage(), SIGNAL(sigAboutToBeDeleted()), this, SLOT(Close()));
        connect(m_view->zoomController(), SIGNAL(zoomChanged(KoZoomMode::Mode, qreal)), this, SLOT(ZoomChanged(KoZoomMode::Mode, qreal)));
        //connect(m_view->mainWindow(), SIGNAL(documentSaved()), this, SLOT(DocumentSaved()));
        //connect(_recorder, SIGNAL(addedAction(const KisRecordedAction&)), this, SLOT(AddAction(const
        //KisRecordedAction&)));

        _agent = new artivity::SoftwareAgent("application://krita.desktop");
        _log = new artivity::ActivityLog();

        LogEvent(artivity::art::Open, NULL);
        ProcessEventQueue();
    }
}

ArtivityPlugin::~ArtivityPlugin()
{
    //StopArtivity();
    m_view = 0;
    _undoStack = NULL;

    // Cleanup
    if( _log != NULL ) delete _log;
    if( _agent != NULL ) delete _agent;
    if( _entity != NULL ) delete _entity;
}



void ArtivityPlugin::indexChanged(int newIdx)
{
    if( _undoStack == NULL || _active == FALSE || !_undoStack->isActive() )
        return; 

    std::cout << "Index: " << newIdx << " Current idx: " << _currentIdx << " Stack size: " << _undoStack->count() <<
    "Last Stack size: " << _lastStackSize << std::endl;


    const KUndo2Command* currentCommand = _undoStack->command(newIdx-1);

    if( currentCommand == NULL )
        return;

    if( _lastStackSize > 0 && _undoStack->count() == 0 )
    {
        std::cout << "Final clear of stack... not logging." << std::endl;
        // Stack was emptied, do not log undos...
        return;
    }

    if( newIdx < _currentIdx )
    {
        int undoneEvents =  _currentIdx - newIdx;
        
        std::cout << "Undo " << undoneEvents << " events" << std::endl;

        int toUndo = 0;
        while(toUndo < undoneEvents )
        {
            currentCommand = _undoStack->command(_currentIdx - toUndo -1);
            LogUndoEvent(currentCommand);
            toUndo += 1; 
        }
        
    }else if( newIdx <= _undoStack->count() && _lastStackSize == _undoStack->count())
    {
        std::cout << "Redo " << newIdx - _currentIdx << " events" << std::endl;
        LogRedoEvent(currentCommand);
    }else
    {
        LogDoEvent(currentCommand);
    }

    

    _currentIdx = newIdx;
    _lastStackSize = _undoStack->count();

  
   
    ProcessEventQueue();

}

void ArtivityPlugin::LogDoEvent(const KUndo2Command* command)
{
    QString text = command->actionText();
    std::cout << "Do " << text.toUtf8().constData() << std::endl;
    LogEvent(art::Update, command);
}

void ArtivityPlugin::LogUndoEvent(const KUndo2Command* command)
{
    QString text = command->actionText();
    std::cout << "Undo: " << text.toUtf8().constData() << std::endl;
    LogEvent(art::Undo, command);
}

void ArtivityPlugin::LogRedoEvent(const KUndo2Command* command)
{
    QString text = command->actionText();
    std::cout << "Redo: " << text.toUtf8().constData() << std::endl;
    LogEvent(art::Redo, command);
}

void ArtivityPlugin::ZoomChanged(KoZoomMode::Mode mode, qreal zoom)
{
    //std::cout << "Zoom: " << zoom << std::endl;
    _zoomFactor = zoom;
}

void ArtivityPlugin::LogLayerAdded()
{
    KisLayerSP layer = m_view->activeLayer();
    std::cout << "Layer added: "<< layer->name().toUtf8().constData()  << std::endl;
}


void ArtivityPlugin::ImageInformation()
{
    KisDoc2* doc = m_view->document();
    std::cout << "File : " << doc->localFilePath().toUtf8().constData() << std::endl;


    KisImageSP image = _canvas->currentImage();
    std::cout << "Image info -> w: " << image->width() << " h: " << image->height() << std::endl;
    std::cout << "Res -> x: " << image->xRes() << " y: " << image->yRes() << std::endl;
    std::cout << "Color -> " << image->colorSpace()->name().toUtf8().constData() << std::endl;
    std::cout << "Unit -> " << _canvas->unit().symbol().toUtf8().constData() << std::endl;
}

void ArtivityPlugin::Selection()
{
    KisSelectionSP sel = m_view->selection();
}

void ArtivityPlugin::LogEvent(const Resource& type, const KUndo2Command* command)
{
    artivity::Activity* activity = new artivity::Activity();
    activity->setValue(artivity::rdf::_type, type);

    time_t now;
    time(&now);
    activity->setTime(&now);

    artivity::Association* association = new artivity::Association();
    association->setAgent(_agent);
    _log->addResource(association);

    activity->addAssociation(association);
    
    if(activity->is(art::Update) || activity->is(art::Undo) || activity->is(art::Redo))
    {
        LogModifyingEvent(activity, command);
    }
    
   
    _log->push_back(activity);
}

void ArtivityPlugin::LogModifyingEvent(artivity::Activity* activity, const KUndo2Command* command)
{
    
    Viewbox* viewbox = new Viewbox();
    viewbox->setZoomFactor(_zoomFactor);

    _log->addResource(viewbox);
    
    Generation* generation = new Generation();
    generation->setViewbox(viewbox);
    
    _log->addResource(generation);
    
    FileDataObject* generated = new FileDataObject("_:UNSET");
    //generated->addProperty(rdf::_type, prov::Collection);
    generated->addProperty(prov::generatedAtTime, activity->getTime());
    generated->setGeneration(generation);
    generated->setLastModificationTime(activity->getTime());

    activity->addGeneratedEntity(generated);

    _log->addResource(generated);

    Invalidation* invalidation = new Invalidation();
    invalidation->setViewbox(viewbox);

    _log->addResource(invalidation);

    FileDataObject* invalidated = new FileDataObject("_:UNSET");
    //invalidated->addProperty(rdf::_type, prov::Collection);
    invalidated->addProperty(prov::invalidatedAtTime,
    activity->getTime());
    invalidated->setInvalidation(invalidation);

    activity->addInvalidatedEntity(invalidated);

    _log->addResource(invalidated);

    LogActivityInformation(activity, command);
}

void ArtivityPlugin::LogActivityInformation(artivity::Activity* activity, const KUndo2Command* command)
{

    // Adding title
    activity->setValue(dces::title, command->actionText().toUtf8().constData());

    // Adding current layer
    KisLayerSP layer = m_view->activeLayer();
    activity->setValue(art::selectedLayer, layer->name().toUtf8().constData());



/*
    if(activity->hasProperty(rdf::_type, art::Undo) || activity->hasProperty(rdf::_type, art::Redo))
        return;


    if( dynamic_cast<const KisImageLayerAddCommand*>(currentCommand) != NULL )
    {
        activity->setValue(rdf::_type, art::Add);

    }else if( dynamic_cast<const KisImageLayerRemoveCommand*>(currentCommand) != NULL )
    {
        activity->setValue(rdf::_type, art::Delete);
    }
    else if( dynamic_cast<const KisImageLayerMoveCommand*>(currentCommand) != NULL )
    {
        std::cout << "Layer move" << std::endl;
    }
*/
}

void ArtivityPlugin::ProcessEventQueue()
{
    if( _log->empty() || !_log->isConnected()) return;

    if( _doc->localFilePath() == NULL )
    {
        return;
    }

    string uri = "file://" + string(_doc->localFilePath().toUtf8().constData());
    if( _entity == NULL )
    {
        // NOTE: We do not know the actual URI of the file data object here.
        // Therefore we add it as a variable and let the server replace
        // the URI with the one it knows. This is hacky, but will eventually
        // not be necessary once the client API is being stabilized.
        string var = "?:" + uri;

        FileDataObject* file = new FileDataObject(var.c_str());
        file->setUrl(uri.c_str());

        _log->addResource(file);

        _entity = file;
    }
    
    artivity::ActivityLogIterator it = _log->begin();
    while( it != _log->end())
    {
        artivity::Activity* activity = *it;
        activity->addUsedEntity(_entity);

        if( activity->is(art::Update) || activity->is(art::Undo) || activity->is(art::Redo))
        {
            if(!activity->getGeneratedEntities().empty())
            {
                Entity* e = *(activity->getGeneratedEntities().begin());
                FileDataObject* generated = dynamic_cast<FileDataObject*>(e);

                if( generated != NULL )
                {
                    stringstream generatedUri;
                    string id = UriGenerator::getRandomId(10);
                    generatedUri << uri << "#" << id ;
                    generated->setUri(generatedUri.str());
                    generated->setUrl(uri.c_str());
                    generated->addGenericEntity(_entity);
                }
            }

            if(!activity->getInvalidatedEntities().empty())
            {
                Entity* e = *(activity->getInvalidatedEntities().begin());
                FileDataObject* invalidated = dynamic_cast<FileDataObject*>(e);

                if( invalidated != NULL )
                {
                    stringstream invalidatedUri;
                    string id = UriGenerator::getRandomId(10);
                    invalidatedUri << uri << "#" << id;

                    invalidated->setUri(invalidatedUri.str());
                    invalidated->setUrl(uri.c_str());
                    invalidated->addGenericEntity(_entity);
                }
            }
        }
        it++;
    }
    _log->transmit();
}



void ArtivityPlugin::Close()
{
    std::cout << "ABORT!" << std::endl;
    if( _holdBack )
    {
        std::cout << "Discard last undo actions." << std::endl;
    }
    LogEvent(artivity::art::Close, NULL);
    ProcessEventQueue();
    _active = FALSE;
}


#include "artivity.moc"
