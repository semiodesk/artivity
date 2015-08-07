#ifndef PROPERTYMAP_H
#define PROPERTYMAP_H

#include <iostream>
#include "PropertyValue.h"

using namespace std;

namespace artivity
{
    namespace client
    {
        typedef multimap<string, PropertyValue>::iterator PropertyIterator;
        
        class PropertyMap : public multimap<string, PropertyValue>
        {
        public:
            PropertyMap() {}
            ~PropertyMap() {}
            
            PropertyIterator findProperty(string property, const Resource& resource)
            {
                PropertyIterator it = begin();
            
                while(it != end())
                {
                    PropertyValue x = it->second;
                    
                    if(x.Value == &resource)
                    {
                        break;
                    }
                    
                    it++;
                }
                
                return it;
            }
            
            PropertyIterator findProperty(string property, string literalValue, const type_info& typeInfo)
            {                                    
                PropertyIterator it = begin();
                                
                while(it != end())
                {                                        
                    PropertyValue x = it->second;
                    
                    if(x.LiteralType == typeInfo.name() && x.LiteralValue == literalValue)
                    {
                        break;
                    }
                    
                    it++;
                }
                                
                return it;
            }
            
            bool hasProperty(string property, const Resource& resource)
            {
                return findProperty(property, resource) != end();
            }
            
            bool hasProperty(string property, string literalValue, const type_info& typeInfo)
            {
                return findProperty(property, literalValue, typeInfo) != end();
            }
            
            void addProperty(string property, const Resource& resource)
            {
                if(hasProperty(property, resource)) return;
                
                insert(pair<string, PropertyValue>(property, PropertyValue(resource)));
            }
            
            void addProperty(string property, string literalValue, const type_info& typeInfo)
            {
                if(hasProperty(property, literalValue, typeInfo)) return;
                
                insert(pair<string, PropertyValue>(property, PropertyValue(literalValue, typeInfo)));
            }
            
            void removeProperty(string property, const Resource& resource)
            {
                PropertyIterator it = findProperty(property, resource);
                
                if(it == end()) return;
                
                erase(it);
            }
            
            void removeProperty(string property, string literalValue, const type_info& typeInfo)
            {
                PropertyIterator it = findProperty(property, literalValue, typeInfo);
                
                if(it == end()) return;
                
                erase(it);
            }
            
            void setProperty(string property, const Resource& resource)
            {
                erase(property);
                
                addProperty(property, resource);
            }
            
            void setProperty(string property, string literalValue, const type_info& typeInfo)
            {
                erase(property);
                
                addProperty(property, literalValue, typeInfo);
            }
        };
    }
}

#endif // PROPERTYMAP_H
