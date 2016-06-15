#ifndef PROPERTYMAP_H
#define PROPERTYMAP_H

#include <iostream>
#include "PropertyValue.h"

using namespace std;

namespace artivity
{
    typedef multimap<string, PropertyValue>::iterator PropertyMapIterator;
    
    class PropertyMap : public multimap<string, PropertyValue>
    {
    public:
        PropertyMap() {}
        ~PropertyMap() {}
        
        PropertyMapIterator findProperty(const string& property, ResourceRef resource)
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
        
        PropertyMapIterator findProperty(const string& property, string literalValue, const type_info& typeInfo)
        {                                    
            PropertyMapIterator it = begin();
                            
            while(it != end())
            {
                string p = it->first;
                string value = it->second.LiteralValue;
                const char* type = it->second.LiteralType;
                
                if(p == property && value == literalValue && type == typeInfo.name())
                {
                    break;
                }
                
                it++;
            }
                            
            return it;
        }
        
        bool hasProperty(const string& property, ResourceRef resource)
        {
            return findProperty(property, resource) != end();
        }
        
        bool hasProperty(const string& property, string literalValue, const type_info& typeInfo)
        {
            return findProperty(property, literalValue, typeInfo) != end();
        }
        
        void addProperty(const string& property, ResourceRef resource)
        {
            if(resource == NULL || hasProperty(property, resource)) return;
            
            insert(pair<string, PropertyValue>(property, PropertyValue(resource)));
        }
        
        void addProperty(const string& property, string literalValue, const type_info& typeInfo)
        {
            if(hasProperty(property, literalValue, typeInfo)) return;
            
            insert(pair<string, PropertyValue>(property, PropertyValue(literalValue, typeInfo)));
        }
        
        void removeProperty(const string& property, ResourceRef resource)
        {
            PropertyMapIterator it = findProperty(property, resource);
            
            if(it == end()) return;
            
            erase(it);
        }
        
        void removeProperty(const string& property, string literalValue, const type_info& typeInfo)
        {
            PropertyMapIterator it = findProperty(property, literalValue, typeInfo);
            
            if(it == end()) return;
            
            erase(it);
        }
        
        void setProperty(const string& property, ResourceRef resource)
        {
            erase(property);
            
            if(resource == NULL) return;
            
            addProperty(property, resource);
        }
        
        void setProperty(const string& property, string literalValue, const type_info& typeInfo)
        {
            erase(property);
            
            addProperty(property, literalValue, typeInfo);
        }
    };
}

#endif // PROPERTYMAP_H
