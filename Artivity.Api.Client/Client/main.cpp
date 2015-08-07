#include <iostream>

#include "Activity.h"
#include "Serializer.h"

#include "Ontologies/as.h"
#include "Ontologies/rdf.h"

using namespace artivity::client;
using namespace artivity::client::ontologies;

int main(int argc, char **argv)
{
    setlocale(LC_ALL, "");
     
    time_t now;
    time(&now);
     
    Activity a0 = Activity("http://localhost:8890/Test#123");
    a0.setDisplayName("Doing something.");
    a0.setTime(now);
    
    cout << Serializer::serialize(a0, N3) << endl;
    
    return 0;
}
