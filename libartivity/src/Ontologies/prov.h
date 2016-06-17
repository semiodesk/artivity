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
// Copyright (c) Semiodesk GmbH 2016

#ifndef PROV_H
#define PROV_H

#define PROV(label) "http://www.w3.org/ns/prov#" label;

namespace artivity
{
    namespace prov
    {
		static const char* NS_PREFIX = "prov:";
		static const char* NS_URI = "http://www.w3.org/ns/prov#";

        static const char* Activity = PROV("Activity");
        static const char* ActivityInfluence = PROV("ActivityInfluence");
        static const char* Generation = PROV("Generation");
        static const char* Invalidation = PROV("Invalidation");
        static const char* Agent = PROV("Agent");
        static const char* AgentInfluence = PROV("AgentInfluence");
        static const char* Association = PROV("Association");
        static const char* Attribution = PROV("Attribution");
        static const char* Bundle = PROV("Bundle")
        static const char* Collection = PROV("Collection");
        static const char* Communication = PROV("Communication");
        static const char* Delegation = PROV("Delegation");
        static const char* Derivation = PROV("Derivation");
        static const char* EmptyCollection = PROV("EmptyCollection");
        static const char* End = PROV("End");
        static const char* Start = PROV("Start");
        static const char* Entity = PROV("Entity");
        static const char* EntityInfluence = PROV("EntityInfluence");
        static const char* Influence = PROV("Influence");
        static const char* InstantaneousEvent = PROV("InstantaneousEvent");
        static const char* Location = PROV("Location");
        static const char* Organization = PROV("Organization");
        static const char* Person = PROV("Person");
        static const char* Plan = PROV("Plan");
        static const char* PrimarySource = PROV("PrimarySource");
        static const char* Revision = PROV("Revision");
        static const char* Role = PROV("Role");
        static const char* SoftwareAgent = PROV("SoftwareAgent");
        static const char* Usage = PROV("Usage");
        static const char* actedOnBehalfOf = PROV("actedOnBehalfOf");
        static const char* activity = PROV("activity");
        static const char* agent = PROV("agent");
        static const char* alternateOf = PROV("alternateOf");
        static const char* aq = PROV("aq");
        static const char* atLocation = PROV("atLocation");
        static const char* atTime = PROV("atTime");
        static const char* category = PROV("category");
        static const char* component = PROV("component");
        static const char* constraints = PROV("constraints");
        static const char* definition = PROV("definition");
        static const char* dm = PROV("dm");
        static const char* editorialNote = PROV("editorialNote");
        static const char* editorsDefinition = PROV("editorsDefinition");
        static const char* endedAtTime = PROV("endedAtTime");
        static const char* entity = PROV("entity");
        static const char* generated = PROV("generated");
        static const char* generatedAtTime = PROV("generatedAtTime");
        static const char* hadActivity = PROV("hadActivity");
        static const char* hadGeneration = PROV("hadGeneration");
        static const char* hadPlan = PROV("hadPlan");
        static const char* hadPrimarySource = PROV("hadPrimarySource");
        static const char* hadRole = PROV("hadRole");
        static const char* hadUsage = PROV("hadUsage");
        static const char* influenced = PROV("influenced");
        static const char* influencer = PROV("influencer");
        static const char* invalidated = PROV("invalidated");
        static const char* invalidatedAtTime = PROV("invalidatedAtTime");
        static const char* inverse = PROV("inverse");
        static const char* n = PROV("n");
        static const char* order = PROV("order");
        static const char* qualifiedAssociation = PROV("qualifiedAssociation");
        static const char* qualifiedAttribution = PROV("qualifiedAttribution");
        static const char* qualifiedCommunication = PROV("qualifiedCommunication");
        static const char* qualifiedDelegation = PROV("qualifiedDelegation");
        static const char* qualifiedEnd = PROV("qualifiedEnd");
        static const char* qualifiedForm = PROV("qualifiedForm");
        static const char* qualifiedGeneration = PROV("qualifiedGeneration");
        static const char* qualifiedInfluence = PROV("qualifiedInfluence");
        static const char* qualifiedInvalidation = PROV("qualifiedInvalidation");
        static const char* qualifiedPrimarySource = PROV("qualifiedPrimarySource");
        static const char* qualifiedQuotation = PROV("qualifiedQuotation");
        static const char* qualifiedRevision = PROV("qualifiedRevision");
        static const char* qualifiedStart = PROV("qualifiedStart");
        static const char* qualifiedUsage = PROV("qualifiedUsage");
        static const char* specializationOf = PROV("specializationOf");
        static const char* startedAtTime = PROV("startedAtTime");
        static const char* todo = PROV("todo");
        static const char* used = PROV("used");
        static const char* value = PROV("value");
        static const char* wasAssociatedWith = PROV("wasAssociatedWith");
        static const char* wasDerivedFrom = PROV("wasDerivedFrom");
        static const char* wasEndedBy = PROV("wasEndedBy");
        static const char* wasGeneratedBy = PROV("wasGeneratedBy");
        static const char* wasInfluencedBy = PROV("wasInfluencedBy");
        static const char* wasInformedBy = PROV("wasInformedBy");
        static const char* wasInvalidatedBy = PROV("wasInvalidatedBy");
        static const char* wasRevisionOf = PROV("wasRevisionOf");
        static const char* wasStartedBy = PROV("wasStartedBy");
    }
}

#endif // PROV_H
