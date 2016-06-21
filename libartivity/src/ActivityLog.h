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
#include <boost/property_tree/ptree.hpp>
#include <boost/property_tree/json_parser.hpp>

#include "defines.h"
#include "Resource.h"
#include "Serializer.h"
#include "ObjectModel/Agent.h"
#include "ObjectModel/Association.h"
#include "ObjectModel/Activity.h"
#include "ObjectModel/Activities/CreateFile.h"
#include "ObjectModel/Activities/EditFile.h"
#include "ObjectModel/Entity.h"
#include "ObjectModel/EntityInfluence.h"
#include "ObjectModel/Entities/FileDataObject.h"
#include "ObjectModel/Entities/Image.h"

namespace artivity
{
    typedef std::list<AssociationRef>::iterator AssociationIterator;
    typedef std::list<InfluenceRef>::iterator InfluenceIterator;

    class ActivityLog;

    typedef boost::shared_ptr<ActivityLog> ActivityLogRef;

	class ActivityLog
    {
    protected:
        std::string _endpoint;

        CURL* _curl;
        
        ActivityRef _activity;
            
		EntityRef _entity;

        std::list<AssociationRef>* _associations;
    
        std::list<InfluenceRef>* _influences;
        
        std::string _fileUrl;
        
        CURL* initializeRequest();
        
		long executeRequest(CURL* curl, std::string url, std::string postFields, std::string& response);
        
		void logError(CURLcode responseCode, std::string msg);
        
		void logInfo(CURLcode responseCode, std::string msg);
        
		std::string getTime();
        
		std::string escapePath(std::string path);

		AssociationRef getAssociation(const char* role, std::string uri = "", std::string version = "");

		void print(boost::property_tree::ptree const& pt);

		std::string getEntityUri(std::string path);

    public:
		ActivityLog();

        ActivityLog(const char* endpointUrl);

		ActivityLog(std::string endpointUrl);

        virtual ~ActivityLog();

		void connect(const char* endpointUrl);

		bool ready();

        // Indicates if there are any entity influences in the log.
        bool empty() { return _influences->empty(); }
        
		// Clears all untransmitted entity influences in the log.
		void clear() { _influences->clear(); }
        
		// Send all items in the queue to the Artivity server.
		void transmit();

		ActivityRef getActivity() { return _activity; }

		// Indicates if there is a connection to the Artivity HTTP API.
		bool setAgent(std::string agentUri, std::string version);

		// Set the file being edited.
        void setFile(ImageRef image, std::string path);

        std::string getFileUri();

		// Add an entity influence to the transmitted RDF stream.
		void addInfluence(InfluenceRef influence);

		// Remove an entity influence to the transmitted RDF stream.
		void removeInfluence(InfluenceRef resource);

        void close(time_t time) {} // close the activity here, if no prov:Revision with entity file is in influence list clear Influences first

		std::string getRenderOutputPath();
    };
}

#endif // _ART_ACTIVITYLOG_H
