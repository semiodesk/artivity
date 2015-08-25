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
            return string("http://semiodesk.com/artivity/1.0/id/" + getRandomId(10));
        }
        
    private:
    
        static string getRandomId(size_t length)
        {
            static const char alphanum[] = "0123456789abcdefghijklmnopqrstuvwxyz"; 
            
            string str = string();
            
            for(size_t i = 0; i < length; ++i)
            {
                str += alphanum[rand() % 25];
            }
            
            return str;
        }
    };
}

#endif // URIGENERATOR_H
