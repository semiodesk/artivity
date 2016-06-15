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

#ifdef WIN32 
#define CURL_STATICLIB
#endif

#include "curlresponse.h"
#include "ActivityLog.h"
#include "JSON/jsonxx.h"

using namespace artivity;

ActivityLog::ActivityLog(std::string server) 
{
    _server = server;
    _curl = initializeRequest(); // For use with curl_easy_escape.
    _resources = ResourceList();
    _fileUri = "";
    _filePath = "";
    _canvasUri = "";
}

ActivityLog::~ActivityLog() 
{
    clear();
    
    // Free all the associated agents still in the log.
    AgentIterator agit = _agents.begin();
    
    _agents.clear();

	curl_easy_cleanup(_curl);
}

bool ActivityLog::connected()
{
    // TODO: Implement CURL call to http://localhost:8890/artivity/1.0/activities
    return true;
}


void ActivityLog::clear()
{

    // Free all the activities still in the log.
    ResourceIterator rit = _resources.begin();
    
   
    if (_resources.size() > 0)
        _resources.clear();
}

ResourceIterator ActivityLog::findResource(const char* uri)
{
    ResourceIterator rit = _resources.begin();
    
    while(rit != _resources.end())
    {
        if((*rit)->Uri == uri)
        {
            return rit;
        }
        
        rit++;
    }
    
    return _resources.end();
}

bool ActivityLog::hasResource(const char* uri)
{        
    return findResource(uri) != _resources.end();
}

void ActivityLog::addResource(ResourceRef resource)
{        
    _resources.push_back(resource);
}

void ActivityLog::removeResource(ResourceRef resource)
{
    _resources.remove(resource);
}

void ActivityLog::setActivity(ActivityRef activity)
{
    _activities = activity;

    AgentIterator agit = _agents.begin();

    while (agit != _agents.end())
    {
        AssociationRef association = createResource<Association>();
        association->setAgent(*agit);

        activity->addAssociation(association);

        agit++;
    }
    
}

ActivityRef ActivityLog::updateActivity(ActivityRef activity)
{
    ActivityRef a = createResource<Activity>(activity->Uri.c_str());
    a->setType(activity->getType());
    
    return a;
}

void ActivityLog::addAgent(AgentRef agent)
{
    _agents.push_back(agent);
}

void ActivityLog::removeAgent(AgentRef agent)
{
    _agents.remove(agent);
}

// Create a new file data object without a path. For use with unsaved new files.
CreateFileRef ActivityLog::createFile(double width, double height, ResourceRef lengthUnit)
{        
    time_t now;
    time(&now);
    
    // Add the new file to the transmitted RDF output.
    FileDataObjectRef file = createResource<FileDataObject>();
    file->setCreationTime(now);
    
    _fileUri = string(file->Uri);
    _filePath = "";

    // Add a canvas to the newly created file.
    createCanvas(file, width, height, lengthUnit);
      
	CreateFileRef activity = createActivity<CreateFile>();
    activity->addUsedEntity(file);
    
    return activity;
}

EditFileRef ActivityLog::editFile(string path, double width, double height, ResourceRef lengthUnit)
{
    _fileUri = string(getFileUri(path));
    _filePath = path;
    
    cout << _fileUri << endl << flush;

    _canvasUri = string(getCanvasUri(path));
    _canvasWidth = width;
    _canvasHeight = height;
    _canvasUnit = lengthUnit;
    
    FileDataObjectRef file = createResource<FileDataObject>(_fileUri.c_str());

    if (_canvasUri == "")
    {
        createCanvas(file, width, height, lengthUnit);
    }


    EditFileRef activity = createActivity<EditFile>();
    activity->addUsedEntity(file);
    
    return activity;
}

bool ActivityLog::hasFile(string path)
{
    return path != "" && path == _filePath;
}

