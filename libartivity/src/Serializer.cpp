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

#include "Ontologies/art.h"
#include "Ontologies/dces.h"
#include "Ontologies/nfo.h"
#include "Ontologies/nie.h"
#include "Ontologies/prov.h"
#include "Ontologies/rdf.h"
#include "Ontologies/rdfs.h"
#include "Ontologies/svg.h"
#include "Ontologies/xml.h"
#include "Ontologies/xsd.h"
#include "Serializer.h"

namespace artivity
{
	using namespace std;

	Serializer::Serializer()
	{
		PREFIX_MAP[art::NS_PREFIX] = art::NS_URI;
		PREFIX_MAP[dces::NS_PREFIX] = dces::NS_URI;
		PREFIX_MAP[nfo::NS_PREFIX] = nfo::NS_URI;
		PREFIX_MAP[nie::NS_PREFIX] = nie::NS_URI;
		PREFIX_MAP[prov::NS_PREFIX] = prov::NS_URI;
		PREFIX_MAP[rdf::NS_PREFIX] = rdf::NS_URI;
        PREFIX_MAP[rdfs::NS_PREFIX] = rdfs::NS_URI;
		PREFIX_MAP[svg::NS_PREFIX] = svg::NS_URI;
		PREFIX_MAP[xml::NS_PREFIX] = xml::NS_URI;
		PREFIX_MAP[xsd::NS_PREFIX] = xsd::NS_URI;
	}

    string Serializer::toString(ResourceRef value)
    {            
        return value->uri;
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
		if (format == N3)
		{
			SerializerContext ctx;

			ctx.queue.push(resource);

			while (!ctx.queue.empty())
			{
				serializeN3(ctx, ctx.queue.front());

				ctx.queue.pop();
			}

			list<string>::const_iterator it = ctx.prefixes.begin();

			while (it != ctx.prefixes.end())
			{
				out << "@prefix " << *it << " <" << PREFIX_MAP.find(*it)->second << "> ." << endl;

				it++;
			}

			out << ctx.out.str();

			return out;
		}

        throw exception();
    }

	void Serializer::serializeN3(SerializerContext& ctx, ResourceRef resource)
	{
        PropertyMap properties = resource->properties;

        size_t n = properties.size();

		if (n == 0)
		{
			return;
		}

		// First, serialize the rdf:types of the resources.
        PropertyMapIterator typeIt = properties.lower_bound(rdf::_type);
        PropertyMapIterator typeItEnd = properties.upper_bound(rdf::_type);

		if (n == 1 && typeIt != properties.end())
		{
			// Do not serialize resources which only have asserted types and no other properties.
			return;
		}

		// Output the initial subject (the resource URI).
		ctx.out << endl << resource;

        // Count the number of serialized properties.
        int i = 0;

        while (typeIt != typeItEnd)
        {
            serializeN3(ctx, typeIt->first, typeIt->second);

            ctx.out << ";" << endl;

            i++;

            typeIt++;
        }

		// Then, serialize all other properties.
        PropertyMapIterator it = properties.begin();

        while (it != properties.end())
		{
			if (it->first != rdf::_type)
			{
				serializeN3(ctx, it->first, it->second);

                i++;

                if (i < n)
                {
                    ctx.out << ";" << endl;
                }
			}

            it++;
		}

		ctx.out << "." << endl;
	}

	void Serializer::serializeN3(SerializerContext& ctx, string property, PropertyValue x)
	{
		ctx.out << " ";

		if (property == rdf::_type)
		{
			ctx.out << "a";
		}
		else
		{
			serializeN3(ctx, property);
		}

		ctx.out << " " << flush;

		if (x.Value != NULL)
		{
			serializeN3(ctx, x.Value->uri);

			ResourceRef r = x.Value;

			if (r->serialize && ctx.track.find(r->uri) == ctx.track.end())
			{
				ctx.track.insert(r->uri);

				ctx.queue.push(r);
			}
		}
		else if (x.LiteralType != NULL)
		{
			if (x.LiteralType == typeid(Resource).name())
			{
				serializeN3(ctx, x.LiteralValue);
			}
			else
			{
				ctx.out << "\"" << x.LiteralValue << "\"";

				XsdTypeMapIterator it = TYPE_MAP.find(x.LiteralType);

				if (it != TYPE_MAP.end())
				{
                    ctx.out << "^^";

                    serializeN3(ctx, it->second);
				}
			}
		}
	}

	void Serializer::serializeN3(SerializerContext& ctx, string uri)
	{
		size_t i = uri.find(':');

		if (i != -1)
		{
			string schema = uri.substr(0, i + 1);

			if (PREFIX_MAP.find(schema) != PREFIX_MAP.end())
			{
				ctx.prefixes.insert(schema);
				ctx.out << uri;

				return;
			}
		}

		ctx.out << "<" << uri << ">";
	}
}

