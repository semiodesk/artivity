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
#include "ObjectModel/Activities/CreateFile.h"
#include "ObjectModel/Activities/EditFile.h"
#include "ObjectModel/Entity.h"
#include "ObjectModel/Entities/FileDataObject.h"
#include "ObjectModel/Geometry/Canvas.h"


namespace artivity
{
    typedef std::list<ResourceRef> ResourceList;
    typedef ResourceList::iterator ResourceIterator;
    typedef std::list<AgentRef>::iterator AgentIterator;

    
    
    class ActivityLog
    {
    protected:
        CURL* _curl;
        
        ActivityRef _activities;
    
        ResourceList _resources;
        
        std::list<AgentRef> _agents;
        
        std::string _fileUri;
        
        std::string _filePath;
        
        std::string _canvasUri;
        
        double _canvasWidth;
        
        double _canvasHeight;
        
        ResourceRef _canvasUnit;
        
        CURL* initializeRequest();
        
        long executeRequest(CURL* curl, std::string url, std::string postFields, std::string& response);
        
        void logError(CURLcode responseCode, std::string msg);
        
        void logInfo(CURLcode responseCode, std::string msg);
        
        std::string getTime();

        std::string getFileUri(std::string path);

        std::string getCanvasUri(std::string path);
        
        string getLatestVersionUri(std::string path);
        
        void createCanvas(FileDataObjectRef file, double width, double height, ResourceRef unit);
        
        ResourceIterator findResource(const char* uri);
        
        std::string _server;
        const char* _uriAPI = "/artivity/1.0/uri";
        const char* _activityAPI = "/artivity/1.0/activities";
        const char* _monitorAPI = "/artivity/1.0/monitor";
        const char* _API = "/artivity/api/1.0";
        
    public:
        ActivityLog(string server);
        
        virtual ~ActivityLog();
        
        // Indicates if there is a connection to the Artivity HTTP API.
        bool connected();
        
        // Indicates if there are any activities in the log.
        bool empty() { if (_activities) return false; else return true; }
        
        void clear();

        // Create a new resource and add it the transmitted RDF output.
        template <typename T> boost::shared_ptr<T> createResource()
        {
            return createResource<T>(UriGenerator::getUri().c_str());
        }
        
        // Create a new resource with a given URI ant add it to the transmitted RDF output.
        template <typename T> boost::shared_ptr<T> createResource(const char* uri)
        {            
            boost::shared_ptr<T> t(new T(uri));
            
            addResource(t);
            
            return t;
        }
        
        template <typename T> boost::shared_ptr<T> getResource(const char* uri)
        {
            ResourceIterator it = findResource(uri);
            
            if(it == _resources.end())
            {
                return NULL;
            }
            
            return boost::dynamic_pointer_cast<T>(*it);
        }
        
        bool hasResource(const char* uri);

        // Create a new activity and add to the transmitted RDF output.
        template <typename T> boost::shared_ptr<T> createActivity()
        {
            time_t now;
            time(&now);
                
            boost::shared_ptr<T> activity = boost::shared_ptr<T>( new T() );
            activity->setStartTime(now);
            
            setActivity(activity);
            
            return activity;
        }
        
        template <typename T> boost::shared_ptr<T> createEntityVersion(boost::shared_ptr<T> entity, CanvasRef canvas)
        {
            boost::shared_ptr<T> version = createResource<T>();
            version->setCanvas(canvas);
            version->addGenericEntity(entity);
            
            return version;
        }
        
        template <typename T> boost::shared_ptr<T> createEntityInfluence(time_t time, ResourceRef type, ViewportRef viewport)
        {
            boost::shared_ptr<T> influence = createResource<T>();
            influence->setType(type);
            influence->setTime(time);
            influence->setViewport(viewport);
            
            return influence;
        }
        
        ActivityRef updateActivity(ActivityRef activity);
        
        // Add an activity to the transmitted RDF output.
        void setActivity(ActivityRef activity);

        // Remove an activity
        void resetActivity();
        
        // Adds an associated agent to any activities which are being logged.
        void addAgent(AgentRef agent);
        
        // Removes an associated agent to any activities which are being logged.
        void removeAgent(AgentRef agent);
        
        bool hasFile(string path);
        
        EditFileRef editFile(std::string path, double width, double height, ResourceRef lengthUnit);
        
        CreateFileRef createFile(double width, double height, ResourceRef lengthUnit);
        
        FileDataObjectRef getFile();

        string getFilePath() { return _filePath; }
                        
        bool hasCanvas(double width, double height, ResourceRef lengthUnit);

        void updateCanvas(double width, double height, ResourceRef lengthUnit);

        CanvasRef getCanvas();
        
        // Send all items in the queue to the Artivity server.
        void transmit();
        
        void enableMonitoring(std::string path, std::string uri);
        
        void disableMonitoring(std::string path);
        
        std::string escapePath(std::string path);

        std::string getThumbnailPath(std::string uri);

        // Add a referenced resource to the transmitted RDF output.
        void addResource(ResourceRef resource);

        // Remove a referenced resource to the transmitted RDF output.
        void removeResource(ResourceRef resource);

    };
}

#endif // ACTIVITYLOG_H
