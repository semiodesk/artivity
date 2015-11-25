
#include <string>
#include "UriGenerator.h"
#include <random>

namespace artivity
{

	string UriGenerator::getRandomId(size_t length)
	{
#ifdef WIN32

		std::random_device rd;
		std::mt19937 gen(rd());
		const char alphanum[] = "0123456789abcdefghijklmnopqrstuvwxyz";

		std::uniform_int_distribution<> dis(0, 25);
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
}