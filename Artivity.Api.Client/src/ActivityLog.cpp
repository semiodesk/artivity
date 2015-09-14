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
        resources = list<Resource*>();
    }

    ActivityLog::~ActivityLog() 
    {
        // Free all the activities still in the log.
        ActivityLogIterator ait = begin();
        
        while(ait != end())
        {
            delete *ait;
            
            ait++;
        }
        
        // Free all the related resources still in the log.
        ResourceIterator rit = resources.begin();
        
        while(rit != resources.end())
        {
            delete *rit;
            
            rit++;
        }
    }
    
    bool ActivityLog::isConnected()
    {
        // TODO: Implement CURL call to http://localhost:8890/artivity/1.0/activities
        return true;
    }
    
    void ActivityLog::setGeneratedEntity(Entity* entity)
    {
        // Add the entity to the RDF output if not already done.
        if(find(resources.begin(), resources.end(), entity) == resources.end())
        {
            addResource(entity);
        }
     
        // Add the entity to the generated entities of all activities.
        ActivityLogIterator ait = begin();
   
        while(ait != end())
        {
            (*ait)->addGeneratedEntity(entity);
            
            ait++;
        }
    }
    
    void ActivityLog::addResource(Resource* resource)
    {
        resources.push_back(resource);
    }
    
    void ActivityLog::removeResource(Resource* resource)
    {
        resources.remove(resource);
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
        
        ActivityLogIterator ait = begin();
        
        while(ait != end())
        {
            Serializer::serialize(stream, **ait, N3);
            
            delete *ait;
            
            ait++;
        }
        
        ResourceIterator rit = resources.begin();
        
        while(rit != resources.end())
        {
            Serializer::serialize(stream, **rit, N3);
            
            delete *rit;
            
            rit++;
        }
        
        resources.clear();
        
        string content = stream.str();
        
        curl_easy_setopt(curl, CURLOPT_POSTFIELDS, content.c_str());
        curl_easy_setopt(curl, CURLOPT_HTTPHEADER, headers);
        
        cout << endl;
        cout << ">>> OPEN: http://localhost:8272/artivity/1.0/model; Method: POST" << endl;
        cout << content << endl;
        
        curl_easy_perform(curl);
        
        if(curl)
        {
            curl_easy_cleanup(curl);
        }
        
        clear();
    }
}

