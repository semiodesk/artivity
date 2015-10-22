#include <iostream>

#include "artivity.h"

using namespace artivity;

int main(int argc, char **argv)
{
    setlocale(LC_ALL, "");
     
    // Initialize the log on application startup.
    ActivityLog log = ActivityLog();
     
    CreateFile* create = log.createActivity<CreateFile>();
    
    if(log.connected())
    {
        FileDataObject* file = log.createResource<FileDataObject>("file://example.svg");
        log.transmit();
    }
    
    return 0;
}
