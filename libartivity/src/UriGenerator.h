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
        
			static string getRandomId(size_t length);
    };
}

#endif // URIGENERATOR_H
