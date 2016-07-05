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

#define prov(label) "prov:" label;
#define PROV(label) "http://www.w3.org/ns/prov#" label;

namespace artivity
{
    namespace prov
    {
		static const char* NS_PREFIX = prov("");
		static const char* NS_URI = PROV("");

        static const char* Activity = prov("Activity");
        static const char* ActivityInfluence = prov("ActivityInfluence");
        static const char* Generation = prov("Generation");
        static const char* Invalidation = prov("Invalidation");
        static const char* Agent = prov("Agent");
        static const char* AgentInfluence = prov("AgentInfluence");
        static const char* Association = prov("Association");
        static const char* Attribution = prov("Attribution");
        static const char* Bundle = prov("Bundle")
        static const char* Collection = prov("Collection");
        static const char* Communication = prov("Communication");
        static const char* Delegation = prov("Delegation");
        static const char* Derivation = prov("Derivation");
        static const char* EmptyCollection = prov("EmptyCollection");
        static const char* End = prov("End");
        static const char* Start = prov("Start");
        static const char* Entity = prov("Entity");
        static const char* EntityInfluence = prov("EntityInfluence");
        static const char* Influence = prov("Influence");
        static const char* InstantaneousEvent = prov("InstantaneousEvent");
        static const char* Location = prov("Location");
        static const char* Organization = prov("Organization");
        static const char* Person = prov("Person");
        static const char* Plan = prov("Plan");
        static const char* PrimarySource = prov("PrimarySource");
        static const char* Revision = prov("Revision");
        static const char* Role = prov("Role");
        static const char* SoftwareAgent = prov("SoftwareAgent");
        static const char* Usage = prov("Usage");
        static const char* actedOnBehalfOf = prov("actedOnBehalfOf");
        static const char* activity = prov("activity");
        static const char* agent = prov("agent");
        static const char* alternateOf = prov("alternateOf");
        static const char* aq = prov("aq");
        static const char* atLocation = prov("atLocation");
        static const char* atTime = prov("atTime");
        static const char* category = prov("category");
        static const char* component = prov("component");
        static const char* constraints = prov("constraints");
        static const char* definition = prov("definition");
        static const char* dm = prov("dm");
        static const char* editorialNote = prov("editorialNote");
        static const char* editorsDefinition = prov("editorsDefinition");
        static const char* endedAtTime = prov("endedAtTime");
        static const char* entity = prov("entity");
        static const char* generated = prov("generated");
        static const char* generatedAtTime = prov("generatedAtTime");
        static const char* hadActivity = prov("hadActivity");
        static const char* hadGeneration = prov("hadGeneration");
        static const char* hadPlan = prov("hadPlan");
        static const char* hadPrimarySource = prov("hadPrimarySource");
        static const char* hadRole = prov("hadRole");
        static const char* hadUsage = prov("hadUsage");
        static const char* influenced = prov("influenced");
        static const char* influencer = prov("influencer");
        static const char* invalidated = prov("invalidated");
        static const char* invalidatedAtTime = prov("invalidatedAtTime");
        static const char* inverse = prov("inverse");
        static const char* n = prov("n");
        static const char* order = prov("order");
        static const char* qualifiedAssociation = prov("qualifiedAssociation");
        static const char* qualifiedAttribution = prov("qualifiedAttribution");
        static const char* qualifiedCommunication = prov("qualifiedCommunication");
        static const char* qualifiedDelegation = prov("qualifiedDelegation");
        static const char* qualifiedDerivation = prov("qualifiedDerivation");
        static const char* qualifiedEnd = prov("qualifiedEnd");
        static const char* qualifiedForm = prov("qualifiedForm");
        static const char* qualifiedGeneration = prov("qualifiedGeneration");
        static const char* qualifiedInfluence = prov("qualifiedInfluence");
        static const char* qualifiedInvalidation = prov("qualifiedInvalidation");
        static const char* qualifiedPrimarySource = prov("qualifiedPrimarySource");
        static const char* qualifiedQuotation = prov("qualifiedQuotation");
        static const char* qualifiedRevision = prov("qualifiedRevision");
        static const char* qualifiedStart = prov("qualifiedStart");
        static const char* qualifiedUsage = prov("qualifiedUsage");
        static const char* specializationOf = prov("specializationOf");
        static const char* startedAtTime = prov("startedAtTime");
        static const char* todo = prov("todo");
        static const char* used = prov("used");
        static const char* value = prov("value");
        static const char* wasAssociatedWith = prov("wasAssociatedWith");
        static const char* wasDerivedFrom = prov("wasDerivedFrom");
        static const char* wasEndedBy = prov("wasEndedBy");
        static const char* wasGeneratedBy = prov("wasGeneratedBy");
        static const char* wasInfluencedBy = prov("wasInfluencedBy");
        static const char* wasInformedBy = prov("wasInformedBy");
        static const char* wasInvalidatedBy = prov("wasInvalidatedBy");
        static const char* wasRevisionOf = prov("wasRevisionOf");
        static const char* wasStartedBy = prov("wasStartedBy");
    }
}

#endif // PROV_H
