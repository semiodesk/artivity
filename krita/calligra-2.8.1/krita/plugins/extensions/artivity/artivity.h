#ifndef _ARTIVITY_H_
#define _ARTIVITY_H_

#include <QVariant>
#include <kis_view_plugin.h>
#include <artivity-client/ActivityLog.h>
#include <kundo2stack.h>
#include <KoShapeController.h>
#include <kis_canvas2.h>
#include <KoDocumentResourceManager.h>
#include <kis_action.h>
#include <recorder/kis_recorded_action.h>
#include <recorder/kis_action_recorder.h>

#include <commands/kis_layer_command.h>


class KisAction;
class KisMacro;
class KisView2;
class KUrl;

class ArtivityPlugin: public KisViewPlugin
{
    Q_OBJECT
public:
    ArtivityPlugin(QObject *parent, const QVariantList &);
    virtual ~ArtivityPlugin();


private slots:
    void StartArtivity();
    void StopArtivity();

    void addedAction(const KisRecordedAction& action);
    void indexChanged(int idx); 

private:

    int _currentIdx;
    artivity::ActivityLog _log;	
    KisView2 * m_view;
    KisCanvas2* _canvas;
    KUndo2Stack* _undoStack;

    KisAction* m_startRecordingMacroAction;
    KisAction* m_stopRecordingMacroAction;


};

#endif // artivityPlugin_H
