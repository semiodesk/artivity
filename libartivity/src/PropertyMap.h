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
#include <map>
#include "PropertyValue.h"

namespace artivity
{
    typedef std::multimap<std::string, PropertyValue>::iterator PropertyMapIterator;
    
    class PropertyMap
    {
        private:
        std::multimap<std::string, PropertyValue> _map;
        public:
        PropertyMap() {}
        ~PropertyMap() 
        {
            _map.clear();
        }
        
        PropertyMapIterator findProperty(const std::string& property, ResourceRef resource);
        
        PropertyMapIterator findProperty(const std::string& property, std::string literalValue, const type_info& typeInfo);

        bool hasProperty(const std::string& property, ResourceRef resource);
        
        bool hasProperty(const std::string& property, std::string literalValue, const type_info& typeInfo);
        
        void addProperty(const std::string& property, ResourceRef resource);
        
        void addProperty(const std::string& property, std::string literalValue, const type_info& typeInfo);
        
        void removeProperty(const std::string& property, ResourceRef resource);
        
        void removeProperty(const std::string& property, std::string literalValue, const type_info& typeInfo);
        
        void setProperty(const std::string& property, ResourceRef resource);

        void setProperty(const std::string& property, std::string literalValue, const type_info& typeInfo);

        void erase(const std::string& property) { _map.erase(property); }

        PropertyMapIterator find(const std::string& property) { return _map.find(property); }

        PropertyMapIterator end() { return _map.end(); }

        PropertyMapIterator begin() { return _map.begin(); }

        void clear() { _map.clear(); }

        size_t size() { return _map.size(); }

        bool empty() { return _map.empty(); }

        PropertyMapIterator lower_bound(const std::string& property) { return _map.lower_bound(property); }

        PropertyMapIterator upper_bound(const std::string& property) { return _map.upper_bound(property); }
    };
}

#endif // _ART_PROPERTYMAP_H
