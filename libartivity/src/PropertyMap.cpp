#include "PropertyMap.h"

namespace artivity
{

    PropertyMapIterator PropertyMap::findProperty(const std::string& property, ResourceRef resource)
    {
        if (resource == NULL) return end();

        PropertyMapIterator it = begin();

        while (it != end())
        {
            PropertyValue x = it->second;

            if (x.Value == resource) break;

            it++;
        }

        return it;
    }

    PropertyMapIterator PropertyMap::findProperty(const std::string& property, std::string literalValue, const type_info& typeInfo)
    {
        PropertyMapIterator it = begin();

        while (it != end())
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
        return findProperty(property, resource) != end();
    }

    bool PropertyMap::hasProperty(const std::string& property, std::string literalValue, const type_info& typeInfo)
    {
        return findProperty(property, literalValue, typeInfo) != end();
    }

    void PropertyMap::addProperty(const std::string& property, ResourceRef resource)
    {
        if (resource == NULL || hasProperty(property, resource)) return;

        insert(std::pair<std::string, PropertyValue>(property, PropertyValue(resource)));
    }

    void PropertyMap::addProperty(const std::string& property, std::string literalValue, const type_info& typeInfo)
    {
        if (hasProperty(property, literalValue, typeInfo)) return;

        insert(std::pair<std::string, PropertyValue>(property, PropertyValue(literalValue, typeInfo)));
    }

    void PropertyMap::removeProperty(const std::string& property, ResourceRef resource)
    {
        PropertyMapIterator it = findProperty(property, resource);

        if (it == end()) return;

        erase(it);
    }

    void PropertyMap::removeProperty(const std::string& property, std::string literalValue, const type_info& typeInfo)
    {
        PropertyMapIterator it = findProperty(property, literalValue, typeInfo);

        if (it == end()) return;

        erase(it);
    }

    void PropertyMap::setProperty(const std::string& property, ResourceRef resource)
    {
        erase(property);

        if (resource == NULL) return;

        addProperty(property, resource);
    }

    void PropertyMap::setProperty(const std::string& property, std::string literalValue, const type_info& typeInfo)
    {
        erase(property);

        addProperty(property, literalValue, typeInfo);
    }

}