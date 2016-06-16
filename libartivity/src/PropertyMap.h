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

#ifndef _ART_PROPERTYMAP_H
#define _ART_PROPERTYMAP_H

#include <iostream>
#include "PropertyValue.h"

namespace artivity
{

    typedef std::multimap<std::string, PropertyValue>::iterator PropertyMapIterator;
    
    class PropertyMap : public std::multimap<std::string, PropertyValue>
    {
    public:
        PropertyMap() {}
        ~PropertyMap() {}
        
        PropertyMapIterator findProperty(const std::string& property, ResourceRef resource)
        {
            if(resource == NULL) return end();
            
            PropertyMapIterator it = begin();
        
            while(it != end())
            {
                PropertyValue x = it->second;
                
                if(x.Value == resource) break;
                
                it++;
            }
            
            return it;
        }
        
        PropertyMapIterator findProperty(const std::string& property, std::string literalValue, const type_info& typeInfo)
        {                                    
            PropertyMapIterator it = begin();
                            
            while(it != end())
            {
                std::string p = it->first;
                std::string value = it->second.LiteralValue;
                const char* type = it->second.LiteralType;
                
                if(p == property && value == literalValue && type == typeInfo.name())
                {
                    break;
                }
                
                it++;
            }
                            
            return it;
        }
        
        bool hasProperty(const std::string& property, ResourceRef resource)
        {
            return findProperty(property, resource) != end();
        }
        
        bool hasProperty(const std::string& property, std::string literalValue, const type_info& typeInfo)
        {
            return findProperty(property, literalValue, typeInfo) != end();
        }
        
        void addProperty(const std::string& property, ResourceRef resource)
        {
            if(resource == NULL || hasProperty(property, resource)) return;
            
            insert(std::pair<std::string, PropertyValue>(property, PropertyValue(resource)));
        }
        
        void addProperty(const std::string& property, std::string literalValue, const type_info& typeInfo)
        {
            if(hasProperty(property, literalValue, typeInfo)) return;
            
            insert(std::pair<std::string, PropertyValue>(property, PropertyValue(literalValue, typeInfo)));
        }
        
        void removeProperty(const std::string& property, ResourceRef resource)
        {
            PropertyMapIterator it = findProperty(property, resource);
            
            if(it == end()) return;
            
            erase(it);
        }
        
        void removeProperty(const std::string& property, std::string literalValue, const type_info& typeInfo)
        {
            PropertyMapIterator it = findProperty(property, literalValue, typeInfo);
            
            if(it == end()) return;
            
            erase(it);
        }
        
        void setProperty(const std::string& property, ResourceRef resource)
        {
            erase(property);
            
            if(resource == NULL) return;
            
            addProperty(property, resource);
        }
        
        void setProperty(const std::string& property, std::string literalValue, const type_info& typeInfo)
        {
            erase(property);
            
            addProperty(property, literalValue, typeInfo);
        }
    };
}

#endif // _ART_PROPERTYMAP_H
