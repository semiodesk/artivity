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

#if _DEBUG
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
#if _DEBUG
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

	bool ActivityLog::connect(string endpointUrl)
	{
		_endpointUrl = endpointUrl;

		// Initialize the entity.
		string uri = getEntityUri(_fileUrl);

		if(!uri.empty())
		{
        	_entity->uri = uri;
		}

		// Remember that we need to add the file to the index.
		_hasDataObject = !uri.empty();

		time_t now;
		time(&now);

		_activity->setStartTime(now);

		// Initialize the associations.
		vector<AssociationRef>::iterator it = _associations.begin();

		while (it != _associations.end())
		{
			AssociationRef association = *it;

            if (!association->uri.empty() || fetchAssociationUri(association))
			{
                _activity->addAssociation(association);
            }

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

			transmit();
		}
	}

	void ActivityLog::addAssociation(const char* roleUri)
	{
		AssociationRef a = AssociationRef(new Association(""));
		a->setRole(RoleRef(new Role(roleUri)));

		_associations.push_back(a);
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

	void ActivityLog::addAssociation(const char* roleUri, string agentUri, string version)
	{
        if (strcmp(roleUri, art::SOFTWARE) != 0 || version.empty()) return;

        stringstream str;
        str << agentUri << "/" << version;

        SoftwareAssociationRef a = SoftwareAssociationRef(new SoftwareAssociation(str.str()));
        a->setAgent(AgentRef(new Agent(agentUri)));
        a->setRole(RoleRef(new Role(roleUri)));
        a->setVersion(version);

        _associations.push_back(a);
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
    
    void ActivityLog::createDerivation(ImageRef image, std::string path)
    {
        // Get time from last save for end/begin of activities
        //Entity oldEntity = _entity;

        
        
        
        
        // TODO:
        // - close old activity
        // - create new activity
        // - set new entity
        // - set entity derived from old entity
    }

	ImageRef ActivityLog::getDocument()
	{
		return _entity;
	}

	string ActivityLog::getEntityUri(string fileUrl)
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
        }
        catch (...)
        {
			uri = "";
        }

		return uri;
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

                if (influence->is(prov::Generation))
                {
                    EntityRef r = _activity->getEntity(entity->uri);

                    if (r)
                    {
                        _activity->addGenerated(r);

                        r->addProperty(prov::qualifiedGeneration, influence);
                    }
                    else
                    {
                        _activity->addGenerated(entity);

                        entity->addProperty(prov::qualifiedGeneration, influence);
                    }
                }
                else if (influence->is(prov::Derivation))
                {
                    EntityRef r = _activity->getEntity(entity->uri);

                    if (r)
                    {
                        _activity->addUsed(r);

                        r->addProperty(prov::qualifiedDerivation, influence);
                    }
                    else
                    {
                        _activity->addUsed(entity);

                        entity->addProperty(prov::qualifiedDerivation, influence);
                    }
                }
                else if (influence->is(prov::Revision) || influence->is(art::Save) || influence->is(art::SaveAs))
                {
                    EntityRef r = _activity->getEntity(entity->uri);

                    if (r)
                    {
                        _activity->addUsed(r);

                        r->addProperty(prov::qualifiedRevision, influence);
                    }
                    else
                    {
                        _activity->addUsed(entity);

                        entity->addProperty(prov::qualifiedRevision, influence);
                    }
                }
                else if (influence->is(prov::Invalidation))
                {
                    EntityRef r = _activity->getEntity(entity->uri);

                    if (r)
                    {
                        _activity->addInvalidated(r);

                        r->addProperty(prov::qualifiedInvalidation, influence);
                    }
                    else
                    {
                        _activity->addInvalidated(entity);

                        entity->addProperty(prov::qualifiedInvalidation, influence);
                    }
                }
                else
                {
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

                    entity->removeProperty(prov::qualifiedGeneration, influence);
                }
                else if (influence->is(prov::Derivation))
                {
                    _activity->removeUsed(entity);

                    entity->removeProperty(prov::qualifiedDerivation, influence);
                }
                else if (influence->is(prov::Revision))
                {
                    _activity->removeUsed(entity);

                    entity->removeProperty(prov::qualifiedRevision, influence);
                }
                else if (influence->is(prov::Invalidation))
                {
                    _activity->removeInvalidated(entity);

                    entity->removeProperty(prov::qualifiedInvalidation, influence);
                }
                else
                {
                    entity->removeProperty(prov::qualifiedInfluence, influence);
                }

                influence->clearActivity();
            }
        }
	}

	string ActivityLog::getRenderOutputPath()
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

    void ActivityLog::transmit()
    {
        stringstream stream;

        Serializer s;

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
#if _DEBUG
            logRequest(url.str(), "0ms", requestData);

            _transmitCount++;
#endif
        }

		clear();
	}

#if _DEBUG
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

    bool ActivityLog::createDataObject(std::string path)
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