FileDataObjectRef ActivityLog::getFile()
{
    if(_fileUri == "")
    {
        return NULL;
    }
    
    const char* uri = _fileUri.c_str();
    
    if(hasResource(uri))
    {
        return getResource<FileDataObject>(uri);
    }
    else
    {
        return createResource<FileDataObject>(uri);
    }
}

string ActivityLog::getLatestVersionUri(string path)
{
    string data;
    stringstream url;
    url << _server << _uriAPI << "?latestVersion=" << escapePath(path);

    CURL* curl = initializeRequest();
    executeRequest(curl, url.str(), "", data);

    jsonxx::Value val;
    bool valid = val.parse(data);

    return valid ? val.get<jsonxx::String>() : "";
}

string ActivityLog::getFileUri(string path)
{
    string data = "";
    stringstream url;
    url << _server << _uriAPI << "?file=" << escapePath(path);
    
    CURL* curl = initializeRequest();    
    executeRequest(curl, url.str(), "", data);
    
    jsonxx::Value val;
    bool valid = val.parse(data);

    return valid ? val.get<jsonxx::String>() : UriGenerator::getUri();
}


string ActivityLog::getThumbnailPath(string fileUri)
{
    string result;
    stringstream url;
	url << _server << _API << "/thumbnails/path?fileUri=" << escapePath(fileUri);
    CURL* curl = initializeRequest();
    executeRequest(curl, url.str(), "", result);

    jsonxx::Array a;
    bool valid = a.parse(result);

    return valid ? a.get<jsonxx::String>((int)a.size() -1) : "";
}

string ActivityLog::getCanvasUri(string path)
{
    string data = "";
    stringstream url;
    url << _server << _uriAPI << "?canvas=" << escapePath(path);

    CURL* curl = initializeRequest();
    executeRequest(curl, url.str(), "", data);

    return data.length() > 2 ? data.substr(1, data.length() - 2) : "";
}


void ActivityLog::createCanvas(FileDataObjectRef file, double width, double height, ResourceRef lengthUnit)
{
    if(file == NULL)
    {
        return;
    }
    
    // Transmit the coorindate system which is associated with the new canvas.
    CoordinateSystemRef coordinateSystem  = createResource<CoordinateSystem>();
    coordinateSystem->setCoordinateDimension(2);
    coordinateSystem->setTransformationMatrix("[1 0 0; 0 1 0; 0 0 0]");
    
    // Transmit the new canvas.
    CanvasRef canvas = createResource<Canvas>();
    canvas->setWidth(width);
    canvas->setHeight(height);
    canvas->setLengthUnit(lengthUnit);
    canvas->setCoordinateSystem(coordinateSystem);
            
    // Store a copy of the canvas.
    _canvasUri = canvas->Uri.c_str();
    _canvasWidth = width;
    _canvasHeight = height;
    _canvasUnit = lengthUnit;
    
    file->setCanvas(canvas);
}

void ActivityLog::updateCanvas(double width, double height, ResourceRef lengthUnit)
{
    if(hasCanvas(width, height, lengthUnit))
    {
        return;
    }
            
    FileDataObjectRef file = getFile();

    createCanvas(file, width, height, lengthUnit);
}

CanvasRef ActivityLog::getCanvas()
{
    if (_canvasUri == "")
    {
        return NULL;
    }

    const char* uri = _canvasUri.c_str();

    if (hasResource(uri))
    {
        return getResource<Canvas>(uri);
    }
    else
    {
        return createResource<Canvas>(uri);
    }

}

bool ActivityLog::hasCanvas(double width, double height, ResourceRef lengthUnit)
{
    return _canvasUri != "" && _canvasWidth == width && _canvasHeight == height && _canvasUnit == lengthUnit;
}

void ActivityLog::transmit()
{        
    stringstream stream;
    
	Serializer s;
         
    s.serialize(stream, _activities, N3);
     
    
    ResourceIterator rit = _resources.begin();
    
    while(rit != _resources.end())
    {
        s.serialize(stream, *rit, N3);
        
        rit++;
    }
    
    string requestData = stream.str();
    string responseData;
    
    CURL* curl = initializeRequest();
    stringstream url;
    url << _server << _activityAPI;
    executeRequest(curl, url.str(), requestData, responseData);
    
    // Cleanup after all resources have been serialized and transmitted.
    clear();
}

