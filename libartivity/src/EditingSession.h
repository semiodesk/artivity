#ifndef _ART_EDITINGSESSION_H
#define _ART_EDITINGSESSION_H

#include "defines.h"
#include "ActivityLog.h"
#include "ProducerConsumer.h"
#include "ObjectModel/Geometry/Canvas.h"
#include "ObjectModel/Change.h"
#include "ObjectModel/Influences/Start.h"
#include "ObjectModel/Influences/End.h"
#include "ObjectModel/Influences/Undo.h"
#include "ObjectModel/Influences/Redo.h"
#include "ObjectModel/Influences/Revision.h"
#include "ObjectModel/Influences/Invalidation.h"
#include "ObjectModel/Influences/Generation.h"
#include "ObjectModel/Influences/Derivation.h"
#include "ObjectModel/Influences/Save.h"
#include "ObjectModel/Influences/SaveAs.h"

namespace artivity
{
    class EditingSession;

    typedef boost::shared_ptr< ProducerConsumer<EditingSession, ResourceRef> > ProducerConsumerRef;

    class EditingSession
    {
    private:
        ActivityRef _activity;

        ActivityLogRef _log;

        //ResourceRef Unit; store in session or log?

        bool pathChanged;

        bool contentChanged;

        std::string _imagePath;

        std::string _filePath;

        std::string _fileUri;

        bool _endTime;

        bool _initial;

        bool _initialized;

        std::vector<std::string> _changes;

        int _currentChangeIndex; 

        void createActivity();

    protected:
        ProducerConsumerRef consumer;

        void handleChanges(ResourceRef res);

        ImageRef document;

        std::string createThumbnailFilePath();
        std::string createImageFilePath(time_t time, int count);
        std::string createImageFilePath(time_t time, std::string suffix);
        std::string createImageFilePath(time_t time, std::string suffix, int count);
        std::string getImageFilePath() { return _imagePath; }
        std::string getFileUri() { return _fileUri; }

        virtual ImageRef getDocument() = 0;
        virtual std::string getDocumentFilePath() = 0;
        virtual std::string getSoftwareAgent() = 0;
        virtual std::string getSoftwareAgentVersion() = 0;

        CanvasRef createCanvas(std::string uri, time_t time, double x, double y, double width, double height);
        CanvasRef updateCanvas(std::string uri, time_t time, double x, double y, double width, double height);

        GenerationRef createGeneration() { return GenerationRef(new Generation()); }
        UndoRef createUndo() { return UndoRef(new Undo()); }
        RedoRef createRedo() { return RedoRef(new Redo()); }
        InvalidationRef createInvalidation() { return InvalidationRef(new Invalidation()); }
        DerivationRef createDerivation() { return DerivationRef(new Derivation()); }
        RevisionRef createRevision() { return RevisionRef(new Revision()); }
        ChangeRef createChange() { return ChangeRef(new Change()); }

        virtual StartRef onEventStart() = 0;
        virtual EndRef onEventEnd() = 0;
        virtual GenerationRef onEventAdd() = 0;
        virtual InvalidationRef onEventDelete() = 0;
        virtual RevisionRef onEventEdit() = 0;
        virtual UndoRef onEventUndo() = 0;
        virtual RedoRef onEventRedo() = 0;
        virtual RevisionRef onEventSave();
        virtual DerivationRef onEventSaveAs();

    public:
        EditingSession();
        virtual ~EditingSession();

        void initialize(std::string server, bool isNewDocument);
        void finalize();

        void transmitQueue();

        bool safeToRemove() { return consumer != NULL && consumer->empty(); }

        void consume(ResourceRef data);

        void eventStart();
        void eventEnd();
        void eventAdd();
        void eventDelete();
        void eventEdit();
        void eventUndo();
        void eventRedo();
        void eventSave();
        void eventSaveAs(std::string targetPath);

        bool fileExists(const std::string& name);
    };
}

#endif //_ART_EDITINGSESSION_H
