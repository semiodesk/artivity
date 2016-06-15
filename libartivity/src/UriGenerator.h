#ifndef URIGENERATOR_H
#define URIGENERATOR_H

#include <stdlib.h>
#include <string>

namespace artivity
{

    class UriGenerator
    {        
        public:
    
            UriGenerator() {}
            ~UriGenerator() {}

            
            static std::string getUri()
            {
                return std::string("http://semiodesk.com/id/" + getRandomId(10));
            }
        
            static std::string getRandomId(unsigned long length);
    };
}

#endif // URIGENERATOR_H
