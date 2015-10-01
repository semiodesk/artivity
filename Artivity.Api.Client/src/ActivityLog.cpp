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

#include "ActivityLog.h"

namespace artivity
{
    ActivityLog::ActivityLog() 
    {
        _activities = deque<Activity*>();
        _resources = list<Resource*>();
    }

    ActivityLog::~ActivityLog() 
    {
        // Free all the activities still in the log.
        ActivityLogIterator ait = _activities.begin();
        
        while(ait != _activities.end())
        {
            delete *ait;
            
            ait++;
        }
        
        // Free all the related _resources still in the log.
        ResourceIterator rit = _resources.begin();
        
        while(rit != _resources.end())
        {
            delete *rit;
            
            rit++;
        }
        
        // Free all the associated agents still in the log.
        AgentIterator agit = _agents.begin();
        
        while(agit != _agents.end())
        {
            delete *agit;
            
            agit++;
        }
    }
    
    bool ActivityLog::connected()
    {
        // TODO: Implement CURL call to http://localhost:8890/artivity/1.0/activities
        return true;
    }
    
    bool ActivityLog::empty()
    {
        return _activities.empty();
    }
    
    ActivityLogIterator ActivityLog::begin()
    {
        return _activities.begin();
    }
    
    ActivityLogIterator ActivityLog::end()
    {
        return _activities.end();
    }
    
    void ActivityLog::clear()
    {
        ActivityLogIterator ait = _activities.begin();
        
        while(ait != _activities.end())
        {
            delete *ait;
            
            ait++;
        }
        
        _activities.clear();
        
        ResourceIterator rit = _resources.begin();
        
        while(rit != _resources.end())
        {           
            delete *rit;
            
            rit++;
        }
        
        _resources.clear();
    }
    
    void ActivityLog::setGeneratedEntity(Entity* entity)
    {
        // Add the entity to the RDF output if not already done.
        if(find(_resources.begin(), _resources.end(), entity) == _resources.end())
        {
            addResource(entity);
        }
     
        // Add the entity to the generated entities of all activities.
        ActivityLogIterator ait = _activities.begin();
   
        while(ait != _activities.end())
        {
            (*ait)->addGeneratedEntity(entity);
            
            ait++;
        }
    }
    
    Activity* ActivityLog::createActivity(const Resource& type)
    {
        time_t now;
        time(&now);
            
        Activity* activity = new Activity();
        activity->setValue(rdf::_type, type);
        activity->setTime(now);
        
        addActivity(activity);
        
        return activity;
    }
    
    void ActivityLog::addActivity(Activity* activity)
    {
        _activities.push_back(activity);
             
        AgentIterator agit = _agents.begin();
        
        while(agit != _agents.end())
        {
            Association* association = new Association();
            association->setAgent(*agit);
            
            addResource(association);
            
            activity->addAssociation(association);
            
            agit++;
        }
    }
    
    void ActivityLog::addAgent(Agent* agent)
    {
        _resources.push_back(agent);
    }
    
    void ActivityLog::removeAgent(Agent* agent)
    {
        _resources.remove(agent);
    }
    
    void ActivityLog::addResource(Resource* resource)
    {
        _resources.push_back(resource);
    }
    
    void ActivityLog::removeResource(Resource* resource)
    {
        _resources.remove(resource);
    }
    
    void ActivityLog::transmit()
    {
        CURL* curl = curl_easy_init();

        if (!curl)
        {
            cout << ">>> ERROR: Failed to initialize CURL." << endl << flush;
            
            return;
        }
        
        struct curl_slist *headers = NULL; // init to NULL is important 
        headers = curl_slist_append(headers, "Accept: text/n3");
        headers = curl_slist_append(headers, "Content-Type: text/n3");
        headers = curl_slist_append(headers, "charsets: utf-8");
        
        curl_easy_reset(curl);
        curl_easy_setopt(curl, CURLOPT_URL, "http://localhost:8272/artivity/1.0/activities");
        
        stringstream stream;
        
        ActivityLogIterator ait = _activities.begin();
        
        while(ait != _activities.end())
        {            
            Serializer::serialize(stream, **ait, N3);
            
            ait++;
        }
        
        ResourceIterator rit = _resources.begin();
        
        while(rit != _resources.end())
        {
            Serializer::serialize(stream, **rit, N3);
            
            rit++;
        }
        
        _resources.clear();
        
        string content = stream.str();
        
        curl_easy_setopt(curl, CURLOPT_POSTFIELDS, content.c_str());
        curl_easy_setopt(curl, CURLOPT_HTTPHEADER, headers);
        
        cout << endl;
        cout << ">>> OPEN: http://localhost:8272/artivity/1.0/model; Method: POST" << endl;
        cout << content << endl;
        
        curl_easy_perform(curl);
        
        // Cleanup AFTER all _resources have been serialized and transmitted.
        if(curl)
        {
            curl_easy_cleanup(curl);
        }
        
        clear();
    }
}

