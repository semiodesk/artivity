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
#include "ObjectModel/Generation.h"
#include "ObjectModel/Invalidation.h"
#include "ObjectModel/Revision.h"
#include "ObjectModel/Entities/FileDataObject.h"
#include "ObjectModel/Entities/Image.h"

namespace artivity
{
    typedef std::list<AssociationRef>::iterator AssociationIterator;

    class ActivityLog;

    typedef boost::shared_ptr<ActivityLog> ActivityLogRef;

	class ActivityLog
    {
        protected:
        std::string _endpointUrl;

        CURL* _curl;
            
        ActivityRef _activity;
            
		ImageRef _entity;

		std::list<AssociationRef> _associations;
        
        std::string _fileUrl;
        
		bool fetchAssociationUri(AssociationRef association);

        CURL* initializeRequest();
        
		long executeRequest(CURL* curl, std::string url, std::string postFields, std::string& response);

		std::string getTime();

		std::string getEntityUri(std::string path);

		std::string escapePath(std::string path);

		void dump(boost::property_tree::ptree const& pt);

        public:
        bool debug;

        ActivityLog();

        virtual ~ActivityLog();

		bool connect(std::string endpointUrl);

		bool ready();

		bool empty() { return _activity->empty(); }
        
		void clear() { _activity->clear(); }
        
		void close();

		void close(time_t time);

		// Send all items in the queue to the Artivity server.
		void transmit();

		ActivityRef getActivity() { return _activity; }

		// Set the file being edited.
		void setDocument(ImageRef image, std::string path);

        ImageRef getDocument();

        void addAssociation(const char* roleUri);
		void addAssociation(const char* roleUri, const char* agentUri, const char* version);
		void addAssociation(const char* roleUri, std::string agentUri, std::string version);
		// Add an entity influence to the transmitted RDF stream.
		void addInfluence(GenerationRef generation);
		void addInfluence(InvalidationRef invalidation);
		void addInfluence(EntityInfluenceRef influence);

		// Remove an entity influence to the transmitted RDF stream.
		void removeInfluence(GenerationRef generation);
		void removeInfluence(InvalidationRef invalidation);
		void removeInfluence(EntityInfluenceRef influence);

		std::string getRenderOutputPath();

#if _DEBUG
		void logError(std::string msg);
		void logInfo(std::string msg);
        void logRequest(std::string url, std::string time, std::string data);
#endif
    };
}

#endif // _ART_ACTIVITYLOG_H
