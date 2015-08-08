#include <exception>
#include <ctime>
#include <string>
#include <typeinfo>

#include "Ontologies/xsd.h"
#include "Serializer.h"

using namespace artivity::client::ontologies;

namespace artivity
{
    namespace client
    {
        XsdTypeMap Serializer::TYPE_MAP = XsdTypeMap();
      
        string Serializer::toString(const Resource& value)
        {            
            return value.Uri;
        }
        
        string Serializer::toString(const char* value)
        {           
            return string(value);
        }
        
        string Serializer::toString(int value)
        {                       
            stringstream s;
                        
            s << value;
   
            return s.str();
        }
        
        string Serializer::toString(long value)
        {           
            stringstream s;
        
            s << value;
            
            return s.str();
        }
        
        string Serializer::toString(float value)
        {           
            stringstream s;
        
            s << value;
            
            return s.str();
        }
        
        string Serializer::toString(double value)
        {           
            stringstream s;
        
            s << value;
            
            return s.str();
        }
        
        string Serializer::toString(const time_t* value)
        {
            char buf[21];
            
            strftime(buf, 21, "%Y-%m-%dT%H:%M:%SZ", gmtime(value));
            
            return string(buf);
        }
        
        string Serializer::serialize(Resource& resource, RdfSerializationFormat format)
        {
            stringstream s;
            
            serialize(s, resource, format);
            
            return s.str();
        }
        
        stringstream& Serializer::serialize(stringstream& out, Resource& resource, RdfSerializationFormat format)
        {
            if(format == N3)
            {               
                out << resource;
                
                PropertyIterator it = resource.Properties.begin();
                
                while(it != resource.Properties.end())
                {
                    if(it != resource.Properties.begin())
                    {
                        out << ";" << endl;
                    }
                    
                    string property = it->first;
                    
                    out << " <" << property << "> " << flush;
                    
                    PropertyValue x = it->second;
                    
                    if(x.Value != NULL)
                    {                        
                        out << "<" << x.Value->Uri << ">";
                    }
                    else if(x.LiteralType != NULL)
                    {
                        out << "\"" << x.LiteralValue << "\"";
                        
                        XsdTypeMapIterator it = TYPE_MAP.find(x.LiteralType);
                        
                        if(it != TYPE_MAP.end())
                        {
                            out << "^^" << it->second;
                        }
                    }
                    
                    it++;
                }
                
                out << "." << endl;
                
                return out;
            }
            
            throw exception();
        }
    }
}

