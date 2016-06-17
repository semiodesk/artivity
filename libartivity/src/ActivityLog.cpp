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

namespace artivity
{
	using namespace std;
	using namespace boost;
	using namespace boost::property_tree;

	ActivityLog::ActivityLog(string endpoint)
	{
		_endpoint = endpoint;
		_curl = initializeRequest();
		_associations = new list<AssociationRef>();
		_influences = new list<EntityInfluenceRef>();
		_fileUri = "";
		_fileUrl = "";
	}

	ActivityLog::~ActivityLog()
	{
		// Dereference the activity.
		_activity = NULL;

		// Free all remaining associations.
		_associations->clear();

		delete _associations;

		_associations = NULL;

		// Free all remaining influences.
		_influences->clear();

		delete _influences;

		_influences = NULL;

		curl_easy_cleanup(_curl);
	}

	bool ActivityLog::setAgent(const char* uri, string version)
	{
		AssociationRef user = getAssociation(art::USER);

		if (user == NULL)
		{
			return false;
		}

		AssociationRef software = getAssociation(art::SOFTWARE, uri, version);

		if (software == NULL)
		{
			return false;
		}

		_associations->push_back(user);
		_associations->push_back(software);

		return true;
	}

	AssociationRef ActivityLog::getAssociation(const char* role, const char* agent, string version)
	{
		try
		{
			CURL* curl = initializeRequest();

			stringstream url;
			
			url << _endpoint << "/agents/associations?role=" << role;

			if (agent != NULL && agent[0] != '\0')
			{
				url << "&agent=" << agent;
			}

			if (!version.empty())
			{
				url << "&version=" << version;
			}

			string response;

			executeRequest(curl, url.str(), "", response);

			stringstream stream;
			stream << response;

			if (response.empty())
			{
				return NULL;
			}

			ptree tree;

			read_json(stream, tree);

			string uri = tree.get_child("association").get_value<string>();

			if (uri.empty())
			{
				return NULL;
			}

			AssociationRef association = AssociationRef(new Association(uri));

			_associations->push_back(association);

			return association;
		}
		catch (std::exception const& e)
		{
			cerr << e.what() << endl;

			return NULL;
		}
	}

	void ActivityLog::setDocument(const char* typeUri, string path)
	{
		if (strcmp(typeUri, nfo::RasterImage) != 0 && strcmp(typeUri, nfo::VectorImage) != 0)
		{
			string msg = "Unsupported document type: ";
			msg += typeUri;

			throw std::exception(msg.c_str());
		}

		_fileUrl = "file://" + escapePath(path);

		if (_fileUrl == "")
		{
			// A new file is being created.
			_activity = CreateFileRef(new CreateFile());
		}
		else
		{
			// An existing file is being edited.
			_activity = EditFileRef(new EditFile());
		}

		time_t now;
		time(&now);

		_activity->setStartTime(now);

		AssociationIterator it = _associations->begin();

		while (it != _associations->end())
		{
			_activity->addAssociation(*it);

			it++;
		}

		Serializer* s = new Serializer();

		cout << s->serialize(_activity, N3) << endl;
	}

	void ActivityLog::addInfluence(EntityInfluenceRef influence)
	{
		_influences->push_back(influence);
	}

	void ActivityLog::removeInfluence(EntityInfluenceRef influence)
	{
		_influences->remove(influence);
	}

	string ActivityLog::getThumbnailPath(string fileUrl)
	{
		//CURL* curl = initializeRequest();

		//stringstream url;
		//url << _endpoint << "/thumbnails/path?fileUri=" << escapePath(fileUrl);

		//string response;

		//executeRequest(curl, url.str(), "", response);

		//jsonxx::Array a;

		//bool isValid = a.parse(response);

		//return isValid ? a.get<jsonxx::String>((int)a.size() - 1) : "";

		return "";
	}

	CURL* ActivityLog::initializeRequest()
	{
		CURL* curl = curl_easy_init();

		if (!curl)
		{
			logError(CURLE_FAILED_INIT, "Failed to initialize CURL.");
		}

		return curl;
	}

	long ActivityLog::executeRequest(CURL* curl, string url, string postFields, string& response)
	{
		if (!curl)
		{
			logError(CURLE_FAILED_INIT, "CURL not initialized.");

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

		CURLcode responseCode = curl_easy_perform(curl);

		if (data.len > 0)
		{
			response = data.ptr;
		}

		curl_easy_cleanup(curl);

		if (responseCode == CURLE_OK)
		{
			logInfo(responseCode, url);
		}
		else
		{
			logError(responseCode, url);
		}

		if (postFields.length() > 0)
		{
			cout << postFields << endl << flush;
		}

		return responseCode;
	}

	void ActivityLog::transmit()
	{
		if (_influences->empty())
		{
			return;
		}

		stringstream stream;

		Serializer s;

		EntityInfluenceIterator it = _influences->begin();

		while (it != _influences->end())
		{
			s.serialize(stream, *it, N3);

			it++;
		}

		string requestData = stream.str();
		string responseData;

		CURL* curl = initializeRequest();

		stringstream url;
		url << _endpoint << "/activities";

		executeRequest(curl, url.str(), requestData, responseData);

		clear();
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
		strftime(date_str, 20, "%Y/%m/%d %H:%M:%S", time);
#else
		strftime(date_str, 20, "%Y/%m/%d %H:%M:%S", localtime(&t));
#endif

		return string(date_str);
	}

	string ActivityLog::escapePath(string path)
	{
		stringstream result;
		string token;

		for (int i = 0; i < path.length(); i++)
		{
			char c = path[i];

			if (c == '/' || c == '\\' || c == ':')
			{
				if (!token.empty())
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

		if (!token.empty())
		{
			char* t = curl_easy_escape(_curl, token.c_str(), (int)token.length());

			result << string(t);

			curl_free(t);
		}

		return result.str();
	}

	void ActivityLog::print(ptree const& pt)
	{
		ptree::const_iterator end = pt.end();

		for (ptree::const_iterator it = pt.begin(); it != end; ++it)
		{
			cout << it->first << ": " << it->second.get_value<string>() << endl;

			print(it->second);
		}
	}
}
