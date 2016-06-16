#include "InformationElement.h"
#include "FileDataObject.h"

using namespace artivity;

void InformationElement::setStoredAs(FileDataObjectRef fdo)
{
    _fileDataObject = fdo;
    addProperty(nie::isStoredAs, fdo);
}

FileDataObjectRef InformationElement::getStoredAs()
{
    return _fileDataObject;
}