#include "FileDataObject.h"
#include "InformationElement.h"

namespace artivity
{

    void FileDataObject::setInterpretedAs(InformationElementRef ie)
    {
        _interpretedAs = ie;
        setValue(nie::interpretedAs, ie);
    }

    InformationElementRef FileDataObject::getInterpretedAs()
    {
        return _interpretedAs;
    }

}
   
