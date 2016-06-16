// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

#include <exception>
#include <ctime>
#include <string>
#include <typeinfo>

#include "Ontologies/xsd.h"
#include "Serializer.h"

namespace artivity
{
	using namespace std;

    string Serializer::toString(ResourceRef value)
    {            
        return value->Uri;
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

		#ifdef WIN32
		tm* time = new tm();
		gmtime_s(time, value);
		strftime(buf, 21, "%Y-%m-%dT%H:%M:%SZ", time);
		#else
		strftime(buf, 21, "%Y-%m-%dT%H:%M:%SZ", gmtime(value));
		#endif

        return string(buf);
    }
    
    string Serializer::serialize(ResourceRef resource, RdfSerializationFormat format)
    {
        stringstream s;
        
        serialize(s, resource, format);
        
        return s.str();
    }
    
    stringstream& Serializer::serialize(stringstream& out, ResourceRef resource, RdfSerializationFormat format)
    {
        if(format == N3)
        {
			if (resource->Properties.size() == 0)
			{
				return out;
			}

            out << resource;
            
            PropertyMapIterator it = resource->Properties.begin();
            
            while(it != resource->Properties.end())
            {
                if(it != resource->Properties.begin())
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
                    if (x.LiteralType == typeid(Resource).name())
                    {
                        out << "<" << x.LiteralValue << ">";
                    }
                    else
                    {
                        out << "\"" << x.LiteralValue << "\"";

                        XsdTypeMapIterator it = TYPE_MAP.find(x.LiteralType);

                        if (it != TYPE_MAP.end())
                        {
                            out << "^^<" << it->second << ">";
                        }
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

