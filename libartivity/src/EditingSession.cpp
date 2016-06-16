#include "EditingSession.h"

#include <boost/filesystem/operations.hpp>
#include "ObjectModel/SoftwareAgent.h"

using namespace artivity;

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
    _consumer->start();

    //_log = ActivityLogRef(new ActivityLog(server));
    //_log->addAgent(SoftwareAgentRef(new SoftwareAgent(*(this->getSoftwareAgent()))));

    _initialized = true;
}


EditingSession::~EditingSession()
{
    if (_wasClosed)
    {
        throw std::exception("EditingSession was destroyed without EventClose(). This is not supposed to happen!");
    }
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
    ResourceRef res = onEventEdit();
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

void EditingSession::eventClose()
{
    ResourceRef res = onEventClose();
    _consumer->push(res);
    _consumer->stop();
    _activity->clear();
    _wasClosed = true;
}

bool EditingSession::fileExists(const std::string& name)
{
    return boost::filesystem::exists(name);
}

void EditingSession::consume(ResourceRef res)
{
    string type = res->getType()->Uri;

    if (type == prov::Generation)
    {
        handleEventAdd(res);
    }
    if (type == prov::Revision)
    {
        handleEventEdit(res);
    }
    else if (type == art::Undo)
    {
        handleEventUndo(res);
    }
    else if (type == art::Redo)
    {
        handleEventRedo(res);
    }
    else if (type == art::Save)
    {
        handleEventSave(res);
    }
    //else if (type == art::SaveAs)//TODO art::SaveAs MISSING!
    //{
    //    handleEventSave(res);
    //}
    //else if (type == art::Close) //TODO art::Close MISSING!
    //{
    //    HandleEventClose(res);
    //}
    else if (type == prov::Invalidation) 
    {
        handleEventDelete(res);
    }

}

void EditingSession::handleEventAdd(ResourceRef data) {}
void EditingSession::handleEventDelete(ResourceRef data) {}
void EditingSession::handleEventEdit(ResourceRef data) {}
void EditingSession::handleEventUndo(ResourceRef data) {}
void EditingSession::handleEventRedo(ResourceRef data) {}
void EditingSession::handleEventClose(ResourceRef data) {}
void EditingSession::handleEventSave(ResourceRef data) {}
void EditingSession::handleEventSaveAs(ResourceRef data) {}
void EditingSession::handleTransmit() {}

