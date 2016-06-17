#include "EditingSession.h"

#include <boost/filesystem/operations.hpp>
#include "ObjectModel/SoftwareAgent.h"


using namespace std;

namespace artivity
{

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
        AssociationRef softwareAgentAssociation = AssociationRef(new Association(this->getSoftwareAgent()->c_str()));
        _log->addAssociation(softwareAgentAssociation);

        _initialized = true;

        _fileUri = stringRef(new string(_log->getFileUri()));
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
        _consumer->push(res);
    }

    void EditingSession::eventDelete()
    {
        ResourceRef res = onEventDelete();
        _consumer->push(res);
    }

    void EditingSession::eventEdit()
    {
        RevisionRef res = onEventEdit();
        
        _consumer->push(res);
    }

    void EditingSession::eventUndo()
    {
        ResourceRef res = onEventUndo();
        _consumer->push(res);
    }

    void EditingSession::eventRedo()
    {
        ResourceRef res = onEventRedo();
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
            //_log->addInfluence(boost::dynamic_pointer_cast<ActivityInfluence>(res));
        }
        else if (strcmp(type, prov::Invalidation) == 0)
        {
            //_log->addInfluence(boost::dynamic_pointer_cast<ActivityInfluence>(res));
        }
        else if (strcmp(type, art::Undo) == 0)
        {
            //_log->addInfluence(boost::dynamic_pointer_cast<ActivityInfluence>(res));
        }
        else if (strcmp(type, art::Redo) == 0)
        {
            //_log->addInfluence(boost::dynamic_pointer_cast<ActivityInfluence>(res));
        }
    }

}