/*
 *  Copyright (c) 2007 Cyrille Berger (cberger@cberger.net)
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
    // Initialization with zero
    _currentIdx = 0;
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
        //connect(m_view->mainWindow(), SIGNAL(documentSaved()), this, SLOT(DocumentSaved()));

        _agent = new artivity::SoftwareAgent("application://krita.desktop");
        _log = new artivity::ActivityLog();

        LogEvent(artivity::art::Open);
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



void ArtivityPlugin::indexChanged(int idx)
{
    if( _undoStack == NULL || _active == FALSE || !_undoStack->isActive() )
        return; 

    if( _undoStack->isClean() )
    {
        // Hold back undo changes until we get another change or a save
        _holdBack = TRUE;
        return;
    }

    if( idx == _currentIdx )
    {
        std::cout << "Do " << std::endl;
    }
    else if( idx < _currentIdx )
    {
        std::cout << "Undo " << _currentIdx - idx << " events" << std::endl;
    }else if( idx < _undoStack->count() )
    {
        std::cout << "Redo " << idx - _currentIdx << " events" << std::endl;
    }
    const KUndo2Command* currentCommand = _undoStack->command(idx-1);

    if( currentCommand == NULL )
        return;


    _currentIdx = idx;
    QString text = currentCommand->actionText();
    QString text2 = currentCommand->text();
    
    std::cout << "Action: " << text.toUtf8().constData() << " :: " << text2.toUtf8().constData() << std::endl;



     if( dynamic_cast<const KisImageLayerAddCommand*>(currentCommand) != NULL )
     {
        LogLayerAdded();
        LayerInformation();
     }

     //ImageInformation();
}


void ArtivityPlugin::LogLayerAdded()
{
    std::cout << "Layer Add command" << std::endl;
}

void ArtivityPlugin::LayerInformation()
{
    KisLayerSP layer = m_view->activeLayer();
    std::cout << "Layer name " << layer->name().toUtf8().constData() << std::endl;
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

void ArtivityPlugin::LogEvent(const artivity::Resource& type)
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

    _log->push_back(activity);

    ProcessEventQueue();
}

void ArtivityPlugin::ProcessEventQueue()
{
    if( _log->empty() || !_log->isConnected()) return;

    if( _doc->localFilePath() == NULL )
    {
        return;
    }

    if( _entity == NULL )
    {
        string uri = "file://" + string(_doc->localFilePath().toUtf8().constData());
        _entity = new artivity::FileDataObject(uri.c_str());
    }
    
    artivity::ActivityLogIterator it = _log->begin();
    while( it != _log->end())
    {
        artivity::Activity* activity = *it;
        activity->addUsedEntity(_entity);
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
    LogEvent(artivity::art::Close);
    _active = FALSE;
}


#include "artivity.moc"
