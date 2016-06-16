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

#include <string>
#include <algorithm>
#include <deque>
#include <vector>
#include <curl/curl.h>
#include <boost/shared_ptr.hpp>

#include "Resource.h"
#include "Serializer.h"
#include "ObjectModel/Agent.h"
#include "ObjectModel/Association.h"
#include "ObjectModel/Activity.h"
#include "ObjectModel/Activities/CreateFile.h";
#include "ObjectModel/Activities/EditFile.h";
#include "ObjectModel/Entity.h"
#include "ObjectModel/EntityInfluence.h"
#include "ObjectModel/Entities/FileDataObject.h"

namespace artivity
{
	typedef list<AssociationRef>::iterator AssociationIterator;

	typedef list<EntityInfluenceRef>::iterator EntityInfluenceIterator;

    class ActivityLog
    {
    protected:
		string _endpoint;

        CURL* _curl;
        
        ActivityRef _activity;
            
        list<AssociationRef>* _associations;
    
		list<EntityInfluenceRef>* _influences;

        string _fileUri;
        
        string _fileUrl;
        
        CURL* initializeRequest();
        
        long executeRequest(CURL* curl, string url, string postFields, string& response);
        
        void logError(CURLcode responseCode, string msg);
        
        void logInfo(CURLcode responseCode, string msg);
        
        string getTime();
        
		string escapePath(string path);

    public:
		ActivityLog(string endpoint);
        
        virtual ~ActivityLog();
        
        // Indicates if there is a connection to the Artivity HTTP API.
        bool connect();

		void initialize();
        
        // Indicates if there are any entity influences in the log.
        bool empty() { return _influences->empty(); }
        
		// Clears all untransmitted entity influences in the log.
		void clear() { _influences->clear(); }
        
		// Send all items in the queue to the Artivity server.
		void transmit();

		ActivityRef getActivity() { return _activity; }

		string getThumbnailPath(string fileUrl);

		// Add an entity influence to the transmitted RDF stream.
		void addInfluence(EntityInfluenceRef resource);

		// Remove an entity influence to the transmitted RDF stream.
		void removeInfluence(EntityInfluenceRef resource);

		//void addGeneration(GenerationRef generation);

		//void addUsage(UsageRef usage);
    };
}

#endif // _ART_ACTIVITYLOG_H
