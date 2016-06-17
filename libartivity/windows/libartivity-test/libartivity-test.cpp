// libartivity-test.cpp : Defines the entry point for the console application.

#include "stdafx.h"
#include <string>
#include <artivity.h>

using namespace std;
using namespace artivity;

int _tmain(int argc, _TCHAR* argv[])
{
	VectorImageRef image = VectorImageRef(new VectorImage());

	// We pass the server URL to the log upon creation.
	ActivityLog* log = new ActivityLog("http://localhost:8262/artivity/api/1.0");

	// Set the agent association with the current agent version.
	log->setAgent("http://adobe.com/products/photoshop", "2015.1");

	// Set the file and retrive, if possible, the URI of the vector image.
	log->setFile(image, "C:/Users/Sebastian/Desktop/Test.ai");

	if (!log->ready())
	{
		// The log could not be set up to transmit properly.
		// Either the agent is not properly installed or logging is disabled.
		cout << "Log not ready." << endl << endl;
		cout << "Press any key to stop.." << endl;

		string line;

		getline(std::cin, line);

		return -1;
	}

	CanvasRef canvas = CanvasRef(new Canvas());
	canvas->setWidth(200);
	canvas->setHeight(200);
	canvas->setLengthUnit(ResourceRef(new Resource(art::mm)));

	image->addCanvas(canvas);

	GenerationRef generation = GenerationRef(new Generation());
	generation->addGenerated(canvas);

	log->addInfluence(generation);
	log->transmit();

	cout << "Press any key to stop.." << endl;

	string line;

	getline(std::cin, line);

	return 0;
}

