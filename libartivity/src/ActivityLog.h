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
#include "curlresponse.h"

#include "Resource.h"
#include "Serializer.h"
#include "ObjectModel/Agent.h"
#include "ObjectModel/Association.h"
#include "ObjectModel/Activity.h"
#include "ObjectModel/Activities/CreateFile.h"
#include "ObjectModel/Activities/EditFile.h"
#include "ObjectModel/Entity.h"
#include "ObjectModel/Entities/FileDataObject.h"
#include "ObjectModel/Geometry/Canvas.h"

using namespace std;

namespace artivity
{
    typedef deque<Activity*>::iterator ActivityLogIterator;
    typedef list<Resource*>::iterator ResourceIterator;
    typedef list<Agent*>::iterator AgentIterator;
    
    class ActivityLog
    {
    protected:
        CURL* _curl;
        
        deque<Activity*> _activities;
    
        list<Resource*> _resources;
        
        list<Agent*> _agents;
        
        string _fileUri;
        
        string _filePath;
        
        string _canvasUri;
        
        double _canvasWidth;
        
        double _canvasHeight;
        
        const Resource* _canvasUnit;
        
        CURL* initializeRequest();
        
        long executeRequest(CURL* curl, string url, string postFields, string& response);
        
        void logError(CURLcode responseCode, string msg);
        
        void logInfo(CURLcode responseCode, string msg);
        
        string getTime();

        const char* getFileUri(string path);

        const char* getCanvasUri(string path);
        
        const char* getLatestVersionUri(string path);
        
        void createCanvas(FileDataObject* file, double width, double height, const Resource* unit);
        
        ResourceIterator findResource(const char* uri);
        
        // Add a referenced resource to the transmitted RDF output.
        void addResource(Resource* resource);
        
        // Remove a referenced resource to the transmitted RDF output.
        void removeResource(Resource* resource);
        
    public:
        ActivityLog();
        
        virtual ~ActivityLog();
        
        // Indicates if there is a connection to the Artivity HTTP API.
        bool connected();
        
        // Indicates if there are any activities in the log.
        bool empty();

        ActivityLogIterator begin();
        
        Activity* first();
        
        ActivityLogIterator end();
        
        Activity* last();
        
        void clear();

        // Create a new resource and add it the transmitted RDF output.
        template <class T> T* createResource()
        {
            return createResource<T>(UriGenerator::getUri().c_str());
        }
        
        // Create a new resource with a given URI ant add it to the transmitted RDF output.
        template <class T> T* createResource(const char* uri)
        {            
            T* t = new T(uri);
            
            addResource(t);
            
            return t;
        }
        
        template <class T> T* getResource(const char* uri)
        {
            ResourceIterator it = findResource(uri);
            
            if(it == _resources.end())
            {
                return NULL;
            }
            
            return dynamic_cast<T*>(*it);
        }
        
        bool hasResource(const char* uri);

        // Create a new activity and add to the transmitted RDF output.
        template <class T> T* createActivity()
        {
            time_t now;
            time(&now);
                
            T* activity = new T();
            activity->setStartTime(now);
            
            addActivity(activity);
            
            return activity;
        }
        
        template <class T> T* createEntityVersion(T* entity, Canvas* canvas)
        {
            T* version = createResource<T>();
            version->setCanvas(canvas);
            version->addGenericEntity(entity);
            
            return version;
        }
        
        template <class T> T* createEntityInfluence(time_t time, const Resource& type, Viewport* viewport)
        {
            T* influence = createResource<T>();
            influence->setType(type);
            influence->setTime(time);
            influence->setViewport(viewport);
            
            return influence;
        }
        
        Activity* updateActivity(Activity* activity);
        
        // Add an activity to the transmitted RDF output.
        void addActivity(Activity* activity);
        
        // Adds an associated agent to any activities which are being logged.
        void addAgent(Agent* agent);
        
        // Removes an associated agent to any activities which are being logged.
        void removeAgent(Agent* agent);
        
        bool hasFile(string path);
        
        EditFile* editFile(string path, double width, double height, const Resource* lengthUnit);
        
        CreateFile* createFile(double width, double height, const Resource* lengthUnit);
        
        FileDataObject* getFile();
                        
        bool hasCanvas(double width, double height, const Resource* lengthUnit);
        
        void updateCanvas(double width, double height, const Resource* lengthUnit);
        
        Canvas* getCanvas();
        
        // Send all items in the queue to the Artivity server.
        void transmit();
        
        void enableMonitoring(string path);
        
        void disableMonitoring(string path);
        
        string escapePath(string path);
    };
}

#endif // ACTIVITYLOG_H
