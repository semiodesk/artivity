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

#ifndef _ARTIVITY_H_
#define _ARTIVITY_H_

#include <QVariant>
#include <kis_view_plugin.h>
#include <artivity-client/artivity.h>
#include <kundo2stack.h>
#include <KoShapeController.h>
#include <kis_canvas2.h>
#include <KoDocumentResourceManager.h>
#include <kis_action.h>
#include <recorder/kis_recorded_action.h>
#include <recorder/kis_action_recorder.h>
#include <kis_view2.h>
#include <kis_doc2.h>
#include <kis_layer.h>
#include <QWidget>
#include <KoZoomController.h>
#include <kis_selection.h>

#include <commands/kis_layer_commands.h>
#include <commands/kis_image_commands.h>
#include <commands/kis_selection_commands.h>


class KisAction;
class KisMacro;
class KUrl;

class ArtivityPlugin: public KisViewPlugin
{
    Q_OBJECT
public:
    ArtivityPlugin(QObject *parent, const QVariantList &);
    virtual ~ArtivityPlugin();


private slots:
    //void addedAction(const KisRecordedAction& action);
    void indexChanged(int newIdx); 
    void Close();
    void ZoomChanged(KoZoomMode::Mode mode, qreal zoom);

protected:
    //void logDrawEvent(const artivity::Resource& type, QString layer);

private:

    int _currentIdx;
    bool _active;
    bool _holdBack;
    qreal _zoomFactor;
    
    artivity::ActivityLog* _log;
    artivity::SoftwareAgent* _agent;
    artivity::FileDataObject* _entity;

    KisView2 * m_view;
    KisCanvas2* _canvas;
    KUndo2Stack* _undoStack;
    KisDoc2* _doc;
    //KisActionRecorder* _recorder;

    void LogLayerAdded();
    void ImageInformation();
    void Selection();
   
    void LogEvent(const artivity::Resource& type, const KUndo2Command* command);
    void LogDoEvent(const KUndo2Command* command);
    void LogUndoEvent(const KUndo2Command* command);
    void LogRedoEvent(const KUndo2Command* command);
    void LogModifyingEvent(artivity::Activity* activity, const KUndo2Command* command);
    void ProcessEventQueue();

};

#endif // artivityPlugin_H
