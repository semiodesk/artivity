#ifndef URIGENERATOR_H
#define URIGENERATOR_H

#include <stdlib.h>
#include <string>

namespace artivity
{
    using namespace std;
    
    class UriGenerator
    {        
        public:
    
            UriGenerator() {}
            ~UriGenerator() {}

            
            static string getUri()
            {
                return string("http://semiodesk.com/id/" + getRandomId(10));
            }
        
            static string getRandomId(size_t length)
            {
                static bool initialized;
                static struct random_data rand_buf;
                static char state[32];

                if( !initialized )
                   {
                       initstate_r(0, state, sizeof(state), &rand_buf); 
                       initialized = true;
                       srandom_r(time(NULL), &rand_buf);
                   }

                static const char alphanum[] = "0123456789abcdefghijklmnopqrstuvwxyz"; 
                
                string str = string();
                
                for(size_t i = 0; i < length; ++i)
                {
                    int res = 0;
                    int var = random_r(&rand_buf, &res);
                    str += alphanum[res % 25];
                }
                
                return str;
            }

    };
}

#endif // URIGENERATOR_H
