
#include "log.h"


ArtivityLog::ArtivityLog(QObject* parent) : QAbstractItemModel(parent)
{

}

void
ArtivityLog::setCanvas(KisCanvas2 *canvas)
{
    _canvas = canvas;
}

void 
ArtivityLog::setStack(KUndo2QStack *stack)
{
    if( _stack == stack)
        return;

    if( _stack != 0 )
    {
         disconnect(_stack, SIGNAL(cleanChanged(bool)), this, SLOT(stackChanged()));  
         disconnect(_stack, SIGNAL(indexChanged(int)), this, SLOT(stackChanged())); 
         disconnect(_stack, SIGNAL(destroyed(QObject*)), this, SLOT(stackDestroyed(QObject*)));
         disconnect(_stack, SIGNAL(indexChanged(int)), this, SLOT(addImage(int))); 
    }

    if( _stack != 0 )
    {
        connect(_stack, SIGNAL(cleanChanged(bool)), this, SLOT(stackChanged()));  
        connect(_stack, SIGNAL(indexChanged(int)), this, SLOT(stackChanged()));
        connect(_stack, SIGNAL(destroyed(QObject*)), this, SLOT(stackDestroyed(QObject*)));
        connect(_stack, SIGNAL(indexChanged(int)), this, SLOT(addImage(int))); 
    }

    stackChanged();
}


void ArtivityLog::stackDestroyed(QObject *obj)
{
    if( obj != _stack )
        return;

    _stack = 0;
    stackChanged();

}


void ArtivityLog::stackChanged()
{
    //std::cout << "Stack changed!" << std::endl;
}


const KUndo2Command* ArtivityLog::Command(const QModelIndex &index) const 
{ 
    if (_stack == 0)
        return NULL;
    if (index.column() != 0)
        return NULL;

    if (index.row() < 0 || index.row() > _stack->count())
        return NULL;

    if(!index.row() == 0)
    {
        const KUndo2Command* currentCommand = _stack->command(index.row() - 1);
        return currentCommand;
    } 
    return NULL;
}


