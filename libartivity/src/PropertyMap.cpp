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

#include "PropertyMap.h"
#include "Resource.h"

namespace artivity
{
    PropertyMapIterator PropertyMap::findProperty(const std::string& property, PropertyValue value)
    {
        PropertyMapIterator it = _map.begin();

        while (it != _map.end())
        {
            PropertyValue x = it->second;

            if (strcmp(x.LiteralType, value.LiteralType) == 0
                && x.LiteralValue == value.LiteralValue
                && x.Value == value.Value)
            {
                break;
            }

            it++;
        }

        return it;
    }

    PropertyMapIterator PropertyMap::findProperty(const std::string& property, ResourceRef resource)
    {
        if (resource == NULL) return _map.end();

        PropertyMapIterator it = _map.begin();

        for (; it != _map.end(); it++)
        {
            PropertyValue x = it->second;

            if (it->first.compare(property) != 0)
                continue;
            
            if (x.Value && x.Value->uri.compare(resource->uri) == 0) break;

            
        }

        return it;
    }

    PropertyMapIterator PropertyMap::findProperty(const std::string& property, std::string literalValue, const type_info& typeInfo)
    {
        PropertyMapIterator it = _map.begin();

        while (it != _map.end())
        {
            std::string p = it->first;
            std::string value = it->second.LiteralValue;
            const char* type = it->second.LiteralType;

            if (p == property && value == literalValue && type == typeInfo.name())
            {
                break;
            }

            it++;
        }

        return it;
    }

    bool PropertyMap::hasProperty(const std::string& property, ResourceRef resource)
    {
        return findProperty(property, resource) != _map.end();
    }

    bool PropertyMap::hasProperty(const std::string& property, std::string literalValue, const type_info& typeInfo)
    {
        return findProperty(property, literalValue, typeInfo) != _map.end();
    }

    void PropertyMap::addProperty(const std::string& property, ResourceRef resource)
    {
        if (resource == NULL || hasProperty(property, resource)) return;

        _map.insert(std::pair<std::string, PropertyValue>(property, PropertyValue(resource)));
    }

    void PropertyMap::addProperty(const std::string& property, std::string literalValue, const type_info& typeInfo)
    {
        if (hasProperty(property, literalValue, typeInfo)) return;

        _map.insert(std::pair<std::string, PropertyValue>(property, PropertyValue(literalValue, typeInfo)));
    }

    void PropertyMap::removeProperty(const std::string& property, PropertyValue value)
    {
        PropertyMapIterator it = findProperty(property, value);

        if (it == _map.end()) return;

        _map.erase(it);
    }

    void PropertyMap::removeProperty(const std::string& property, ResourceRef resource)
    {
        PropertyMapIterator it = findProperty(property, resource);

        if (it == _map.end()) return;

        _map.erase(it);
    }

    void PropertyMap::removeProperty(const std::string& property, std::string literalValue, const type_info& typeInfo)
    {
        PropertyMapIterator it = findProperty(property, literalValue, typeInfo);

        if (it == _map.end()) return;

        _map.erase(it);
    }

    void PropertyMap::setProperty(const std::string& property, PropertyValue value)
    {
        _map.erase(property);

        addProperty(property, value);
    }

    void PropertyMap::setProperty(const std::string& property, ResourceRef resource)
    {
        _map.erase(property);

        if (resource == NULL) return;

        addProperty(property, resource);
    }

    void PropertyMap::setProperty(const std::string& property, std::string literalValue, const type_info& typeInfo)
    {
        _map.erase(property);

        addProperty(property, literalValue, typeInfo);
    }

    void PropertyMap::addProperty(const std::string& property, PropertyValue value)
    {
        _map.insert(std::pair<std::string, PropertyValue>(property, value));
    }
}