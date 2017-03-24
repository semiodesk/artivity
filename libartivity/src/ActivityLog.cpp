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

#include "Ontologies/prov.h"
#include "Resource.h"

#include "ActivityLog.h"

#include "ObjectModel/Activity.h"
#include "ObjectModel/Influences/Generation.h"

namespace artivity
{
	using namespace std;
	using namespace boost;
    using namespace boost::chrono;
	using namespace boost::property_tree;

	ActivityLog::ActivityLog()
	{
		_curl = initializeRequest();

		_fileUrl = "";

		_transmitCount = 0;
        debug = false;
	}

	ActivityLog::~ActivityLog()
	{
		// Dereference the activity.
		_activity = ActivityRef();

		curl_easy_cleanup(_curl);
	}

	CURL* ActivityLog::initializeRequest()
	{
		CURL* curl = curl_easy_init();

#ifdef _DEBUG
		if (!curl)
		{
			logError("Failed to initialize CURL.");
		}
#endif

		return curl;
	}

	long ActivityLog::executeRequest(CURL* curl, string url, string postFields, string& response)
	{
		if (!curl)
		{
#ifdef _DEBUG
			logError("CURL not initialized.");
#endif

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

		if (postFields.length() > 0)
		{
			curl_easy_setopt(curl, CURLOPT_POSTFIELDS, postFields.c_str());
		}

#ifdef _DEBUG
        high_resolution_clock::time_point t1 = high_resolution_clock::now();
#endif

		CURLcode responseCode = curl_easy_perform(curl);

#ifdef _DEBUG
        high_resolution_clock::time_point t2 = high_resolution_clock::now();
#endif

		if (data.len > 0)
		{
			response = data.ptr;
		}

		curl_easy_cleanup(curl);

#ifdef _DEBUG
		if (responseCode == CURLE_OK)
		{
            stringstream time;
            time << duration_cast<milliseconds>(t2 - t1).count() << "ms";

			logRequest(url, time.str(), postFields);
		}
		else
		{
			logError(url);
		}
#endif

		return responseCode;
	}

    bool ActivityLog::fetchInitialData(string fileUrl, string* latestEntityUri, string* fileDataObjectUri, string* renderOutputPath, string* userAssociationUri)
    {
        try
        {
            CURL* curl = initializeRequest();

            stringstream url;
            url << _endpointUrl << "/plugin/file/open?fileUrl=" << fileUrl;

            string response;

            executeRequest(curl, url.str(), "", response);

            stringstream stream;
            stream << response;

            if (response.empty())
            {
                return false;
            }

            ptree tree;

            read_json(stream, tree);



            // storing latest entity
            if (latestEntityUri != NULL)
                latestEntityUri->assign(tree.get_child("latestEntityUri").get_value<string>());

            // connecting file data object
            if ( fileDataObjectUri != NULL)
                fileDataObjectUri->assign(tree.get_child("fileDataObjectUri").get_value<string>());

            // store render output path
            if ( renderOutputPath != NULL)
                renderOutputPath->assign(tree.get_child("renderOutputPath").get_value<string>()); 
            
            
            // Create user association
            if ( userAssociationUri != NULL )
                userAssociationUri->assign(tree.get_child("userAssociationUri").get_value<string>());
            
            return true;

        }
        catch (...)
        {
            return false;
        }


    }

	bool ActivityLog::connect(string endpointUrl)
	{
        time_t now;
        time(&now);

		_endpointUrl = endpointUrl;

        string latestEntityUri = "";
        string fileDataObjectUri = "";
        string renderOutputPath = "";
        string userAssociationUri = "";
        fetchInitialData(_fileUrl, &latestEntityUri, &fileDataObjectUri, &renderOutputPath, &userAssociationUri);

        if (!latestEntityUri.empty())
        {
            _prevEntity = ImageRef(new Image(latestEntityUri.c_str()));
            _entity->uri = UriGenerator::removeTimeFragment(latestEntityUri);
        }


        if (!fileDataObjectUri.empty())
        {
            _entity->setDataObject(FileDataObjectRef(new FileDataObject(fileDataObjectUri.c_str())));
            _hasDataObject = true;
        }
        else
        {
            _hasDataObject = false;
        }

        if (!renderOutputPath.empty())
            _renderOutputPath = renderOutputPath;

        if (!userAssociationUri.empty())
        {
            AssociationRef a = AssociationRef(new Association(userAssociationUri));
            a->setRole(RoleRef(new Role(art::USER)));

            _associations.push_back(a);
        }

		_activity->setStartTime(now);

		// Initialize the associations.
		auto it = _associations.begin();

		while (it != _associations.end())
		{
            _activity->addAssociation(*it);
            it++;
		}

		return ready();
	}

	bool ActivityLog::ready()
	{
		return !_endpointUrl.empty() && _entity != NULL && _activity != NULL;
	}

	void ActivityLog::close()
	{
        // TODO: Investigate, is this called before save? Race condition?
        clear();

		time_t now;
		time(&now);

		close(now);
	}

	void ActivityLog::close(time_t time)
	{
		// Only transmit the session end if there has already been transmitted data
		// or there is transmittable activity data available.
		if(_transmitCount > 0 || !_activity->empty())
		{
			_activity->setEndTime(time);
            _activity->clearUsedEntities();
			transmit();
		}
	}


	void ActivityLog::addAssociation(const char* roleUri, const char* agentUri, const char* version)
	{
        if (strcmp(roleUri, art::SOFTWARE) != 0 || strcmp(version, "\0") == 0) return;

        stringstream str;
        str << agentUri << "/" << version;

        SoftwareAssociationRef a = SoftwareAssociationRef(new SoftwareAssociation(str.str()));
		a->setAgent(AgentRef(new Agent(agentUri)));
		a->setRole(RoleRef(new Role(roleUri)));
		a->setVersion(version);

		_associations.push_back(a);
	}

	void ActivityLog::addAssociation(string roleUri, string agentUri, string version)
	{
        addAssociation(roleUri.c_str(), agentUri.c_str(), version.c_str());
	}

    void ActivityLog::setDocument(ImageRef image, std::string path, bool create)
	{
		_entity = image;
		_fileUrl = UriGenerator::getUrl(path);

        if (create)
        {
            // A new file is being created.
            _activity = CreateFileRef(new CreateFile());

            GenerationRef generation = GenerationRef(new Generation());
            generation->addEntity(_entity);

            addInfluence(generation);
        }
        else
        {
            // An existing file is being edited.
            _activity = EditFileRef(new EditFile());
            _activity->addUsed(_entity);
        }
	}

    void ActivityLog::save()
    {
        time_t now;
        time(&now);

        auto uri = _entity->uri;
        _entity->uri = UriGenerator::appendTimeFragment(uri, now);
        if ( _prevEntity != NULL )
            _prevEntity->serialize = false;
        transmit();

        _prevEntity = ImageRef(new Image(_entity->uri.c_str()));
        _entity->uri = uri;
        _entity->clearRenderings();
    }

    void ActivityLog::createDerivation(DerivationRef saveAs, ImageRef targetImage, std::string targetPath)
    {
        // We cannot create derivations of newly created files or overwritten files.
        if (_fileUrl.empty() || _fileUrl == UriGenerator::getUrl(targetPath))
        {
            return;
        }

        ImageRef sourceImage = _entity;

        // Temporarily store a copy of all untransmitted influences.
        std::list<InfluenceRef> influences = _activity->getInfluences();

        // NOTE: These are only influences which are not directly associated with
        // an Entity such as undos and redos.
        influences.insert(influences.end(), _influences.begin(), _influences.end());

        for (auto it = influences.begin(); it != influences.end(); it++)
        {
            removeInfluence(*it);
        }

        time_t now;
        time(&now);

        // Close and transmit the current activity, but only if it changes to the original have been made
        if (_transmitCount > 0)
        {
            _activity->clearUsedEntities();
            _activity->setEndTime(now);

            transmit();
        }

        // Check if there is already a file at the given location.
        string latestEntityUri = "";
        string fileDataObjectUri = "";
        string renderOutputPath = "";
        string userAssociationUri = "";
        fetchInitialData(targetPath, &latestEntityUri, &fileDataObjectUri, &renderOutputPath, &userAssociationUri);

        if (!latestEntityUri.empty())
        {
            targetImage->uri = latestEntityUri;
            targetImage->setDataObject(FileDataObjectRef(new FileDataObject(fileDataObjectUri.c_str())));
        }

        // Create a new activity for the new image.
        setDocument(targetImage, targetPath, true);
        

        if (_prevEntity != NULL)
        {
            // Assert a qualified derivation of the new file from the old one.
            DerivationRef derivation = DerivationRef(new Derivation());
            derivation->addEntity(_prevEntity);
            derivation->setActivity(_activity);
            derivation->setTime(now);

            // Get document creates a new instance of the plugin's entity type.
            targetImage->addInfluence(derivation);
        }

        _activity->setStartTime(now);
        _activity->addUsed(_prevEntity);

        // Do not serialize the old image as it should already be stored in the database.
        sourceImage->serialize = false;

        // NOTE: This assumes that the file system monitor will update any existing file data objects.
        fetchNewDataObject(targetPath);

        // Transfer the current associations to the new activity.
        for (auto it = _associations.begin(); it != _associations.end(); it++)
        {
            // Note: only adds triples which are not already stored.
            _activity->addAssociation(*it);
        }

        // Associated all untransmitted activities with the new activity.
        for (auto it = influences.begin(); it != influences.end(); it++)
        {
            addInfluence(*it);
        }

        latestEntityUri = _entity->uri;
        _entity->uri = UriGenerator::appendTimeFragment(latestEntityUri, now);
        if (_prevEntity != NULL)
            _prevEntity->serialize = false;
        transmit();

        _prevEntity = ImageRef(new Image(_entity->uri.c_str()));
        _entity->uri = latestEntityUri;
    }

    void ActivityLog::populateRevision(RevisionRef revision)
    {

        if (_prevEntity != NULL)
        {
            _activity->addInvalidated(_prevEntity);
            revision->addEntity(_prevEntity);
            _entity->addInfluence(revision);
        }
        _activity->addGenerated(_entity);
        revision->setActivity(_activity);
        if ( _prevEntity != NULL )
        revision->addEntity(_prevEntity);

    }

	ImageRef ActivityLog::getDocument()
	{
		return _entity;
	}

	string ActivityLog::fetchEntityUri(string fileUrl, string* fileUri)
	{
        string uri = "";

        try
        {
            CURL* curl = initializeRequest();

            stringstream url;
            url << _endpointUrl << "/uris?fileUrl=" << fileUrl;

            string response;

            executeRequest(curl, url.str(), "", response);

            stringstream stream;
            stream << response;

            if (response.empty())
            {
                return uri;
            }

            ptree tree;

            read_json(stream, tree);

            uri = tree.get_child("uri").get_value<string>();
            fileUri->assign(tree.get_child("file").get_value<string>());
        }
        catch (...)
        {
			uri = "";
        }

		return uri;
	}

    string ActivityLog::fetchRenderOutputPath()
    {
        if (_entity->uri.empty())
        {
            return "";
        }

        CURL* curl = initializeRequest();

        stringstream url;
        url << _endpointUrl << "/renderings/path?uri=" << UriGenerator::escapePath(_entity->uri) << "&create";

        string response;

        executeRequest(curl, url.str(), "", response);

        stringstream stream;
        stream << response;

        if (response.empty())
        {
            return "";
        }

        ptree tree;

        read_json(stream, tree);

        string path = tree.front().second.get_value<string>();

        return path;
    }

    bool ActivityLog::fetchAssociationUri(AssociationRef association)
    {
        CURL* curl = initializeRequest();

        stringstream url;

        url << _endpointUrl << "/agents/associations";

        if (strcmp(association->getType(), art::SoftwareAssociation) == 0)
        {
            SoftwareAssociationRef software = boost::dynamic_pointer_cast<SoftwareAssociation>(association);
            url << "?role=" << software->getRole()->uri;
            url << "&agent=" << software->getAgent()->uri;
            url << "&version=" << software->getVersion();
        }
        else
        {
            url << "?role=" << association->getRole()->uri;
        }

        string response;

        executeRequest(curl, url.str(), "", response);

        stringstream stream;
        stream << response;

        if (response.empty())
        {
            return false;
        }

        ptree tree;

        read_json(stream, tree);

        string uri = tree.get_child("association").get_value<string>();

        if (uri.empty())
        {
            return false;
        }

        association->uri = uri;

        return true;
    }


    void ActivityLog::addInfluence(InfluenceRef influence)
    {
        list<EntityRef> entities = influence->getEntities();

        if (influence->is(art::Undo) || influence->is(art::Redo))
        {
            _influences.push_back(influence);

            influence->setActivity(_activity);
        }
        else if (influence->is(prov::Start))
        {
            _activity->addProperty(prov::qualifiedStart, influence);
        }
        else if (influence->is(prov::End))
        {
            _activity->addProperty(prov::qualifiedEnd, influence);
        }
        else
        {
            for (list<EntityRef>::iterator it = entities.begin(); it != entities.end(); ++it)
            {
                EntityRef entity = *it;
                if (entity == NULL)
                    continue;

                if (influence->is(prov::Generation))
                {
                    EntityRef r = _activity->getEntity(entity->uri);

                    GenerationRef generation = dynamic_pointer_cast<Generation>(influence);
                    
                    if (r)
                    {
                        r->addInfluence(generation);
                        
                        _activity->addGenerated(r);
                    }
                    else
                    {
                        entity->addInfluence(generation);
                        
                        _activity->addGenerated(entity);
                    }
                }
                else if (influence->is(prov::Invalidation))
                {
                    EntityRef r = _activity->getEntity(entity->uri);
                    
                    InvalidationRef invalidation = dynamic_pointer_cast<Invalidation>(influence);
                    
                    if (r)
                    {
                        r->addInfluence(invalidation);
                        
                        _activity->addInvalidated(r);
                    }
                    else
                    {
                        entity->addInfluence(invalidation);
                        
                        _activity->addInvalidated(entity);
                    }
                }
                else if (influence->is(prov::Derivation))
                {
                    EntityRef r = _activity->getEntity(entity->uri);

                    DerivationRef derivation = dynamic_pointer_cast<Derivation>(influence);
                    
                    if (r)
                    {
                        r->addInfluence(derivation);
                        
                        _activity->addUsed(r);
                    }
                    else
                    {
                        entity->addInfluence(derivation);
                        
                        _activity->addUsed(entity);
                    }
                }
                else if (influence->is(prov::Revision))
                {
                    EntityRef r = _activity->getEntity(entity->uri);
                    
                    RevisionRef revision = dynamic_pointer_cast<Revision>(influence);

                    if (r)
                    {
                        r->addInfluence(revision);
                        
                        _activity->addUsed(r);
                    }
                    else
                    {
                        entity->addInfluence(revision);
                        
                        _activity->addUsed(entity);
                    }
                }
                else
                {
                    // TODO: Handle this error case properly. Adding a generic Influence to an entity might be wrong;
                    // it's only allowed to add EntityInfluences to entities.
                    entity->addProperty(prov::qualifiedInfluence, influence);
                }

                influence->setActivity(_activity);
            }
        }
    }

	void ActivityLog::removeInfluence(InfluenceRef influence)
	{
        list<EntityRef> entities = influence->getEntities();

        if (influence->is(art::Undo) || influence->is(art::Redo))
        {
            _influences.remove(influence);

            influence->clearActivity();
        }
        else
        {
            for (list<EntityRef>::iterator it = entities.begin(); it != entities.end(); ++it)
            {
                EntityRef entity = *it;

                if (influence->is(prov::Generation))
                {
                    _activity->removeGenerated(entity);

                    GenerationRef generation = dynamic_pointer_cast<Generation>(influence);
                    
                    entity->removeInfluence(generation);
                }
                else if (influence->is(prov::Invalidation))
                {
                    _activity->removeInvalidated(entity);
                    
                    InvalidationRef invalidation = dynamic_pointer_cast<Invalidation>(influence);
                    
                    entity->removeInfluence(invalidation);
                }
                else if (influence->is(prov::Derivation))
                {
                    _activity->removeUsed(entity);

                    DerivationRef derivation = dynamic_pointer_cast<Derivation>(influence);
                    
                    entity->removeInfluence(derivation);
                }
                else if (influence->is(prov::Revision))
                {
                    _activity->removeUsed(entity);

                    RevisionRef revision = dynamic_pointer_cast<Revision>(influence);
                    
                    entity->removeInfluence(revision);
                }
                else
                {
                    // TODO: Handle this error case properly. Removing a generic Influence to an entity might be wrong;
                    // it's only allowed to add EntityInfluences to entities.
                    entity->removeProperty(prov::qualifiedInfluence, influence);
                }

                influence->clearActivity();
            }
        }
	}

    string ActivityLog::getRenderOutputPath()
    {
        return _renderOutputPath;
    }

    void ActivityLog::transmit()
    {
        stringstream stream;

        Serializer s = Serializer();

        // Recursively serialize the activity and all related resources..
        s.serialize(stream, _activity, N3);

        // Additionally, serialize the influences which are not directly
        // referenced in any entities in the activity..
        for (InfluenceIterator it = _influences.begin(); it != _influences.end(); ++it)
        {
            s.serialize(stream, *it, N3);
        }

        if (stream.rdbuf()->in_avail() == 0)
        {
            return;
        }

        // Transmit the RDF output.
        string requestData = stream.str();
        string responseData;

        CURL* curl = initializeRequest();

        stringstream url;
        url << _endpointUrl << "/activities";

        if (!debug)
        {
            long status = executeRequest(curl, url.str(), requestData, responseData);

            if (status == CURLE_OK)
            {
                _transmitCount++;
            }
        }
        else
        {
#ifdef _DEBUG
            logRequest(url.str(), "0ms", requestData);

            _transmitCount++;
#endif
        }

		clear();
	}

#ifdef _DEBUG
	void ActivityLog::logError(string msg)
	{
		cout << getTime() << " [ERROR] " << msg << endl << flush;
	}

	void ActivityLog::logInfo(string msg)
	{
		cout << getTime() << " [INFO ] " << msg << endl << flush;
	}

    void ActivityLog::logRequest(string url, string time, string data)
    {
        cout << getTime() << " [INFO ] " << time << " - " << url << endl << flush;

        if (!data.empty())
        {
            cout << data << endl << flush;
        }
    }
#endif

	string ActivityLog::getTime()
	{
		time_t t = time(NULL);

		char date_str[20];

#ifdef WIN32
		tm* time = new tm();
		localtime_s(time, &t);
		strftime(date_str, 20, "%Y/%m/%d %H:%M:%S", time);
#else
		strftime(date_str, 20, "%Y/%m/%d %H:%M:%S", localtime(&t));
#endif

		return string(date_str);
	}

	void ActivityLog::dump(ptree const& pt)
	{
		ptree::const_iterator end = pt.end();

		for (ptree::const_iterator it = pt.begin(); it != end; ++it)
		{
			cout << it->first << ": " << it->second.get_value<string>() << endl;

			dump(it->second);
		}
	}

    bool ActivityLog::fetchNewDataObject(std::string path)
    {
        std::string fileUri = UriGenerator::getUri();

        std::string fileUrl = UriGenerator::getUrl(path);

        CURL* curl = initializeRequest();

        stringstream url;
        url << _endpointUrl << "/files?uri=" << fileUri << "&url=" << fileUrl << "&create";

        string response;

        long status = executeRequest(curl, url.str(), "", response);

        stringstream stream;
        stream << response;

        _entity->setDataObject(FileDataObjectRef(new FileDataObject(fileUri.c_str())));

		_hasDataObject = (status == CURLE_OK);

        return _hasDataObject;
    }
}
