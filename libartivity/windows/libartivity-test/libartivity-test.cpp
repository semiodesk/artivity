// libartivity-test.cpp : Defines the entry point for the console application.

#include "stdafx.h"
#include <string>
#include <artivity.h>

using namespace std;
using namespace artivity;

int _tmain(int argc, _TCHAR* argv[])
{
	ActivityLog* log = new ActivityLog("http://localhost:8262/artivity/api/1.0");

	if (!log->setAgent("http://adobe.com/products/photoshop", "2015.1"))
	{
		// TODO: We cannot yet distinguish if an agent is disabled or not installed.
	}

	string path = "C:\Users\Sebastian\Desktop\Test.ai";

	log->setDocument(nfo::VectorImage, path);

	cout << "Press any key to stop.." << endl;

	string line;

	getline(std::cin, line);

	return 0;
}

