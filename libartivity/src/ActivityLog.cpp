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

ActivityLog::ActivityLog() 
{
    _curl = initializeRequest(); // For use with curl_easy_escape.
    _activities = deque<Activity*>();
    _resources = list<Resource*>();
    _fileUri = "";
    _filePath = "";
    _canvasUri = "";
}

ActivityLog::~ActivityLog() 
{
    clear();
    
    // Free all the activities in the log.
    ActivityLogIterator ait = _activities.begin();
    
    while(ait != _activities.end())
    {
        delete *ait;
        
        ait++;
    }
    
    _activities.clear();
    
    // Free all the associated agents still in the log.
    AgentIterator agit = _agents.begin();
    
    while(agit != _agents.end())
    {
        delete *agit;
        
        agit++;
    }

	curl_easy_cleanup(_curl);
}

bool ActivityLog::connected()
{
    // TODO: Implement CURL call to http://localhost:8890/artivity/1.0/activities
    return true;
}

bool ActivityLog::empty()
{
    return _activities.empty() && _resources.empty();
}

ActivityLogIterator ActivityLog::begin()
{
    return _activities.begin();
}

Activity* ActivityLog::first()
{
    return _activities.empty() ? NULL : *(_activities.begin());
}

ActivityLogIterator ActivityLog::end()
{
    return _activities.end();
}

Activity* ActivityLog::last()
{
    return _activities.empty() ? NULL : *(_activities.end());
}

void ActivityLog::clear()
{
    ActivityLogIterator ait = _activities.begin();
    
    while(ait != _activities.end())
    {
        // Just clear the properties so that the activities
        // can be reused at a later time.
        (*ait)->clear();
        
        ait++;
    }
    
    // Free all the activities still in the log.
    ResourceIterator rit = _resources.begin();
    
    while(rit != _resources.end())
    {           
        delete *rit;
        
        rit++;
    }
    
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

void ActivityLog::addResource(Resource* resource)
{        
    _resources.push_back(resource);
}

void ActivityLog::removeResource(Resource* resource)
{
    _resources.remove(resource);
}

void ActivityLog::addActivity(Activity* activity)
{
    _activities.push_back(activity);
         
    AgentIterator agit = _agents.begin();
    
    while(agit != _agents.end())
    {
        Association* association = createResource<Association>();
        association->setAgent(*agit);
        
        activity->addAssociation(association);
        
        agit++;
    }
}

void ActivityLog::removeActivity(Activity* activity)
{
    ActivityLogIterator it = std::find(_activities.begin(), _activities.end(), activity);
    if (it != _activities.end())
        _activities.erase(it);
}

Activity* ActivityLog::updateActivity(Activity* activity)
{
    Activity* a = createResource<Activity>(activity->Uri.c_str());
    a->setType(activity->getType());
    
    return a;
}

void ActivityLog::addAgent(Agent* agent)
{
    _agents.push_back(agent);
}

void ActivityLog::removeAgent(Agent* agent)
{
    _agents.remove(agent);
}

// Create a new file data object without a path. For use with unsaved new files.
artivity::CreateFile* ActivityLog::createFile(double width, double height, const Resource* lengthUnit)
{        
    time_t now;
    time(&now);
    
    // Add the new file to the transmitted RDF output.
    FileDataObject* file = createResource<FileDataObject>();
    file->setCreationTime(now);
    
    _fileUri = string(file->Uri);
    _filePath = "";

    // Add a canvas to the newly created file.
    createCanvas(file, width, height, lengthUnit);
      
	artivity::CreateFile* activity = createActivity<artivity::CreateFile>();
    activity->addUsedEntity(file);
    
    return activity;
}

EditFile* ActivityLog::editFile(string path, double width, double height, const Resource* lengthUnit)
{
    _fileUri = string(getFileUri(path));
    _filePath = path;
    
    cout << _fileUri << endl << flush;

    _canvasUri = string(getCanvasUri(path));
    _canvasWidth = width;
    _canvasHeight = height;
    _canvasUnit = lengthUnit;
    
    FileDataObject* file = createResource<FileDataObject>(_fileUri.c_str());

    if (_canvasUri == "")
    {
        createCanvas(file, width, height, lengthUnit);
    }


    EditFile* activity = createActivity<EditFile>();
    activity->addUsedEntity(file);
    
    return activity;
}

bool ActivityLog::hasFile(string path)
{
    return path != "" && path == _filePath;
}

FileDataObject* ActivityLog::getFile()
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


string ActivityLog::getThumbnailPath(string path)
{
    string result;
    stringstream url;
    url << _server << _API << "/thumbnails/paths?fileUrl=" << escapePath(path);
    CURL* curl = initializeRequest();
    executeRequest(curl, url.str(), "", result);

    jsonxx::Array a;
    bool valid = a.parse(result);

    return valid ? a.get<jsonxx::String>(a.size() -1) : "";
}

string ActivityLog::getCanvasUri(string path)
{
    string data = "";
    stringstream url;
    url << _server << _uriAPI << "?canvas=" << escapePath(path);

    CURL* curl = initializeRequest();
    executeRequest(curl, url.str(), "", data);

    return data.length() > 2 ? data.substr(1, data.length() - 2) : UriGenerator::getUri();
}


void ActivityLog::createCanvas(FileDataObject* file, double width, double height, const Resource* lengthUnit)
{
    if(file == NULL)
    {
        return;
    }
    
    // Transmit the coorindate system which is associated with the new canvas.
    CoordinateSystem* coordinateSystem = createResource<CoordinateSystem>();
    coordinateSystem->setCoordinateDimension(2);
    coordinateSystem->setTransformationMatrix("[1 0 0; 0 1 0; 0 0 0]");
    
    // Transmit the new canvas.
    Canvas* canvas = createResource<Canvas>();
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

void ActivityLog::updateCanvas(double width, double height, const Resource* lengthUnit)
{
    if(hasCanvas(width, height, lengthUnit))
    {
        return;
    }
            
    FileDataObject* file = getFile();

    createCanvas(file, width, height, lengthUnit);
}

Canvas* ActivityLog::getCanvas()
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

bool ActivityLog::hasCanvas(double width, double height, const Resource* lengthUnit)
{
    return _canvasUri != "" && _canvasWidth == width && _canvasHeight == height && _canvasUnit == lengthUnit;
}

void ActivityLog::transmit()
{        
    stringstream stream;
    
    ActivityLogIterator ait = _activities.begin();
	Serializer s;
    while(ait != _activities.end())
    {            
        s.serialize(stream, **ait, N3);
        
        ait++;
    }
    
    ResourceIterator rit = _resources.begin();
    
    while(rit != _resources.end())
    {
        s.serialize(stream, **rit, N3);
        
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

void ActivityLog::enableMonitoring(string path)
{   
    string data;
    stringstream url;
    url << _server << _monitorAPI << "/add?file=" << escapePath(path);
 
    CURL* curl = initializeRequest();   
    executeRequest(curl, url.str(), "", data);
}

void ActivityLog::disableMonitoring(string path)
{
    string data;
   
    stringstream url;
    url << _server << _monitorAPI << "/remove?file=" << escapePath(path);
 
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
                char* t = curl_easy_escape(_curl, token.c_str(), token.length());
                
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
        char* t = curl_easy_escape(_curl, token.c_str(), token.length());
        
        result << string(t);
        
        curl_free(t);
    }
  
    return result.str();
}