CURL* ActivityLog::initializeRequest()
{
    CURL* curl = curl_easy_init();

    if (!curl)
    {
        string msg = "Failed to initialize CURL.";
        
        logError(CURLE_FAILED_INIT, msg);
    }
    
    return curl;
}

long ActivityLog::executeRequest(CURL* curl, string url, string postFields, string& response)
{       
    if(!curl)
    {
        return CURLE_FAILED_INIT;
    }
    
    struct curl_slist *headers = NULL; // init to NULL is important
    headers = curl_slist_append(headers, "charsets: utf-8");
    
    struct curl_string data;
    curl_init_string(&data);
    
    curl_easy_reset(curl);
    curl_easy_setopt(curl, CURLOPT_URL, url.c_str());
    curl_easy_setopt(curl, CURLOPT_HTTPHEADER, headers);
    curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, curl_write_string);
    curl_easy_setopt(curl, CURLOPT_WRITEDATA, &data);
    curl_easy_setopt(curl, CURLOPT_TIMEOUT, 60L);
    
    if(postFields.length() > 0)
    {
        curl_easy_setopt(curl, CURLOPT_POSTFIELDS, postFields.c_str());
    }
    
    CURLcode responseCode = curl_easy_perform(curl);
    
    if(data.len > 0)
    {
        response = data.ptr;
    }
    
    curl_easy_cleanup(curl);
    
    if(responseCode == CURLE_OK)
    {
        logInfo(responseCode, url);
    }
    else
    {
        logError(responseCode, url);
    }
    
    if(postFields.length() > 0)
    {
        cout << postFields << endl << flush;
    }
    
    return responseCode;
}

void ActivityLog::logError(CURLcode responseCode, string msg)
{
    cout << "[" << getTime() << "] " << responseCode << " " << msg << endl << flush;
}

void ActivityLog::logInfo(CURLcode responseCode, string msg)
{
    cout << "[" << getTime() << "] " << responseCode << " " << msg << endl << flush;
}

string ActivityLog::getTime()
{
    time_t t = time(NULL);
    
    char date_str[20];
	#ifdef WIN32
	tm* time = new tm();
	localtime_s(time, &t);
    strftime(date_str, 20, "%Y/%m/%d %H:%M:%S",time);
	#else
	strftime(date_str, 20, "%Y/%m/%d %H:%M:%S", localtime(&t));
	#endif
    
    return string(date_str);
}

void ActivityLog::enableMonitoring(string path, string uri)
{   
    string data;
    stringstream url;

    url << _server << _monitorAPI << "/add?uri=" << uri << "&filePath=" << escapePath(path);
 
    CURL* curl = initializeRequest();   
    executeRequest(curl, url.str(), "", data);
}

void ActivityLog::disableMonitoring(string path)
{
    string data;  
    stringstream url;

    url << _server << _monitorAPI << "/remove?filePath=" << escapePath(path);
 
    CURL* curl = initializeRequest();   
    executeRequest(curl, url.str(), "", data);
}

string ActivityLog::escapePath(string path)
{
    stringstream result;
    string token;
  
    for(int i = 0; i < path.length(); i++)
    {
        char c = path[i];
        
        if(c == '/' || c == '\\' || c == ':')
        {            
            if(!token.empty())
            {
                char* t = curl_easy_escape(_curl, token.c_str(), (int)token.length());
                
                result << string(t);
                
                curl_free(t);
                
                token = "";
            }
            
            if (c == '\\')
                result << '/';
            else
                result << c;
        }
        else
        {
            token += c;
        }
    }
    
    if(!token.empty())
    {
        char* t = curl_easy_escape(_curl, token.c_str(), (int)token.length());
        
        result << string(t);
        
        curl_free(t);
    }
  
    return result.str();
}
