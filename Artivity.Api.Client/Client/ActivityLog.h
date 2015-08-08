#ifndef ACTIVITYLOG_H
#define ACTIVITYLOG_H

#include <deque>

#include "Activity.h"

using namespace std;

namespace artivity
{
    namespace client
    {
        typedef deque<Activity>::iterator ActivityLogIterator;
        
        class ActivityLog : public deque<Activity>
        {
        public:
            ActivityLog();
            ~ActivityLog();
            
            // Indicates if there is a connection to the Artivity HTTP API.
            bool isConnected();
            
            // Set an instrument on all activites in the queue.
            void setInstrument(Resource& instrument);
            
            // Set a target on all activites in the queue.
            void setTarget(Resource& object);
            
            // Send all items in the queue to the Artivity server.
            void transmit();
        };
    }
}

#endif // ACTIVITYLOG_H
