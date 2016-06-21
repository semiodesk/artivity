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
#include "UriGenerator.h"
#include <random>

namespace artivity
{
    using namespace std;

	string UriGenerator::getRandomId(unsigned long length)
	{
#if defined(WIN32) || defined(CXX11)

		random_device rd;
		mt19937 gen(rd());
		const char alphanum[] = "0123456789abcdefghijklmnopqrstuvwxyz";

		uniform_int_distribution<> dis(0, 25);
		string str = string();

		for (size_t i = 0; i < length; ++i)
		{
			int res = 0;
			str += alphanum[dis(gen)];
		}

		return str;
#else
		static bool initialized;
		static struct random_data rand_buf;
		static char state[32];

		if (!initialized)
		{
			initstate_r(0, state, sizeof(state), &rand_buf);
			initialized = true;
			srandom_r(time(NULL), &rand_buf);
		}

		static const char alphanum[] = "0123456789abcdefghijklmnopqrstuvwxyz";

		string str = string();

		for (size_t i = 0; i < length; ++i)
		{
			int res = 0;
			int var = random_r(&rand_buf, &res);
			str += alphanum[res % 25];
		}

		return str;
#endif
	}

    string UriGenerator::escapePath(string path)
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
}
