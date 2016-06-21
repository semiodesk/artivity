#include "EditingSession.h"

#include <boost/filesystem/operations.hpp>
#include "ObjectModel/SoftwareAgent.h"


using namespace std;

namespace artivity
{
    typedef vector<string>::reverse_iterator rchangeIt;
    typedef vector<string>::iterator changeIt;

    EditingSession::EditingSession()
    {
    }

    void EditingSession::initialize(string server, bool isNewDocument)
    {
        if (!isNewDocument)
        {
            _filePath = getDocumentFilePath();
        }

        _consumer = ProducerConsumerRef(new ProducerConsumer<EditingSession, ResourceRef>());
        _consumer->ConsumeHandler = &EditingSession::consume;
        _consumer->setClassObject(this);
        _consumer->start();

        _log = ActivityLogRef(new ActivityLog(server));
        _log->setAgent(getSoftwareAgent(), getSoftwareAgentVersion());
        

        _initialized = true;

        _fileUri = string(_log->getFileUri());

        _currentChangeIndex = 0;
    }


    EditingSession::~EditingSession()
    {
        time_t now;
        time(&now);
        _log->close(now);
    }

    void EditingSession::eventAdd()
    {
        ResourceRef res = onEventAdd();
        handleChanges(res);
        
        _consumer->push(res);
    }

    void EditingSession::eventDelete()
    {
        ResourceRef res = onEventDelete();
        handleChanges(res);
        _consumer->push(res);
    }

    void EditingSession::handleChanges(ResourceRef res)
    {
        if (_currentChangeIndex < _changes.size())
        {
            changeIt it = _changes.begin() + _currentChangeIndex;
            _changes.erase(it);
        }
        _currentChangeIndex++;
        _changes.push_back(res->Uri);
    }

    void EditingSession::eventEdit()
    {
        RevisionRef res = onEventEdit();
        handleChanges(res);
        _consumer->push(res);
    }

    void EditingSession::eventUndo()
    {
        UndoRef res = onEventUndo();
        int count = res->getCount();

        changeIt current = _changes.begin() + _currentChangeIndex;
        _currentChangeIndex -= count;
        for (changeIt it = _changes.begin()+_currentChangeIndex;  it != current; it++)
        {
            res->addRevision(*it);
        }

        _consumer->push(res);
    }

    void EditingSession::eventRedo()
    {
        RedoRef res = onEventRedo();
        int count = res->getCount();

         int beforeIndex = _currentChangeIndex;
        _currentChangeIndex += count;
        changeIt current = _changes.begin() + _currentChangeIndex;
        for (changeIt it = _changes.begin() + beforeIndex; it != current; it++)
        {
            res->addRevision(*it);
        }

        _consumer->push(res);
    }

    void EditingSession::eventSave()
    {
        ResourceRef res = onEventSave();
        _consumer->push(res);
    }

    void EditingSession::eventSaveAs()
    {
        ResourceRef res = onEventSaveAs();
        _consumer->push(res);
    }

    bool EditingSession::fileExists(const std::string& name)
    {
        return boost::filesystem::exists(name);
    }

    void EditingSession::consume(ResourceRef res)
    {
        const char* type = res->getType();

        if (strcmp(type, prov::Revision) == 0)
        {
            _log->addInfluence(boost::dynamic_pointer_cast<EntityInfluence>(res));
        }
        else if (strcmp(type, prov::Derivation) == 0)
        {
            _log->addInfluence(boost::dynamic_pointer_cast<EntityInfluence>(res));
        }
        else if (strcmp(type, prov::Generation) == 0)
        {
            _log->addInfluence(boost::dynamic_pointer_cast<ActivityInfluence>(res));
        }
        else if (strcmp(type, prov::Invalidation) == 0)
        {
            _log->addInfluence(boost::dynamic_pointer_cast<ActivityInfluence>(res));
        }
        else if (strcmp(type, art::Undo) == 0)
        {
            _log->addInfluence(boost::dynamic_pointer_cast<ActivityInfluence>(res));
        }
        else if (strcmp(type, art::Redo) == 0)
        {
            _log->addInfluence(boost::dynamic_pointer_cast<ActivityInfluence>(res));
        }
    }

}