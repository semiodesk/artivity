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

#include <string>
#include <random>
#include <boost/random/random_device.hpp>
#include <boost/random/uniform_int_distribution.hpp>
#include "UriGenerator.h"

namespace artivity
{
    using namespace std;

	string UriGenerator::getRandomId(unsigned long length)
	{
		boost::random::random_device rd;
		mt19937 gen(rd());
		const char alphanum[] = "0123456789abcdefghijklmnopqrstuvwxyz";

		boost::random::uniform_int_distribution<> dis(0, 25);
		string str = string();

		for (size_t i = 0; i < length; ++i)
		{
			int res = 0;
			str += alphanum[dis(gen)];
		}

		return str;
	}

    string UriGenerator::getUrl(string path)
    {
        stringstream stream;

#if WIN32
        stream << "file:///";
#else
        stream << "file://";
#endif

        stream << UriGenerator::escapePath(path);

        return stream.str();
    }

    string UriGenerator::escapePath(string path)
    {
        CURL* curl = curl_easy_init();

        stringstream result;
        string token;

        for (int i = 0; i < path.length(); i++)
        {
            char c = path[i];

            if (c == '/' || c == '\\' || c == ':')
            {
                if (!token.empty())
                {
                    char* t = curl_easy_escape(curl, token.c_str(), (int)token.length());

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
            char* t = curl_easy_escape(curl, token.c_str(), (int)token.length());

            result << string(t);

            curl_free(t);
        }

        return result.str();
    }
}
