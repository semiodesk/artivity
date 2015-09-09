#ifndef PROV_H
#define PROV_H

#include "../Property.h"

#define PROV(label) "http://www.w3.org/ns/prov#"label;

namespace artivity
{
    namespace prov
    {
        static const Resource Activity = PROV("Activity");
        static const Resource ActivityInfluence = PROV("ActivityInfluence");
        static const Resource Agent = PROV("Agent");
        static const Resource AgentInfluence = PROV("AgentInfluence");
        static const Resource Association = PROV("Association");
        static const Resource Attribution = PROV("Attribution");
        static const Resource Bundle = PROV("Bundle")
        static const Resource Collection = PROV("Collection");
        static const Resource Communication = PROV("Communication");
        static const Resource Delegation = PROV("Delegation");
        static const Resource Derivation = PROV("Derivation");
        static const Resource EmptyCollection = PROV("EmptyCollection");
        static const Resource End = PROV("End");
        static const Resource Start = PROV("Start");
        static const Resource Entity = PROV("Entity");
        static const Resource EntityInfluence = PROV("EntityInfluence");
        static const Resource Influence = PROV("Influence");
        static const Resource InstantaneousEvent = PROV("InstantaneousEvent");
        static const Resource Location = PROV("Location");
        static const Resource Organization = PROV("Organization");
        static const Resource Person = PROV("Person");
        static const Resource Plan = PROV("Plan");
        static const Resource PrimarySource = PROV("PrimarySource");
        static const Resource Revision = PROV("Revision");
        static const Resource Role = PROV("Role");
        static const Resource SoftwareAgent = PROV("SoftwareAgent");
        static const Resource Usage = PROV("Usage");
        static const Property actedOnBehalfOf = PROV("actedOnBehalfOf");
        static const Property activity = PROV("activity");
        static const Property agent = PROV("agent");
        static const Property alternateOf = PROV("alternateOf");
        static const Property aq = PROV("aq");
        static const Property atLocation = PROV("atLocation");
        static const Property atTime = PROV("atTime");
        static const Property category = PROV("category");
        static const Property component = PROV("component");
        static const Property constraints = PROV("constraints");
        static const Property definition = PROV("definition");
        static const Property dm = PROV("dm");
        static const Property editorialNote = PROV("editorialNote");
        static const Property editorsDefinition = PROV("editorsDefinition");
        static const Property endedAtTime = PROV("endedAtTime");
        static const Property entity = PROV("entity");
        static const Property generated = PROV("generated");
        static const Property generatedAtTime = PROV("generatedAtTime");
        static const Property hadActivity = PROV("hadActivity");
        static const Property hadGeneration = PROV("hadGeneration");
        static const Property hadPlan = PROV("hadPlan");
        static const Property hadPrimarySource = PROV("hadPrimarySource");
        static const Property hadRole = PROV("hadRole");
        static const Property hadUsage = PROV("hadUsage");
        static const Property influenced = PROV("influenced");
        static const Property influencer = PROV("influencer");
        static const Property invalidated = PROV("invalidated");
        static const Property invalidatedAtTime = PROV("invalidatedAtTime");
        static const Property inverse = PROV("inverse");
        static const Property n = PROV("n");
        static const Property order = PROV("order");
        static const Property qualifiedAssociation = PROV("qualifiedAssociation");
        static const Property qualifiedAttribution = PROV("qualifiedAttribution");
        static const Property qualifiedCommunication = PROV("qualifiedCommunication");
        static const Property qualifiedDelegation = PROV("qualifiedDelegation");
        static const Property qualifiedEnd = PROV("qualifiedEnd");
        static const Property qualifiedForm = PROV("qualifiedForm");
        static const Property qualifiedGeneration = PROV("qualifiedGeneration");
        static const Property qualifiedInfluence = PROV("qualifiedInfluence");
        static const Property qualifiedInvalidation = PROV("qualifiedInvalidation");
        static const Property qualifiedPrimarySource = PROV("qualifiedPrimarySource");
        static const Property qualifiedQuotation = PROV("qualifiedQuotation");
        static const Property qualifiedRevision = PROV("qualifiedRevision");
        static const Property qualifiedStart = PROV("qualifiedStart");
        static const Property qualifiedUsage = PROV("qualifiedUsage");
        static const Property specializationOf = PROV("specializationOf");
        static const Property startedAtTime = PROV("startedAtTime");
        static const Property todo = PROV("todo");
        static const Property used = PROV("used");
        static const Property value = PROV("value");
        static const Property wasAssociatedWith = PROV("wasAssociatedWith");
        static const Property wasDerivedFrom = PROV("wasDerivedFrom");
        static const Property wasEndedBy = PROV("wasEndedBy");
        static const Property wasGeneratedBy = PROV("wasGeneratedBy");
        static const Property wasInfluencedBy = PROV("wasInfluencedBy");
        static const Property wasInformedBy = PROV("wasInformedBy");
        static const Property wasInvalidatedBy = PROV("wasInvalidatedBy");
        static const Property wasRevisionOf = PROV("wasRevisionOf");
        static const Property wasStartedBy = PROV("wasStartedBy");
    }
}

#endif // PROV_H
