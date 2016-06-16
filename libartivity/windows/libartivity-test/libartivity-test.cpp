// libartivity-test.cpp : Defines the entry point for the console application.

#include "stdafx.h"
#include <string>
#include <artivity.h>

using namespace std;
using namespace artivity;

int _tmain(int argc, _TCHAR* argv[])
{
	ActivityLog* log = new ActivityLog("http://localhost:8262/artivity/api/1.0/");
	log->connect();

	cout << "Press any key to stop.." << endl;

	string line;

	getline(std::cin, line);

	return 0;
}

