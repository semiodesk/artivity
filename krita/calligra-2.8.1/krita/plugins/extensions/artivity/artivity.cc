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
#include <recorder/kis_action_recorder.h>
#include <recorder/kis_macro.h>
#include <recorder/kis_macro_player.h>
#include <recorder/kis_play_info.h>
#include <recorder/kis_recorded_action_factory_registry.h>
#include <recorder/kis_recorded_action.h>
#include <recorder/kis_recorded_action_load_context.h>
#include <recorder/kis_recorded_action_save_context.h>

#include <QDesktopServices>
#include <QApplication>
#include <QFileDialog>

K_PLUGIN_FACTORY(ArtivityPluginFactory, registerPlugin<ArtivityPlugin>();)
K_EXPORT_PLUGIN(ArtivityPluginFactory("krita"))

class RecordedActionSaveContext : public KisRecordedActionSaveContext {
    public:
        virtual void saveGradient(const KoAbstractGradient* ) {}
        virtual void savePattern(const KoPattern* ) {}
};

class RecordedActionLoadContext : public KisRecordedActionLoadContext {
    public:
        virtual KoAbstractGradient* gradient(const QString& name) const
        {
            return KoResourceServerProvider::instance()->gradientServer()->resourceByName(name);
        }
        virtual KoPattern* pattern(const QString& name) const
        {
            return KoResourceServerProvider::instance()->patternServer()->resourceByName(name);
        }
};

ArtivityPlugin::ArtivityPlugin(QObject *parent, const QVariantList &)
        : KisViewPlugin(parent, "kritaplugins/artivity.rc")
        , m_recorder(0)
{
	_log = artivity::ActivityLog();
    if (parent->inherits("KisView2")) {
        m_view = (KisView2*) parent;



        //KisAction* action = 0;

        // Start recording action
        m_startRecordingMacroAction = new KisAction(koIcon("media-record"), i18n("Start Artivity"), this);
        addAction("Recording_Start_Recording_Macro", m_startRecordingMacroAction);
        connect(m_startRecordingMacroAction, SIGNAL(triggered()), this, SLOT(slotStartRecordingMacro()));

        // Save recorded action
        m_stopRecordingMacroAction  = new KisAction(koIcon("media-playback-stop"), i18n("Stop Artivity"), this);
        addAction("Recording_Stop_Recording_Macro", m_stopRecordingMacroAction);
        connect(m_stopRecordingMacroAction, SIGNAL(triggered()), this, SLOT(slotStopRecordingMacro()));
        m_stopRecordingMacroAction->setEnabled(false);
    }
}

ArtivityPlugin::~ArtivityPlugin()
{
    m_view = 0;
    delete m_recorder;
}

void ArtivityPlugin::slotSave()
{
    saveMacro(m_view->image()->actionRecorder(), KUrl());
}

void ArtivityPlugin::slotOpenPlay()
{
}


void ArtivityPlugin::slotOpenEdit()
{
}

void ArtivityPlugin::slotStartRecordingMacro()
{
    std::cout << "Test" << std::endl;
    _log.transmit();
    dbgPlugins << "Start recording macro";
    if (m_recorder) return;
    // Alternate actions
    m_startRecordingMacroAction->setEnabled(false);
    m_stopRecordingMacroAction->setEnabled(true);

    // Create recorder
    m_recorder = new KisMacro();
    connect(m_view->image()->actionRecorder(), SIGNAL(addedAction(const KisRecordedAction&)),
            m_recorder, SLOT(addAction(const KisRecordedAction&)));
}

void ArtivityPlugin::slotStopRecordingMacro()
{
    dbgPlugins << "Stop recording macro";
    if (!m_recorder) return;
    // Alternate actions
    m_startRecordingMacroAction->setEnabled(true);
    m_stopRecordingMacroAction->setEnabled(false);
    // Save the macro
    saveMacro(m_recorder, KUrl());
    // Delete recorder
    delete m_recorder;
    m_recorder = 0;
}

KisMacro* ArtivityPlugin::openMacro(KUrl* url)
{

    Q_UNUSED(url);
    QStringList mimeFilter;
    mimeFilter << "*.krarec|Recorded actions (*.krarec)";

    QString filename = KoFileDialogHelper::getOpenFileName(m_view,
                                                           i18n("Open Macro"),
                                                           QDesktopServices::storageLocation(QDesktopServices::PicturesLocation),
                                                           mimeFilter,
                                                           "",
                                                           "OpenDocument");
    RecordedActionLoadContext loadContext;

    if (!filename.isNull()) {
        QDomDocument doc;
        QFile f(filename);
        if (f.exists()) {
            dbgPlugins << f.open(QIODevice::ReadOnly);
            QString err;
            int line, col;
            if (!doc.setContent(&f, &err, &line, &col)) {
                // TODO error message
                dbgPlugins << err << " line = " << line << " col = " << col;
                f.close();
                return 0;
            }
            f.close();
            QDomElement docElem = doc.documentElement();
            if (!docElem.isNull() && docElem.tagName() == "RecordedActions") {
                dbgPlugins << "Load the macro";
                KisMacro* m = new KisMacro();
                m->fromXML(docElem, &loadContext);
                return m;
            } else {
                // TODO error message
            }
        } else {
            dbgPlugins << "Unexistant file : " << filename;
        }
    }
    return 0;
}

void ArtivityPlugin::saveMacro(const KisMacro* macro, const KUrl& url)
{
    QString filename = QFileDialog::getSaveFileName(m_view, i18n("Save Macro"), url.url(), "*.krarec|Recorded actions (*.krarec)");
    if (!filename.isNull()) {
        QDomDocument doc;
        QDomElement e = doc.createElement("RecordedActions");
        RecordedActionSaveContext context;
        macro->toXML(doc, e, &context);

        doc.appendChild(e);
        QFile f(filename);
        f.open(QIODevice::WriteOnly);
        QTextStream stream(&f);
        doc.save(stream, 2);
        f.close();
    }
}

#include "artivity.moc"
