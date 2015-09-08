#include "ActivityLog.h"
#include "Serializer.h"

#include <string>

namespace artivity
{
    ActivityLog::ActivityLog() 
    {
        annotations = std::vector<Resource*>();
    }

    ActivityLog::~ActivityLog() 
    {
        vector<Resource*>::iterator anoIt = annotations.begin();
        
        while(anoIt != annotations.end() )
        {
            delete *anoIt;
            anoIt++;
        }
    }
    
    bool ActivityLog::isConnected()
    {            
        return true;
    }
    
    void ActivityLog::setGeneratedEntity(Entity* entity)
    {
        ActivityLogIterator it = begin();
        
        while(it != end())
        {
            it->addGeneratedEntity(entity);
            
            it++;
        }
    }
    
    void ActivityLog::addAnnotation(Resource* resource)
    {
        annotations.push_back(resource);
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
        curl_easy_setopt(curl, CURLOPT_URL, "http://localhost:8272/artivity/1.0/model");
        
        stringstream stream;
        
        ActivityLogIterator it = begin();
        
        while(it != end())
        {
            Serializer::serialize(stream, *it, N3);
            
            it++;
        }
        
        vector<Resource*>::iterator anoIt = annotations.begin();
        
        while(anoIt != annotations.end() )
        {
            Serializer::serialize(stream, **anoIt, N3);
            
            delete *anoIt;
            
            anoIt++;
        }
        
        annotations.clear();
        
        string content = stream.str();
        
        curl_easy_setopt(curl, CURLOPT_POSTFIELDS, content.c_str());
        curl_easy_setopt(curl, CURLOPT_HTTPHEADER, headers);
        
        cout << endl;
        cout << ">>> OPEN: http://localhost:8272/artivity/1.0/model; Method: POST" << endl;
        cout << content << endl;
        
        curl_easy_perform(curl);
        
        // always cleanup
        if(curl)
        {
            curl_easy_cleanup(curl);
        }
        
        clear();
    }
}

