// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

#ifndef _ART_ACTIVITYLOG_H
#define _ART_ACTIVITYLOG_H

#include <iostream>
#include <sstream>
#include <string>
#include <algorithm>
#include <deque>
#include <vector>
#include <curl/curl.h>
#include <boost/shared_ptr.hpp>
#include <boost/format.hpp>
#include <boost/filesystem.hpp>
#include <boost/chrono.hpp>
#include <boost/property_tree/ptree.hpp>
#include <boost/property_tree/json_parser.hpp>

#include "defines.h"
#include "Resource.h"
#include "Serializer.h"
#include "ObjectModel/Agent.h"
#include "ObjectModel/Association.h"
#include "ObjectModel/SoftwareAssociation.h"
#include "ObjectModel/Activity.h"
#include "ObjectModel/Activities/CreateFile.h"
#include "ObjectModel/Activities/EditFile.h"
#include "ObjectModel/Entity.h"
#include "ObjectModel/Influences/Generation.h"
#include "ObjectModel/Influences/Invalidation.h"
#include "ObjectModel/Influences/Revision.h"
#include "ObjectModel/Influences/Undo.h"
#include "ObjectModel/Influences/Redo.h"
#include "ObjectModel/FileDataObject.h"
#include "ObjectModel/Entities/Image.h"

namespace artivity
{
    typedef std::list<InfluenceRef>::iterator InfluenceIterator;

    class ActivityLog;

    typedef boost::shared_ptr<ActivityLog> ActivityLogRef;

	class ActivityLog
    {
    protected:
        std::string _endpointUrl;

        CURL* _curl;
            
        ActivityRef _activity;

        ImageRef _entity;

        // Contains a handle to the previous revision of the image
        ImageRef _prevEntity;

		std::vector<AssociationRef> _associations;

        // Influences which need to be transmitted separatly, because they have 
        // no relation with either the activity or an entity. Currently this
        // is undo and redos.
        std::list<InfluenceRef> _influences;

        std::string _fileUrl;

        CURL* initializeRequest();
        
		long executeRequest(CURL* curl, std::string url, std::string postFields, std::string& response);

		std::string getTime();

        bool fetchInitialData(std::string fileUrl, std::string* latestEntityUri, std::string* fileDataObjectUri, std::string* renderOutputPath, std::string* userAssociationUri);

		void dump(boost::property_tree::ptree const& pt);

		int _transmitCount;

		bool _hasDataObject;

        std::string _renderOutputPath;

    public:
        bool debug;

        ActivityLog();

        virtual ~ActivityLog();

		bool connect(std::string endpointUrl);

		bool ready();

		bool empty() { return _activity->empty(); }
        
		void clear()
        {
            _activity->clear();
            _influences.clear();
            _entity->clearInfluences();
        }
        
		void close();

		void close(time_t time);

		// Send all items in the queue to the Artivity server.
		void transmit();

        void save();

		ActivityRef getActivity() { return _activity; }

		// Set the file being edited.
		void setDocument(ImageRef image, std::string path, bool create);
        
        void populateRevision(RevisionRef revision);

        void createDerivation(DerivationRef saveAs, ImageRef targetImage, std::string targetPath);

        ImageRef getDocument();

		bool hasDataObject() { return _hasDataObject; }

        bool fetchNewDataObject(std::string path);

		void addAssociation(const char* roleUri, const char* agentUri, const char* version);
        void addAssociation(std::string roleUri, std::string agentUri, std::string version);

		// Add an entity influence to the transmitted RDF stream.
		void addInfluence(InfluenceRef influence);

		// Remove an entity influence to the transmitted RDF stream.
        void removeInfluence(InfluenceRef influence);

		std::string getRenderOutputPath();

#ifdef _DEBUG
		void logError(std::string msg);
		void logInfo(std::string msg);
        void logRequest(std::string url, std::string time, std::string data);
#endif
    };
}

#endif // _ART_ACTIVITYLOG_H
