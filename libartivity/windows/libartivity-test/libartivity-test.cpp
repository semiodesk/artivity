// libartivity-test.cpp : Defines the entry point for the console application.

#include "stdafx.h"
#include <string>
#include <artivity.h>
#include <chrono>

using namespace std;
using namespace std::chrono;
using namespace artivity;

int _tmain(int argc, _TCHAR* argv[])
{
	high_resolution_clock::time_point t1 = high_resolution_clock::now();

	// We pass the server URL to the log upon creation.
	ActivityLog* log = new ActivityLog();

	// Set the agent association with the current agent version.
	log->addAssociation(art::USER);
	log->addAssociation(art::SOFTWARE, "http://adobe.com/products/photoshop", "2015.1");

	VectorImageRef image = VectorImageRef(new VectorImage());

	// Set the file and retrive, if possible, the URI of the vector image.
	log->setFile(image, "C:/Users/Sebastian/Desktop/Hello 2.ai");

	if (!log->connect("http://localhost:8262/artivity/api/1.0"))
	{
		// The log could not be set up to transmit properly.
		// Either the agent is not properly installed or logging is disabled.
		cout << "Log not ready." << endl << endl;
		cout << "Press any key to stop.." << endl;

		string line;

		getline(std::cin, line);

		return -1;
	}

	high_resolution_clock::time_point t2 = high_resolution_clock::now();

	auto duration = duration_cast<milliseconds>(t2 - t1).count();

	cout << "Initialization took " << duration << "ms" << endl << endl;

	log->logInfo("http://localhost:8262/artivity/api/1.0/activities");

	CanvasRef canvas = CanvasRef(new Canvas());
	canvas->setWidth(200);
	canvas->setHeight(200);
	canvas->setLengthUnit(ResourceRef(new Resource(art::mm)));

	GenerationRef generation = GenerationRef(new Generation());
	generation->addGenerated(canvas);

	log->addInfluence(generation);
	log->transmit();

	log->logInfo("http://localhost:8262/artivity/api/1.0/activities");

	BoundingRectangleRef bounds = BoundingRectangleRef(new BoundingRectangle());
	bounds->setWidth(100);
	bounds->setHeight(100);
	bounds->setPosition(50, 50);

	UndoRef undo = UndoRef(new Undo());
	undo->setBoundaries(bounds);

	log->addInfluence(undo);
	log->transmit();

	log->logInfo("http://localhost:8262/artivity/api/1.0/activities");

	log->close();

	high_resolution_clock::time_point t3 = high_resolution_clock::now();

	duration = duration_cast<milliseconds>(t3 - t1).count();

	cout << "Total roundtrip took " << duration << "ms" << endl;
	cout << "Press any key to stop.." << endl;

	string line;

	getline(std::cin, line);

	return 0;
}

