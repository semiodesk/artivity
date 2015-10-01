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

#ifndef ACTIVITYLOG_H
#define ACTIVITYLOG_H

#include <string>
#include <algorithm>
#include <deque>
#include <vector>
#include <curl/curl.h>

#include "Resource.h"
#include "Serializer.h"
#include "ObjectModel/Activity.h"
#include "ObjectModel/Entity.h"
#include "ObjectModel/Agent.h"
#include "ObjectModel/Association.h"

using namespace std;

namespace artivity
{
    typedef deque<Activity*>::iterator ActivityLogIterator;
    typedef list<Resource*>::iterator ResourceIterator;
    typedef list<Agent*>::iterator AgentIterator;
    
    class ActivityLog
    {
    protected:
        deque<Activity*> _activities;
    
        list<Resource*> _resources;
        
        list<Agent*> _agents;
        
    public:
        ActivityLog();
        virtual ~ActivityLog();
        
        // Indicates if there is a connection to the Artivity HTTP API.
        bool connected();
        
        // Indicates if there are any activities in the log.
        bool empty();

        ActivityLogIterator begin();
        
        ActivityLogIterator end();
        
        void clear();

        // Adds an activity to the transmitted RDF output.
        Activity* createActivity(const Resource& type);
        
        // Adds an activity to the transmitted RDF output.
        void addActivity(Activity* activity);
        
        // Add a referenced resource to the transmitted RDF output.
        void addResource(Resource* resource);
        
        // Remove a referenced resource to the transmitted RDF output.
        void removeResource(Resource* resource);
        
        // Adds an associated agent to any activities which are being logged.
        void addAgent(Agent* agent);
        
        // Removes an associated agent to any activities which are being logged.
        void removeAgent(Agent* agent);
        
        // Set a target on all activites in the queue.
        void setGeneratedEntity(Entity* entity);
        
        // Send all items in the queue to the Artivity server.
        void transmit();
    };
}

#endif // ACTIVITYLOG_H
