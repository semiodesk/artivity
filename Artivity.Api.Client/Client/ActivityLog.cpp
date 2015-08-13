#include "ActivityLog.h"
#include "Serializer.h"

#include <curl/curl.h>
#include <string>

namespace artivity
{
    ActivityLog::ActivityLog() {}

    ActivityLog::~ActivityLog() {}
    
    bool ActivityLog::isConnected()
    {            
        return true;
    }
    
    void ActivityLog::setInstrument(Resource& instrument)
    {
        ActivityLogIterator it = begin();
        
        while(it != end())
        {
            it->setInstrument(instrument);
            
            it++;
        }
    }
    
    void ActivityLog::setTarget(Resource& target)
    {
        ActivityLogIterator it = begin();
        
        while(it != end())
        {
            it->setTarget(target);
            
            it++;
        }
    }
    
    void ActivityLog::transmit()
    {
        // TODO: Send the N3 serialized content to http://localhost:8272/artivitiy/1.0/model
        
        
        CURL *curl;
        
        struct curl_slist *headers = NULL; // init to NULL is important 
        headers = curl_slist_append(headers, "Accept: text/n3");
        headers = curl_slist_append(headers, "Content-Type: text/n3");
        headers = curl_slist_append(headers, "charsets: utf-8");

        curl = curl_easy_init();
        
        if (!curl) return;
        
        curl_easy_setopt(curl, CURLOPT_URL, "http://localhost:8272/artivity/1.0/model");
        
        ActivityLogIterator it = begin();
        stringstream content;
        while(it != end())
        {
            Serializer::serialize(content, *it, N3);
            
            it++;
        }
        
        curl_easy_setopt(curl, CURLOPT_POSTFIELDS, content.str().c_str());
        curl_easy_setopt(curl, CURLOPT_HTTPHEADER, headers);
         cout << ">>> OPEN: http://localhost:8272/artivity/1.0/model; Method: POST" << endl;
        cout << ">>> SEND:" << endl;
        cout << content.str() << endl;
        
        CURLcode res = curl_easy_perform(curl);
        cout << res << endl;
        // always cleanup
        curl_easy_cleanup(curl);
        
        
        cout << ">>> DONE." << endl;
    }
}

