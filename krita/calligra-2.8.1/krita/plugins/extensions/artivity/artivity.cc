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
    _currentIdx = 0;
    _log = artivity::ActivityLog();
    if (parent->inherits("KisView2")) 
    {
        m_view = (KisView2*) parent;

        _canvas = m_view->canvasBase();
        _undoStack = _canvas->shapeController()->resourceManager()->undoStack();

        // Start recording action
        m_startRecordingMacroAction = new KisAction(koIcon("media-record"), i18n("Start"), this);
        addAction("Start_Artivity", m_startRecordingMacroAction);
        connect(m_startRecordingMacroAction, SIGNAL(triggered()), this, SLOT(StartArtivity()));

        // Save recorded action
        m_stopRecordingMacroAction  = new KisAction(koIcon("media-playback-stop"), i18n("Stop"), this);
        addAction("Stop_Artivity", m_stopRecordingMacroAction);
        connect(m_stopRecordingMacroAction, SIGNAL(triggered()), this, SLOT(StopArtivity()));
        m_stopRecordingMacroAction->setEnabled(false);
    }
}

ArtivityPlugin::~ArtivityPlugin()
{
    m_view = 0;
}


void ArtivityPlugin::StartArtivity()
{
    connect(m_view->image()->actionRecorder(), SIGNAL(addedAction(const KisRecordedAction&)), 
        this, SLOT(addedAction(const KisRecordedAction&)));

    connect(_undoStack, SIGNAL(indexChanged(int)), this, SLOT(indexChanged(int)));
}

void ArtivityPlugin::StopArtivity()
{
}

void ArtivityPlugin::addedAction(const KisRecordedAction& action)
{
    std::cout << "Action: " << action.name().toUtf8().constData() << std::endl;
    
    QDomDocument doc;
    QDomElement e = doc.createElement("RecordedActions");
    RecordedActionSaveContext context;
    action.toXML(doc, e, &context);
    doc.appendChild(e);

    //std::cout << "Content: " << doc.toString().toUtf8().constData() << std::endl;



    //std::cout << action.toXML() << std::endl;
}

void ArtivityPlugin::indexChanged(int idx)
{
    if( idx < _currentIdx )
    {
        std::cout << "Undo " << _currentIdx - idx << " events" << std::endl;
    }else if( idx < _undoStack->count() )
    {
        std::cout << "Redo " << idx - _currentIdx << " events" << std::endl;
    }
    const KUndo2Command* currentCommand = _undoStack->command(idx-1);
     _currentIdx = idx;

     if( dynamic_cast<const KisLayerCommand*>(currentCommand) == NULL )
     {
         std::cout << "Layer command" << std::endl;
     }
}




#include "artivity.moc"
