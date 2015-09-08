#include <iostream>

#include "Activity.h"
#include "ActivityLog.h"

#include "Activities/Create.h"
#include "Activities/Update.h"

#include "Entities/FileDataObject.h"

#include "Ontologies/prov.h"
#include "Ontologies/rdf.h"

using namespace artivity;

int main(int argc, char **argv)
{
    setlocale(LC_ALL, "");
     
    // Initialize the log on application startup.
    ActivityLog log = ActivityLog();
         
    time_t now;
    time(&now);
     
    Create a0 = "http://localhost:8890/artivity/1.0/activities#0";
    a0.setTime(now);
    
    log.push_back(a0);
    
    time(&now);
        
    Update a1 = "http://localhost:8890/artivity/1.0/activities#1";
    a1.setTime(now);
    
    log.push_back(a1);
    
    time(&now);
        
    Update a2 = "http://localhost:8890/artivity/1.0/activities#2";
    a2.setTime(now);
    
    log.push_back(a2);
    
    if(log.isConnected())
    {
        FileDataObject file = "file://example.svg";
        
        log.setGeneratedEntity(&file);
        
        log.transmit();
    }
    
    return 0;
}
