#ifndef ARTIVITYLOG_H
#define ARTIVITYLOG_H

#include <QAbstractItemModel>
#include <kundo2qstack.h>
#include <kundo2command.h>
#include <kis_canvas2.h>

#include <artivity-client/ActivityLog.h>


class ArtivityLog : public QAbstractItemModel
{
    Q_OBJECT

    public:
        ArtivityLog(QObject *parent = 0);

        void setCanvas(KisCanvas2* canvas);

        const KUndo2Command* Command(const QModelIndex &index) const;


    public slots:
        void setStack(KUndo2QStack *stack);
        
    private:
        KUndo2QStack *_stack;
        KisCanvas2* _canvas;

    private slots:
        void stackChanged();
        void stackDestroyed( QObject *obj);

};
    
#endif
