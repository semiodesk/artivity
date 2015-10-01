#include <iostream>

#include "artivity.h"

using namespace artivity;

int main(int argc, char **argv)
{
    setlocale(LC_ALL, "");
     
    // Initialize the log on application startup.
    ActivityLog log = ActivityLog();
         
    time_t now;
    time(&now);
     
    Create* a0 = new Create();
    a0->setTime(now);
    
    log.addActivity(a0);
    
    time(&now);
        
    Update* a1 = new Update();
    a1->setTime(now);
    
    log.addActivity(a1);
    
    time(&now);
        
    Update* a2 = new Update();
    a2->setTime(now);
    
    log.addActivity(a2);
    
    if(log.connected())
    {
        FileDataObject* file = new FileDataObject("file://example.svg");
        
        log.setGeneratedEntity(file);
        log.transmit();
    }
    
    return 0;
}
