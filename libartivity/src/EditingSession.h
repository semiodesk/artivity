#ifndef _ART_EDITINGSESSION_H
#define _ART_EDITINGSESSION_H

#include "ActivityLog.h"
#include "ProducerConsumer.h"
#include "defines.h"
#include "ObjectModel/Influences\Undo.h"
#include "ObjectModel/Influences\Redo.h"
#include "ObjectModel/Influences\Save.h"
#include "ObjectModel/Revision.h"
#include "ObjectModel/Derivation.h"

namespace artivity
{
    class EditingSession;
    typedef boost::shared_ptr<ProducerConsumer<EditingSession, ResourceRef>> ProducerConsumerRef;

    class EditingSession
    {
        private:
        ActivityRef _activity;
        stringRef _filePath;
        ActivityLogRef _log;
        //ResourceRef Unit; store in session or log?

        bool pathChanged = true;
        bool contentChanged = false;
        ProducerConsumerRef _consumer;
        stringRef _imagePath;
        stringRef _fileUri;

        bool _endTime = false;
        bool _initialTransmit = true;
        stringRef CurrentTool;

        bool _initialized = false;

        void createActivity();
        stringRef createImageFilePath(time_t time);

        protected:

        stringRef getFileUri() { return _fileUri; }

		virtual ImageRef getDocument() = 0;
        virtual stringRef getDocumentFilePath() = 0;
        virtual stringRef getSoftwareAgent() = 0;
		virtual stringRef getSoftwareVersion() = 0;

        GenerationRef createGeneration() { return GenerationRef(new Generation()); }
        UndoRef createUndo() { return UndoRef(new Undo()); }
        RedoRef createRedo() { return RedoRef(new Redo()); }
        InvalidationRef createInvalidation() { return InvalidationRef(new Invalidation()); }
        DerivationRef createDerivation() { return DerivationRef(new Derivation()); }
        RevisionRef createRevision() { return RevisionRef(new Revision()); }

        virtual GenerationRef onEventAdd() = 0;
        virtual InvalidationRef onEventDelete() = 0;
        virtual RevisionRef onEventEdit() = 0;
        virtual UndoRef onEventUndo() = 0;
        virtual RedoRef onEventRedo() = 0;
        virtual RevisionRef onEventSave() = 0;
        virtual DerivationRef onEventSaveAs() = 0;

        public:
        EditingSession();
        void initialize(std::string server, bool isNewDocument);
        virtual ~EditingSession();

        void transmitQueue();

        bool safeToRemove() { return _endTime && _consumer->empty(); }

        void consume(ResourceRef data);

        void eventAdd();
        void eventDelete();
        void eventEdit();
        void eventUndo();
        void eventRedo();
        void eventSave();
        void eventSaveAs();

        bool fileExists(const std::string& name);

    };


}

#endif //_ART_EDITINGSESSION_H