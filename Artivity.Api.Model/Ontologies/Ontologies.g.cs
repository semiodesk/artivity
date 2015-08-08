// Attention: This file is generated. Any modifications will eventually be overwritten.
// Date: 08.08.2015 13:35:31

using System;
using System.Collections.Generic;
using System.Text;
using Semiodesk.Trinity;

namespace Artivity.Model
{
	
///<summary>
///The RDF Concepts Vocabulary (RDF)
///
///</summary>
public class rdf : Ontology
{
    public static readonly Uri Namespace = new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "rdf";
    public static string GetPrefix() { return Prefix; }     

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#"/>
    ///</summary>
    public static readonly Resource _22_rdf_syntax_ns = new Resource(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));    

    ///<summary>
    ///The datatype of RDF literals storing fragments of HTML content
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#HTML"/>
    ///</summary>
    public static readonly Resource HTML = new Resource(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#HTML"));    

    ///<summary>
    ///The datatype of language-tagged string values
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#langString"/>
    ///</summary>
    public static readonly Resource langString = new Resource(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#langString"));    

    ///<summary>
    ///The class of plain (i.e. untyped) literal values, as used in RIF and OWL 2
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#PlainLiteral"/>
    ///</summary>
    public static readonly Resource PlainLiteral = new Resource(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#PlainLiteral"));    

    ///<summary>
    ///The subject is an instance of a class.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#type"/>
    ///</summary>
    public static readonly Property type = new Property(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"));    

    ///<summary>
    ///The class of RDF properties.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#Property"/>
    ///</summary>
    public static readonly Class Property = new Class(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#Property"));    

    ///<summary>
    ///The class of RDF statements.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#Statement"/>
    ///</summary>
    public static readonly Class Statement = new Class(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#Statement"));    

    ///<summary>
    ///The subject of the subject RDF statement.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#subject"/>
    ///</summary>
    public static readonly Property subject = new Property(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#subject"));    

    ///<summary>
    ///The predicate of the subject RDF statement.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#predicate"/>
    ///</summary>
    public static readonly Property predicate = new Property(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#predicate"));    

    ///<summary>
    ///The object of the subject RDF statement.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#object"/>
    ///</summary>
    public static readonly Property _object = new Property(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#object"));    

    ///<summary>
    ///The class of unordered containers.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#Bag"/>
    ///</summary>
    public static readonly Class Bag = new Class(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#Bag"));    

    ///<summary>
    ///The class of ordered containers.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#Seq"/>
    ///</summary>
    public static readonly Class Seq = new Class(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#Seq"));    

    ///<summary>
    ///The class of containers of alternatives.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#Alt"/>
    ///</summary>
    public static readonly Class Alt = new Class(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#Alt"));    

    ///<summary>
    ///Idiomatic property used for structured values.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#value"/>
    ///</summary>
    public static readonly Property value = new Property(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#value"));    

    ///<summary>
    ///The class of RDF Lists.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#List"/>
    ///</summary>
    public static readonly Class List = new Class(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#List"));    

    ///<summary>
    ///The empty list, with no items in it. If the rest of a list is nil then the list has no more items in it.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#nil"/>
    ///</summary>
    public static readonly Resource nil = new Resource(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#nil"));    

    ///<summary>
    ///The first item in the subject RDF list.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#first"/>
    ///</summary>
    public static readonly Property first = new Property(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#first"));    

    ///<summary>
    ///The rest of the subject RDF list after the first item.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#rest"/>
    ///</summary>
    public static readonly Property rest = new Property(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#rest"));    

    ///<summary>
    ///The datatype of XML literal values.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#XMLLiteral"/>
    ///</summary>
    public static readonly Resource XMLLiteral = new Resource(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#XMLLiteral"));
}
///<summary>
///The RDF Concepts Vocabulary (RDF)
///
///</summary>
public static class RDF
{
    public static readonly Uri Namespace = new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "RDF";
    public static string GetPrefix() { return Prefix; } 

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#"/>
    ///</summary>
    public const string _22_rdf_syntax_ns = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";

    ///<summary>
    ///The datatype of RDF literals storing fragments of HTML content
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#HTML"/>
    ///</summary>
    public const string HTML = "http://www.w3.org/1999/02/22-rdf-syntax-ns#HTML";

    ///<summary>
    ///The datatype of language-tagged string values
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#langString"/>
    ///</summary>
    public const string langString = "http://www.w3.org/1999/02/22-rdf-syntax-ns#langString";

    ///<summary>
    ///The class of plain (i.e. untyped) literal values, as used in RIF and OWL 2
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#PlainLiteral"/>
    ///</summary>
    public const string PlainLiteral = "http://www.w3.org/1999/02/22-rdf-syntax-ns#PlainLiteral";

    ///<summary>
    ///The subject is an instance of a class.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#type"/>
    ///</summary>
    public const string type = "http://www.w3.org/1999/02/22-rdf-syntax-ns#type";

    ///<summary>
    ///The class of RDF properties.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#Property"/>
    ///</summary>
    public const string Property = "http://www.w3.org/1999/02/22-rdf-syntax-ns#Property";

    ///<summary>
    ///The class of RDF statements.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#Statement"/>
    ///</summary>
    public const string Statement = "http://www.w3.org/1999/02/22-rdf-syntax-ns#Statement";

    ///<summary>
    ///The subject of the subject RDF statement.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#subject"/>
    ///</summary>
    public const string subject = "http://www.w3.org/1999/02/22-rdf-syntax-ns#subject";

    ///<summary>
    ///The predicate of the subject RDF statement.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#predicate"/>
    ///</summary>
    public const string predicate = "http://www.w3.org/1999/02/22-rdf-syntax-ns#predicate";

    ///<summary>
    ///The object of the subject RDF statement.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#object"/>
    ///</summary>
    public const string _object = "http://www.w3.org/1999/02/22-rdf-syntax-ns#object";

    ///<summary>
    ///The class of unordered containers.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#Bag"/>
    ///</summary>
    public const string Bag = "http://www.w3.org/1999/02/22-rdf-syntax-ns#Bag";

    ///<summary>
    ///The class of ordered containers.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#Seq"/>
    ///</summary>
    public const string Seq = "http://www.w3.org/1999/02/22-rdf-syntax-ns#Seq";

    ///<summary>
    ///The class of containers of alternatives.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#Alt"/>
    ///</summary>
    public const string Alt = "http://www.w3.org/1999/02/22-rdf-syntax-ns#Alt";

    ///<summary>
    ///Idiomatic property used for structured values.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#value"/>
    ///</summary>
    public const string value = "http://www.w3.org/1999/02/22-rdf-syntax-ns#value";

    ///<summary>
    ///The class of RDF Lists.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#List"/>
    ///</summary>
    public const string List = "http://www.w3.org/1999/02/22-rdf-syntax-ns#List";

    ///<summary>
    ///The empty list, with no items in it. If the rest of a list is nil then the list has no more items in it.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#nil"/>
    ///</summary>
    public const string nil = "http://www.w3.org/1999/02/22-rdf-syntax-ns#nil";

    ///<summary>
    ///The first item in the subject RDF list.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#first"/>
    ///</summary>
    public const string first = "http://www.w3.org/1999/02/22-rdf-syntax-ns#first";

    ///<summary>
    ///The rest of the subject RDF list after the first item.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#rest"/>
    ///</summary>
    public const string rest = "http://www.w3.org/1999/02/22-rdf-syntax-ns#rest";

    ///<summary>
    ///The datatype of XML literal values.
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#XMLLiteral"/>
    ///</summary>
    public const string XMLLiteral = "http://www.w3.org/1999/02/22-rdf-syntax-ns#XMLLiteral";
}
///<summary>
///The RDF Schema vocabulary (RDFS)
///
///</summary>
public class rdfs : Ontology
{
    public static readonly Uri Namespace = new Uri("http://www.w3.org/2000/01/rdf-schema#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "rdfs";
    public static string GetPrefix() { return Prefix; }     

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#"/>
    ///</summary>
    public static readonly Resource rdf_schema = new Resource(new Uri("http://www.w3.org/2000/01/rdf-schema#"));    

    ///<summary>
    ///The class resource, everything.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#Resource"/>
    ///</summary>
    public static readonly Class Resource = new Class(new Uri("http://www.w3.org/2000/01/rdf-schema#Resource"));    

    ///<summary>
    ///The class of classes.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#Class"/>
    ///</summary>
    public static readonly Class Class = new Class(new Uri("http://www.w3.org/2000/01/rdf-schema#Class"));    

    ///<summary>
    ///The subject is a subclass of a class.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#subClassOf"/>
    ///</summary>
    public static readonly Property subClassOf = new Property(new Uri("http://www.w3.org/2000/01/rdf-schema#subClassOf"));    

    ///<summary>
    ///The subject is a subproperty of a property.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#subPropertyOf"/>
    ///</summary>
    public static readonly Property subPropertyOf = new Property(new Uri("http://www.w3.org/2000/01/rdf-schema#subPropertyOf"));    

    ///<summary>
    ///A description of the subject resource.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#comment"/>
    ///</summary>
    public static readonly Property comment = new Property(new Uri("http://www.w3.org/2000/01/rdf-schema#comment"));    

    ///<summary>
    ///A human-readable name for the subject.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#label"/>
    ///</summary>
    public static readonly Property label = new Property(new Uri("http://www.w3.org/2000/01/rdf-schema#label"));    

    ///<summary>
    ///A domain of the subject property.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#domain"/>
    ///</summary>
    public static readonly Property domain = new Property(new Uri("http://www.w3.org/2000/01/rdf-schema#domain"));    

    ///<summary>
    ///A range of the subject property.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#range"/>
    ///</summary>
    public static readonly Property range = new Property(new Uri("http://www.w3.org/2000/01/rdf-schema#range"));    

    ///<summary>
    ///Further information about the subject resource.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#seeAlso"/>
    ///</summary>
    public static readonly Property seeAlso = new Property(new Uri("http://www.w3.org/2000/01/rdf-schema#seeAlso"));    

    ///<summary>
    ///The defininition of the subject resource.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#isDefinedBy"/>
    ///</summary>
    public static readonly Property isDefinedBy = new Property(new Uri("http://www.w3.org/2000/01/rdf-schema#isDefinedBy"));    

    ///<summary>
    ///The class of literal values, eg. textual strings and integers.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#Literal"/>
    ///</summary>
    public static readonly Class Literal = new Class(new Uri("http://www.w3.org/2000/01/rdf-schema#Literal"));    

    ///<summary>
    ///The class of RDF containers.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#Container"/>
    ///</summary>
    public static readonly Class Container = new Class(new Uri("http://www.w3.org/2000/01/rdf-schema#Container"));    

    ///<summary>
    ///The class of container membership properties, rdf:_1, rdf:_2, ...,
    ///                    all of which are sub-properties of 'member'.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#ContainerMembershipProperty"/>
    ///</summary>
    public static readonly Class ContainerMembershipProperty = new Class(new Uri("http://www.w3.org/2000/01/rdf-schema#ContainerMembershipProperty"));    

    ///<summary>
    ///A member of the subject resource.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#member"/>
    ///</summary>
    public static readonly Property member = new Property(new Uri("http://www.w3.org/2000/01/rdf-schema#member"));    

    ///<summary>
    ///The class of RDF datatypes.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#Datatype"/>
    ///</summary>
    public static readonly Class Datatype = new Class(new Uri("http://www.w3.org/2000/01/rdf-schema#Datatype"));
}
///<summary>
///The RDF Schema vocabulary (RDFS)
///
///</summary>
public static class RDFS
{
    public static readonly Uri Namespace = new Uri("http://www.w3.org/2000/01/rdf-schema#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "RDFS";
    public static string GetPrefix() { return Prefix; } 

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#"/>
    ///</summary>
    public const string rdf_schema = "http://www.w3.org/2000/01/rdf-schema#";

    ///<summary>
    ///The class resource, everything.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#Resource"/>
    ///</summary>
    public const string Resource = "http://www.w3.org/2000/01/rdf-schema#Resource";

    ///<summary>
    ///The class of classes.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#Class"/>
    ///</summary>
    public const string Class = "http://www.w3.org/2000/01/rdf-schema#Class";

    ///<summary>
    ///The subject is a subclass of a class.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#subClassOf"/>
    ///</summary>
    public const string subClassOf = "http://www.w3.org/2000/01/rdf-schema#subClassOf";

    ///<summary>
    ///The subject is a subproperty of a property.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#subPropertyOf"/>
    ///</summary>
    public const string subPropertyOf = "http://www.w3.org/2000/01/rdf-schema#subPropertyOf";

    ///<summary>
    ///A description of the subject resource.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#comment"/>
    ///</summary>
    public const string comment = "http://www.w3.org/2000/01/rdf-schema#comment";

    ///<summary>
    ///A human-readable name for the subject.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#label"/>
    ///</summary>
    public const string label = "http://www.w3.org/2000/01/rdf-schema#label";

    ///<summary>
    ///A domain of the subject property.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#domain"/>
    ///</summary>
    public const string domain = "http://www.w3.org/2000/01/rdf-schema#domain";

    ///<summary>
    ///A range of the subject property.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#range"/>
    ///</summary>
    public const string range = "http://www.w3.org/2000/01/rdf-schema#range";

    ///<summary>
    ///Further information about the subject resource.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#seeAlso"/>
    ///</summary>
    public const string seeAlso = "http://www.w3.org/2000/01/rdf-schema#seeAlso";

    ///<summary>
    ///The defininition of the subject resource.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#isDefinedBy"/>
    ///</summary>
    public const string isDefinedBy = "http://www.w3.org/2000/01/rdf-schema#isDefinedBy";

    ///<summary>
    ///The class of literal values, eg. textual strings and integers.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#Literal"/>
    ///</summary>
    public const string Literal = "http://www.w3.org/2000/01/rdf-schema#Literal";

    ///<summary>
    ///The class of RDF containers.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#Container"/>
    ///</summary>
    public const string Container = "http://www.w3.org/2000/01/rdf-schema#Container";

    ///<summary>
    ///The class of container membership properties, rdf:_1, rdf:_2, ...,
    ///                    all of which are sub-properties of 'member'.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#ContainerMembershipProperty"/>
    ///</summary>
    public const string ContainerMembershipProperty = "http://www.w3.org/2000/01/rdf-schema#ContainerMembershipProperty";

    ///<summary>
    ///A member of the subject resource.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#member"/>
    ///</summary>
    public const string member = "http://www.w3.org/2000/01/rdf-schema#member";

    ///<summary>
    ///The class of RDF datatypes.
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#Datatype"/>
    ///</summary>
    public const string Datatype = "http://www.w3.org/2000/01/rdf-schema#Datatype";
}
///<summary>
///The OWL 2 Schema vocabulary (OWL 2)
///
///</summary>
public class owl : Ontology
{
    public static readonly Uri Namespace = new Uri("http://www.w3.org/2002/07/owl#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "owl";
    public static string GetPrefix() { return Prefix; }     

    ///<summary>
    ///
    ///  This ontology partially describes the built-in classes and
    ///  properties that together form the basis of the RDF/XML syntax of OWL 2.
    ///  The content of this ontology is based on Tables 6.1 and 6.2
    ///  in Section 6.4 of the OWL 2 RDF-Based Semantics specification,
    ///  available at http://www.w3.org/TR/owl2-rdf-based-semantics/.
    ///  Please note that those tables do not include the different annotations
    ///  (labels, comments and rdfs:isDefinedBy links) used in this file.
    ///  Also note that the descriptions provided in this ontology do not
    ///  provide a complete and correct formal description of either the syntax
    ///  or the semantics of the introduced terms (please see the OWL 2
    ///  recommendations for the complete and normative specifications).
    ///  Furthermore, the information provided by this ontology may be
    ///  misleading if not used with care. This ontology SHOULD NOT be imported
    ///  into OWL ontologies. Importing this file into an OWL 2 DL ontology
    ///  will cause it to become an OWL 2 Full ontology and may have other,
    ///  unexpected, consequences.
    ///   
    ///<see cref="http://www.w3.org/2002/07/owl"/>
    ///</summary>
    public static readonly Resource owl_0 = new Resource(new Uri("http://www.w3.org/2002/07/owl"));    

    ///<summary>
    ///The class of collections of pairwise different individuals.
    ///<see cref="http://www.w3.org/2002/07/owl#AllDifferent"/>
    ///</summary>
    public static readonly Class AllDifferent = new Class(new Uri("http://www.w3.org/2002/07/owl#AllDifferent"));    

    ///<summary>
    ///The class of collections of pairwise disjoint classes.
    ///<see cref="http://www.w3.org/2002/07/owl#AllDisjointClasses"/>
    ///</summary>
    public static readonly Class AllDisjointClasses = new Class(new Uri("http://www.w3.org/2002/07/owl#AllDisjointClasses"));    

    ///<summary>
    ///The class of collections of pairwise disjoint properties.
    ///<see cref="http://www.w3.org/2002/07/owl#AllDisjointProperties"/>
    ///</summary>
    public static readonly Class AllDisjointProperties = new Class(new Uri("http://www.w3.org/2002/07/owl#AllDisjointProperties"));    

    ///<summary>
    ///The class of annotated annotations for which the RDF serialization consists of an annotated subject, predicate and object.
    ///<see cref="http://www.w3.org/2002/07/owl#Annotation"/>
    ///</summary>
    public static readonly Class Annotation = new Class(new Uri("http://www.w3.org/2002/07/owl#Annotation"));    

    ///<summary>
    ///The class of annotation properties.
    ///<see cref="http://www.w3.org/2002/07/owl#AnnotationProperty"/>
    ///</summary>
    public static readonly Class AnnotationProperty = new Class(new Uri("http://www.w3.org/2002/07/owl#AnnotationProperty"));    

    ///<summary>
    ///The class of asymmetric properties.
    ///<see cref="http://www.w3.org/2002/07/owl#AsymmetricProperty"/>
    ///</summary>
    public static readonly Class AsymmetricProperty = new Class(new Uri("http://www.w3.org/2002/07/owl#AsymmetricProperty"));    

    ///<summary>
    ///The class of annotated axioms for which the RDF serialization consists of an annotated subject, predicate and object.
    ///<see cref="http://www.w3.org/2002/07/owl#Axiom"/>
    ///</summary>
    public static readonly Class Axiom = new Class(new Uri("http://www.w3.org/2002/07/owl#Axiom"));    

    ///<summary>
    ///The class of OWL classes.
    ///<see cref="http://www.w3.org/2002/07/owl#Class"/>
    ///</summary>
    public static readonly Class Class = new Class(new Uri("http://www.w3.org/2002/07/owl#Class"));    

    ///<summary>
    ///The class of OWL data ranges, which are special kinds of datatypes. Note: The use of the IRI owl:DataRange has been deprecated as of OWL 2. The IRI rdfs:Datatype SHOULD be used instead.
    ///<see cref="http://www.w3.org/2002/07/owl#DataRange"/>
    ///</summary>
    public static readonly Class DataRange = new Class(new Uri("http://www.w3.org/2002/07/owl#DataRange"));    

    ///<summary>
    ///The class of data properties.
    ///<see cref="http://www.w3.org/2002/07/owl#DatatypeProperty"/>
    ///</summary>
    public static readonly Class DatatypeProperty = new Class(new Uri("http://www.w3.org/2002/07/owl#DatatypeProperty"));    

    ///<summary>
    ///The class of deprecated classes.
    ///<see cref="http://www.w3.org/2002/07/owl#DeprecatedClass"/>
    ///</summary>
    public static readonly Class DeprecatedClass = new Class(new Uri("http://www.w3.org/2002/07/owl#DeprecatedClass"));    

    ///<summary>
    ///The class of deprecated properties.
    ///<see cref="http://www.w3.org/2002/07/owl#DeprecatedProperty"/>
    ///</summary>
    public static readonly Class DeprecatedProperty = new Class(new Uri("http://www.w3.org/2002/07/owl#DeprecatedProperty"));    

    ///<summary>
    ///The class of functional properties.
    ///<see cref="http://www.w3.org/2002/07/owl#FunctionalProperty"/>
    ///</summary>
    public static readonly Class FunctionalProperty = new Class(new Uri("http://www.w3.org/2002/07/owl#FunctionalProperty"));    

    ///<summary>
    ///The class of inverse-functional properties.
    ///<see cref="http://www.w3.org/2002/07/owl#InverseFunctionalProperty"/>
    ///</summary>
    public static readonly Class InverseFunctionalProperty = new Class(new Uri("http://www.w3.org/2002/07/owl#InverseFunctionalProperty"));    

    ///<summary>
    ///The class of irreflexive properties.
    ///<see cref="http://www.w3.org/2002/07/owl#IrreflexiveProperty"/>
    ///</summary>
    public static readonly Class IrreflexiveProperty = new Class(new Uri("http://www.w3.org/2002/07/owl#IrreflexiveProperty"));    

    ///<summary>
    ///The class of named individuals.
    ///<see cref="http://www.w3.org/2002/07/owl#NamedIndividual"/>
    ///</summary>
    public static readonly Class NamedIndividual = new Class(new Uri("http://www.w3.org/2002/07/owl#NamedIndividual"));    

    ///<summary>
    ///The class of negative property assertions.
    ///<see cref="http://www.w3.org/2002/07/owl#NegativePropertyAssertion"/>
    ///</summary>
    public static readonly Class NegativePropertyAssertion = new Class(new Uri("http://www.w3.org/2002/07/owl#NegativePropertyAssertion"));    

    ///<summary>
    ///This is the empty class.
    ///<see cref="http://www.w3.org/2002/07/owl#Nothing"/>
    ///</summary>
    public static readonly Class Nothing = new Class(new Uri("http://www.w3.org/2002/07/owl#Nothing"));    

    ///<summary>
    ///The class of object properties.
    ///<see cref="http://www.w3.org/2002/07/owl#ObjectProperty"/>
    ///</summary>
    public static readonly Class ObjectProperty = new Class(new Uri("http://www.w3.org/2002/07/owl#ObjectProperty"));    

    ///<summary>
    ///The class of ontologies.
    ///<see cref="http://www.w3.org/2002/07/owl#Ontology"/>
    ///</summary>
    public static readonly Class Ontology = new Class(new Uri("http://www.w3.org/2002/07/owl#Ontology"));    

    ///<summary>
    ///The class of ontology properties.
    ///<see cref="http://www.w3.org/2002/07/owl#OntologyProperty"/>
    ///</summary>
    public static readonly Class OntologyProperty = new Class(new Uri("http://www.w3.org/2002/07/owl#OntologyProperty"));    

    ///<summary>
    ///The class of reflexive properties.
    ///<see cref="http://www.w3.org/2002/07/owl#ReflexiveProperty"/>
    ///</summary>
    public static readonly Class ReflexiveProperty = new Class(new Uri("http://www.w3.org/2002/07/owl#ReflexiveProperty"));    

    ///<summary>
    ///The class of property restrictions.
    ///<see cref="http://www.w3.org/2002/07/owl#Restriction"/>
    ///</summary>
    public static readonly Class Restriction = new Class(new Uri("http://www.w3.org/2002/07/owl#Restriction"));    

    ///<summary>
    ///The class of symmetric properties.
    ///<see cref="http://www.w3.org/2002/07/owl#SymmetricProperty"/>
    ///</summary>
    public static readonly Class SymmetricProperty = new Class(new Uri("http://www.w3.org/2002/07/owl#SymmetricProperty"));    

    ///<summary>
    ///The class of transitive properties.
    ///<see cref="http://www.w3.org/2002/07/owl#TransitiveProperty"/>
    ///</summary>
    public static readonly Class TransitiveProperty = new Class(new Uri("http://www.w3.org/2002/07/owl#TransitiveProperty"));    

    ///<summary>
    ///The class of OWL individuals.
    ///<see cref="http://www.w3.org/2002/07/owl#Thing"/>
    ///</summary>
    public static readonly Class Thing = new Class(new Uri("http://www.w3.org/2002/07/owl#Thing"));    

    ///<summary>
    ///The property that determines the class that a universal property restriction refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#allValuesFrom"/>
    ///</summary>
    public static readonly Property allValuesFrom = new Property(new Uri("http://www.w3.org/2002/07/owl#allValuesFrom"));    

    ///<summary>
    ///The property that determines the predicate of an annotated axiom or annotated annotation.
    ///<see cref="http://www.w3.org/2002/07/owl#annotatedProperty"/>
    ///</summary>
    public static readonly Property annotatedProperty = new Property(new Uri("http://www.w3.org/2002/07/owl#annotatedProperty"));    

    ///<summary>
    ///The property that determines the subject of an annotated axiom or annotated annotation.
    ///<see cref="http://www.w3.org/2002/07/owl#annotatedSource"/>
    ///</summary>
    public static readonly Property annotatedSource = new Property(new Uri("http://www.w3.org/2002/07/owl#annotatedSource"));    

    ///<summary>
    ///The property that determines the object of an annotated axiom or annotated annotation.
    ///<see cref="http://www.w3.org/2002/07/owl#annotatedTarget"/>
    ///</summary>
    public static readonly Property annotatedTarget = new Property(new Uri("http://www.w3.org/2002/07/owl#annotatedTarget"));    

    ///<summary>
    ///The property that determines the predicate of a negative property assertion.
    ///<see cref="http://www.w3.org/2002/07/owl#assertionProperty"/>
    ///</summary>
    public static readonly Property assertionProperty = new Property(new Uri("http://www.w3.org/2002/07/owl#assertionProperty"));    

    ///<summary>
    ///The annotation property that indicates that a given ontology is backward compatible with another ontology.
    ///<see cref="http://www.w3.org/2002/07/owl#backwardCompatibleWith"/>
    ///</summary>
    public static readonly Property backwardCompatibleWith = new Property(new Uri("http://www.w3.org/2002/07/owl#backwardCompatibleWith"));    

    ///<summary>
    ///The data property that does not relate any individual to any data value.
    ///<see cref="http://www.w3.org/2002/07/owl#bottomDataProperty"/>
    ///</summary>
    public static readonly Property bottomDataProperty = new Property(new Uri("http://www.w3.org/2002/07/owl#bottomDataProperty"));    

    ///<summary>
    ///The object property that does not relate any two individuals.
    ///<see cref="http://www.w3.org/2002/07/owl#bottomObjectProperty"/>
    ///</summary>
    public static readonly Property bottomObjectProperty = new Property(new Uri("http://www.w3.org/2002/07/owl#bottomObjectProperty"));    

    ///<summary>
    ///The property that determines the cardinality of an exact cardinality restriction.
    ///<see cref="http://www.w3.org/2002/07/owl#cardinality"/>
    ///</summary>
    public static readonly Property cardinality = new Property(new Uri("http://www.w3.org/2002/07/owl#cardinality"));    

    ///<summary>
    ///The property that determines that a given class is the complement of another class.
    ///<see cref="http://www.w3.org/2002/07/owl#complementOf"/>
    ///</summary>
    public static readonly Property complementOf = new Property(new Uri("http://www.w3.org/2002/07/owl#complementOf"));    

    ///<summary>
    ///The property that determines that a given data range is the complement of another data range with respect to the data domain.
    ///<see cref="http://www.w3.org/2002/07/owl#datatypeComplementOf"/>
    ///</summary>
    public static readonly Property datatypeComplementOf = new Property(new Uri("http://www.w3.org/2002/07/owl#datatypeComplementOf"));    

    ///<summary>
    ///The annotation property that indicates that a given entity has been deprecated.
    ///<see cref="http://www.w3.org/2002/07/owl#deprecated"/>
    ///</summary>
    public static readonly Property deprecated = new Property(new Uri("http://www.w3.org/2002/07/owl#deprecated"));    

    ///<summary>
    ///The property that determines that two given individuals are different.
    ///<see cref="http://www.w3.org/2002/07/owl#differentFrom"/>
    ///</summary>
    public static readonly Property differentFrom = new Property(new Uri("http://www.w3.org/2002/07/owl#differentFrom"));    

    ///<summary>
    ///The property that determines that a given class is equivalent to the disjoint union of a collection of other classes.
    ///<see cref="http://www.w3.org/2002/07/owl#disjointUnionOf"/>
    ///</summary>
    public static readonly Property disjointUnionOf = new Property(new Uri("http://www.w3.org/2002/07/owl#disjointUnionOf"));    

    ///<summary>
    ///The property that determines that two given classes are disjoint.
    ///<see cref="http://www.w3.org/2002/07/owl#disjointWith"/>
    ///</summary>
    public static readonly Property disjointWith = new Property(new Uri("http://www.w3.org/2002/07/owl#disjointWith"));    

    ///<summary>
    ///The property that determines the collection of pairwise different individuals in a owl:AllDifferent axiom.
    ///<see cref="http://www.w3.org/2002/07/owl#distinctMembers"/>
    ///</summary>
    public static readonly Property distinctMembers = new Property(new Uri("http://www.w3.org/2002/07/owl#distinctMembers"));    

    ///<summary>
    ///The property that determines that two given classes are equivalent, and that is used to specify datatype definitions.
    ///<see cref="http://www.w3.org/2002/07/owl#equivalentClass"/>
    ///</summary>
    public static readonly Property equivalentClass = new Property(new Uri("http://www.w3.org/2002/07/owl#equivalentClass"));    

    ///<summary>
    ///The property that determines that two given properties are equivalent.
    ///<see cref="http://www.w3.org/2002/07/owl#equivalentProperty"/>
    ///</summary>
    public static readonly Property equivalentProperty = new Property(new Uri("http://www.w3.org/2002/07/owl#equivalentProperty"));    

    ///<summary>
    ///The property that determines the collection of properties that jointly build a key.
    ///<see cref="http://www.w3.org/2002/07/owl#hasKey"/>
    ///</summary>
    public static readonly Property hasKey = new Property(new Uri("http://www.w3.org/2002/07/owl#hasKey"));    

    ///<summary>
    ///The property that determines the property that a self restriction refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#hasSelf"/>
    ///</summary>
    public static readonly Property hasSelf = new Property(new Uri("http://www.w3.org/2002/07/owl#hasSelf"));    

    ///<summary>
    ///The property that determines the individual that a has-value restriction refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#hasValue"/>
    ///</summary>
    public static readonly Property hasValue = new Property(new Uri("http://www.w3.org/2002/07/owl#hasValue"));    

    ///<summary>
    ///The property that is used for importing other ontologies into a given ontology.
    ///<see cref="http://www.w3.org/2002/07/owl#imports"/>
    ///</summary>
    public static readonly Property imports = new Property(new Uri("http://www.w3.org/2002/07/owl#imports"));    

    ///<summary>
    ///The annotation property that indicates that a given ontology is incompatible with another ontology.
    ///<see cref="http://www.w3.org/2002/07/owl#incompatibleWith"/>
    ///</summary>
    public static readonly Property incompatibleWith = new Property(new Uri("http://www.w3.org/2002/07/owl#incompatibleWith"));    

    ///<summary>
    ///The property that determines the collection of classes or data ranges that build an intersection.
    ///<see cref="http://www.w3.org/2002/07/owl#intersectionOf"/>
    ///</summary>
    public static readonly Property intersectionOf = new Property(new Uri("http://www.w3.org/2002/07/owl#intersectionOf"));    

    ///<summary>
    ///The property that determines that two given properties are inverse.
    ///<see cref="http://www.w3.org/2002/07/owl#inverseOf"/>
    ///</summary>
    public static readonly Property inverseOf = new Property(new Uri("http://www.w3.org/2002/07/owl#inverseOf"));    

    ///<summary>
    ///The property that determines the cardinality of a maximum cardinality restriction.
    ///<see cref="http://www.w3.org/2002/07/owl#maxCardinality"/>
    ///</summary>
    public static readonly Property maxCardinality = new Property(new Uri("http://www.w3.org/2002/07/owl#maxCardinality"));    

    ///<summary>
    ///The property that determines the cardinality of a maximum qualified cardinality restriction.
    ///<see cref="http://www.w3.org/2002/07/owl#maxQualifiedCardinality"/>
    ///</summary>
    public static readonly Property maxQualifiedCardinality = new Property(new Uri("http://www.w3.org/2002/07/owl#maxQualifiedCardinality"));    

    ///<summary>
    ///The property that determines the collection of members in either a owl:AllDifferent, owl:AllDisjointClasses or owl:AllDisjointProperties axiom.
    ///<see cref="http://www.w3.org/2002/07/owl#members"/>
    ///</summary>
    public static readonly Property members = new Property(new Uri("http://www.w3.org/2002/07/owl#members"));    

    ///<summary>
    ///The property that determines the cardinality of a minimum cardinality restriction.
    ///<see cref="http://www.w3.org/2002/07/owl#minCardinality"/>
    ///</summary>
    public static readonly Property minCardinality = new Property(new Uri("http://www.w3.org/2002/07/owl#minCardinality"));    

    ///<summary>
    ///The property that determines the cardinality of a minimum qualified cardinality restriction.
    ///<see cref="http://www.w3.org/2002/07/owl#minQualifiedCardinality"/>
    ///</summary>
    public static readonly Property minQualifiedCardinality = new Property(new Uri("http://www.w3.org/2002/07/owl#minQualifiedCardinality"));    

    ///<summary>
    ///The property that determines the class that a qualified object cardinality restriction refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#onClass"/>
    ///</summary>
    public static readonly Property onClass = new Property(new Uri("http://www.w3.org/2002/07/owl#onClass"));    

    ///<summary>
    ///The property that determines the data range that a qualified data cardinality restriction refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#onDataRange"/>
    ///</summary>
    public static readonly Property onDataRange = new Property(new Uri("http://www.w3.org/2002/07/owl#onDataRange"));    

    ///<summary>
    ///The property that determines the datatype that a datatype restriction refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#onDatatype"/>
    ///</summary>
    public static readonly Property onDatatype = new Property(new Uri("http://www.w3.org/2002/07/owl#onDatatype"));    

    ///<summary>
    ///The property that determines the collection of individuals or data values that build an enumeration.
    ///<see cref="http://www.w3.org/2002/07/owl#oneOf"/>
    ///</summary>
    public static readonly Property oneOf = new Property(new Uri("http://www.w3.org/2002/07/owl#oneOf"));    

    ///<summary>
    ///The property that determines the n-tuple of properties that a property restriction on an n-ary data range refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#onProperties"/>
    ///</summary>
    public static readonly Property onProperties = new Property(new Uri("http://www.w3.org/2002/07/owl#onProperties"));    

    ///<summary>
    ///The property that determines the property that a property restriction refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#onProperty"/>
    ///</summary>
    public static readonly Property onProperty = new Property(new Uri("http://www.w3.org/2002/07/owl#onProperty"));    

    ///<summary>
    ///The annotation property that indicates the predecessor ontology of a given ontology.
    ///<see cref="http://www.w3.org/2002/07/owl#priorVersion"/>
    ///</summary>
    public static readonly Property priorVersion = new Property(new Uri("http://www.w3.org/2002/07/owl#priorVersion"));    

    ///<summary>
    ///The property that determines the n-tuple of properties that build a sub property chain of a given property.
    ///<see cref="http://www.w3.org/2002/07/owl#propertyChainAxiom"/>
    ///</summary>
    public static readonly Property propertyChainAxiom = new Property(new Uri("http://www.w3.org/2002/07/owl#propertyChainAxiom"));    

    ///<summary>
    ///The property that determines that two given properties are disjoint.
    ///<see cref="http://www.w3.org/2002/07/owl#propertyDisjointWith"/>
    ///</summary>
    public static readonly Property propertyDisjointWith = new Property(new Uri("http://www.w3.org/2002/07/owl#propertyDisjointWith"));    

    ///<summary>
    ///The property that determines the cardinality of an exact qualified cardinality restriction.
    ///<see cref="http://www.w3.org/2002/07/owl#qualifiedCardinality"/>
    ///</summary>
    public static readonly Property qualifiedCardinality = new Property(new Uri("http://www.w3.org/2002/07/owl#qualifiedCardinality"));    

    ///<summary>
    ///The property that determines that two given individuals are equal.
    ///<see cref="http://www.w3.org/2002/07/owl#sameAs"/>
    ///</summary>
    public static readonly Property sameAs = new Property(new Uri("http://www.w3.org/2002/07/owl#sameAs"));    

    ///<summary>
    ///The property that determines the class that an existential property restriction refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#someValuesFrom"/>
    ///</summary>
    public static readonly Property someValuesFrom = new Property(new Uri("http://www.w3.org/2002/07/owl#someValuesFrom"));    

    ///<summary>
    ///The property that determines the subject of a negative property assertion.
    ///<see cref="http://www.w3.org/2002/07/owl#sourceIndividual"/>
    ///</summary>
    public static readonly Property sourceIndividual = new Property(new Uri("http://www.w3.org/2002/07/owl#sourceIndividual"));    

    ///<summary>
    ///The property that determines the object of a negative object property assertion.
    ///<see cref="http://www.w3.org/2002/07/owl#targetIndividual"/>
    ///</summary>
    public static readonly Property targetIndividual = new Property(new Uri("http://www.w3.org/2002/07/owl#targetIndividual"));    

    ///<summary>
    ///The property that determines the value of a negative data property assertion.
    ///<see cref="http://www.w3.org/2002/07/owl#targetValue"/>
    ///</summary>
    public static readonly Property targetValue = new Property(new Uri("http://www.w3.org/2002/07/owl#targetValue"));    

    ///<summary>
    ///The data property that relates every individual to every data value.
    ///<see cref="http://www.w3.org/2002/07/owl#topDataProperty"/>
    ///</summary>
    public static readonly Property topDataProperty = new Property(new Uri("http://www.w3.org/2002/07/owl#topDataProperty"));    

    ///<summary>
    ///The object property that relates every two individuals.
    ///<see cref="http://www.w3.org/2002/07/owl#topObjectProperty"/>
    ///</summary>
    public static readonly Property topObjectProperty = new Property(new Uri("http://www.w3.org/2002/07/owl#topObjectProperty"));    

    ///<summary>
    ///The property that determines the collection of classes or data ranges that build a union.
    ///<see cref="http://www.w3.org/2002/07/owl#unionOf"/>
    ///</summary>
    public static readonly Property unionOf = new Property(new Uri("http://www.w3.org/2002/07/owl#unionOf"));    

    ///<summary>
    ///The annotation property that provides version information for an ontology or another OWL construct.
    ///<see cref="http://www.w3.org/2002/07/owl#versionInfo"/>
    ///</summary>
    public static readonly Property versionInfo = new Property(new Uri("http://www.w3.org/2002/07/owl#versionInfo"));    

    ///<summary>
    ///The property that identifies the version IRI of an ontology.
    ///<see cref="http://www.w3.org/2002/07/owl#versionIRI"/>
    ///</summary>
    public static readonly Property versionIRI = new Property(new Uri("http://www.w3.org/2002/07/owl#versionIRI"));    

    ///<summary>
    ///The property that determines the collection of facet-value pairs that define a datatype restriction.
    ///<see cref="http://www.w3.org/2002/07/owl#withRestrictions"/>
    ///</summary>
    public static readonly Property withRestrictions = new Property(new Uri("http://www.w3.org/2002/07/owl#withRestrictions"));
}
///<summary>
///The OWL 2 Schema vocabulary (OWL 2)
///
///</summary>
public static class OWL
{
    public static readonly Uri Namespace = new Uri("http://www.w3.org/2002/07/owl#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "OWL";
    public static string GetPrefix() { return Prefix; } 

    ///<summary>
    ///
    ///  This ontology partially describes the built-in classes and
    ///  properties that together form the basis of the RDF/XML syntax of OWL 2.
    ///  The content of this ontology is based on Tables 6.1 and 6.2
    ///  in Section 6.4 of the OWL 2 RDF-Based Semantics specification,
    ///  available at http://www.w3.org/TR/owl2-rdf-based-semantics/.
    ///  Please note that those tables do not include the different annotations
    ///  (labels, comments and rdfs:isDefinedBy links) used in this file.
    ///  Also note that the descriptions provided in this ontology do not
    ///  provide a complete and correct formal description of either the syntax
    ///  or the semantics of the introduced terms (please see the OWL 2
    ///  recommendations for the complete and normative specifications).
    ///  Furthermore, the information provided by this ontology may be
    ///  misleading if not used with care. This ontology SHOULD NOT be imported
    ///  into OWL ontologies. Importing this file into an OWL 2 DL ontology
    ///  will cause it to become an OWL 2 Full ontology and may have other,
    ///  unexpected, consequences.
    ///   
    ///<see cref="http://www.w3.org/2002/07/owl"/>
    ///</summary>
    public const string owl_0 = "http://www.w3.org/2002/07/owl";

    ///<summary>
    ///The class of collections of pairwise different individuals.
    ///<see cref="http://www.w3.org/2002/07/owl#AllDifferent"/>
    ///</summary>
    public const string AllDifferent = "http://www.w3.org/2002/07/owl#AllDifferent";

    ///<summary>
    ///The class of collections of pairwise disjoint classes.
    ///<see cref="http://www.w3.org/2002/07/owl#AllDisjointClasses"/>
    ///</summary>
    public const string AllDisjointClasses = "http://www.w3.org/2002/07/owl#AllDisjointClasses";

    ///<summary>
    ///The class of collections of pairwise disjoint properties.
    ///<see cref="http://www.w3.org/2002/07/owl#AllDisjointProperties"/>
    ///</summary>
    public const string AllDisjointProperties = "http://www.w3.org/2002/07/owl#AllDisjointProperties";

    ///<summary>
    ///The class of annotated annotations for which the RDF serialization consists of an annotated subject, predicate and object.
    ///<see cref="http://www.w3.org/2002/07/owl#Annotation"/>
    ///</summary>
    public const string Annotation = "http://www.w3.org/2002/07/owl#Annotation";

    ///<summary>
    ///The class of annotation properties.
    ///<see cref="http://www.w3.org/2002/07/owl#AnnotationProperty"/>
    ///</summary>
    public const string AnnotationProperty = "http://www.w3.org/2002/07/owl#AnnotationProperty";

    ///<summary>
    ///The class of asymmetric properties.
    ///<see cref="http://www.w3.org/2002/07/owl#AsymmetricProperty"/>
    ///</summary>
    public const string AsymmetricProperty = "http://www.w3.org/2002/07/owl#AsymmetricProperty";

    ///<summary>
    ///The class of annotated axioms for which the RDF serialization consists of an annotated subject, predicate and object.
    ///<see cref="http://www.w3.org/2002/07/owl#Axiom"/>
    ///</summary>
    public const string Axiom = "http://www.w3.org/2002/07/owl#Axiom";

    ///<summary>
    ///The class of OWL classes.
    ///<see cref="http://www.w3.org/2002/07/owl#Class"/>
    ///</summary>
    public const string Class = "http://www.w3.org/2002/07/owl#Class";

    ///<summary>
    ///The class of OWL data ranges, which are special kinds of datatypes. Note: The use of the IRI owl:DataRange has been deprecated as of OWL 2. The IRI rdfs:Datatype SHOULD be used instead.
    ///<see cref="http://www.w3.org/2002/07/owl#DataRange"/>
    ///</summary>
    public const string DataRange = "http://www.w3.org/2002/07/owl#DataRange";

    ///<summary>
    ///The class of data properties.
    ///<see cref="http://www.w3.org/2002/07/owl#DatatypeProperty"/>
    ///</summary>
    public const string DatatypeProperty = "http://www.w3.org/2002/07/owl#DatatypeProperty";

    ///<summary>
    ///The class of deprecated classes.
    ///<see cref="http://www.w3.org/2002/07/owl#DeprecatedClass"/>
    ///</summary>
    public const string DeprecatedClass = "http://www.w3.org/2002/07/owl#DeprecatedClass";

    ///<summary>
    ///The class of deprecated properties.
    ///<see cref="http://www.w3.org/2002/07/owl#DeprecatedProperty"/>
    ///</summary>
    public const string DeprecatedProperty = "http://www.w3.org/2002/07/owl#DeprecatedProperty";

    ///<summary>
    ///The class of functional properties.
    ///<see cref="http://www.w3.org/2002/07/owl#FunctionalProperty"/>
    ///</summary>
    public const string FunctionalProperty = "http://www.w3.org/2002/07/owl#FunctionalProperty";

    ///<summary>
    ///The class of inverse-functional properties.
    ///<see cref="http://www.w3.org/2002/07/owl#InverseFunctionalProperty"/>
    ///</summary>
    public const string InverseFunctionalProperty = "http://www.w3.org/2002/07/owl#InverseFunctionalProperty";

    ///<summary>
    ///The class of irreflexive properties.
    ///<see cref="http://www.w3.org/2002/07/owl#IrreflexiveProperty"/>
    ///</summary>
    public const string IrreflexiveProperty = "http://www.w3.org/2002/07/owl#IrreflexiveProperty";

    ///<summary>
    ///The class of named individuals.
    ///<see cref="http://www.w3.org/2002/07/owl#NamedIndividual"/>
    ///</summary>
    public const string NamedIndividual = "http://www.w3.org/2002/07/owl#NamedIndividual";

    ///<summary>
    ///The class of negative property assertions.
    ///<see cref="http://www.w3.org/2002/07/owl#NegativePropertyAssertion"/>
    ///</summary>
    public const string NegativePropertyAssertion = "http://www.w3.org/2002/07/owl#NegativePropertyAssertion";

    ///<summary>
    ///This is the empty class.
    ///<see cref="http://www.w3.org/2002/07/owl#Nothing"/>
    ///</summary>
    public const string Nothing = "http://www.w3.org/2002/07/owl#Nothing";

    ///<summary>
    ///The class of object properties.
    ///<see cref="http://www.w3.org/2002/07/owl#ObjectProperty"/>
    ///</summary>
    public const string ObjectProperty = "http://www.w3.org/2002/07/owl#ObjectProperty";

    ///<summary>
    ///The class of ontologies.
    ///<see cref="http://www.w3.org/2002/07/owl#Ontology"/>
    ///</summary>
    public const string Ontology = "http://www.w3.org/2002/07/owl#Ontology";

    ///<summary>
    ///The class of ontology properties.
    ///<see cref="http://www.w3.org/2002/07/owl#OntologyProperty"/>
    ///</summary>
    public const string OntologyProperty = "http://www.w3.org/2002/07/owl#OntologyProperty";

    ///<summary>
    ///The class of reflexive properties.
    ///<see cref="http://www.w3.org/2002/07/owl#ReflexiveProperty"/>
    ///</summary>
    public const string ReflexiveProperty = "http://www.w3.org/2002/07/owl#ReflexiveProperty";

    ///<summary>
    ///The class of property restrictions.
    ///<see cref="http://www.w3.org/2002/07/owl#Restriction"/>
    ///</summary>
    public const string Restriction = "http://www.w3.org/2002/07/owl#Restriction";

    ///<summary>
    ///The class of symmetric properties.
    ///<see cref="http://www.w3.org/2002/07/owl#SymmetricProperty"/>
    ///</summary>
    public const string SymmetricProperty = "http://www.w3.org/2002/07/owl#SymmetricProperty";

    ///<summary>
    ///The class of transitive properties.
    ///<see cref="http://www.w3.org/2002/07/owl#TransitiveProperty"/>
    ///</summary>
    public const string TransitiveProperty = "http://www.w3.org/2002/07/owl#TransitiveProperty";

    ///<summary>
    ///The class of OWL individuals.
    ///<see cref="http://www.w3.org/2002/07/owl#Thing"/>
    ///</summary>
    public const string Thing = "http://www.w3.org/2002/07/owl#Thing";

    ///<summary>
    ///The property that determines the class that a universal property restriction refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#allValuesFrom"/>
    ///</summary>
    public const string allValuesFrom = "http://www.w3.org/2002/07/owl#allValuesFrom";

    ///<summary>
    ///The property that determines the predicate of an annotated axiom or annotated annotation.
    ///<see cref="http://www.w3.org/2002/07/owl#annotatedProperty"/>
    ///</summary>
    public const string annotatedProperty = "http://www.w3.org/2002/07/owl#annotatedProperty";

    ///<summary>
    ///The property that determines the subject of an annotated axiom or annotated annotation.
    ///<see cref="http://www.w3.org/2002/07/owl#annotatedSource"/>
    ///</summary>
    public const string annotatedSource = "http://www.w3.org/2002/07/owl#annotatedSource";

    ///<summary>
    ///The property that determines the object of an annotated axiom or annotated annotation.
    ///<see cref="http://www.w3.org/2002/07/owl#annotatedTarget"/>
    ///</summary>
    public const string annotatedTarget = "http://www.w3.org/2002/07/owl#annotatedTarget";

    ///<summary>
    ///The property that determines the predicate of a negative property assertion.
    ///<see cref="http://www.w3.org/2002/07/owl#assertionProperty"/>
    ///</summary>
    public const string assertionProperty = "http://www.w3.org/2002/07/owl#assertionProperty";

    ///<summary>
    ///The annotation property that indicates that a given ontology is backward compatible with another ontology.
    ///<see cref="http://www.w3.org/2002/07/owl#backwardCompatibleWith"/>
    ///</summary>
    public const string backwardCompatibleWith = "http://www.w3.org/2002/07/owl#backwardCompatibleWith";

    ///<summary>
    ///The data property that does not relate any individual to any data value.
    ///<see cref="http://www.w3.org/2002/07/owl#bottomDataProperty"/>
    ///</summary>
    public const string bottomDataProperty = "http://www.w3.org/2002/07/owl#bottomDataProperty";

    ///<summary>
    ///The object property that does not relate any two individuals.
    ///<see cref="http://www.w3.org/2002/07/owl#bottomObjectProperty"/>
    ///</summary>
    public const string bottomObjectProperty = "http://www.w3.org/2002/07/owl#bottomObjectProperty";

    ///<summary>
    ///The property that determines the cardinality of an exact cardinality restriction.
    ///<see cref="http://www.w3.org/2002/07/owl#cardinality"/>
    ///</summary>
    public const string cardinality = "http://www.w3.org/2002/07/owl#cardinality";

    ///<summary>
    ///The property that determines that a given class is the complement of another class.
    ///<see cref="http://www.w3.org/2002/07/owl#complementOf"/>
    ///</summary>
    public const string complementOf = "http://www.w3.org/2002/07/owl#complementOf";

    ///<summary>
    ///The property that determines that a given data range is the complement of another data range with respect to the data domain.
    ///<see cref="http://www.w3.org/2002/07/owl#datatypeComplementOf"/>
    ///</summary>
    public const string datatypeComplementOf = "http://www.w3.org/2002/07/owl#datatypeComplementOf";

    ///<summary>
    ///The annotation property that indicates that a given entity has been deprecated.
    ///<see cref="http://www.w3.org/2002/07/owl#deprecated"/>
    ///</summary>
    public const string deprecated = "http://www.w3.org/2002/07/owl#deprecated";

    ///<summary>
    ///The property that determines that two given individuals are different.
    ///<see cref="http://www.w3.org/2002/07/owl#differentFrom"/>
    ///</summary>
    public const string differentFrom = "http://www.w3.org/2002/07/owl#differentFrom";

    ///<summary>
    ///The property that determines that a given class is equivalent to the disjoint union of a collection of other classes.
    ///<see cref="http://www.w3.org/2002/07/owl#disjointUnionOf"/>
    ///</summary>
    public const string disjointUnionOf = "http://www.w3.org/2002/07/owl#disjointUnionOf";

    ///<summary>
    ///The property that determines that two given classes are disjoint.
    ///<see cref="http://www.w3.org/2002/07/owl#disjointWith"/>
    ///</summary>
    public const string disjointWith = "http://www.w3.org/2002/07/owl#disjointWith";

    ///<summary>
    ///The property that determines the collection of pairwise different individuals in a owl:AllDifferent axiom.
    ///<see cref="http://www.w3.org/2002/07/owl#distinctMembers"/>
    ///</summary>
    public const string distinctMembers = "http://www.w3.org/2002/07/owl#distinctMembers";

    ///<summary>
    ///The property that determines that two given classes are equivalent, and that is used to specify datatype definitions.
    ///<see cref="http://www.w3.org/2002/07/owl#equivalentClass"/>
    ///</summary>
    public const string equivalentClass = "http://www.w3.org/2002/07/owl#equivalentClass";

    ///<summary>
    ///The property that determines that two given properties are equivalent.
    ///<see cref="http://www.w3.org/2002/07/owl#equivalentProperty"/>
    ///</summary>
    public const string equivalentProperty = "http://www.w3.org/2002/07/owl#equivalentProperty";

    ///<summary>
    ///The property that determines the collection of properties that jointly build a key.
    ///<see cref="http://www.w3.org/2002/07/owl#hasKey"/>
    ///</summary>
    public const string hasKey = "http://www.w3.org/2002/07/owl#hasKey";

    ///<summary>
    ///The property that determines the property that a self restriction refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#hasSelf"/>
    ///</summary>
    public const string hasSelf = "http://www.w3.org/2002/07/owl#hasSelf";

    ///<summary>
    ///The property that determines the individual that a has-value restriction refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#hasValue"/>
    ///</summary>
    public const string hasValue = "http://www.w3.org/2002/07/owl#hasValue";

    ///<summary>
    ///The property that is used for importing other ontologies into a given ontology.
    ///<see cref="http://www.w3.org/2002/07/owl#imports"/>
    ///</summary>
    public const string imports = "http://www.w3.org/2002/07/owl#imports";

    ///<summary>
    ///The annotation property that indicates that a given ontology is incompatible with another ontology.
    ///<see cref="http://www.w3.org/2002/07/owl#incompatibleWith"/>
    ///</summary>
    public const string incompatibleWith = "http://www.w3.org/2002/07/owl#incompatibleWith";

    ///<summary>
    ///The property that determines the collection of classes or data ranges that build an intersection.
    ///<see cref="http://www.w3.org/2002/07/owl#intersectionOf"/>
    ///</summary>
    public const string intersectionOf = "http://www.w3.org/2002/07/owl#intersectionOf";

    ///<summary>
    ///The property that determines that two given properties are inverse.
    ///<see cref="http://www.w3.org/2002/07/owl#inverseOf"/>
    ///</summary>
    public const string inverseOf = "http://www.w3.org/2002/07/owl#inverseOf";

    ///<summary>
    ///The property that determines the cardinality of a maximum cardinality restriction.
    ///<see cref="http://www.w3.org/2002/07/owl#maxCardinality"/>
    ///</summary>
    public const string maxCardinality = "http://www.w3.org/2002/07/owl#maxCardinality";

    ///<summary>
    ///The property that determines the cardinality of a maximum qualified cardinality restriction.
    ///<see cref="http://www.w3.org/2002/07/owl#maxQualifiedCardinality"/>
    ///</summary>
    public const string maxQualifiedCardinality = "http://www.w3.org/2002/07/owl#maxQualifiedCardinality";

    ///<summary>
    ///The property that determines the collection of members in either a owl:AllDifferent, owl:AllDisjointClasses or owl:AllDisjointProperties axiom.
    ///<see cref="http://www.w3.org/2002/07/owl#members"/>
    ///</summary>
    public const string members = "http://www.w3.org/2002/07/owl#members";

    ///<summary>
    ///The property that determines the cardinality of a minimum cardinality restriction.
    ///<see cref="http://www.w3.org/2002/07/owl#minCardinality"/>
    ///</summary>
    public const string minCardinality = "http://www.w3.org/2002/07/owl#minCardinality";

    ///<summary>
    ///The property that determines the cardinality of a minimum qualified cardinality restriction.
    ///<see cref="http://www.w3.org/2002/07/owl#minQualifiedCardinality"/>
    ///</summary>
    public const string minQualifiedCardinality = "http://www.w3.org/2002/07/owl#minQualifiedCardinality";

    ///<summary>
    ///The property that determines the class that a qualified object cardinality restriction refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#onClass"/>
    ///</summary>
    public const string onClass = "http://www.w3.org/2002/07/owl#onClass";

    ///<summary>
    ///The property that determines the data range that a qualified data cardinality restriction refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#onDataRange"/>
    ///</summary>
    public const string onDataRange = "http://www.w3.org/2002/07/owl#onDataRange";

    ///<summary>
    ///The property that determines the datatype that a datatype restriction refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#onDatatype"/>
    ///</summary>
    public const string onDatatype = "http://www.w3.org/2002/07/owl#onDatatype";

    ///<summary>
    ///The property that determines the collection of individuals or data values that build an enumeration.
    ///<see cref="http://www.w3.org/2002/07/owl#oneOf"/>
    ///</summary>
    public const string oneOf = "http://www.w3.org/2002/07/owl#oneOf";

    ///<summary>
    ///The property that determines the n-tuple of properties that a property restriction on an n-ary data range refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#onProperties"/>
    ///</summary>
    public const string onProperties = "http://www.w3.org/2002/07/owl#onProperties";

    ///<summary>
    ///The property that determines the property that a property restriction refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#onProperty"/>
    ///</summary>
    public const string onProperty = "http://www.w3.org/2002/07/owl#onProperty";

    ///<summary>
    ///The annotation property that indicates the predecessor ontology of a given ontology.
    ///<see cref="http://www.w3.org/2002/07/owl#priorVersion"/>
    ///</summary>
    public const string priorVersion = "http://www.w3.org/2002/07/owl#priorVersion";

    ///<summary>
    ///The property that determines the n-tuple of properties that build a sub property chain of a given property.
    ///<see cref="http://www.w3.org/2002/07/owl#propertyChainAxiom"/>
    ///</summary>
    public const string propertyChainAxiom = "http://www.w3.org/2002/07/owl#propertyChainAxiom";

    ///<summary>
    ///The property that determines that two given properties are disjoint.
    ///<see cref="http://www.w3.org/2002/07/owl#propertyDisjointWith"/>
    ///</summary>
    public const string propertyDisjointWith = "http://www.w3.org/2002/07/owl#propertyDisjointWith";

    ///<summary>
    ///The property that determines the cardinality of an exact qualified cardinality restriction.
    ///<see cref="http://www.w3.org/2002/07/owl#qualifiedCardinality"/>
    ///</summary>
    public const string qualifiedCardinality = "http://www.w3.org/2002/07/owl#qualifiedCardinality";

    ///<summary>
    ///The property that determines that two given individuals are equal.
    ///<see cref="http://www.w3.org/2002/07/owl#sameAs"/>
    ///</summary>
    public const string sameAs = "http://www.w3.org/2002/07/owl#sameAs";

    ///<summary>
    ///The property that determines the class that an existential property restriction refers to.
    ///<see cref="http://www.w3.org/2002/07/owl#someValuesFrom"/>
    ///</summary>
    public const string someValuesFrom = "http://www.w3.org/2002/07/owl#someValuesFrom";

    ///<summary>
    ///The property that determines the subject of a negative property assertion.
    ///<see cref="http://www.w3.org/2002/07/owl#sourceIndividual"/>
    ///</summary>
    public const string sourceIndividual = "http://www.w3.org/2002/07/owl#sourceIndividual";

    ///<summary>
    ///The property that determines the object of a negative object property assertion.
    ///<see cref="http://www.w3.org/2002/07/owl#targetIndividual"/>
    ///</summary>
    public const string targetIndividual = "http://www.w3.org/2002/07/owl#targetIndividual";

    ///<summary>
    ///The property that determines the value of a negative data property assertion.
    ///<see cref="http://www.w3.org/2002/07/owl#targetValue"/>
    ///</summary>
    public const string targetValue = "http://www.w3.org/2002/07/owl#targetValue";

    ///<summary>
    ///The data property that relates every individual to every data value.
    ///<see cref="http://www.w3.org/2002/07/owl#topDataProperty"/>
    ///</summary>
    public const string topDataProperty = "http://www.w3.org/2002/07/owl#topDataProperty";

    ///<summary>
    ///The object property that relates every two individuals.
    ///<see cref="http://www.w3.org/2002/07/owl#topObjectProperty"/>
    ///</summary>
    public const string topObjectProperty = "http://www.w3.org/2002/07/owl#topObjectProperty";

    ///<summary>
    ///The property that determines the collection of classes or data ranges that build a union.
    ///<see cref="http://www.w3.org/2002/07/owl#unionOf"/>
    ///</summary>
    public const string unionOf = "http://www.w3.org/2002/07/owl#unionOf";

    ///<summary>
    ///The annotation property that provides version information for an ontology or another OWL construct.
    ///<see cref="http://www.w3.org/2002/07/owl#versionInfo"/>
    ///</summary>
    public const string versionInfo = "http://www.w3.org/2002/07/owl#versionInfo";

    ///<summary>
    ///The property that identifies the version IRI of an ontology.
    ///<see cref="http://www.w3.org/2002/07/owl#versionIRI"/>
    ///</summary>
    public const string versionIRI = "http://www.w3.org/2002/07/owl#versionIRI";

    ///<summary>
    ///The property that determines the collection of facet-value pairs that define a datatype restriction.
    ///<see cref="http://www.w3.org/2002/07/owl#withRestrictions"/>
    ///</summary>
    public const string withRestrictions = "http://www.w3.org/2002/07/owl#withRestrictions";
}
///<summary>
///
///
///</summary>
public class as2 : Ontology
{
    public static readonly Uri Namespace = new Uri("http://www.w3.org/ns/activitystreams#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "as2";
    public static string GetPrefix() { return Prefix; }     

    ///<summary>
    ///Extended Activity Streams 2.0 Vocabulary
    ///<see cref="http://www.w3.org/ns/activitystreams#"/>
    ///</summary>
    public static readonly Resource activitystreams = new Resource(new Uri("http://www.w3.org/ns/activitystreams#"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#langString"/>
    ///</summary>
    public static readonly Resource langString = new Resource(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#langString"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/XMLSchema#duration"/>
    ///</summary>
    public static readonly Resource duration = new Resource(new Uri("http://www.w3.org/2001/XMLSchema#duration"));    

    ///<summary>
    ///Subproperty of as:attributedTo that identifies the primary actor
    ///<see cref="http://www.w3.org/ns/activitystreams#actor"/>
    ///</summary>
    public static readonly Property actor = new Property(new Uri("http://www.w3.org/ns/activitystreams#actor"));    

    ///<summary>
    ///Identifies an entity to which an object is attributed
    ///<see cref="http://www.w3.org/ns/activitystreams#attributedTo"/>
    ///</summary>
    public static readonly Property attributedTo = new Property(new Uri("http://www.w3.org/ns/activitystreams#attributedTo"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#attachment"/>
    ///</summary>
    public static readonly Property attachment = new Property(new Uri("http://www.w3.org/ns/activitystreams#attachment"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#attachments"/>
    ///</summary>
    public static readonly Property attachments = new Property(new Uri("http://www.w3.org/ns/activitystreams#attachments"));    

    ///<summary>
    ///Identifies the author of an object. Deprecated. Use as:attributedTo instead
    ///<see cref="http://www.w3.org/ns/activitystreams#author"/>
    ///</summary>
    public static readonly Property author = new Property(new Uri("http://www.w3.org/ns/activitystreams#author"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#bcc"/>
    ///</summary>
    public static readonly Property bcc = new Property(new Uri("http://www.w3.org/ns/activitystreams#bcc"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#bto"/>
    ///</summary>
    public static readonly Property bto = new Property(new Uri("http://www.w3.org/ns/activitystreams#bto"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#cc"/>
    ///</summary>
    public static readonly Property cc = new Property(new Uri("http://www.w3.org/ns/activitystreams#cc"));    

    ///<summary>
    ///Specifies the context within which an object exists or an activity was performed
    ///<see cref="http://www.w3.org/ns/activitystreams#context"/>
    ///</summary>
    public static readonly Property context = new Property(new Uri("http://www.w3.org/ns/activitystreams#context"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#current"/>
    ///</summary>
    public static readonly Property current = new Property(new Uri("http://www.w3.org/ns/activitystreams#current"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#first"/>
    ///</summary>
    public static readonly Property first = new Property(new Uri("http://www.w3.org/ns/activitystreams#first"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#generator"/>
    ///</summary>
    public static readonly Property generator = new Property(new Uri("http://www.w3.org/ns/activitystreams#generator"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#icon"/>
    ///</summary>
    public static readonly Property icon = new Property(new Uri("http://www.w3.org/ns/activitystreams#icon"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#image"/>
    ///</summary>
    public static readonly Property image = new Property(new Uri("http://www.w3.org/ns/activitystreams#image"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#inReplyTo"/>
    ///</summary>
    public static readonly Property inReplyTo = new Property(new Uri("http://www.w3.org/ns/activitystreams#inReplyTo"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#items"/>
    ///</summary>
    public static readonly Property items = new Property(new Uri("http://www.w3.org/ns/activitystreams#items"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#last"/>
    ///</summary>
    public static readonly Property last = new Property(new Uri("http://www.w3.org/ns/activitystreams#last"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#location"/>
    ///</summary>
    public static readonly Property location = new Property(new Uri("http://www.w3.org/ns/activitystreams#location"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#next"/>
    ///</summary>
    public static readonly Property next = new Property(new Uri("http://www.w3.org/ns/activitystreams#next"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#object"/>
    ///</summary>
    public static readonly Property _object = new Property(new Uri("http://www.w3.org/ns/activitystreams#object"));    

    ///<summary>
    ///Describes a possible exclusive answer or option for a question.
    ///<see cref="http://www.w3.org/ns/activitystreams#oneOf"/>
    ///</summary>
    public static readonly Property oneOf = new Property(new Uri("http://www.w3.org/ns/activitystreams#oneOf"));    

    ///<summary>
    ///Describes a possible inclusive answer or option for a question.
    ///<see cref="http://www.w3.org/ns/activitystreams#anyOf"/>
    ///</summary>
    public static readonly Property anyOf = new Property(new Uri("http://www.w3.org/ns/activitystreams#anyOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#prev"/>
    ///</summary>
    public static readonly Property prev = new Property(new Uri("http://www.w3.org/ns/activitystreams#prev"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#preview"/>
    ///</summary>
    public static readonly Property preview = new Property(new Uri("http://www.w3.org/ns/activitystreams#preview"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#provider"/>
    ///</summary>
    public static readonly Property provider = new Property(new Uri("http://www.w3.org/ns/activitystreams#provider"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#replies"/>
    ///</summary>
    public static readonly Property replies = new Property(new Uri("http://www.w3.org/ns/activitystreams#replies"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#result"/>
    ///</summary>
    public static readonly Property result = new Property(new Uri("http://www.w3.org/ns/activitystreams#result"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#scope"/>
    ///</summary>
    public static readonly Property scope = new Property(new Uri("http://www.w3.org/ns/activitystreams#scope"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#self"/>
    ///</summary>
    public static readonly Property self = new Property(new Uri("http://www.w3.org/ns/activitystreams#self"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#tag"/>
    ///</summary>
    public static readonly Property tag = new Property(new Uri("http://www.w3.org/ns/activitystreams#tag"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#tags"/>
    ///</summary>
    public static readonly Property tags = new Property(new Uri("http://www.w3.org/ns/activitystreams#tags"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#target"/>
    ///</summary>
    public static readonly Property target = new Property(new Uri("http://www.w3.org/ns/activitystreams#target"));    

    ///<summary>
    ///For certain activities, specifies the entity from which the action is directed.
    ///<see cref="http://www.w3.org/ns/activitystreams#origin"/>
    ///</summary>
    public static readonly Property origin = new Property(new Uri("http://www.w3.org/ns/activitystreams#origin"));    

    ///<summary>
    ///Indentifies an object used (or to be used) to complete an activity
    ///<see cref="http://www.w3.org/ns/activitystreams#instrument"/>
    ///</summary>
    public static readonly Property instrument = new Property(new Uri("http://www.w3.org/ns/activitystreams#instrument"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#to"/>
    ///</summary>
    public static readonly Property to = new Property(new Uri("http://www.w3.org/ns/activitystreams#to"));    

    ///<summary>
    ///Specifies a link to a specific representation of the Object
    ///<see cref="http://www.w3.org/ns/activitystreams#url"/>
    ///</summary>
    public static readonly Property url = new Property(new Uri("http://www.w3.org/ns/activitystreams#url"));    

    ///<summary>
    ///On a Relationship object, identifies the subject. e.g. when saying "John is connected to Sally", 'subject' refers to 'John'
    ///<see cref="http://www.w3.org/ns/activitystreams#subject"/>
    ///</summary>
    public static readonly Property subject = new Property(new Uri("http://www.w3.org/ns/activitystreams#subject"));    

    ///<summary>
    ///On a Relationship object, describes the type of relationship
    ///<see cref="http://www.w3.org/ns/activitystreams#relationship"/>
    ///</summary>
    public static readonly Property relationship = new Property(new Uri("http://www.w3.org/ns/activitystreams#relationship"));    

    ///<summary>
    ///On a Profile object, describes the object described by the profile
    ///<see cref="http://www.w3.org/ns/activitystreams#describes"/>
    ///</summary>
    public static readonly Property describes = new Property(new Uri("http://www.w3.org/ns/activitystreams#describes"));    

    ///<summary>
    ///Specifies the accuracy around the point established by the longitude and latitude
    ///<see cref="http://www.w3.org/ns/activitystreams#accuracy"/>
    ///</summary>
    public static readonly Property accuracy = new Property(new Uri("http://www.w3.org/ns/activitystreams#accuracy"));    

    ///<summary>
    ///An alternative, domain specific alias for an object
    ///<see cref="http://www.w3.org/ns/activitystreams#alias"/>
    ///</summary>
    public static readonly Property alias = new Property(new Uri("http://www.w3.org/ns/activitystreams#alias"));    

    ///<summary>
    ///The altitude of a place
    ///<see cref="http://www.w3.org/ns/activitystreams#altitude"/>
    ///</summary>
    public static readonly Property altitude = new Property(new Uri("http://www.w3.org/ns/activitystreams#altitude"));    

    ///<summary>
    ///The content of the object.
    ///<see cref="http://www.w3.org/ns/activitystreams#content"/>
    ///</summary>
    public static readonly Property content = new Property(new Uri("http://www.w3.org/ns/activitystreams#content"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#displayName"/>
    ///</summary>
    public static readonly Property displayName = new Property(new Uri("http://www.w3.org/ns/activitystreams#displayName"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#downstreamDuplicates"/>
    ///</summary>
    public static readonly Property downstreamDuplicates = new Property(new Uri("http://www.w3.org/ns/activitystreams#downstreamDuplicates"));    

    ///<summary>
    ///The duration of the object
    ///<see cref="http://www.w3.org/ns/activitystreams#duration"/>
    ///</summary>
    public static readonly Property duration_0 = new Property(new Uri("http://www.w3.org/ns/activitystreams#duration"));    

    ///<summary>
    ///The ending time of the object
    ///<see cref="http://www.w3.org/ns/activitystreams#endTime"/>
    ///</summary>
    public static readonly Property endTime = new Property(new Uri("http://www.w3.org/ns/activitystreams#endTime"));    

    ///<summary>
    ///The display height expressed as device independent pixels
    ///<see cref="http://www.w3.org/ns/activitystreams#height"/>
    ///</summary>
    public static readonly Property height = new Property(new Uri("http://www.w3.org/ns/activitystreams#height"));    

    ///<summary>
    ///The target URI of the Link
    ///<see cref="http://www.w3.org/ns/activitystreams#href"/>
    ///</summary>
    public static readonly Property href = new Property(new Uri("http://www.w3.org/ns/activitystreams#href"));    

    ///<summary>
    ///A hint about the language of the referenced resource
    ///<see cref="http://www.w3.org/ns/activitystreams#hreflang"/>
    ///</summary>
    public static readonly Property hreflang = new Property(new Uri("http://www.w3.org/ns/activitystreams#hreflang"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#id"/>
    ///</summary>
    public static readonly Property id = new Property(new Uri("http://www.w3.org/ns/activitystreams#id"));    

    ///<summary>
    ///The maximum number of items per page in a logical Collection
    ///<see cref="http://www.w3.org/ns/activitystreams#itemsPerPage"/>
    ///</summary>
    public static readonly Property itemsPerPage = new Property(new Uri("http://www.w3.org/ns/activitystreams#itemsPerPage"));    

    ///<summary>
    ///The latitude
    ///<see cref="http://www.w3.org/ns/activitystreams#latitude"/>
    ///</summary>
    public static readonly Property latitude = new Property(new Uri("http://www.w3.org/ns/activitystreams#latitude"));    

    ///<summary>
    ///The longitude
    ///<see cref="http://www.w3.org/ns/activitystreams#longitude"/>
    ///</summary>
    public static readonly Property longitude = new Property(new Uri("http://www.w3.org/ns/activitystreams#longitude"));    

    ///<summary>
    ///The MIME Media Type
    ///<see cref="http://www.w3.org/ns/activitystreams#mediaType"/>
    ///</summary>
    public static readonly Property mediaType = new Property(new Uri("http://www.w3.org/ns/activitystreams#mediaType"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#objectType"/>
    ///</summary>
    public static readonly Property objectType = new Property(new Uri("http://www.w3.org/ns/activitystreams#objectType"));    

    ///<summary>
    ///Specifies the relative priority of the Activity
    ///<see cref="http://www.w3.org/ns/activitystreams#priority"/>
    ///</summary>
    public static readonly Property priority = new Property(new Uri("http://www.w3.org/ns/activitystreams#priority"));    

    ///<summary>
    ///Specifies the date and time the object was published
    ///<see cref="http://www.w3.org/ns/activitystreams#published"/>
    ///</summary>
    public static readonly Property published = new Property(new Uri("http://www.w3.org/ns/activitystreams#published"));    

    ///<summary>
    ///Specifies a radius around the point established by the longitude and latitude
    ///<see cref="http://www.w3.org/ns/activitystreams#radius"/>
    ///</summary>
    public static readonly Property radius = new Property(new Uri("http://www.w3.org/ns/activitystreams#radius"));    

    ///<summary>
    ///A numeric rating (>= 0.0, <= 5.0) for the object
    ///<see cref="http://www.w3.org/ns/activitystreams#rating"/>
    ///</summary>
    public static readonly Property rating = new Property(new Uri("http://www.w3.org/ns/activitystreams#rating"));    

    ///<summary>
    ///The RFC 5988 or HTML5 Link Relation associated with the Link
    ///<see cref="http://www.w3.org/ns/activitystreams#rel"/>
    ///</summary>
    public static readonly Property rel = new Property(new Uri("http://www.w3.org/ns/activitystreams#rel"));    

    ///<summary>
    ///In a strictly ordered logical collection, specifies the index position of the first item in the items list
    ///<see cref="http://www.w3.org/ns/activitystreams#startIndex"/>
    ///</summary>
    public static readonly Property startIndex = new Property(new Uri("http://www.w3.org/ns/activitystreams#startIndex"));    

    ///<summary>
    ///The starting time of the object
    ///<see cref="http://www.w3.org/ns/activitystreams#startTime"/>
    ///</summary>
    public static readonly Property startTime = new Property(new Uri("http://www.w3.org/ns/activitystreams#startTime"));    

    ///<summary>
    ///A short summary of the object
    ///<see cref="http://www.w3.org/ns/activitystreams#summary"/>
    ///</summary>
    public static readonly Property summary = new Property(new Uri("http://www.w3.org/ns/activitystreams#summary"));    

    ///<summary>
    ///The title of the object, HTML markup is permitted.
    ///<see cref="http://www.w3.org/ns/activitystreams#title"/>
    ///</summary>
    public static readonly Property title = new Property(new Uri("http://www.w3.org/ns/activitystreams#title"));    

    ///<summary>
    ///The total number of items in a logical collection
    ///<see cref="http://www.w3.org/ns/activitystreams#totalItems"/>
    ///</summary>
    public static readonly Property totalItems = new Property(new Uri("http://www.w3.org/ns/activitystreams#totalItems"));    

    ///<summary>
    ///Identifies the unit of measurement used by the radius, altitude and accuracy properties. The value can be expressed either as one of a set of predefined units or as a well-known common URI that identifies units.
    ///<see cref="http://www.w3.org/ns/activitystreams#units"/>
    ///</summary>
    public static readonly Property units = new Property(new Uri("http://www.w3.org/ns/activitystreams#units"));    

    ///<summary>
    ///Specifies when the object was last updated
    ///<see cref="http://www.w3.org/ns/activitystreams#updated"/>
    ///</summary>
    public static readonly Property updated = new Property(new Uri("http://www.w3.org/ns/activitystreams#updated"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#upstreamDuplicates"/>
    ///</summary>
    public static readonly Property upstreamDuplicates = new Property(new Uri("http://www.w3.org/ns/activitystreams#upstreamDuplicates"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#verb"/>
    ///</summary>
    public static readonly Property verb = new Property(new Uri("http://www.w3.org/ns/activitystreams#verb"));    

    ///<summary>
    ///Specifies the preferred display width of the content, expressed in terms of device independent pixels.
    ///<see cref="http://www.w3.org/ns/activitystreams#width"/>
    ///</summary>
    public static readonly Property width = new Property(new Uri("http://www.w3.org/ns/activitystreams#width"));    

    ///<summary>
    ///Actor accepts the Object
    ///<see cref="http://www.w3.org/ns/activitystreams#Accept"/>
    ///</summary>
    public static readonly Class Accept = new Class(new Uri("http://www.w3.org/ns/activitystreams#Accept"));    

    ///<summary>
    ///An Object representing some form of Action that has been taken
    ///<see cref="http://www.w3.org/ns/activitystreams#Activity"/>
    ///</summary>
    public static readonly Class Activity = new Class(new Uri("http://www.w3.org/ns/activitystreams#Activity"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#Block"/>
    ///</summary>
    public static readonly Class Block = new Class(new Uri("http://www.w3.org/ns/activitystreams#Block"));    

    ///<summary>
    ///An Activity that has no direct object
    ///<see cref="http://www.w3.org/ns/activitystreams#IntransitiveActivity"/>
    ///</summary>
    public static readonly Class IntransitiveActivity = new Class(new Uri("http://www.w3.org/ns/activitystreams#IntransitiveActivity"));    

    ///<summary>
    ///Any entity that can do something
    ///<see cref="http://www.w3.org/ns/activitystreams#Actor"/>
    ///</summary>
    public static readonly Class Actor = new Class(new Uri("http://www.w3.org/ns/activitystreams#Actor"));    

    ///<summary>
    ///To Add an Object or Link to Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Add"/>
    ///</summary>
    public static readonly Class Add = new Class(new Uri("http://www.w3.org/ns/activitystreams#Add"));    

    ///<summary>
    ///An Album.. typically a collection of photos
    ///<see cref="http://www.w3.org/ns/activitystreams#Album"/>
    ///</summary>
    public static readonly Class Album = new Class(new Uri("http://www.w3.org/ns/activitystreams#Album"));    

    ///<summary>
    ///Actor announces the object to the target
    ///<see cref="http://www.w3.org/ns/activitystreams#Announce"/>
    ///</summary>
    public static readonly Class Announce = new Class(new Uri("http://www.w3.org/ns/activitystreams#Announce"));    

    ///<summary>
    ///Represents a software application of any sort
    ///<see cref="http://www.w3.org/ns/activitystreams#Application"/>
    ///</summary>
    public static readonly Class Application = new Class(new Uri("http://www.w3.org/ns/activitystreams#Application"));    

    ///<summary>
    ///To Arrive Somewhere (can be used, for instance, to indicate that a particular entity is currently located somewhere, e.g. a "check-in")
    ///<see cref="http://www.w3.org/ns/activitystreams#Arrive"/>
    ///</summary>
    public static readonly Class Arrive = new Class(new Uri("http://www.w3.org/ns/activitystreams#Arrive"));    

    ///<summary>
    ///A written work. Typically several paragraphs long. For example, a blog post or a news article.
    ///<see cref="http://www.w3.org/ns/activitystreams#Article"/>
    ///</summary>
    public static readonly Class Article = new Class(new Uri("http://www.w3.org/ns/activitystreams#Article"));    

    ///<summary>
    ///An audio file
    ///<see cref="http://www.w3.org/ns/activitystreams#Audio"/>
    ///</summary>
    public static readonly Class Audio = new Class(new Uri("http://www.w3.org/ns/activitystreams#Audio"));    

    ///<summary>
    ///An ordered or unordered collection of Objects or Links
    ///<see cref="http://www.w3.org/ns/activitystreams#Collection"/>
    ///</summary>
    public static readonly Class Collection = new Class(new Uri("http://www.w3.org/ns/activitystreams#Collection"));    

    ///<summary>
    ///Represents a Social Graph relationship between two Individuals (indicated by the 'a' and 'b' properties)
    ///<see cref="http://www.w3.org/ns/activitystreams#Relationship"/>
    ///</summary>
    public static readonly Class Relationship = new Class(new Uri("http://www.w3.org/ns/activitystreams#Relationship"));    

    ///<summary>
    ///An Object that has content
    ///<see cref="http://www.w3.org/ns/activitystreams#Content"/>
    ///</summary>
    public static readonly Class Content = new Class(new Uri("http://www.w3.org/ns/activitystreams#Content"));    

    ///<summary>
    ///To Create Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Create"/>
    ///</summary>
    public static readonly Class Create = new Class(new Uri("http://www.w3.org/ns/activitystreams#Create"));    

    ///<summary>
    ///To Delete Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Delete"/>
    ///</summary>
    public static readonly Class Delete = new Class(new Uri("http://www.w3.org/ns/activitystreams#Delete"));    

    ///<summary>
    ///The actor dislikes the object
    ///<see cref="http://www.w3.org/ns/activitystreams#Dislike"/>
    ///</summary>
    public static readonly Class Dislike = new Class(new Uri("http://www.w3.org/ns/activitystreams#Dislike"));    

    ///<summary>
    ///Represents a digital document/file of any sort
    ///<see cref="http://www.w3.org/ns/activitystreams#Document"/>
    ///</summary>
    public static readonly Class Document = new Class(new Uri("http://www.w3.org/ns/activitystreams#Document"));    

    ///<summary>
    ///An Event of any kind
    ///<see cref="http://www.w3.org/ns/activitystreams#Event"/>
    ///</summary>
    public static readonly Class Event = new Class(new Uri("http://www.w3.org/ns/activitystreams#Event"));    

    ///<summary>
    ///To flag something (e.g. flag as inappropriate, flag as spam, etc)
    ///<see cref="http://www.w3.org/ns/activitystreams#Flag"/>
    ///</summary>
    public static readonly Class Flag = new Class(new Uri("http://www.w3.org/ns/activitystreams#Flag"));    

    ///<summary>
    ///Typically, a collection of files
    ///<see cref="http://www.w3.org/ns/activitystreams#Folder"/>
    ///</summary>
    public static readonly Class Folder = new Class(new Uri("http://www.w3.org/ns/activitystreams#Folder"));    

    ///<summary>
    ///To Express Interest in Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Follow"/>
    ///</summary>
    public static readonly Class Follow = new Class(new Uri("http://www.w3.org/ns/activitystreams#Follow"));    

    ///<summary>
    ///A Group of any kind.
    ///<see cref="http://www.w3.org/ns/activitystreams#Group"/>
    ///</summary>
    public static readonly Class Group = new Class(new Uri("http://www.w3.org/ns/activitystreams#Group"));    

    ///<summary>
    ///Actor is ignoring the Object
    ///<see cref="http://www.w3.org/ns/activitystreams#Ignore"/>
    ///</summary>
    public static readonly Class Ignore = new Class(new Uri("http://www.w3.org/ns/activitystreams#Ignore"));    

    ///<summary>
    ///An Image file
    ///<see cref="http://www.w3.org/ns/activitystreams#Image"/>
    ///</summary>
    public static readonly Class Image = new Class(new Uri("http://www.w3.org/ns/activitystreams#Image"));    

    ///<summary>
    ///To invite someone or something to something
    ///<see cref="http://www.w3.org/ns/activitystreams#Invite"/>
    ///</summary>
    public static readonly Class Invite = new Class(new Uri("http://www.w3.org/ns/activitystreams#Invite"));    

    ///<summary>
    ///To Join Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Join"/>
    ///</summary>
    public static readonly Class Join = new Class(new Uri("http://www.w3.org/ns/activitystreams#Join"));    

    ///<summary>
    ///To Leave Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Leave"/>
    ///</summary>
    public static readonly Class Leave = new Class(new Uri("http://www.w3.org/ns/activitystreams#Leave"));    

    ///<summary>
    ///To Like Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Like"/>
    ///</summary>
    public static readonly Class Like = new Class(new Uri("http://www.w3.org/ns/activitystreams#Like"));    

    ///<summary>
    ///The actor experienced the object
    ///<see cref="http://www.w3.org/ns/activitystreams#Experience"/>
    ///</summary>
    public static readonly Class Experience = new Class(new Uri("http://www.w3.org/ns/activitystreams#Experience"));    

    ///<summary>
    ///The actor viewed the object
    ///<see cref="http://www.w3.org/ns/activitystreams#View"/>
    ///</summary>
    public static readonly Class View = new Class(new Uri("http://www.w3.org/ns/activitystreams#View"));    

    ///<summary>
    ///The actor listened to the object
    ///<see cref="http://www.w3.org/ns/activitystreams#Listen"/>
    ///</summary>
    public static readonly Class Listen = new Class(new Uri("http://www.w3.org/ns/activitystreams#Listen"));    

    ///<summary>
    ///The actor read the object
    ///<see cref="http://www.w3.org/ns/activitystreams#Read"/>
    ///</summary>
    public static readonly Class Read = new Class(new Uri("http://www.w3.org/ns/activitystreams#Read"));    

    ///<summary>
    ///The actor is moving the object. The target specifies where the object is moving to. The origin specifies where the object is moving from.
    ///<see cref="http://www.w3.org/ns/activitystreams#Move"/>
    ///</summary>
    public static readonly Class Move = new Class(new Uri("http://www.w3.org/ns/activitystreams#Move"));    

    ///<summary>
    ///The actor is traveling to the target. The origin specifies where the actor is traveling from.
    ///<see cref="http://www.w3.org/ns/activitystreams#Travel"/>
    ///</summary>
    public static readonly Class Travel = new Class(new Uri("http://www.w3.org/ns/activitystreams#Travel"));    

    ///<summary>
    ///Represents a qualified reference to another resource. Patterned after the RFC5988 Web Linking Model
    ///<see cref="http://www.w3.org/ns/activitystreams#Link"/>
    ///</summary>
    public static readonly Class Link = new Class(new Uri("http://www.w3.org/ns/activitystreams#Link"));    

    ///<summary>
    ///A specialized Link that represents an @mention
    ///<see cref="http://www.w3.org/ns/activitystreams#Mention"/>
    ///</summary>
    public static readonly Class Mention = new Class(new Uri("http://www.w3.org/ns/activitystreams#Mention"));    

    ///<summary>
    ///A Short note, typically less than a single paragraph. A "tweet" is an example, or a "status update"
    ///<see cref="http://www.w3.org/ns/activitystreams#Note"/>
    ///</summary>
    public static readonly Class Note = new Class(new Uri("http://www.w3.org/ns/activitystreams#Note"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#Object"/>
    ///</summary>
    public static readonly Class Object = new Class(new Uri("http://www.w3.org/ns/activitystreams#Object"));    

    ///<summary>
    ///To Offer something to someone or something
    ///<see cref="http://www.w3.org/ns/activitystreams#Offer"/>
    ///</summary>
    public static readonly Class Offer = new Class(new Uri("http://www.w3.org/ns/activitystreams#Offer"));    

    ///<summary>
    ///A variation of Collection in which items are strictly ordered
    ///<see cref="http://www.w3.org/ns/activitystreams#OrderedCollection"/>
    ///</summary>
    public static readonly Class OrderedCollection = new Class(new Uri("http://www.w3.org/ns/activitystreams#OrderedCollection"));    

    ///<summary>
    ///A rdf:List variant for Objects and Links
    ///<see cref="http://www.w3.org/ns/activitystreams#OrderedItems"/>
    ///</summary>
    public static readonly Class OrderedItems = new Class(new Uri("http://www.w3.org/ns/activitystreams#OrderedItems"));    

    ///<summary>
    ///A Web Page
    ///<see cref="http://www.w3.org/ns/activitystreams#Page"/>
    ///</summary>
    public static readonly Class Page = new Class(new Uri("http://www.w3.org/ns/activitystreams#Page"));    

    ///<summary>
    ///A Person
    ///<see cref="http://www.w3.org/ns/activitystreams#Person"/>
    ///</summary>
    public static readonly Class Person = new Class(new Uri("http://www.w3.org/ns/activitystreams#Person"));    

    ///<summary>
    ///An Organization
    ///<see cref="http://www.w3.org/ns/activitystreams#Organization"/>
    ///</summary>
    public static readonly Class Organization = new Class(new Uri("http://www.w3.org/ns/activitystreams#Organization"));    

    ///<summary>
    ///A Profile Document
    ///<see cref="http://www.w3.org/ns/activitystreams#Profile"/>
    ///</summary>
    public static readonly Class Profile = new Class(new Uri("http://www.w3.org/ns/activitystreams#Profile"));    

    ///<summary>
    ///A physical or logical location
    ///<see cref="http://www.w3.org/ns/activitystreams#Place"/>
    ///</summary>
    public static readonly Class Place = new Class(new Uri("http://www.w3.org/ns/activitystreams#Place"));    

    ///<summary>
    ///Any form of short or long running process
    ///<see cref="http://www.w3.org/ns/activitystreams#Process"/>
    ///</summary>
    public static readonly Class Process = new Class(new Uri("http://www.w3.org/ns/activitystreams#Process"));    

    ///<summary>
    ///A question of any sort.
    ///<see cref="http://www.w3.org/ns/activitystreams#Question"/>
    ///</summary>
    public static readonly Class Question = new Class(new Uri("http://www.w3.org/ns/activitystreams#Question"));    

    ///<summary>
    ///Actor rejects the Object
    ///<see cref="http://www.w3.org/ns/activitystreams#Reject"/>
    ///</summary>
    public static readonly Class Reject = new Class(new Uri("http://www.w3.org/ns/activitystreams#Reject"));    

    ///<summary>
    ///To Remove Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Remove"/>
    ///</summary>
    public static readonly Class Remove = new Class(new Uri("http://www.w3.org/ns/activitystreams#Remove"));    

    ///<summary>
    ///A service provided by some entity
    ///<see cref="http://www.w3.org/ns/activitystreams#Service"/>
    ///</summary>
    public static readonly Class Service = new Class(new Uri("http://www.w3.org/ns/activitystreams#Service"));    

    ///<summary>
    ///An ordered collection of content sharing a common purpose or characteristic
    ///<see cref="http://www.w3.org/ns/activitystreams#Story"/>
    ///</summary>
    public static readonly Class Story = new Class(new Uri("http://www.w3.org/ns/activitystreams#Story"));    

    ///<summary>
    ///Actor tentatively accepts the Object
    ///<see cref="http://www.w3.org/ns/activitystreams#TentativeAccept"/>
    ///</summary>
    public static readonly Class TentativeAccept = new Class(new Uri("http://www.w3.org/ns/activitystreams#TentativeAccept"));    

    ///<summary>
    ///Actor tentatively rejects the object
    ///<see cref="http://www.w3.org/ns/activitystreams#TentativeReject"/>
    ///</summary>
    public static readonly Class TentativeReject = new Class(new Uri("http://www.w3.org/ns/activitystreams#TentativeReject"));    

    ///<summary>
    ///To Undo Something. This would typically be used to indicate that a previous Activity has been undone.
    ///<see cref="http://www.w3.org/ns/activitystreams#Undo"/>
    ///</summary>
    public static readonly Class Undo = new Class(new Uri("http://www.w3.org/ns/activitystreams#Undo"));    

    ///<summary>
    ///To Update/Modify Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Update"/>
    ///</summary>
    public static readonly Class Update = new Class(new Uri("http://www.w3.org/ns/activitystreams#Update"));    

    ///<summary>
    ///A Video document of any kind.
    ///<see cref="http://www.w3.org/ns/activitystreams#Video"/>
    ///</summary>
    public static readonly Class Video = new Class(new Uri("http://www.w3.org/ns/activitystreams#Video"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#nil"/>
    ///</summary>
    public static readonly Resource nil = new Resource(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#nil"));
}
///<summary>
///
///
///</summary>
public static class AS2
{
    public static readonly Uri Namespace = new Uri("http://www.w3.org/ns/activitystreams#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "AS2";
    public static string GetPrefix() { return Prefix; } 

    ///<summary>
    ///Extended Activity Streams 2.0 Vocabulary
    ///<see cref="http://www.w3.org/ns/activitystreams#"/>
    ///</summary>
    public const string activitystreams = "http://www.w3.org/ns/activitystreams#";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#langString"/>
    ///</summary>
    public const string langString = "http://www.w3.org/1999/02/22-rdf-syntax-ns#langString";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/XMLSchema#duration"/>
    ///</summary>
    public const string duration = "http://www.w3.org/2001/XMLSchema#duration";

    ///<summary>
    ///Subproperty of as:attributedTo that identifies the primary actor
    ///<see cref="http://www.w3.org/ns/activitystreams#actor"/>
    ///</summary>
    public const string actor = "http://www.w3.org/ns/activitystreams#actor";

    ///<summary>
    ///Identifies an entity to which an object is attributed
    ///<see cref="http://www.w3.org/ns/activitystreams#attributedTo"/>
    ///</summary>
    public const string attributedTo = "http://www.w3.org/ns/activitystreams#attributedTo";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#attachment"/>
    ///</summary>
    public const string attachment = "http://www.w3.org/ns/activitystreams#attachment";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#attachments"/>
    ///</summary>
    public const string attachments = "http://www.w3.org/ns/activitystreams#attachments";

    ///<summary>
    ///Identifies the author of an object. Deprecated. Use as:attributedTo instead
    ///<see cref="http://www.w3.org/ns/activitystreams#author"/>
    ///</summary>
    public const string author = "http://www.w3.org/ns/activitystreams#author";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#bcc"/>
    ///</summary>
    public const string bcc = "http://www.w3.org/ns/activitystreams#bcc";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#bto"/>
    ///</summary>
    public const string bto = "http://www.w3.org/ns/activitystreams#bto";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#cc"/>
    ///</summary>
    public const string cc = "http://www.w3.org/ns/activitystreams#cc";

    ///<summary>
    ///Specifies the context within which an object exists or an activity was performed
    ///<see cref="http://www.w3.org/ns/activitystreams#context"/>
    ///</summary>
    public const string context = "http://www.w3.org/ns/activitystreams#context";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#current"/>
    ///</summary>
    public const string current = "http://www.w3.org/ns/activitystreams#current";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#first"/>
    ///</summary>
    public const string first = "http://www.w3.org/ns/activitystreams#first";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#generator"/>
    ///</summary>
    public const string generator = "http://www.w3.org/ns/activitystreams#generator";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#icon"/>
    ///</summary>
    public const string icon = "http://www.w3.org/ns/activitystreams#icon";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#image"/>
    ///</summary>
    public const string image = "http://www.w3.org/ns/activitystreams#image";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#inReplyTo"/>
    ///</summary>
    public const string inReplyTo = "http://www.w3.org/ns/activitystreams#inReplyTo";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#items"/>
    ///</summary>
    public const string items = "http://www.w3.org/ns/activitystreams#items";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#last"/>
    ///</summary>
    public const string last = "http://www.w3.org/ns/activitystreams#last";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#location"/>
    ///</summary>
    public const string location = "http://www.w3.org/ns/activitystreams#location";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#next"/>
    ///</summary>
    public const string next = "http://www.w3.org/ns/activitystreams#next";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#object"/>
    ///</summary>
    public const string _object = "http://www.w3.org/ns/activitystreams#object";

    ///<summary>
    ///Describes a possible exclusive answer or option for a question.
    ///<see cref="http://www.w3.org/ns/activitystreams#oneOf"/>
    ///</summary>
    public const string oneOf = "http://www.w3.org/ns/activitystreams#oneOf";

    ///<summary>
    ///Describes a possible inclusive answer or option for a question.
    ///<see cref="http://www.w3.org/ns/activitystreams#anyOf"/>
    ///</summary>
    public const string anyOf = "http://www.w3.org/ns/activitystreams#anyOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#prev"/>
    ///</summary>
    public const string prev = "http://www.w3.org/ns/activitystreams#prev";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#preview"/>
    ///</summary>
    public const string preview = "http://www.w3.org/ns/activitystreams#preview";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#provider"/>
    ///</summary>
    public const string provider = "http://www.w3.org/ns/activitystreams#provider";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#replies"/>
    ///</summary>
    public const string replies = "http://www.w3.org/ns/activitystreams#replies";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#result"/>
    ///</summary>
    public const string result = "http://www.w3.org/ns/activitystreams#result";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#scope"/>
    ///</summary>
    public const string scope = "http://www.w3.org/ns/activitystreams#scope";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#self"/>
    ///</summary>
    public const string self = "http://www.w3.org/ns/activitystreams#self";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#tag"/>
    ///</summary>
    public const string tag = "http://www.w3.org/ns/activitystreams#tag";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#tags"/>
    ///</summary>
    public const string tags = "http://www.w3.org/ns/activitystreams#tags";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#target"/>
    ///</summary>
    public const string target = "http://www.w3.org/ns/activitystreams#target";

    ///<summary>
    ///For certain activities, specifies the entity from which the action is directed.
    ///<see cref="http://www.w3.org/ns/activitystreams#origin"/>
    ///</summary>
    public const string origin = "http://www.w3.org/ns/activitystreams#origin";

    ///<summary>
    ///Indentifies an object used (or to be used) to complete an activity
    ///<see cref="http://www.w3.org/ns/activitystreams#instrument"/>
    ///</summary>
    public const string instrument = "http://www.w3.org/ns/activitystreams#instrument";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#to"/>
    ///</summary>
    public const string to = "http://www.w3.org/ns/activitystreams#to";

    ///<summary>
    ///Specifies a link to a specific representation of the Object
    ///<see cref="http://www.w3.org/ns/activitystreams#url"/>
    ///</summary>
    public const string url = "http://www.w3.org/ns/activitystreams#url";

    ///<summary>
    ///On a Relationship object, identifies the subject. e.g. when saying "John is connected to Sally", 'subject' refers to 'John'
    ///<see cref="http://www.w3.org/ns/activitystreams#subject"/>
    ///</summary>
    public const string subject = "http://www.w3.org/ns/activitystreams#subject";

    ///<summary>
    ///On a Relationship object, describes the type of relationship
    ///<see cref="http://www.w3.org/ns/activitystreams#relationship"/>
    ///</summary>
    public const string relationship = "http://www.w3.org/ns/activitystreams#relationship";

    ///<summary>
    ///On a Profile object, describes the object described by the profile
    ///<see cref="http://www.w3.org/ns/activitystreams#describes"/>
    ///</summary>
    public const string describes = "http://www.w3.org/ns/activitystreams#describes";

    ///<summary>
    ///Specifies the accuracy around the point established by the longitude and latitude
    ///<see cref="http://www.w3.org/ns/activitystreams#accuracy"/>
    ///</summary>
    public const string accuracy = "http://www.w3.org/ns/activitystreams#accuracy";

    ///<summary>
    ///An alternative, domain specific alias for an object
    ///<see cref="http://www.w3.org/ns/activitystreams#alias"/>
    ///</summary>
    public const string alias = "http://www.w3.org/ns/activitystreams#alias";

    ///<summary>
    ///The altitude of a place
    ///<see cref="http://www.w3.org/ns/activitystreams#altitude"/>
    ///</summary>
    public const string altitude = "http://www.w3.org/ns/activitystreams#altitude";

    ///<summary>
    ///The content of the object.
    ///<see cref="http://www.w3.org/ns/activitystreams#content"/>
    ///</summary>
    public const string content = "http://www.w3.org/ns/activitystreams#content";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#displayName"/>
    ///</summary>
    public const string displayName = "http://www.w3.org/ns/activitystreams#displayName";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#downstreamDuplicates"/>
    ///</summary>
    public const string downstreamDuplicates = "http://www.w3.org/ns/activitystreams#downstreamDuplicates";

    ///<summary>
    ///The duration of the object
    ///<see cref="http://www.w3.org/ns/activitystreams#duration"/>
    ///</summary>
    public const string duration_0 = "http://www.w3.org/ns/activitystreams#duration";

    ///<summary>
    ///The ending time of the object
    ///<see cref="http://www.w3.org/ns/activitystreams#endTime"/>
    ///</summary>
    public const string endTime = "http://www.w3.org/ns/activitystreams#endTime";

    ///<summary>
    ///The display height expressed as device independent pixels
    ///<see cref="http://www.w3.org/ns/activitystreams#height"/>
    ///</summary>
    public const string height = "http://www.w3.org/ns/activitystreams#height";

    ///<summary>
    ///The target URI of the Link
    ///<see cref="http://www.w3.org/ns/activitystreams#href"/>
    ///</summary>
    public const string href = "http://www.w3.org/ns/activitystreams#href";

    ///<summary>
    ///A hint about the language of the referenced resource
    ///<see cref="http://www.w3.org/ns/activitystreams#hreflang"/>
    ///</summary>
    public const string hreflang = "http://www.w3.org/ns/activitystreams#hreflang";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#id"/>
    ///</summary>
    public const string id = "http://www.w3.org/ns/activitystreams#id";

    ///<summary>
    ///The maximum number of items per page in a logical Collection
    ///<see cref="http://www.w3.org/ns/activitystreams#itemsPerPage"/>
    ///</summary>
    public const string itemsPerPage = "http://www.w3.org/ns/activitystreams#itemsPerPage";

    ///<summary>
    ///The latitude
    ///<see cref="http://www.w3.org/ns/activitystreams#latitude"/>
    ///</summary>
    public const string latitude = "http://www.w3.org/ns/activitystreams#latitude";

    ///<summary>
    ///The longitude
    ///<see cref="http://www.w3.org/ns/activitystreams#longitude"/>
    ///</summary>
    public const string longitude = "http://www.w3.org/ns/activitystreams#longitude";

    ///<summary>
    ///The MIME Media Type
    ///<see cref="http://www.w3.org/ns/activitystreams#mediaType"/>
    ///</summary>
    public const string mediaType = "http://www.w3.org/ns/activitystreams#mediaType";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#objectType"/>
    ///</summary>
    public const string objectType = "http://www.w3.org/ns/activitystreams#objectType";

    ///<summary>
    ///Specifies the relative priority of the Activity
    ///<see cref="http://www.w3.org/ns/activitystreams#priority"/>
    ///</summary>
    public const string priority = "http://www.w3.org/ns/activitystreams#priority";

    ///<summary>
    ///Specifies the date and time the object was published
    ///<see cref="http://www.w3.org/ns/activitystreams#published"/>
    ///</summary>
    public const string published = "http://www.w3.org/ns/activitystreams#published";

    ///<summary>
    ///Specifies a radius around the point established by the longitude and latitude
    ///<see cref="http://www.w3.org/ns/activitystreams#radius"/>
    ///</summary>
    public const string radius = "http://www.w3.org/ns/activitystreams#radius";

    ///<summary>
    ///A numeric rating (>= 0.0, <= 5.0) for the object
    ///<see cref="http://www.w3.org/ns/activitystreams#rating"/>
    ///</summary>
    public const string rating = "http://www.w3.org/ns/activitystreams#rating";

    ///<summary>
    ///The RFC 5988 or HTML5 Link Relation associated with the Link
    ///<see cref="http://www.w3.org/ns/activitystreams#rel"/>
    ///</summary>
    public const string rel = "http://www.w3.org/ns/activitystreams#rel";

    ///<summary>
    ///In a strictly ordered logical collection, specifies the index position of the first item in the items list
    ///<see cref="http://www.w3.org/ns/activitystreams#startIndex"/>
    ///</summary>
    public const string startIndex = "http://www.w3.org/ns/activitystreams#startIndex";

    ///<summary>
    ///The starting time of the object
    ///<see cref="http://www.w3.org/ns/activitystreams#startTime"/>
    ///</summary>
    public const string startTime = "http://www.w3.org/ns/activitystreams#startTime";

    ///<summary>
    ///A short summary of the object
    ///<see cref="http://www.w3.org/ns/activitystreams#summary"/>
    ///</summary>
    public const string summary = "http://www.w3.org/ns/activitystreams#summary";

    ///<summary>
    ///The title of the object, HTML markup is permitted.
    ///<see cref="http://www.w3.org/ns/activitystreams#title"/>
    ///</summary>
    public const string title = "http://www.w3.org/ns/activitystreams#title";

    ///<summary>
    ///The total number of items in a logical collection
    ///<see cref="http://www.w3.org/ns/activitystreams#totalItems"/>
    ///</summary>
    public const string totalItems = "http://www.w3.org/ns/activitystreams#totalItems";

    ///<summary>
    ///Identifies the unit of measurement used by the radius, altitude and accuracy properties. The value can be expressed either as one of a set of predefined units or as a well-known common URI that identifies units.
    ///<see cref="http://www.w3.org/ns/activitystreams#units"/>
    ///</summary>
    public const string units = "http://www.w3.org/ns/activitystreams#units";

    ///<summary>
    ///Specifies when the object was last updated
    ///<see cref="http://www.w3.org/ns/activitystreams#updated"/>
    ///</summary>
    public const string updated = "http://www.w3.org/ns/activitystreams#updated";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#upstreamDuplicates"/>
    ///</summary>
    public const string upstreamDuplicates = "http://www.w3.org/ns/activitystreams#upstreamDuplicates";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#verb"/>
    ///</summary>
    public const string verb = "http://www.w3.org/ns/activitystreams#verb";

    ///<summary>
    ///Specifies the preferred display width of the content, expressed in terms of device independent pixels.
    ///<see cref="http://www.w3.org/ns/activitystreams#width"/>
    ///</summary>
    public const string width = "http://www.w3.org/ns/activitystreams#width";

    ///<summary>
    ///Actor accepts the Object
    ///<see cref="http://www.w3.org/ns/activitystreams#Accept"/>
    ///</summary>
    public const string Accept = "http://www.w3.org/ns/activitystreams#Accept";

    ///<summary>
    ///An Object representing some form of Action that has been taken
    ///<see cref="http://www.w3.org/ns/activitystreams#Activity"/>
    ///</summary>
    public const string Activity = "http://www.w3.org/ns/activitystreams#Activity";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#Block"/>
    ///</summary>
    public const string Block = "http://www.w3.org/ns/activitystreams#Block";

    ///<summary>
    ///An Activity that has no direct object
    ///<see cref="http://www.w3.org/ns/activitystreams#IntransitiveActivity"/>
    ///</summary>
    public const string IntransitiveActivity = "http://www.w3.org/ns/activitystreams#IntransitiveActivity";

    ///<summary>
    ///Any entity that can do something
    ///<see cref="http://www.w3.org/ns/activitystreams#Actor"/>
    ///</summary>
    public const string Actor = "http://www.w3.org/ns/activitystreams#Actor";

    ///<summary>
    ///To Add an Object or Link to Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Add"/>
    ///</summary>
    public const string Add = "http://www.w3.org/ns/activitystreams#Add";

    ///<summary>
    ///An Album.. typically a collection of photos
    ///<see cref="http://www.w3.org/ns/activitystreams#Album"/>
    ///</summary>
    public const string Album = "http://www.w3.org/ns/activitystreams#Album";

    ///<summary>
    ///Actor announces the object to the target
    ///<see cref="http://www.w3.org/ns/activitystreams#Announce"/>
    ///</summary>
    public const string Announce = "http://www.w3.org/ns/activitystreams#Announce";

    ///<summary>
    ///Represents a software application of any sort
    ///<see cref="http://www.w3.org/ns/activitystreams#Application"/>
    ///</summary>
    public const string Application = "http://www.w3.org/ns/activitystreams#Application";

    ///<summary>
    ///To Arrive Somewhere (can be used, for instance, to indicate that a particular entity is currently located somewhere, e.g. a "check-in")
    ///<see cref="http://www.w3.org/ns/activitystreams#Arrive"/>
    ///</summary>
    public const string Arrive = "http://www.w3.org/ns/activitystreams#Arrive";

    ///<summary>
    ///A written work. Typically several paragraphs long. For example, a blog post or a news article.
    ///<see cref="http://www.w3.org/ns/activitystreams#Article"/>
    ///</summary>
    public const string Article = "http://www.w3.org/ns/activitystreams#Article";

    ///<summary>
    ///An audio file
    ///<see cref="http://www.w3.org/ns/activitystreams#Audio"/>
    ///</summary>
    public const string Audio = "http://www.w3.org/ns/activitystreams#Audio";

    ///<summary>
    ///An ordered or unordered collection of Objects or Links
    ///<see cref="http://www.w3.org/ns/activitystreams#Collection"/>
    ///</summary>
    public const string Collection = "http://www.w3.org/ns/activitystreams#Collection";

    ///<summary>
    ///Represents a Social Graph relationship between two Individuals (indicated by the 'a' and 'b' properties)
    ///<see cref="http://www.w3.org/ns/activitystreams#Relationship"/>
    ///</summary>
    public const string Relationship = "http://www.w3.org/ns/activitystreams#Relationship";

    ///<summary>
    ///An Object that has content
    ///<see cref="http://www.w3.org/ns/activitystreams#Content"/>
    ///</summary>
    public const string Content = "http://www.w3.org/ns/activitystreams#Content";

    ///<summary>
    ///To Create Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Create"/>
    ///</summary>
    public const string Create = "http://www.w3.org/ns/activitystreams#Create";

    ///<summary>
    ///To Delete Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Delete"/>
    ///</summary>
    public const string Delete = "http://www.w3.org/ns/activitystreams#Delete";

    ///<summary>
    ///The actor dislikes the object
    ///<see cref="http://www.w3.org/ns/activitystreams#Dislike"/>
    ///</summary>
    public const string Dislike = "http://www.w3.org/ns/activitystreams#Dislike";

    ///<summary>
    ///Represents a digital document/file of any sort
    ///<see cref="http://www.w3.org/ns/activitystreams#Document"/>
    ///</summary>
    public const string Document = "http://www.w3.org/ns/activitystreams#Document";

    ///<summary>
    ///An Event of any kind
    ///<see cref="http://www.w3.org/ns/activitystreams#Event"/>
    ///</summary>
    public const string Event = "http://www.w3.org/ns/activitystreams#Event";

    ///<summary>
    ///To flag something (e.g. flag as inappropriate, flag as spam, etc)
    ///<see cref="http://www.w3.org/ns/activitystreams#Flag"/>
    ///</summary>
    public const string Flag = "http://www.w3.org/ns/activitystreams#Flag";

    ///<summary>
    ///Typically, a collection of files
    ///<see cref="http://www.w3.org/ns/activitystreams#Folder"/>
    ///</summary>
    public const string Folder = "http://www.w3.org/ns/activitystreams#Folder";

    ///<summary>
    ///To Express Interest in Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Follow"/>
    ///</summary>
    public const string Follow = "http://www.w3.org/ns/activitystreams#Follow";

    ///<summary>
    ///A Group of any kind.
    ///<see cref="http://www.w3.org/ns/activitystreams#Group"/>
    ///</summary>
    public const string Group = "http://www.w3.org/ns/activitystreams#Group";

    ///<summary>
    ///Actor is ignoring the Object
    ///<see cref="http://www.w3.org/ns/activitystreams#Ignore"/>
    ///</summary>
    public const string Ignore = "http://www.w3.org/ns/activitystreams#Ignore";

    ///<summary>
    ///An Image file
    ///<see cref="http://www.w3.org/ns/activitystreams#Image"/>
    ///</summary>
    public const string Image = "http://www.w3.org/ns/activitystreams#Image";

    ///<summary>
    ///To invite someone or something to something
    ///<see cref="http://www.w3.org/ns/activitystreams#Invite"/>
    ///</summary>
    public const string Invite = "http://www.w3.org/ns/activitystreams#Invite";

    ///<summary>
    ///To Join Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Join"/>
    ///</summary>
    public const string Join = "http://www.w3.org/ns/activitystreams#Join";

    ///<summary>
    ///To Leave Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Leave"/>
    ///</summary>
    public const string Leave = "http://www.w3.org/ns/activitystreams#Leave";

    ///<summary>
    ///To Like Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Like"/>
    ///</summary>
    public const string Like = "http://www.w3.org/ns/activitystreams#Like";

    ///<summary>
    ///The actor experienced the object
    ///<see cref="http://www.w3.org/ns/activitystreams#Experience"/>
    ///</summary>
    public const string Experience = "http://www.w3.org/ns/activitystreams#Experience";

    ///<summary>
    ///The actor viewed the object
    ///<see cref="http://www.w3.org/ns/activitystreams#View"/>
    ///</summary>
    public const string View = "http://www.w3.org/ns/activitystreams#View";

    ///<summary>
    ///The actor listened to the object
    ///<see cref="http://www.w3.org/ns/activitystreams#Listen"/>
    ///</summary>
    public const string Listen = "http://www.w3.org/ns/activitystreams#Listen";

    ///<summary>
    ///The actor read the object
    ///<see cref="http://www.w3.org/ns/activitystreams#Read"/>
    ///</summary>
    public const string Read = "http://www.w3.org/ns/activitystreams#Read";

    ///<summary>
    ///The actor is moving the object. The target specifies where the object is moving to. The origin specifies where the object is moving from.
    ///<see cref="http://www.w3.org/ns/activitystreams#Move"/>
    ///</summary>
    public const string Move = "http://www.w3.org/ns/activitystreams#Move";

    ///<summary>
    ///The actor is traveling to the target. The origin specifies where the actor is traveling from.
    ///<see cref="http://www.w3.org/ns/activitystreams#Travel"/>
    ///</summary>
    public const string Travel = "http://www.w3.org/ns/activitystreams#Travel";

    ///<summary>
    ///Represents a qualified reference to another resource. Patterned after the RFC5988 Web Linking Model
    ///<see cref="http://www.w3.org/ns/activitystreams#Link"/>
    ///</summary>
    public const string Link = "http://www.w3.org/ns/activitystreams#Link";

    ///<summary>
    ///A specialized Link that represents an @mention
    ///<see cref="http://www.w3.org/ns/activitystreams#Mention"/>
    ///</summary>
    public const string Mention = "http://www.w3.org/ns/activitystreams#Mention";

    ///<summary>
    ///A Short note, typically less than a single paragraph. A "tweet" is an example, or a "status update"
    ///<see cref="http://www.w3.org/ns/activitystreams#Note"/>
    ///</summary>
    public const string Note = "http://www.w3.org/ns/activitystreams#Note";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/activitystreams#Object"/>
    ///</summary>
    public const string Object = "http://www.w3.org/ns/activitystreams#Object";

    ///<summary>
    ///To Offer something to someone or something
    ///<see cref="http://www.w3.org/ns/activitystreams#Offer"/>
    ///</summary>
    public const string Offer = "http://www.w3.org/ns/activitystreams#Offer";

    ///<summary>
    ///A variation of Collection in which items are strictly ordered
    ///<see cref="http://www.w3.org/ns/activitystreams#OrderedCollection"/>
    ///</summary>
    public const string OrderedCollection = "http://www.w3.org/ns/activitystreams#OrderedCollection";

    ///<summary>
    ///A rdf:List variant for Objects and Links
    ///<see cref="http://www.w3.org/ns/activitystreams#OrderedItems"/>
    ///</summary>
    public const string OrderedItems = "http://www.w3.org/ns/activitystreams#OrderedItems";

    ///<summary>
    ///A Web Page
    ///<see cref="http://www.w3.org/ns/activitystreams#Page"/>
    ///</summary>
    public const string Page = "http://www.w3.org/ns/activitystreams#Page";

    ///<summary>
    ///A Person
    ///<see cref="http://www.w3.org/ns/activitystreams#Person"/>
    ///</summary>
    public const string Person = "http://www.w3.org/ns/activitystreams#Person";

    ///<summary>
    ///An Organization
    ///<see cref="http://www.w3.org/ns/activitystreams#Organization"/>
    ///</summary>
    public const string Organization = "http://www.w3.org/ns/activitystreams#Organization";

    ///<summary>
    ///A Profile Document
    ///<see cref="http://www.w3.org/ns/activitystreams#Profile"/>
    ///</summary>
    public const string Profile = "http://www.w3.org/ns/activitystreams#Profile";

    ///<summary>
    ///A physical or logical location
    ///<see cref="http://www.w3.org/ns/activitystreams#Place"/>
    ///</summary>
    public const string Place = "http://www.w3.org/ns/activitystreams#Place";

    ///<summary>
    ///Any form of short or long running process
    ///<see cref="http://www.w3.org/ns/activitystreams#Process"/>
    ///</summary>
    public const string Process = "http://www.w3.org/ns/activitystreams#Process";

    ///<summary>
    ///A question of any sort.
    ///<see cref="http://www.w3.org/ns/activitystreams#Question"/>
    ///</summary>
    public const string Question = "http://www.w3.org/ns/activitystreams#Question";

    ///<summary>
    ///Actor rejects the Object
    ///<see cref="http://www.w3.org/ns/activitystreams#Reject"/>
    ///</summary>
    public const string Reject = "http://www.w3.org/ns/activitystreams#Reject";

    ///<summary>
    ///To Remove Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Remove"/>
    ///</summary>
    public const string Remove = "http://www.w3.org/ns/activitystreams#Remove";

    ///<summary>
    ///A service provided by some entity
    ///<see cref="http://www.w3.org/ns/activitystreams#Service"/>
    ///</summary>
    public const string Service = "http://www.w3.org/ns/activitystreams#Service";

    ///<summary>
    ///An ordered collection of content sharing a common purpose or characteristic
    ///<see cref="http://www.w3.org/ns/activitystreams#Story"/>
    ///</summary>
    public const string Story = "http://www.w3.org/ns/activitystreams#Story";

    ///<summary>
    ///Actor tentatively accepts the Object
    ///<see cref="http://www.w3.org/ns/activitystreams#TentativeAccept"/>
    ///</summary>
    public const string TentativeAccept = "http://www.w3.org/ns/activitystreams#TentativeAccept";

    ///<summary>
    ///Actor tentatively rejects the object
    ///<see cref="http://www.w3.org/ns/activitystreams#TentativeReject"/>
    ///</summary>
    public const string TentativeReject = "http://www.w3.org/ns/activitystreams#TentativeReject";

    ///<summary>
    ///To Undo Something. This would typically be used to indicate that a previous Activity has been undone.
    ///<see cref="http://www.w3.org/ns/activitystreams#Undo"/>
    ///</summary>
    public const string Undo = "http://www.w3.org/ns/activitystreams#Undo";

    ///<summary>
    ///To Update/Modify Something
    ///<see cref="http://www.w3.org/ns/activitystreams#Update"/>
    ///</summary>
    public const string Update = "http://www.w3.org/ns/activitystreams#Update";

    ///<summary>
    ///A Video document of any kind.
    ///<see cref="http://www.w3.org/ns/activitystreams#Video"/>
    ///</summary>
    public const string Video = "http://www.w3.org/ns/activitystreams#Video";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#nil"/>
    ///</summary>
    public const string nil = "http://www.w3.org/1999/02/22-rdf-syntax-ns#nil";
}
///<summary>
///
///
///</summary>
public class prov : Ontology
{
    public static readonly Uri Namespace = new Uri("http://www.w3.org/ns/prov#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "prov";
    public static string GetPrefix() { return Prefix; }     

    ///<summary>
    ///This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page).
    ///
    ///If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/
    ///Archives/Public/public-prov-comments/). All feedback is welcome.
    ///<see cref="http://www.w3.org/ns/prov#"/>
    ///</summary>
    public static readonly Resource prov_0 = new Resource(new Uri("http://www.w3.org/ns/prov#"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#comment"/>
    ///</summary>
    public static readonly Property comment = new Property(new Uri("http://www.w3.org/2000/01/rdf-schema#comment"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#isDefinedBy"/>
    ///</summary>
    public static readonly Property isDefinedBy = new Property(new Uri("http://www.w3.org/2000/01/rdf-schema#isDefinedBy"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#label"/>
    ///</summary>
    public static readonly Property label = new Property(new Uri("http://www.w3.org/2000/01/rdf-schema#label"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#seeAlso"/>
    ///</summary>
    public static readonly Property seeAlso = new Property(new Uri("http://www.w3.org/2000/01/rdf-schema#seeAlso"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2002/07/owl#Thing"/>
    ///</summary>
    public static readonly Class Thing = new Class(new Uri("http://www.w3.org/2002/07/owl#Thing"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2002/07/owl#versionInfo"/>
    ///</summary>
    public static readonly Property versionInfo = new Property(new Uri("http://www.w3.org/2002/07/owl#versionInfo"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Activity"/>
    ///</summary>
    public static readonly Class Activity = new Class(new Uri("http://www.w3.org/ns/prov#Activity"));    

    ///<summary>
    ///ActivityInfluence provides additional descriptions of an Activity's binary influence upon any other kind of resource. Instances of ActivityInfluence use the prov:activity property to cite the influencing Activity.
    ///<see cref="http://www.w3.org/ns/prov#ActivityInfluence"/>
    ///</summary>
    public static readonly Class ActivityInfluence = new Class(new Uri("http://www.w3.org/ns/prov#ActivityInfluence"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Agent"/>
    ///</summary>
    public static readonly Class Agent = new Class(new Uri("http://www.w3.org/ns/prov#Agent"));    

    ///<summary>
    ///AgentInfluence provides additional descriptions of an Agent's binary influence upon any other kind of resource. Instances of AgentInfluence use the prov:agent property to cite the influencing Agent.
    ///<see cref="http://www.w3.org/ns/prov#AgentInfluence"/>
    ///</summary>
    public static readonly Class AgentInfluence = new Class(new Uri("http://www.w3.org/ns/prov#AgentInfluence"));    

    ///<summary>
    ///An instance of prov:Association provides additional descriptions about the binary prov:wasAssociatedWith relation from an prov:Activity to some prov:Agent that had some responsiblity for it. For example, :baking prov:wasAssociatedWith :baker; prov:qualifiedAssociation [ a prov:Association; prov:agent :baker; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Association"/>
    ///</summary>
    public static readonly Class Association = new Class(new Uri("http://www.w3.org/ns/prov#Association"));    

    ///<summary>
    ///An instance of prov:Attribution provides additional descriptions about the binary prov:wasAttributedTo relation from an prov:Entity to some prov:Agent that had some responsible for it. For example, :cake prov:wasAttributedTo :baker; prov:qualifiedAttribution [ a prov:Attribution; prov:entity :baker; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Attribution"/>
    ///</summary>
    public static readonly Class Attribution = new Class(new Uri("http://www.w3.org/ns/prov#Attribution"));    

    ///<summary>
    ///Note that there are kinds of bundles (e.g. handwritten letters, audio recordings, etc.) that are not expressed in PROV-O, but can be still be described by PROV-O.
    ///<see cref="http://www.w3.org/ns/prov#Bundle"/>
    ///</summary>
    public static readonly Class Bundle = new Class(new Uri("http://www.w3.org/ns/prov#Bundle"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Collection"/>
    ///</summary>
    public static readonly Class Collection = new Class(new Uri("http://www.w3.org/ns/prov#Collection"));    

    ///<summary>
    ///An instance of prov:Communication provides additional descriptions about the binary prov:wasInformedBy relation from an informed prov:Activity to the prov:Activity that informed it. For example, :you_jumping_off_bridge prov:wasInformedBy :everyone_else_jumping_off_bridge; prov:qualifiedCommunication [ a prov:Communication; prov:activity :everyone_else_jumping_off_bridge; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Communication"/>
    ///</summary>
    public static readonly Class Communication = new Class(new Uri("http://www.w3.org/ns/prov#Communication"));    

    ///<summary>
    ///An instance of prov:Delegation provides additional descriptions about the binary prov:actedOnBehalfOf relation from a performing prov:Agent to some prov:Agent for whom it was performed. For example, :mixing prov:wasAssociatedWith :toddler . :toddler prov:actedOnBehalfOf :mother; prov:qualifiedDelegation [ a prov:Delegation; prov:entity :mother; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Delegation"/>
    ///</summary>
    public static readonly Class Delegation = new Class(new Uri("http://www.w3.org/ns/prov#Delegation"));    

    ///<summary>
    ///An instance of prov:Derivation provides additional descriptions about the binary prov:wasDerivedFrom relation from some derived prov:Entity to another prov:Entity from which it was derived. For example, :chewed_bubble_gum prov:wasDerivedFrom :unwrapped_bubble_gum; prov:qualifiedDerivation [ a prov:Derivation; prov:entity :unwrapped_bubble_gum; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Derivation"/>
    ///</summary>
    public static readonly Class Derivation = new Class(new Uri("http://www.w3.org/ns/prov#Derivation"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#EmptyCollection"/>
    ///</summary>
    public static readonly Class EmptyCollection = new Class(new Uri("http://www.w3.org/ns/prov#EmptyCollection"));    

    ///<summary>
    ///An instance of prov:End provides additional descriptions about the binary prov:wasEndedBy relation from some ended prov:Activity to an prov:Entity that ended it. For example, :ball_game prov:wasEndedBy :buzzer; prov:qualifiedEnd [ a prov:End; prov:entity :buzzer; :foo :bar; prov:atTime '2012-03-09T08:05:08-05:00'^^xsd:dateTime ].
    ///<see cref="http://www.w3.org/ns/prov#End"/>
    ///</summary>
    public static readonly Class End = new Class(new Uri("http://www.w3.org/ns/prov#End"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Entity"/>
    ///</summary>
    public static readonly Class Entity = new Class(new Uri("http://www.w3.org/ns/prov#Entity"));    

    ///<summary>
    ///EntityInfluence provides additional descriptions of an Entity's binary influence upon any other kind of resource. Instances of EntityInfluence use the prov:entity property to cite the influencing Entity.
    ///<see cref="http://www.w3.org/ns/prov#EntityInfluence"/>
    ///</summary>
    public static readonly Class EntityInfluence = new Class(new Uri("http://www.w3.org/ns/prov#EntityInfluence"));    

    ///<summary>
    ///An instance of prov:Generation provides additional descriptions about the binary prov:wasGeneratedBy relation from a generated prov:Entity to the prov:Activity that generated it. For example, :cake prov:wasGeneratedBy :baking; prov:qualifiedGeneration [ a prov:Generation; prov:activity :baking; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Generation"/>
    ///</summary>
    public static readonly Class Generation = new Class(new Uri("http://www.w3.org/ns/prov#Generation"));    

    ///<summary>
    ///An instance of prov:Influence provides additional descriptions about the binary prov:wasInfluencedBy relation from some influenced Activity, Entity, or Agent to the influencing Activity, Entity, or Agent. For example, :stomach_ache prov:wasInfluencedBy :spoon; prov:qualifiedInfluence [ a prov:Influence; prov:entity :spoon; :foo :bar ] . Because prov:Influence is a broad relation, the more specific relations (Communication, Delegation, End, etc.) should be used when applicable.
    ///<see cref="http://www.w3.org/ns/prov#Influence"/>
    ///</summary>
    public static readonly Class Influence = new Class(new Uri("http://www.w3.org/ns/prov#Influence"));    

    ///<summary>
    ///An instantaneous event, or event for short, happens in the world and marks a change in the world, in its activities and in its entities. The term 'event' is commonly used in process algebra with a similar meaning. Events represent communications or interactions; they are assumed to be atomic and instantaneous.
    ///<see cref="http://www.w3.org/ns/prov#InstantaneousEvent"/>
    ///</summary>
    public static readonly Class InstantaneousEvent = new Class(new Uri("http://www.w3.org/ns/prov#InstantaneousEvent"));    

    ///<summary>
    ///An instance of prov:Invalidation provides additional descriptions about the binary prov:wasInvalidatedBy relation from an invalidated prov:Entity to the prov:Activity that invalidated it. For example, :uncracked_egg prov:wasInvalidatedBy :baking; prov:qualifiedInvalidation [ a prov:Invalidation; prov:activity :baking; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Invalidation"/>
    ///</summary>
    public static readonly Class Invalidation = new Class(new Uri("http://www.w3.org/ns/prov#Invalidation"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Location"/>
    ///</summary>
    public static readonly Class Location = new Class(new Uri("http://www.w3.org/ns/prov#Location"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Organization"/>
    ///</summary>
    public static readonly Class Organization = new Class(new Uri("http://www.w3.org/ns/prov#Organization"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Person"/>
    ///</summary>
    public static readonly Class Person = new Class(new Uri("http://www.w3.org/ns/prov#Person"));    

    ///<summary>
    ///There exist no prescriptive requirement on the nature of plans, their representation, the actions or steps they consist of, or their intended goals. Since plans may evolve over time, it may become necessary to track their provenance, so plans themselves are entities. Representing the plan explicitly in the provenance can be useful for various tasks: for example, to validate the execution as represented in the provenance record, to manage expectation failures, or to provide explanations.
    ///<see cref="http://www.w3.org/ns/prov#Plan"/>
    ///</summary>
    public static readonly Class Plan = new Class(new Uri("http://www.w3.org/ns/prov#Plan"));    

    ///<summary>
    ///An instance of prov:PrimarySource provides additional descriptions about the binary prov:hadPrimarySource relation from some secondary prov:Entity to an earlier, primary prov:Entity. For example, :blog prov:hadPrimarySource :newsArticle; prov:qualifiedPrimarySource [ a prov:PrimarySource; prov:entity :newsArticle; :foo :bar ] .
    ///<see cref="http://www.w3.org/ns/prov#PrimarySource"/>
    ///</summary>
    public static readonly Class PrimarySource = new Class(new Uri("http://www.w3.org/ns/prov#PrimarySource"));    

    ///<summary>
    ///An instance of prov:Quotation provides additional descriptions about the binary prov:wasQuotedFrom relation from some taken prov:Entity from an earlier, larger prov:Entity. For example, :here_is_looking_at_you_kid prov:wasQuotedFrom :casablanca_script; prov:qualifiedQuotation [ a prov:Quotation; prov:entity :casablanca_script; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Quotation"/>
    ///</summary>
    public static readonly Class Quotation = new Class(new Uri("http://www.w3.org/ns/prov#Quotation"));    

    ///<summary>
    ///An instance of prov:Revision provides additional descriptions about the binary prov:wasRevisionOf relation from some newer prov:Entity to an earlier prov:Entity. For example, :draft_2 prov:wasRevisionOf :draft_1; prov:qualifiedRevision [ a prov:Revision; prov:entity :draft_1; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Revision"/>
    ///</summary>
    public static readonly Class Revision = new Class(new Uri("http://www.w3.org/ns/prov#Revision"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Role"/>
    ///</summary>
    public static readonly Class Role = new Class(new Uri("http://www.w3.org/ns/prov#Role"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#SoftwareAgent"/>
    ///</summary>
    public static readonly Class SoftwareAgent = new Class(new Uri("http://www.w3.org/ns/prov#SoftwareAgent"));    

    ///<summary>
    ///An instance of prov:Start provides additional descriptions about the binary prov:wasStartedBy relation from some started prov:Activity to an prov:Entity that started it. For example, :foot_race prov:wasStartedBy :bang; prov:qualifiedStart [ a prov:Start; prov:entity :bang; :foo :bar; prov:atTime '2012-03-09T08:05:08-05:00'^^xsd:dateTime ] .
    ///<see cref="http://www.w3.org/ns/prov#Start"/>
    ///</summary>
    public static readonly Class Start = new Class(new Uri("http://www.w3.org/ns/prov#Start"));    

    ///<summary>
    ///An instance of prov:Usage provides additional descriptions about the binary prov:used relation from some prov:Activity to an prov:Entity that it used. For example, :keynote prov:used :podium; prov:qualifiedUsage [ a prov:Usage; prov:entity :podium; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Usage"/>
    ///</summary>
    public static readonly Class Usage = new Class(new Uri("http://www.w3.org/ns/prov#Usage"));    

    ///<summary>
    ///An object property to express the accountability of an agent towards another agent. The subordinate agent acted on behalf of the responsible agent in an actual activity. 
    ///<see cref="http://www.w3.org/ns/prov#actedOnBehalfOf"/>
    ///</summary>
    public static readonly Property actedOnBehalfOf = new Property(new Uri("http://www.w3.org/ns/prov#actedOnBehalfOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#activity"/>
    ///</summary>
    public static readonly Property activity = new Property(new Uri("http://www.w3.org/ns/prov#activity"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#agent"/>
    ///</summary>
    public static readonly Property agent = new Property(new Uri("http://www.w3.org/ns/prov#agent"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#alternateOf"/>
    ///</summary>
    public static readonly Property alternateOf = new Property(new Uri("http://www.w3.org/ns/prov#alternateOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#aq"/>
    ///</summary>
    public static readonly Property aq = new Property(new Uri("http://www.w3.org/ns/prov#aq"));    

    ///<summary>
    ///The Location of any resource.
    ///<see cref="http://www.w3.org/ns/prov#atLocation"/>
    ///</summary>
    public static readonly Property atLocation = new Property(new Uri("http://www.w3.org/ns/prov#atLocation"));    

    ///<summary>
    ///The time at which an InstantaneousEvent occurred, in the form of xsd:dateTime.
    ///<see cref="http://www.w3.org/ns/prov#atTime"/>
    ///</summary>
    public static readonly Property atTime = new Property(new Uri("http://www.w3.org/ns/prov#atTime"));    

    ///<summary>
    ///Classify prov-o terms into three categories, including 'starting-point', 'qualifed', and 'extended'. This classification is used by the prov-o html document to gently introduce prov-o terms to its users. 
    ///<see cref="http://www.w3.org/ns/prov#category"/>
    ///</summary>
    public static readonly Property category = new Property(new Uri("http://www.w3.org/ns/prov#category"));    

    ///<summary>
    ///Classify prov-o terms into six components according to prov-dm, including 'agents-responsibility', 'alternate', 'annotations', 'collections', 'derivations', and 'entities-activities'. This classification is used so that readers of prov-o specification can find its correspondence with the prov-dm specification.
    ///<see cref="http://www.w3.org/ns/prov#component"/>
    ///</summary>
    public static readonly Property component = new Property(new Uri("http://www.w3.org/ns/prov#component"));    

    ///<summary>
    ///A reference to the principal section of the PROV-CONSTRAINTS document that describes this concept.
    ///<see cref="http://www.w3.org/ns/prov#constraints"/>
    ///</summary>
    public static readonly Property constraints = new Property(new Uri("http://www.w3.org/ns/prov#constraints"));    

    ///<summary>
    ///A definition quoted from PROV-DM or PROV-CONSTRAINTS that describes the concept expressed with this OWL term.
    ///<see cref="http://www.w3.org/ns/prov#definition"/>
    ///</summary>
    public static readonly Property definition = new Property(new Uri("http://www.w3.org/ns/prov#definition"));    

    ///<summary>
    ///A reference to the principal section of the PROV-DM document that describes this concept.
    ///<see cref="http://www.w3.org/ns/prov#dm"/>
    ///</summary>
    public static readonly Property dm = new Property(new Uri("http://www.w3.org/ns/prov#dm"));    

    ///<summary>
    ///A note by the OWL development team about how this term expresses the PROV-DM concept, or how it should be used in context of semantic web or linked data.
    ///<see cref="http://www.w3.org/ns/prov#editorialNote"/>
    ///</summary>
    public static readonly Property editorialNote = new Property(new Uri("http://www.w3.org/ns/prov#editorialNote"));    

    ///<summary>
    ///When the prov-o term does not have a definition drawn from prov-dm, and the prov-o editor provides one.
    ///<see cref="http://www.w3.org/ns/prov#editorsDefinition"/>
    ///</summary>
    public static readonly Property editorsDefinition = new Property(new Uri("http://www.w3.org/ns/prov#editorsDefinition"));    

    ///<summary>
    ///The time at which an activity ended. See also prov:startedAtTime.
    ///<see cref="http://www.w3.org/ns/prov#endedAtTime"/>
    ///</summary>
    public static readonly Property endedAtTime = new Property(new Uri("http://www.w3.org/ns/prov#endedAtTime"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#entity"/>
    ///</summary>
    public static readonly Property entity = new Property(new Uri("http://www.w3.org/ns/prov#entity"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#generated"/>
    ///</summary>
    public static readonly Property generated = new Property(new Uri("http://www.w3.org/ns/prov#generated"));    

    ///<summary>
    ///The time at which an entity was completely created and is available for use.
    ///<see cref="http://www.w3.org/ns/prov#generatedAtTime"/>
    ///</summary>
    public static readonly Property generatedAtTime = new Property(new Uri("http://www.w3.org/ns/prov#generatedAtTime"));    

    ///<summary>
    ///The _optional_ Activity of an Influence, which used, generated, invalidated, or was the responsibility of some Entity. This property is _not_ used by ActivityInfluence (use prov:activity instead).
    ///<see cref="http://www.w3.org/ns/prov#hadActivity"/>
    ///</summary>
    public static readonly Property hadActivity = new Property(new Uri("http://www.w3.org/ns/prov#hadActivity"));    

    ///<summary>
    ///The _optional_ Generation involved in an Entity's Derivation.
    ///<see cref="http://www.w3.org/ns/prov#hadGeneration"/>
    ///</summary>
    public static readonly Property hadGeneration = new Property(new Uri("http://www.w3.org/ns/prov#hadGeneration"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadMember"/>
    ///</summary>
    public static readonly Property hadMember = new Property(new Uri("http://www.w3.org/ns/prov#hadMember"));    

    ///<summary>
    ///The _optional_ Plan adopted by an Agent in Association with some Activity. Plan specifications are out of the scope of this specification.
    ///<see cref="http://www.w3.org/ns/prov#hadPlan"/>
    ///</summary>
    public static readonly Property hadPlan = new Property(new Uri("http://www.w3.org/ns/prov#hadPlan"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadPrimarySource"/>
    ///</summary>
    public static readonly Property hadPrimarySource = new Property(new Uri("http://www.w3.org/ns/prov#hadPrimarySource"));    

    ///<summary>
    ///The _optional_ Role that an Entity assumed in the context of an Activity. For example, :baking prov:used :spoon; prov:qualified [ a prov:Usage; prov:entity :spoon; prov:hadRole roles:mixing_implement ].
    ///<see cref="http://www.w3.org/ns/prov#hadRole"/>
    ///</summary>
    public static readonly Property hadRole = new Property(new Uri("http://www.w3.org/ns/prov#hadRole"));    

    ///<summary>
    ///The _optional_ Usage involved in an Entity's Derivation.
    ///<see cref="http://www.w3.org/ns/prov#hadUsage"/>
    ///</summary>
    public static readonly Property hadUsage = new Property(new Uri("http://www.w3.org/ns/prov#hadUsage"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#influenced"/>
    ///</summary>
    public static readonly Property influenced = new Property(new Uri("http://www.w3.org/ns/prov#influenced"));    

    ///<summary>
    ///Subproperties of prov:influencer are used to cite the object of an unqualified PROV-O triple whose predicate is a subproperty of prov:wasInfluencedBy (e.g. prov:used, prov:wasGeneratedBy). prov:influencer is used much like rdf:object is used.
    ///<see cref="http://www.w3.org/ns/prov#influencer"/>
    ///</summary>
    public static readonly Property influencer = new Property(new Uri("http://www.w3.org/ns/prov#influencer"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#invalidated"/>
    ///</summary>
    public static readonly Property invalidated = new Property(new Uri("http://www.w3.org/ns/prov#invalidated"));    

    ///<summary>
    ///The time at which an entity was invalidated (i.e., no longer usable).
    ///<see cref="http://www.w3.org/ns/prov#invalidatedAtTime"/>
    ///</summary>
    public static readonly Property invalidatedAtTime = new Property(new Uri("http://www.w3.org/ns/prov#invalidatedAtTime"));    

    ///<summary>
    ///PROV-O does not define all property inverses. The directionalities defined in PROV-O should be given preference over those not defined. However, if users wish to name the inverse of a PROV-O property, the local name given by prov:inverse should be used.
    ///<see cref="http://www.w3.org/ns/prov#inverse"/>
    ///</summary>
    public static readonly Property inverse = new Property(new Uri("http://www.w3.org/ns/prov#inverse"));    

    ///<summary>
    ///A reference to the principal section of the PROV-DM document that describes this concept.
    ///<see cref="http://www.w3.org/ns/prov#n"/>
    ///</summary>
    public static readonly Property n = new Property(new Uri("http://www.w3.org/ns/prov#n"));    

    ///<summary>
    ///The position that this OWL term should be listed within documentation. The scope of the documentation (e.g., among all terms, among terms within a prov:category, among properties applying to a particular class, etc.) is unspecified.
    ///<see cref="http://www.w3.org/ns/prov#order"/>
    ///</summary>
    public static readonly Property order = new Property(new Uri("http://www.w3.org/ns/prov#order"));    

    ///<summary>
    ///If this Activity prov:wasAssociatedWith Agent :ag, then it can qualify the Association using prov:qualifiedAssociation [ a prov:Association;  prov:agent :ag; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedAssociation"/>
    ///</summary>
    public static readonly Property qualifiedAssociation = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedAssociation"));    

    ///<summary>
    ///If this Entity prov:wasAttributedTo Agent :ag, then it can qualify how it was influenced using prov:qualifiedAttribution [ a prov:Attribution;  prov:agent :ag; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedAttribution"/>
    ///</summary>
    public static readonly Property qualifiedAttribution = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedAttribution"));    

    ///<summary>
    ///If this Activity prov:wasInformedBy Activity :a, then it can qualify how it was influenced using prov:qualifiedCommunication [ a prov:Communication;  prov:activity :a; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedCommunication"/>
    ///</summary>
    public static readonly Property qualifiedCommunication = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedCommunication"));    

    ///<summary>
    ///If this Agent prov:actedOnBehalfOf Agent :ag, then it can qualify how with prov:qualifiedResponsibility [ a prov:Responsibility;  prov:agent :ag; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedDelegation"/>
    ///</summary>
    public static readonly Property qualifiedDelegation = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedDelegation"));    

    ///<summary>
    ///If this Entity prov:wasDerivedFrom Entity :e, then it can qualify how it was derived using prov:qualifiedDerivation [ a prov:Derivation;  prov:entity :e; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedDerivation"/>
    ///</summary>
    public static readonly Property qualifiedDerivation = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedDerivation"));    

    ///<summary>
    ///If this Activity prov:wasEndedBy Entity :e1, then it can qualify how it was ended using prov:qualifiedEnd [ a prov:End;  prov:entity :e1; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedEnd"/>
    ///</summary>
    public static readonly Property qualifiedEnd = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedEnd"));    

    ///<summary>
    ///This annotation property links a subproperty of prov:wasInfluencedBy with the subclass of prov:Influence and the qualifying property that are used to qualify it. 
    ///
    ///Example annotation:
    ///
    ///    prov:wasGeneratedBy prov:qualifiedForm prov:qualifiedGeneration, prov:Generation .
    ///
    ///Then this unqualified assertion:
    ///
    ///    :entity1 prov:wasGeneratedBy :activity1 .
    ///
    ///can be qualified by adding:
    ///
    ///   :entity1 prov:qualifiedGeneration :entity1Gen .
    ///   :entity1Gen 
    ///       a prov:Generation, prov:Influence;
    ///       prov:activity :activity1;
    ///       :customValue 1337 .
    ///
    ///Note how the value of the unqualified influence (prov:wasGeneratedBy :activity1) is mirrored as the value of the prov:activity (or prov:entity, or prov:agent) property on the influence class.
    ///<see cref="http://www.w3.org/ns/prov#qualifiedForm"/>
    ///</summary>
    public static readonly Property qualifiedForm = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedForm"));    

    ///<summary>
    ///If this Activity prov:generated Entity :e, then it can qualify how it performed the Generation using prov:qualifiedGeneration [ a prov:Generation;  prov:entity :e; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedGeneration"/>
    ///</summary>
    public static readonly Property qualifiedGeneration = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedGeneration"));    

    ///<summary>
    ///Because prov:qualifiedInfluence is a broad relation, the more specific relations (qualifiedCommunication, qualifiedDelegation, qualifiedEnd, etc.) should be used when applicable.
    ///<see cref="http://www.w3.org/ns/prov#qualifiedInfluence"/>
    ///</summary>
    public static readonly Property qualifiedInfluence = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedInfluence"));    

    ///<summary>
    ///If this Entity prov:wasInvalidatedBy Activity :a, then it can qualify how it was invalidated using prov:qualifiedInvalidation [ a prov:Invalidation;  prov:activity :a; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedInvalidation"/>
    ///</summary>
    public static readonly Property qualifiedInvalidation = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedInvalidation"));    

    ///<summary>
    ///If this Entity prov:hadPrimarySource Entity :e, then it can qualify how using prov:qualifiedPrimarySource [ a prov:PrimarySource; prov:entity :e; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedPrimarySource"/>
    ///</summary>
    public static readonly Property qualifiedPrimarySource = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedPrimarySource"));    

    ///<summary>
    ///If this Entity prov:wasQuotedFrom Entity :e, then it can qualify how using prov:qualifiedQuotation [ a prov:Quotation;  prov:entity :e; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedQuotation"/>
    ///</summary>
    public static readonly Property qualifiedQuotation = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedQuotation"));    

    ///<summary>
    ///If this Entity prov:wasRevisionOf Entity :e, then it can qualify how it was revised using prov:qualifiedRevision [ a prov:Revision;  prov:entity :e; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedRevision"/>
    ///</summary>
    public static readonly Property qualifiedRevision = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedRevision"));    

    ///<summary>
    ///If this Activity prov:wasStartedBy Entity :e1, then it can qualify how it was started using prov:qualifiedStart [ a prov:Start;  prov:entity :e1; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedStart"/>
    ///</summary>
    public static readonly Property qualifiedStart = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedStart"));    

    ///<summary>
    ///If this Activity prov:used Entity :e, then it can qualify how it used it using prov:qualifiedUsage [ a prov:Usage; prov:entity :e; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedUsage"/>
    ///</summary>
    public static readonly Property qualifiedUsage = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedUsage"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#sharesDefinitionWith"/>
    ///</summary>
    public static readonly Property sharesDefinitionWith = new Property(new Uri("http://www.w3.org/ns/prov#sharesDefinitionWith"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#specializationOf"/>
    ///</summary>
    public static readonly Property specializationOf = new Property(new Uri("http://www.w3.org/ns/prov#specializationOf"));    

    ///<summary>
    ///The time at which an activity started. See also prov:endedAtTime.
    ///<see cref="http://www.w3.org/ns/prov#startedAtTime"/>
    ///</summary>
    public static readonly Property startedAtTime = new Property(new Uri("http://www.w3.org/ns/prov#startedAtTime"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#todo"/>
    ///</summary>
    public static readonly Property todo = new Property(new Uri("http://www.w3.org/ns/prov#todo"));    

    ///<summary>
    ///Classes and properties used to qualify relationships are annotated with prov:unqualifiedForm to indicate the property used to assert an unqualified provenance relation.
    ///<see cref="http://www.w3.org/ns/prov#unqualifiedForm"/>
    ///</summary>
    public static readonly Property unqualifiedForm = new Property(new Uri("http://www.w3.org/ns/prov#unqualifiedForm"));    

    ///<summary>
    ///A prov:Entity that was used by this prov:Activity. For example, :baking prov:used :spoon, :egg, :oven .
    ///<see cref="http://www.w3.org/ns/prov#used"/>
    ///</summary>
    public static readonly Property used = new Property(new Uri("http://www.w3.org/ns/prov#used"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#value"/>
    ///</summary>
    public static readonly Property value = new Property(new Uri("http://www.w3.org/ns/prov#value"));    

    ///<summary>
    ///An prov:Agent that had some (unspecified) responsibility for the occurrence of this prov:Activity.
    ///<see cref="http://www.w3.org/ns/prov#wasAssociatedWith"/>
    ///</summary>
    public static readonly Property wasAssociatedWith = new Property(new Uri("http://www.w3.org/ns/prov#wasAssociatedWith"));    

    ///<summary>
    ///Attribution is the ascribing of an entity to an agent.
    ///<see cref="http://www.w3.org/ns/prov#wasAttributedTo"/>
    ///</summary>
    public static readonly Property wasAttributedTo = new Property(new Uri("http://www.w3.org/ns/prov#wasAttributedTo"));    

    ///<summary>
    ///The more specific subproperties of prov:wasDerivedFrom (i.e., prov:wasQuotedFrom, prov:wasRevisionOf, prov:hadPrimarySource) should be used when applicable.
    ///<see cref="http://www.w3.org/ns/prov#wasDerivedFrom"/>
    ///</summary>
    public static readonly Property wasDerivedFrom = new Property(new Uri("http://www.w3.org/ns/prov#wasDerivedFrom"));    

    ///<summary>
    ///End is when an activity is deemed to have ended. An end may refer to an entity, known as trigger, that terminated the activity.
    ///<see cref="http://www.w3.org/ns/prov#wasEndedBy"/>
    ///</summary>
    public static readonly Property wasEndedBy = new Property(new Uri("http://www.w3.org/ns/prov#wasEndedBy"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasGeneratedBy"/>
    ///</summary>
    public static readonly Property wasGeneratedBy = new Property(new Uri("http://www.w3.org/ns/prov#wasGeneratedBy"));    

    ///<summary>
    ///Because prov:wasInfluencedBy is a broad relation, its more specific subproperties (e.g. prov:wasInformedBy, prov:actedOnBehalfOf, prov:wasEndedBy, etc.) should be used when applicable.
    ///<see cref="http://www.w3.org/ns/prov#wasInfluencedBy"/>
    ///</summary>
    public static readonly Property wasInfluencedBy = new Property(new Uri("http://www.w3.org/ns/prov#wasInfluencedBy"));    

    ///<summary>
    ///An activity a2 is dependent on or informed by another activity a1, by way of some unspecified entity that is generated by a1 and used by a2.
    ///<see cref="http://www.w3.org/ns/prov#wasInformedBy"/>
    ///</summary>
    public static readonly Property wasInformedBy = new Property(new Uri("http://www.w3.org/ns/prov#wasInformedBy"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasInvalidatedBy"/>
    ///</summary>
    public static readonly Property wasInvalidatedBy = new Property(new Uri("http://www.w3.org/ns/prov#wasInvalidatedBy"));    

    ///<summary>
    ///An entity is derived from an original entity by copying, or 'quoting', some or all of it.
    ///<see cref="http://www.w3.org/ns/prov#wasQuotedFrom"/>
    ///</summary>
    public static readonly Property wasQuotedFrom = new Property(new Uri("http://www.w3.org/ns/prov#wasQuotedFrom"));    

    ///<summary>
    ///A revision is a derivation that revises an entity into a revised version.
    ///<see cref="http://www.w3.org/ns/prov#wasRevisionOf"/>
    ///</summary>
    public static readonly Property wasRevisionOf = new Property(new Uri("http://www.w3.org/ns/prov#wasRevisionOf"));    

    ///<summary>
    ///Start is when an activity is deemed to have started. A start may refer to an entity, known as trigger, that initiated the activity.
    ///<see cref="http://www.w3.org/ns/prov#wasStartedBy"/>
    ///</summary>
    public static readonly Property wasStartedBy = new Property(new Uri("http://www.w3.org/ns/prov#wasStartedBy"));    

    ///<summary>
    ///This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page). 
    ///
    ///If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/Archives/Public/public-prov-comments/). All feedback is welcome.
    ///<see cref="http://www.w3.org/ns/prov-o#"/>
    ///</summary>
    public static readonly Resource prov_o = new Resource(new Uri("http://www.w3.org/ns/prov-o#"));    

    ///<summary>
    ///
    ///<see cref="file:///D:/Projects/Artivity/Artivity.Api.Model/Ontologies/prov.ttl#"/>
    ///</summary>
    public static readonly Resource prov_ttl = new Resource(new Uri("file:///D:/Projects/Artivity/Artivity.Api.Model/Ontologies/prov.ttl#"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadDelegate"/>
    ///</summary>
    public static readonly Resource hadDelegate = new Resource(new Uri("http://www.w3.org/ns/prov#hadDelegate"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#activityOfInfluence"/>
    ///</summary>
    public static readonly Resource activityOfInfluence = new Resource(new Uri("http://www.w3.org/ns/prov#activityOfInfluence"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#agentOfInfluence"/>
    ///</summary>
    public static readonly Resource agentOfInfluence = new Resource(new Uri("http://www.w3.org/ns/prov#agentOfInfluence"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#locationOf"/>
    ///</summary>
    public static readonly Resource locationOf = new Resource(new Uri("http://www.w3.org/ns/prov#locationOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#entityOfInfluence"/>
    ///</summary>
    public static readonly Resource entityOfInfluence = new Resource(new Uri("http://www.w3.org/ns/prov#entityOfInfluence"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasActivityOfInfluence"/>
    ///</summary>
    public static readonly Resource wasActivityOfInfluence = new Resource(new Uri("http://www.w3.org/ns/prov#wasActivityOfInfluence"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#generatedAsDerivation"/>
    ///</summary>
    public static readonly Resource generatedAsDerivation = new Resource(new Uri("http://www.w3.org/ns/prov#generatedAsDerivation"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasMemberOf"/>
    ///</summary>
    public static readonly Resource wasMemberOf = new Resource(new Uri("http://www.w3.org/ns/prov#wasMemberOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasPlanOf"/>
    ///</summary>
    public static readonly Resource wasPlanOf = new Resource(new Uri("http://www.w3.org/ns/prov#wasPlanOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasPrimarySourceOf"/>
    ///</summary>
    public static readonly Resource wasPrimarySourceOf = new Resource(new Uri("http://www.w3.org/ns/prov#wasPrimarySourceOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasRoleIn"/>
    ///</summary>
    public static readonly Resource wasRoleIn = new Resource(new Uri("http://www.w3.org/ns/prov#wasRoleIn"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasUsedInDerivation"/>
    ///</summary>
    public static readonly Resource wasUsedInDerivation = new Resource(new Uri("http://www.w3.org/ns/prov#wasUsedInDerivation"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadInfluence"/>
    ///</summary>
    public static readonly Resource hadInfluence = new Resource(new Uri("http://www.w3.org/ns/prov#hadInfluence"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedAssociationOf"/>
    ///</summary>
    public static readonly Resource qualifiedAssociationOf = new Resource(new Uri("http://www.w3.org/ns/prov#qualifiedAssociationOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedAttributionOf"/>
    ///</summary>
    public static readonly Resource qualifiedAttributionOf = new Resource(new Uri("http://www.w3.org/ns/prov#qualifiedAttributionOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedCommunicationOf"/>
    ///</summary>
    public static readonly Resource qualifiedCommunicationOf = new Resource(new Uri("http://www.w3.org/ns/prov#qualifiedCommunicationOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedDelegationOf"/>
    ///</summary>
    public static readonly Resource qualifiedDelegationOf = new Resource(new Uri("http://www.w3.org/ns/prov#qualifiedDelegationOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedDerivationOf"/>
    ///</summary>
    public static readonly Resource qualifiedDerivationOf = new Resource(new Uri("http://www.w3.org/ns/prov#qualifiedDerivationOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedEndOf"/>
    ///</summary>
    public static readonly Resource qualifiedEndOf = new Resource(new Uri("http://www.w3.org/ns/prov#qualifiedEndOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedGenerationOf"/>
    ///</summary>
    public static readonly Resource qualifiedGenerationOf = new Resource(new Uri("http://www.w3.org/ns/prov#qualifiedGenerationOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedInfluenceOf"/>
    ///</summary>
    public static readonly Resource qualifiedInfluenceOf = new Resource(new Uri("http://www.w3.org/ns/prov#qualifiedInfluenceOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedInvalidationOf"/>
    ///</summary>
    public static readonly Resource qualifiedInvalidationOf = new Resource(new Uri("http://www.w3.org/ns/prov#qualifiedInvalidationOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedSourceOf"/>
    ///</summary>
    public static readonly Resource qualifiedSourceOf = new Resource(new Uri("http://www.w3.org/ns/prov#qualifiedSourceOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedQuotationOf"/>
    ///</summary>
    public static readonly Resource qualifiedQuotationOf = new Resource(new Uri("http://www.w3.org/ns/prov#qualifiedQuotationOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#revisedEntity"/>
    ///</summary>
    public static readonly Resource revisedEntity = new Resource(new Uri("http://www.w3.org/ns/prov#revisedEntity"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedStartOf"/>
    ///</summary>
    public static readonly Resource qualifiedStartOf = new Resource(new Uri("http://www.w3.org/ns/prov#qualifiedStartOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedUsingActivity"/>
    ///</summary>
    public static readonly Resource qualifiedUsingActivity = new Resource(new Uri("http://www.w3.org/ns/prov#qualifiedUsingActivity"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#generalizationOf"/>
    ///</summary>
    public static readonly Resource generalizationOf = new Resource(new Uri("http://www.w3.org/ns/prov#generalizationOf"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasUsedBy"/>
    ///</summary>
    public static readonly Resource wasUsedBy = new Resource(new Uri("http://www.w3.org/ns/prov#wasUsedBy"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasAssociateFor"/>
    ///</summary>
    public static readonly Resource wasAssociateFor = new Resource(new Uri("http://www.w3.org/ns/prov#wasAssociateFor"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#contributed"/>
    ///</summary>
    public static readonly Resource contributed = new Resource(new Uri("http://www.w3.org/ns/prov#contributed"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadDerivation"/>
    ///</summary>
    public static readonly Resource hadDerivation = new Resource(new Uri("http://www.w3.org/ns/prov#hadDerivation"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#ended"/>
    ///</summary>
    public static readonly Resource ended = new Resource(new Uri("http://www.w3.org/ns/prov#ended"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#informed"/>
    ///</summary>
    public static readonly Resource informed = new Resource(new Uri("http://www.w3.org/ns/prov#informed"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#quotedAs"/>
    ///</summary>
    public static readonly Resource quotedAs = new Resource(new Uri("http://www.w3.org/ns/prov#quotedAs"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadRevision"/>
    ///</summary>
    public static readonly Resource hadRevision = new Resource(new Uri("http://www.w3.org/ns/prov#hadRevision"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#started"/>
    ///</summary>
    public static readonly Resource started = new Resource(new Uri("http://www.w3.org/ns/prov#started"));    

    ///<summary>
    ///0.2
    ///<see cref="http://www.w3.org/ns/prov-aq#"/>
    ///</summary>
    public static readonly Resource prov_aq = new Resource(new Uri("http://www.w3.org/ns/prov-aq#"));    

    ///<summary>
    ///Type for a generic provenance query service. Mainly for use in RDF provenance query service descriptions, to facilitate discovery in linked data environments.
    ///<see cref="http://www.w3.org/ns/prov#ServiceDescription"/>
    ///</summary>
    public static readonly Class ServiceDescription = new Class(new Uri("http://www.w3.org/ns/prov#ServiceDescription"));    

    ///<summary>
    ///Type for a generic provenance query service. Mainly for use in RDF provenance query service descriptions, to facilitate discovery in linked data environments.
    ///<see cref="http://www.w3.org/ns/prov#DirectQueryService"/>
    ///</summary>
    public static readonly Class DirectQueryService = new Class(new Uri("http://www.w3.org/ns/prov#DirectQueryService"));    

    ///<summary>
    ///Indicates anchor URI for a potentially dynamic resource instance.
    ///<see cref="http://www.w3.org/ns/prov#has_anchor"/>
    ///</summary>
    public static readonly Property has_anchor = new Property(new Uri("http://www.w3.org/ns/prov#has_anchor"));    

    ///<summary>
    ///Indicates a provenance-URI for a resource; the resource identified by this property presents a provenance record about its subject or anchor resource.
    ///<see cref="http://www.w3.org/ns/prov#has_provenance"/>
    ///</summary>
    public static readonly Property has_provenance = new Property(new Uri("http://www.w3.org/ns/prov#has_provenance"));    

    ///<summary>
    ///Indicates a provenance query service that can access provenance related to its subject or anchor resource.
    ///<see cref="http://www.w3.org/ns/prov#has_query_service"/>
    ///</summary>
    public static readonly Property has_query_service = new Property(new Uri("http://www.w3.org/ns/prov#has_query_service"));    

    ///<summary>
    ///relates a generic provenance query service resource (type prov:ServiceDescription) to a specific query service description (e.g. a prov:DirectQueryService or a sd:Service).
    ///<see cref="http://www.w3.org/ns/prov#describesService"/>
    ///</summary>
    public static readonly Property describesService = new Property(new Uri("http://www.w3.org/ns/prov#describesService"));    

    ///<summary>
    ///Relates a provenance service to a URI template string for constructing provenance-URIs.
    ///<see cref="http://www.w3.org/ns/prov#provenanceUriTemplate"/>
    ///</summary>
    public static readonly Property provenanceUriTemplate = new Property(new Uri("http://www.w3.org/ns/prov#provenanceUriTemplate"));    

    ///<summary>
    ///Relates a resource to a provenance pingback service that may receive additional provenance links about the resource.
    ///<see cref="http://www.w3.org/ns/prov#pingback"/>
    ///</summary>
    public static readonly Property pingback = new Property(new Uri("http://www.w3.org/ns/prov#pingback"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2002/07/owl#topObjectProperty"/>
    ///</summary>
    public static readonly Property topObjectProperty = new Property(new Uri("http://www.w3.org/2002/07/owl#topObjectProperty"));    

    ///<summary>
    ///This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page). 
    ///
    ///If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/Archives/Public/public-prov-comments/). All feedback is welcome.
    ///<see cref="http://www.w3.org/ns/prov-dc#"/>
    ///</summary>
    public static readonly Resource prov_dc = new Resource(new Uri("http://www.w3.org/ns/prov-dc#"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Accept"/>
    ///</summary>
    public static readonly Class Accept = new Class(new Uri("http://www.w3.org/ns/prov#Accept"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Contribute"/>
    ///</summary>
    public static readonly Class Contribute = new Class(new Uri("http://www.w3.org/ns/prov#Contribute"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Contributor"/>
    ///</summary>
    public static readonly Class Contributor = new Class(new Uri("http://www.w3.org/ns/prov#Contributor"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Copyright"/>
    ///</summary>
    public static readonly Class Copyright = new Class(new Uri("http://www.w3.org/ns/prov#Copyright"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Create"/>
    ///</summary>
    public static readonly Class Create = new Class(new Uri("http://www.w3.org/ns/prov#Create"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Creator"/>
    ///</summary>
    public static readonly Class Creator = new Class(new Uri("http://www.w3.org/ns/prov#Creator"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Modify"/>
    ///</summary>
    public static readonly Class Modify = new Class(new Uri("http://www.w3.org/ns/prov#Modify"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Publish"/>
    ///</summary>
    public static readonly Class Publish = new Class(new Uri("http://www.w3.org/ns/prov#Publish"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Publisher"/>
    ///</summary>
    public static readonly Class Publisher = new Class(new Uri("http://www.w3.org/ns/prov#Publisher"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Replace"/>
    ///</summary>
    public static readonly Class Replace = new Class(new Uri("http://www.w3.org/ns/prov#Replace"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#RightsAssignment"/>
    ///</summary>
    public static readonly Class RightsAssignment = new Class(new Uri("http://www.w3.org/ns/prov#RightsAssignment"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#RightsHolder"/>
    ///</summary>
    public static readonly Class RightsHolder = new Class(new Uri("http://www.w3.org/ns/prov#RightsHolder"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Submit"/>
    ///</summary>
    public static readonly Class Submit = new Class(new Uri("http://www.w3.org/ns/prov#Submit"));    

    ///<summary>
    ///This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page). 
    ///
    ///If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/Archives/Public/public-prov-comments/). All feedback is welcome.
    ///<see cref="http://www.w3.org/ns/prov-dictionary#"/>
    ///</summary>
    public static readonly Resource prov_dictionary = new Resource(new Uri("http://www.w3.org/ns/prov-dictionary#"));    

    ///<summary>
    ///This concept allows for the provenance of the dictionary, but also of its constituents to be expressed. Such a notion of dictionary corresponds to a wide variety of concrete data structures, such as a maps or associative arrays.
    ///<see cref="http://www.w3.org/ns/prov#Dictionary"/>
    ///</summary>
    public static readonly Class Dictionary = new Class(new Uri("http://www.w3.org/ns/prov#Dictionary"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#EmptyDictionary"/>
    ///</summary>
    public static readonly Class EmptyDictionary = new Class(new Uri("http://www.w3.org/ns/prov#EmptyDictionary"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#KeyEntityPair"/>
    ///</summary>
    public static readonly Class KeyEntityPair = new Class(new Uri("http://www.w3.org/ns/prov#KeyEntityPair"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Insertion"/>
    ///</summary>
    public static readonly Class Insertion = new Class(new Uri("http://www.w3.org/ns/prov#Insertion"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Removal"/>
    ///</summary>
    public static readonly Class Removal = new Class(new Uri("http://www.w3.org/ns/prov#Removal"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#dictionary"/>
    ///</summary>
    public static readonly Property dictionary = new Property(new Uri("http://www.w3.org/ns/prov#dictionary"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#derivedByInsertionFrom"/>
    ///</summary>
    public static readonly Property derivedByInsertionFrom = new Property(new Uri("http://www.w3.org/ns/prov#derivedByInsertionFrom"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#derivedByRemovalFrom"/>
    ///</summary>
    public static readonly Property derivedByRemovalFrom = new Property(new Uri("http://www.w3.org/ns/prov#derivedByRemovalFrom"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#insertedKeyEntityPair"/>
    ///</summary>
    public static readonly Property insertedKeyEntityPair = new Property(new Uri("http://www.w3.org/ns/prov#insertedKeyEntityPair"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadDictionaryMember"/>
    ///</summary>
    public static readonly Property hadDictionaryMember = new Property(new Uri("http://www.w3.org/ns/prov#hadDictionaryMember"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#pairKey"/>
    ///</summary>
    public static readonly Property pairKey = new Property(new Uri("http://www.w3.org/ns/prov#pairKey"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#pairEntity"/>
    ///</summary>
    public static readonly Property pairEntity = new Property(new Uri("http://www.w3.org/ns/prov#pairEntity"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedInsertion"/>
    ///</summary>
    public static readonly Property qualifiedInsertion = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedInsertion"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedRemoval"/>
    ///</summary>
    public static readonly Property qualifiedRemoval = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedRemoval"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#removedKey"/>
    ///</summary>
    public static readonly Property removedKey = new Property(new Uri("http://www.w3.org/ns/prov#removedKey"));    

    ///<summary>
    ///This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page). If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/Archives/Public/public-prov-comments/
    ///). All feedback is welcome.
    ///<see cref="http://www.w3.org/ns/prov-links#"/>
    ///</summary>
    public static readonly Resource prov_links = new Resource(new Uri("http://www.w3.org/ns/prov-links#"));    

    ///<summary>
    ///prov:asInBundle is used to specify which bundle the general entity of a prov:mentionOf property is described.
    ///
    ///When :x prov:mentionOf :y and :y is described in Bundle :b, the triple :x prov:asInBundle :b is also asserted to cite the Bundle in which :y was described.
    ///<see cref="http://www.w3.org/ns/prov#asInBundle"/>
    ///</summary>
    public static readonly Property asInBundle = new Property(new Uri("http://www.w3.org/ns/prov#asInBundle"));    

    ///<summary>
    ///prov:mentionOf is used to specialize an entity as described in another bundle. It is to be used in conjuction with prov:asInBundle.
    ///
    ///prov:asInBundle is used to cite the Bundle in which the generalization was mentioned.
    ///<see cref="http://www.w3.org/ns/prov#mentionOf"/>
    ///</summary>
    public static readonly Property mentionOf = new Property(new Uri("http://www.w3.org/ns/prov#mentionOf"));
}
///<summary>
///
///
///</summary>
public static class PROV
{
    public static readonly Uri Namespace = new Uri("http://www.w3.org/ns/prov#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "PROV";
    public static string GetPrefix() { return Prefix; } 

    ///<summary>
    ///This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page).
    ///
    ///If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/
    ///Archives/Public/public-prov-comments/). All feedback is welcome.
    ///<see cref="http://www.w3.org/ns/prov#"/>
    ///</summary>
    public const string prov_0 = "http://www.w3.org/ns/prov#";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#comment"/>
    ///</summary>
    public const string comment = "http://www.w3.org/2000/01/rdf-schema#comment";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#isDefinedBy"/>
    ///</summary>
    public const string isDefinedBy = "http://www.w3.org/2000/01/rdf-schema#isDefinedBy";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#label"/>
    ///</summary>
    public const string label = "http://www.w3.org/2000/01/rdf-schema#label";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#seeAlso"/>
    ///</summary>
    public const string seeAlso = "http://www.w3.org/2000/01/rdf-schema#seeAlso";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2002/07/owl#Thing"/>
    ///</summary>
    public const string Thing = "http://www.w3.org/2002/07/owl#Thing";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2002/07/owl#versionInfo"/>
    ///</summary>
    public const string versionInfo = "http://www.w3.org/2002/07/owl#versionInfo";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Activity"/>
    ///</summary>
    public const string Activity = "http://www.w3.org/ns/prov#Activity";

    ///<summary>
    ///ActivityInfluence provides additional descriptions of an Activity's binary influence upon any other kind of resource. Instances of ActivityInfluence use the prov:activity property to cite the influencing Activity.
    ///<see cref="http://www.w3.org/ns/prov#ActivityInfluence"/>
    ///</summary>
    public const string ActivityInfluence = "http://www.w3.org/ns/prov#ActivityInfluence";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Agent"/>
    ///</summary>
    public const string Agent = "http://www.w3.org/ns/prov#Agent";

    ///<summary>
    ///AgentInfluence provides additional descriptions of an Agent's binary influence upon any other kind of resource. Instances of AgentInfluence use the prov:agent property to cite the influencing Agent.
    ///<see cref="http://www.w3.org/ns/prov#AgentInfluence"/>
    ///</summary>
    public const string AgentInfluence = "http://www.w3.org/ns/prov#AgentInfluence";

    ///<summary>
    ///An instance of prov:Association provides additional descriptions about the binary prov:wasAssociatedWith relation from an prov:Activity to some prov:Agent that had some responsiblity for it. For example, :baking prov:wasAssociatedWith :baker; prov:qualifiedAssociation [ a prov:Association; prov:agent :baker; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Association"/>
    ///</summary>
    public const string Association = "http://www.w3.org/ns/prov#Association";

    ///<summary>
    ///An instance of prov:Attribution provides additional descriptions about the binary prov:wasAttributedTo relation from an prov:Entity to some prov:Agent that had some responsible for it. For example, :cake prov:wasAttributedTo :baker; prov:qualifiedAttribution [ a prov:Attribution; prov:entity :baker; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Attribution"/>
    ///</summary>
    public const string Attribution = "http://www.w3.org/ns/prov#Attribution";

    ///<summary>
    ///Note that there are kinds of bundles (e.g. handwritten letters, audio recordings, etc.) that are not expressed in PROV-O, but can be still be described by PROV-O.
    ///<see cref="http://www.w3.org/ns/prov#Bundle"/>
    ///</summary>
    public const string Bundle = "http://www.w3.org/ns/prov#Bundle";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Collection"/>
    ///</summary>
    public const string Collection = "http://www.w3.org/ns/prov#Collection";

    ///<summary>
    ///An instance of prov:Communication provides additional descriptions about the binary prov:wasInformedBy relation from an informed prov:Activity to the prov:Activity that informed it. For example, :you_jumping_off_bridge prov:wasInformedBy :everyone_else_jumping_off_bridge; prov:qualifiedCommunication [ a prov:Communication; prov:activity :everyone_else_jumping_off_bridge; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Communication"/>
    ///</summary>
    public const string Communication = "http://www.w3.org/ns/prov#Communication";

    ///<summary>
    ///An instance of prov:Delegation provides additional descriptions about the binary prov:actedOnBehalfOf relation from a performing prov:Agent to some prov:Agent for whom it was performed. For example, :mixing prov:wasAssociatedWith :toddler . :toddler prov:actedOnBehalfOf :mother; prov:qualifiedDelegation [ a prov:Delegation; prov:entity :mother; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Delegation"/>
    ///</summary>
    public const string Delegation = "http://www.w3.org/ns/prov#Delegation";

    ///<summary>
    ///An instance of prov:Derivation provides additional descriptions about the binary prov:wasDerivedFrom relation from some derived prov:Entity to another prov:Entity from which it was derived. For example, :chewed_bubble_gum prov:wasDerivedFrom :unwrapped_bubble_gum; prov:qualifiedDerivation [ a prov:Derivation; prov:entity :unwrapped_bubble_gum; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Derivation"/>
    ///</summary>
    public const string Derivation = "http://www.w3.org/ns/prov#Derivation";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#EmptyCollection"/>
    ///</summary>
    public const string EmptyCollection = "http://www.w3.org/ns/prov#EmptyCollection";

    ///<summary>
    ///An instance of prov:End provides additional descriptions about the binary prov:wasEndedBy relation from some ended prov:Activity to an prov:Entity that ended it. For example, :ball_game prov:wasEndedBy :buzzer; prov:qualifiedEnd [ a prov:End; prov:entity :buzzer; :foo :bar; prov:atTime '2012-03-09T08:05:08-05:00'^^xsd:dateTime ].
    ///<see cref="http://www.w3.org/ns/prov#End"/>
    ///</summary>
    public const string End = "http://www.w3.org/ns/prov#End";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Entity"/>
    ///</summary>
    public const string Entity = "http://www.w3.org/ns/prov#Entity";

    ///<summary>
    ///EntityInfluence provides additional descriptions of an Entity's binary influence upon any other kind of resource. Instances of EntityInfluence use the prov:entity property to cite the influencing Entity.
    ///<see cref="http://www.w3.org/ns/prov#EntityInfluence"/>
    ///</summary>
    public const string EntityInfluence = "http://www.w3.org/ns/prov#EntityInfluence";

    ///<summary>
    ///An instance of prov:Generation provides additional descriptions about the binary prov:wasGeneratedBy relation from a generated prov:Entity to the prov:Activity that generated it. For example, :cake prov:wasGeneratedBy :baking; prov:qualifiedGeneration [ a prov:Generation; prov:activity :baking; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Generation"/>
    ///</summary>
    public const string Generation = "http://www.w3.org/ns/prov#Generation";

    ///<summary>
    ///An instance of prov:Influence provides additional descriptions about the binary prov:wasInfluencedBy relation from some influenced Activity, Entity, or Agent to the influencing Activity, Entity, or Agent. For example, :stomach_ache prov:wasInfluencedBy :spoon; prov:qualifiedInfluence [ a prov:Influence; prov:entity :spoon; :foo :bar ] . Because prov:Influence is a broad relation, the more specific relations (Communication, Delegation, End, etc.) should be used when applicable.
    ///<see cref="http://www.w3.org/ns/prov#Influence"/>
    ///</summary>
    public const string Influence = "http://www.w3.org/ns/prov#Influence";

    ///<summary>
    ///An instantaneous event, or event for short, happens in the world and marks a change in the world, in its activities and in its entities. The term 'event' is commonly used in process algebra with a similar meaning. Events represent communications or interactions; they are assumed to be atomic and instantaneous.
    ///<see cref="http://www.w3.org/ns/prov#InstantaneousEvent"/>
    ///</summary>
    public const string InstantaneousEvent = "http://www.w3.org/ns/prov#InstantaneousEvent";

    ///<summary>
    ///An instance of prov:Invalidation provides additional descriptions about the binary prov:wasInvalidatedBy relation from an invalidated prov:Entity to the prov:Activity that invalidated it. For example, :uncracked_egg prov:wasInvalidatedBy :baking; prov:qualifiedInvalidation [ a prov:Invalidation; prov:activity :baking; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Invalidation"/>
    ///</summary>
    public const string Invalidation = "http://www.w3.org/ns/prov#Invalidation";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Location"/>
    ///</summary>
    public const string Location = "http://www.w3.org/ns/prov#Location";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Organization"/>
    ///</summary>
    public const string Organization = "http://www.w3.org/ns/prov#Organization";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Person"/>
    ///</summary>
    public const string Person = "http://www.w3.org/ns/prov#Person";

    ///<summary>
    ///There exist no prescriptive requirement on the nature of plans, their representation, the actions or steps they consist of, or their intended goals. Since plans may evolve over time, it may become necessary to track their provenance, so plans themselves are entities. Representing the plan explicitly in the provenance can be useful for various tasks: for example, to validate the execution as represented in the provenance record, to manage expectation failures, or to provide explanations.
    ///<see cref="http://www.w3.org/ns/prov#Plan"/>
    ///</summary>
    public const string Plan = "http://www.w3.org/ns/prov#Plan";

    ///<summary>
    ///An instance of prov:PrimarySource provides additional descriptions about the binary prov:hadPrimarySource relation from some secondary prov:Entity to an earlier, primary prov:Entity. For example, :blog prov:hadPrimarySource :newsArticle; prov:qualifiedPrimarySource [ a prov:PrimarySource; prov:entity :newsArticle; :foo :bar ] .
    ///<see cref="http://www.w3.org/ns/prov#PrimarySource"/>
    ///</summary>
    public const string PrimarySource = "http://www.w3.org/ns/prov#PrimarySource";

    ///<summary>
    ///An instance of prov:Quotation provides additional descriptions about the binary prov:wasQuotedFrom relation from some taken prov:Entity from an earlier, larger prov:Entity. For example, :here_is_looking_at_you_kid prov:wasQuotedFrom :casablanca_script; prov:qualifiedQuotation [ a prov:Quotation; prov:entity :casablanca_script; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Quotation"/>
    ///</summary>
    public const string Quotation = "http://www.w3.org/ns/prov#Quotation";

    ///<summary>
    ///An instance of prov:Revision provides additional descriptions about the binary prov:wasRevisionOf relation from some newer prov:Entity to an earlier prov:Entity. For example, :draft_2 prov:wasRevisionOf :draft_1; prov:qualifiedRevision [ a prov:Revision; prov:entity :draft_1; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Revision"/>
    ///</summary>
    public const string Revision = "http://www.w3.org/ns/prov#Revision";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Role"/>
    ///</summary>
    public const string Role = "http://www.w3.org/ns/prov#Role";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#SoftwareAgent"/>
    ///</summary>
    public const string SoftwareAgent = "http://www.w3.org/ns/prov#SoftwareAgent";

    ///<summary>
    ///An instance of prov:Start provides additional descriptions about the binary prov:wasStartedBy relation from some started prov:Activity to an prov:Entity that started it. For example, :foot_race prov:wasStartedBy :bang; prov:qualifiedStart [ a prov:Start; prov:entity :bang; :foo :bar; prov:atTime '2012-03-09T08:05:08-05:00'^^xsd:dateTime ] .
    ///<see cref="http://www.w3.org/ns/prov#Start"/>
    ///</summary>
    public const string Start = "http://www.w3.org/ns/prov#Start";

    ///<summary>
    ///An instance of prov:Usage provides additional descriptions about the binary prov:used relation from some prov:Activity to an prov:Entity that it used. For example, :keynote prov:used :podium; prov:qualifiedUsage [ a prov:Usage; prov:entity :podium; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#Usage"/>
    ///</summary>
    public const string Usage = "http://www.w3.org/ns/prov#Usage";

    ///<summary>
    ///An object property to express the accountability of an agent towards another agent. The subordinate agent acted on behalf of the responsible agent in an actual activity. 
    ///<see cref="http://www.w3.org/ns/prov#actedOnBehalfOf"/>
    ///</summary>
    public const string actedOnBehalfOf = "http://www.w3.org/ns/prov#actedOnBehalfOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#activity"/>
    ///</summary>
    public const string activity = "http://www.w3.org/ns/prov#activity";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#agent"/>
    ///</summary>
    public const string agent = "http://www.w3.org/ns/prov#agent";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#alternateOf"/>
    ///</summary>
    public const string alternateOf = "http://www.w3.org/ns/prov#alternateOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#aq"/>
    ///</summary>
    public const string aq = "http://www.w3.org/ns/prov#aq";

    ///<summary>
    ///The Location of any resource.
    ///<see cref="http://www.w3.org/ns/prov#atLocation"/>
    ///</summary>
    public const string atLocation = "http://www.w3.org/ns/prov#atLocation";

    ///<summary>
    ///The time at which an InstantaneousEvent occurred, in the form of xsd:dateTime.
    ///<see cref="http://www.w3.org/ns/prov#atTime"/>
    ///</summary>
    public const string atTime = "http://www.w3.org/ns/prov#atTime";

    ///<summary>
    ///Classify prov-o terms into three categories, including 'starting-point', 'qualifed', and 'extended'. This classification is used by the prov-o html document to gently introduce prov-o terms to its users. 
    ///<see cref="http://www.w3.org/ns/prov#category"/>
    ///</summary>
    public const string category = "http://www.w3.org/ns/prov#category";

    ///<summary>
    ///Classify prov-o terms into six components according to prov-dm, including 'agents-responsibility', 'alternate', 'annotations', 'collections', 'derivations', and 'entities-activities'. This classification is used so that readers of prov-o specification can find its correspondence with the prov-dm specification.
    ///<see cref="http://www.w3.org/ns/prov#component"/>
    ///</summary>
    public const string component = "http://www.w3.org/ns/prov#component";

    ///<summary>
    ///A reference to the principal section of the PROV-CONSTRAINTS document that describes this concept.
    ///<see cref="http://www.w3.org/ns/prov#constraints"/>
    ///</summary>
    public const string constraints = "http://www.w3.org/ns/prov#constraints";

    ///<summary>
    ///A definition quoted from PROV-DM or PROV-CONSTRAINTS that describes the concept expressed with this OWL term.
    ///<see cref="http://www.w3.org/ns/prov#definition"/>
    ///</summary>
    public const string definition = "http://www.w3.org/ns/prov#definition";

    ///<summary>
    ///A reference to the principal section of the PROV-DM document that describes this concept.
    ///<see cref="http://www.w3.org/ns/prov#dm"/>
    ///</summary>
    public const string dm = "http://www.w3.org/ns/prov#dm";

    ///<summary>
    ///A note by the OWL development team about how this term expresses the PROV-DM concept, or how it should be used in context of semantic web or linked data.
    ///<see cref="http://www.w3.org/ns/prov#editorialNote"/>
    ///</summary>
    public const string editorialNote = "http://www.w3.org/ns/prov#editorialNote";

    ///<summary>
    ///When the prov-o term does not have a definition drawn from prov-dm, and the prov-o editor provides one.
    ///<see cref="http://www.w3.org/ns/prov#editorsDefinition"/>
    ///</summary>
    public const string editorsDefinition = "http://www.w3.org/ns/prov#editorsDefinition";

    ///<summary>
    ///The time at which an activity ended. See also prov:startedAtTime.
    ///<see cref="http://www.w3.org/ns/prov#endedAtTime"/>
    ///</summary>
    public const string endedAtTime = "http://www.w3.org/ns/prov#endedAtTime";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#entity"/>
    ///</summary>
    public const string entity = "http://www.w3.org/ns/prov#entity";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#generated"/>
    ///</summary>
    public const string generated = "http://www.w3.org/ns/prov#generated";

    ///<summary>
    ///The time at which an entity was completely created and is available for use.
    ///<see cref="http://www.w3.org/ns/prov#generatedAtTime"/>
    ///</summary>
    public const string generatedAtTime = "http://www.w3.org/ns/prov#generatedAtTime";

    ///<summary>
    ///The _optional_ Activity of an Influence, which used, generated, invalidated, or was the responsibility of some Entity. This property is _not_ used by ActivityInfluence (use prov:activity instead).
    ///<see cref="http://www.w3.org/ns/prov#hadActivity"/>
    ///</summary>
    public const string hadActivity = "http://www.w3.org/ns/prov#hadActivity";

    ///<summary>
    ///The _optional_ Generation involved in an Entity's Derivation.
    ///<see cref="http://www.w3.org/ns/prov#hadGeneration"/>
    ///</summary>
    public const string hadGeneration = "http://www.w3.org/ns/prov#hadGeneration";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadMember"/>
    ///</summary>
    public const string hadMember = "http://www.w3.org/ns/prov#hadMember";

    ///<summary>
    ///The _optional_ Plan adopted by an Agent in Association with some Activity. Plan specifications are out of the scope of this specification.
    ///<see cref="http://www.w3.org/ns/prov#hadPlan"/>
    ///</summary>
    public const string hadPlan = "http://www.w3.org/ns/prov#hadPlan";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadPrimarySource"/>
    ///</summary>
    public const string hadPrimarySource = "http://www.w3.org/ns/prov#hadPrimarySource";

    ///<summary>
    ///The _optional_ Role that an Entity assumed in the context of an Activity. For example, :baking prov:used :spoon; prov:qualified [ a prov:Usage; prov:entity :spoon; prov:hadRole roles:mixing_implement ].
    ///<see cref="http://www.w3.org/ns/prov#hadRole"/>
    ///</summary>
    public const string hadRole = "http://www.w3.org/ns/prov#hadRole";

    ///<summary>
    ///The _optional_ Usage involved in an Entity's Derivation.
    ///<see cref="http://www.w3.org/ns/prov#hadUsage"/>
    ///</summary>
    public const string hadUsage = "http://www.w3.org/ns/prov#hadUsage";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#influenced"/>
    ///</summary>
    public const string influenced = "http://www.w3.org/ns/prov#influenced";

    ///<summary>
    ///Subproperties of prov:influencer are used to cite the object of an unqualified PROV-O triple whose predicate is a subproperty of prov:wasInfluencedBy (e.g. prov:used, prov:wasGeneratedBy). prov:influencer is used much like rdf:object is used.
    ///<see cref="http://www.w3.org/ns/prov#influencer"/>
    ///</summary>
    public const string influencer = "http://www.w3.org/ns/prov#influencer";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#invalidated"/>
    ///</summary>
    public const string invalidated = "http://www.w3.org/ns/prov#invalidated";

    ///<summary>
    ///The time at which an entity was invalidated (i.e., no longer usable).
    ///<see cref="http://www.w3.org/ns/prov#invalidatedAtTime"/>
    ///</summary>
    public const string invalidatedAtTime = "http://www.w3.org/ns/prov#invalidatedAtTime";

    ///<summary>
    ///PROV-O does not define all property inverses. The directionalities defined in PROV-O should be given preference over those not defined. However, if users wish to name the inverse of a PROV-O property, the local name given by prov:inverse should be used.
    ///<see cref="http://www.w3.org/ns/prov#inverse"/>
    ///</summary>
    public const string inverse = "http://www.w3.org/ns/prov#inverse";

    ///<summary>
    ///A reference to the principal section of the PROV-DM document that describes this concept.
    ///<see cref="http://www.w3.org/ns/prov#n"/>
    ///</summary>
    public const string n = "http://www.w3.org/ns/prov#n";

    ///<summary>
    ///The position that this OWL term should be listed within documentation. The scope of the documentation (e.g., among all terms, among terms within a prov:category, among properties applying to a particular class, etc.) is unspecified.
    ///<see cref="http://www.w3.org/ns/prov#order"/>
    ///</summary>
    public const string order = "http://www.w3.org/ns/prov#order";

    ///<summary>
    ///If this Activity prov:wasAssociatedWith Agent :ag, then it can qualify the Association using prov:qualifiedAssociation [ a prov:Association;  prov:agent :ag; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedAssociation"/>
    ///</summary>
    public const string qualifiedAssociation = "http://www.w3.org/ns/prov#qualifiedAssociation";

    ///<summary>
    ///If this Entity prov:wasAttributedTo Agent :ag, then it can qualify how it was influenced using prov:qualifiedAttribution [ a prov:Attribution;  prov:agent :ag; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedAttribution"/>
    ///</summary>
    public const string qualifiedAttribution = "http://www.w3.org/ns/prov#qualifiedAttribution";

    ///<summary>
    ///If this Activity prov:wasInformedBy Activity :a, then it can qualify how it was influenced using prov:qualifiedCommunication [ a prov:Communication;  prov:activity :a; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedCommunication"/>
    ///</summary>
    public const string qualifiedCommunication = "http://www.w3.org/ns/prov#qualifiedCommunication";

    ///<summary>
    ///If this Agent prov:actedOnBehalfOf Agent :ag, then it can qualify how with prov:qualifiedResponsibility [ a prov:Responsibility;  prov:agent :ag; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedDelegation"/>
    ///</summary>
    public const string qualifiedDelegation = "http://www.w3.org/ns/prov#qualifiedDelegation";

    ///<summary>
    ///If this Entity prov:wasDerivedFrom Entity :e, then it can qualify how it was derived using prov:qualifiedDerivation [ a prov:Derivation;  prov:entity :e; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedDerivation"/>
    ///</summary>
    public const string qualifiedDerivation = "http://www.w3.org/ns/prov#qualifiedDerivation";

    ///<summary>
    ///If this Activity prov:wasEndedBy Entity :e1, then it can qualify how it was ended using prov:qualifiedEnd [ a prov:End;  prov:entity :e1; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedEnd"/>
    ///</summary>
    public const string qualifiedEnd = "http://www.w3.org/ns/prov#qualifiedEnd";

    ///<summary>
    ///This annotation property links a subproperty of prov:wasInfluencedBy with the subclass of prov:Influence and the qualifying property that are used to qualify it. 
    ///
    ///Example annotation:
    ///
    ///    prov:wasGeneratedBy prov:qualifiedForm prov:qualifiedGeneration, prov:Generation .
    ///
    ///Then this unqualified assertion:
    ///
    ///    :entity1 prov:wasGeneratedBy :activity1 .
    ///
    ///can be qualified by adding:
    ///
    ///   :entity1 prov:qualifiedGeneration :entity1Gen .
    ///   :entity1Gen 
    ///       a prov:Generation, prov:Influence;
    ///       prov:activity :activity1;
    ///       :customValue 1337 .
    ///
    ///Note how the value of the unqualified influence (prov:wasGeneratedBy :activity1) is mirrored as the value of the prov:activity (or prov:entity, or prov:agent) property on the influence class.
    ///<see cref="http://www.w3.org/ns/prov#qualifiedForm"/>
    ///</summary>
    public const string qualifiedForm = "http://www.w3.org/ns/prov#qualifiedForm";

    ///<summary>
    ///If this Activity prov:generated Entity :e, then it can qualify how it performed the Generation using prov:qualifiedGeneration [ a prov:Generation;  prov:entity :e; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedGeneration"/>
    ///</summary>
    public const string qualifiedGeneration = "http://www.w3.org/ns/prov#qualifiedGeneration";

    ///<summary>
    ///Because prov:qualifiedInfluence is a broad relation, the more specific relations (qualifiedCommunication, qualifiedDelegation, qualifiedEnd, etc.) should be used when applicable.
    ///<see cref="http://www.w3.org/ns/prov#qualifiedInfluence"/>
    ///</summary>
    public const string qualifiedInfluence = "http://www.w3.org/ns/prov#qualifiedInfluence";

    ///<summary>
    ///If this Entity prov:wasInvalidatedBy Activity :a, then it can qualify how it was invalidated using prov:qualifiedInvalidation [ a prov:Invalidation;  prov:activity :a; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedInvalidation"/>
    ///</summary>
    public const string qualifiedInvalidation = "http://www.w3.org/ns/prov#qualifiedInvalidation";

    ///<summary>
    ///If this Entity prov:hadPrimarySource Entity :e, then it can qualify how using prov:qualifiedPrimarySource [ a prov:PrimarySource; prov:entity :e; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedPrimarySource"/>
    ///</summary>
    public const string qualifiedPrimarySource = "http://www.w3.org/ns/prov#qualifiedPrimarySource";

    ///<summary>
    ///If this Entity prov:wasQuotedFrom Entity :e, then it can qualify how using prov:qualifiedQuotation [ a prov:Quotation;  prov:entity :e; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedQuotation"/>
    ///</summary>
    public const string qualifiedQuotation = "http://www.w3.org/ns/prov#qualifiedQuotation";

    ///<summary>
    ///If this Entity prov:wasRevisionOf Entity :e, then it can qualify how it was revised using prov:qualifiedRevision [ a prov:Revision;  prov:entity :e; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedRevision"/>
    ///</summary>
    public const string qualifiedRevision = "http://www.w3.org/ns/prov#qualifiedRevision";

    ///<summary>
    ///If this Activity prov:wasStartedBy Entity :e1, then it can qualify how it was started using prov:qualifiedStart [ a prov:Start;  prov:entity :e1; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedStart"/>
    ///</summary>
    public const string qualifiedStart = "http://www.w3.org/ns/prov#qualifiedStart";

    ///<summary>
    ///If this Activity prov:used Entity :e, then it can qualify how it used it using prov:qualifiedUsage [ a prov:Usage; prov:entity :e; :foo :bar ].
    ///<see cref="http://www.w3.org/ns/prov#qualifiedUsage"/>
    ///</summary>
    public const string qualifiedUsage = "http://www.w3.org/ns/prov#qualifiedUsage";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#sharesDefinitionWith"/>
    ///</summary>
    public const string sharesDefinitionWith = "http://www.w3.org/ns/prov#sharesDefinitionWith";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#specializationOf"/>
    ///</summary>
    public const string specializationOf = "http://www.w3.org/ns/prov#specializationOf";

    ///<summary>
    ///The time at which an activity started. See also prov:endedAtTime.
    ///<see cref="http://www.w3.org/ns/prov#startedAtTime"/>
    ///</summary>
    public const string startedAtTime = "http://www.w3.org/ns/prov#startedAtTime";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#todo"/>
    ///</summary>
    public const string todo = "http://www.w3.org/ns/prov#todo";

    ///<summary>
    ///Classes and properties used to qualify relationships are annotated with prov:unqualifiedForm to indicate the property used to assert an unqualified provenance relation.
    ///<see cref="http://www.w3.org/ns/prov#unqualifiedForm"/>
    ///</summary>
    public const string unqualifiedForm = "http://www.w3.org/ns/prov#unqualifiedForm";

    ///<summary>
    ///A prov:Entity that was used by this prov:Activity. For example, :baking prov:used :spoon, :egg, :oven .
    ///<see cref="http://www.w3.org/ns/prov#used"/>
    ///</summary>
    public const string used = "http://www.w3.org/ns/prov#used";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#value"/>
    ///</summary>
    public const string value = "http://www.w3.org/ns/prov#value";

    ///<summary>
    ///An prov:Agent that had some (unspecified) responsibility for the occurrence of this prov:Activity.
    ///<see cref="http://www.w3.org/ns/prov#wasAssociatedWith"/>
    ///</summary>
    public const string wasAssociatedWith = "http://www.w3.org/ns/prov#wasAssociatedWith";

    ///<summary>
    ///Attribution is the ascribing of an entity to an agent.
    ///<see cref="http://www.w3.org/ns/prov#wasAttributedTo"/>
    ///</summary>
    public const string wasAttributedTo = "http://www.w3.org/ns/prov#wasAttributedTo";

    ///<summary>
    ///The more specific subproperties of prov:wasDerivedFrom (i.e., prov:wasQuotedFrom, prov:wasRevisionOf, prov:hadPrimarySource) should be used when applicable.
    ///<see cref="http://www.w3.org/ns/prov#wasDerivedFrom"/>
    ///</summary>
    public const string wasDerivedFrom = "http://www.w3.org/ns/prov#wasDerivedFrom";

    ///<summary>
    ///End is when an activity is deemed to have ended. An end may refer to an entity, known as trigger, that terminated the activity.
    ///<see cref="http://www.w3.org/ns/prov#wasEndedBy"/>
    ///</summary>
    public const string wasEndedBy = "http://www.w3.org/ns/prov#wasEndedBy";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasGeneratedBy"/>
    ///</summary>
    public const string wasGeneratedBy = "http://www.w3.org/ns/prov#wasGeneratedBy";

    ///<summary>
    ///Because prov:wasInfluencedBy is a broad relation, its more specific subproperties (e.g. prov:wasInformedBy, prov:actedOnBehalfOf, prov:wasEndedBy, etc.) should be used when applicable.
    ///<see cref="http://www.w3.org/ns/prov#wasInfluencedBy"/>
    ///</summary>
    public const string wasInfluencedBy = "http://www.w3.org/ns/prov#wasInfluencedBy";

    ///<summary>
    ///An activity a2 is dependent on or informed by another activity a1, by way of some unspecified entity that is generated by a1 and used by a2.
    ///<see cref="http://www.w3.org/ns/prov#wasInformedBy"/>
    ///</summary>
    public const string wasInformedBy = "http://www.w3.org/ns/prov#wasInformedBy";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasInvalidatedBy"/>
    ///</summary>
    public const string wasInvalidatedBy = "http://www.w3.org/ns/prov#wasInvalidatedBy";

    ///<summary>
    ///An entity is derived from an original entity by copying, or 'quoting', some or all of it.
    ///<see cref="http://www.w3.org/ns/prov#wasQuotedFrom"/>
    ///</summary>
    public const string wasQuotedFrom = "http://www.w3.org/ns/prov#wasQuotedFrom";

    ///<summary>
    ///A revision is a derivation that revises an entity into a revised version.
    ///<see cref="http://www.w3.org/ns/prov#wasRevisionOf"/>
    ///</summary>
    public const string wasRevisionOf = "http://www.w3.org/ns/prov#wasRevisionOf";

    ///<summary>
    ///Start is when an activity is deemed to have started. A start may refer to an entity, known as trigger, that initiated the activity.
    ///<see cref="http://www.w3.org/ns/prov#wasStartedBy"/>
    ///</summary>
    public const string wasStartedBy = "http://www.w3.org/ns/prov#wasStartedBy";

    ///<summary>
    ///This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page). 
    ///
    ///If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/Archives/Public/public-prov-comments/). All feedback is welcome.
    ///<see cref="http://www.w3.org/ns/prov-o#"/>
    ///</summary>
    public const string prov_o = "http://www.w3.org/ns/prov-o#";

    ///<summary>
    ///
    ///<see cref="file:///D:/Projects/Artivity/Artivity.Api.Model/Ontologies/prov.ttl#"/>
    ///</summary>
    public const string prov_ttl = "file:///D:/Projects/Artivity/Artivity.Api.Model/Ontologies/prov.ttl#";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadDelegate"/>
    ///</summary>
    public const string hadDelegate = "http://www.w3.org/ns/prov#hadDelegate";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#activityOfInfluence"/>
    ///</summary>
    public const string activityOfInfluence = "http://www.w3.org/ns/prov#activityOfInfluence";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#agentOfInfluence"/>
    ///</summary>
    public const string agentOfInfluence = "http://www.w3.org/ns/prov#agentOfInfluence";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#locationOf"/>
    ///</summary>
    public const string locationOf = "http://www.w3.org/ns/prov#locationOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#entityOfInfluence"/>
    ///</summary>
    public const string entityOfInfluence = "http://www.w3.org/ns/prov#entityOfInfluence";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasActivityOfInfluence"/>
    ///</summary>
    public const string wasActivityOfInfluence = "http://www.w3.org/ns/prov#wasActivityOfInfluence";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#generatedAsDerivation"/>
    ///</summary>
    public const string generatedAsDerivation = "http://www.w3.org/ns/prov#generatedAsDerivation";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasMemberOf"/>
    ///</summary>
    public const string wasMemberOf = "http://www.w3.org/ns/prov#wasMemberOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasPlanOf"/>
    ///</summary>
    public const string wasPlanOf = "http://www.w3.org/ns/prov#wasPlanOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasPrimarySourceOf"/>
    ///</summary>
    public const string wasPrimarySourceOf = "http://www.w3.org/ns/prov#wasPrimarySourceOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasRoleIn"/>
    ///</summary>
    public const string wasRoleIn = "http://www.w3.org/ns/prov#wasRoleIn";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasUsedInDerivation"/>
    ///</summary>
    public const string wasUsedInDerivation = "http://www.w3.org/ns/prov#wasUsedInDerivation";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadInfluence"/>
    ///</summary>
    public const string hadInfluence = "http://www.w3.org/ns/prov#hadInfluence";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedAssociationOf"/>
    ///</summary>
    public const string qualifiedAssociationOf = "http://www.w3.org/ns/prov#qualifiedAssociationOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedAttributionOf"/>
    ///</summary>
    public const string qualifiedAttributionOf = "http://www.w3.org/ns/prov#qualifiedAttributionOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedCommunicationOf"/>
    ///</summary>
    public const string qualifiedCommunicationOf = "http://www.w3.org/ns/prov#qualifiedCommunicationOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedDelegationOf"/>
    ///</summary>
    public const string qualifiedDelegationOf = "http://www.w3.org/ns/prov#qualifiedDelegationOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedDerivationOf"/>
    ///</summary>
    public const string qualifiedDerivationOf = "http://www.w3.org/ns/prov#qualifiedDerivationOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedEndOf"/>
    ///</summary>
    public const string qualifiedEndOf = "http://www.w3.org/ns/prov#qualifiedEndOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedGenerationOf"/>
    ///</summary>
    public const string qualifiedGenerationOf = "http://www.w3.org/ns/prov#qualifiedGenerationOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedInfluenceOf"/>
    ///</summary>
    public const string qualifiedInfluenceOf = "http://www.w3.org/ns/prov#qualifiedInfluenceOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedInvalidationOf"/>
    ///</summary>
    public const string qualifiedInvalidationOf = "http://www.w3.org/ns/prov#qualifiedInvalidationOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedSourceOf"/>
    ///</summary>
    public const string qualifiedSourceOf = "http://www.w3.org/ns/prov#qualifiedSourceOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedQuotationOf"/>
    ///</summary>
    public const string qualifiedQuotationOf = "http://www.w3.org/ns/prov#qualifiedQuotationOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#revisedEntity"/>
    ///</summary>
    public const string revisedEntity = "http://www.w3.org/ns/prov#revisedEntity";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedStartOf"/>
    ///</summary>
    public const string qualifiedStartOf = "http://www.w3.org/ns/prov#qualifiedStartOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedUsingActivity"/>
    ///</summary>
    public const string qualifiedUsingActivity = "http://www.w3.org/ns/prov#qualifiedUsingActivity";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#generalizationOf"/>
    ///</summary>
    public const string generalizationOf = "http://www.w3.org/ns/prov#generalizationOf";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasUsedBy"/>
    ///</summary>
    public const string wasUsedBy = "http://www.w3.org/ns/prov#wasUsedBy";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasAssociateFor"/>
    ///</summary>
    public const string wasAssociateFor = "http://www.w3.org/ns/prov#wasAssociateFor";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#contributed"/>
    ///</summary>
    public const string contributed = "http://www.w3.org/ns/prov#contributed";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadDerivation"/>
    ///</summary>
    public const string hadDerivation = "http://www.w3.org/ns/prov#hadDerivation";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#ended"/>
    ///</summary>
    public const string ended = "http://www.w3.org/ns/prov#ended";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#informed"/>
    ///</summary>
    public const string informed = "http://www.w3.org/ns/prov#informed";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#quotedAs"/>
    ///</summary>
    public const string quotedAs = "http://www.w3.org/ns/prov#quotedAs";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadRevision"/>
    ///</summary>
    public const string hadRevision = "http://www.w3.org/ns/prov#hadRevision";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#started"/>
    ///</summary>
    public const string started = "http://www.w3.org/ns/prov#started";

    ///<summary>
    ///0.2
    ///<see cref="http://www.w3.org/ns/prov-aq#"/>
    ///</summary>
    public const string prov_aq = "http://www.w3.org/ns/prov-aq#";

    ///<summary>
    ///Type for a generic provenance query service. Mainly for use in RDF provenance query service descriptions, to facilitate discovery in linked data environments.
    ///<see cref="http://www.w3.org/ns/prov#ServiceDescription"/>
    ///</summary>
    public const string ServiceDescription = "http://www.w3.org/ns/prov#ServiceDescription";

    ///<summary>
    ///Type for a generic provenance query service. Mainly for use in RDF provenance query service descriptions, to facilitate discovery in linked data environments.
    ///<see cref="http://www.w3.org/ns/prov#DirectQueryService"/>
    ///</summary>
    public const string DirectQueryService = "http://www.w3.org/ns/prov#DirectQueryService";

    ///<summary>
    ///Indicates anchor URI for a potentially dynamic resource instance.
    ///<see cref="http://www.w3.org/ns/prov#has_anchor"/>
    ///</summary>
    public const string has_anchor = "http://www.w3.org/ns/prov#has_anchor";

    ///<summary>
    ///Indicates a provenance-URI for a resource; the resource identified by this property presents a provenance record about its subject or anchor resource.
    ///<see cref="http://www.w3.org/ns/prov#has_provenance"/>
    ///</summary>
    public const string has_provenance = "http://www.w3.org/ns/prov#has_provenance";

    ///<summary>
    ///Indicates a provenance query service that can access provenance related to its subject or anchor resource.
    ///<see cref="http://www.w3.org/ns/prov#has_query_service"/>
    ///</summary>
    public const string has_query_service = "http://www.w3.org/ns/prov#has_query_service";

    ///<summary>
    ///relates a generic provenance query service resource (type prov:ServiceDescription) to a specific query service description (e.g. a prov:DirectQueryService or a sd:Service).
    ///<see cref="http://www.w3.org/ns/prov#describesService"/>
    ///</summary>
    public const string describesService = "http://www.w3.org/ns/prov#describesService";

    ///<summary>
    ///Relates a provenance service to a URI template string for constructing provenance-URIs.
    ///<see cref="http://www.w3.org/ns/prov#provenanceUriTemplate"/>
    ///</summary>
    public const string provenanceUriTemplate = "http://www.w3.org/ns/prov#provenanceUriTemplate";

    ///<summary>
    ///Relates a resource to a provenance pingback service that may receive additional provenance links about the resource.
    ///<see cref="http://www.w3.org/ns/prov#pingback"/>
    ///</summary>
    public const string pingback = "http://www.w3.org/ns/prov#pingback";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2002/07/owl#topObjectProperty"/>
    ///</summary>
    public const string topObjectProperty = "http://www.w3.org/2002/07/owl#topObjectProperty";

    ///<summary>
    ///This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page). 
    ///
    ///If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/Archives/Public/public-prov-comments/). All feedback is welcome.
    ///<see cref="http://www.w3.org/ns/prov-dc#"/>
    ///</summary>
    public const string prov_dc = "http://www.w3.org/ns/prov-dc#";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Accept"/>
    ///</summary>
    public const string Accept = "http://www.w3.org/ns/prov#Accept";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Contribute"/>
    ///</summary>
    public const string Contribute = "http://www.w3.org/ns/prov#Contribute";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Contributor"/>
    ///</summary>
    public const string Contributor = "http://www.w3.org/ns/prov#Contributor";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Copyright"/>
    ///</summary>
    public const string Copyright = "http://www.w3.org/ns/prov#Copyright";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Create"/>
    ///</summary>
    public const string Create = "http://www.w3.org/ns/prov#Create";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Creator"/>
    ///</summary>
    public const string Creator = "http://www.w3.org/ns/prov#Creator";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Modify"/>
    ///</summary>
    public const string Modify = "http://www.w3.org/ns/prov#Modify";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Publish"/>
    ///</summary>
    public const string Publish = "http://www.w3.org/ns/prov#Publish";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Publisher"/>
    ///</summary>
    public const string Publisher = "http://www.w3.org/ns/prov#Publisher";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Replace"/>
    ///</summary>
    public const string Replace = "http://www.w3.org/ns/prov#Replace";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#RightsAssignment"/>
    ///</summary>
    public const string RightsAssignment = "http://www.w3.org/ns/prov#RightsAssignment";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#RightsHolder"/>
    ///</summary>
    public const string RightsHolder = "http://www.w3.org/ns/prov#RightsHolder";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Submit"/>
    ///</summary>
    public const string Submit = "http://www.w3.org/ns/prov#Submit";

    ///<summary>
    ///This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page). 
    ///
    ///If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/Archives/Public/public-prov-comments/). All feedback is welcome.
    ///<see cref="http://www.w3.org/ns/prov-dictionary#"/>
    ///</summary>
    public const string prov_dictionary = "http://www.w3.org/ns/prov-dictionary#";

    ///<summary>
    ///This concept allows for the provenance of the dictionary, but also of its constituents to be expressed. Such a notion of dictionary corresponds to a wide variety of concrete data structures, such as a maps or associative arrays.
    ///<see cref="http://www.w3.org/ns/prov#Dictionary"/>
    ///</summary>
    public const string Dictionary = "http://www.w3.org/ns/prov#Dictionary";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#EmptyDictionary"/>
    ///</summary>
    public const string EmptyDictionary = "http://www.w3.org/ns/prov#EmptyDictionary";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#KeyEntityPair"/>
    ///</summary>
    public const string KeyEntityPair = "http://www.w3.org/ns/prov#KeyEntityPair";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Insertion"/>
    ///</summary>
    public const string Insertion = "http://www.w3.org/ns/prov#Insertion";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Removal"/>
    ///</summary>
    public const string Removal = "http://www.w3.org/ns/prov#Removal";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#dictionary"/>
    ///</summary>
    public const string dictionary = "http://www.w3.org/ns/prov#dictionary";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#derivedByInsertionFrom"/>
    ///</summary>
    public const string derivedByInsertionFrom = "http://www.w3.org/ns/prov#derivedByInsertionFrom";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#derivedByRemovalFrom"/>
    ///</summary>
    public const string derivedByRemovalFrom = "http://www.w3.org/ns/prov#derivedByRemovalFrom";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#insertedKeyEntityPair"/>
    ///</summary>
    public const string insertedKeyEntityPair = "http://www.w3.org/ns/prov#insertedKeyEntityPair";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadDictionaryMember"/>
    ///</summary>
    public const string hadDictionaryMember = "http://www.w3.org/ns/prov#hadDictionaryMember";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#pairKey"/>
    ///</summary>
    public const string pairKey = "http://www.w3.org/ns/prov#pairKey";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#pairEntity"/>
    ///</summary>
    public const string pairEntity = "http://www.w3.org/ns/prov#pairEntity";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedInsertion"/>
    ///</summary>
    public const string qualifiedInsertion = "http://www.w3.org/ns/prov#qualifiedInsertion";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#qualifiedRemoval"/>
    ///</summary>
    public const string qualifiedRemoval = "http://www.w3.org/ns/prov#qualifiedRemoval";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#removedKey"/>
    ///</summary>
    public const string removedKey = "http://www.w3.org/ns/prov#removedKey";

    ///<summary>
    ///This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page). If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/Archives/Public/public-prov-comments/
    ///). All feedback is welcome.
    ///<see cref="http://www.w3.org/ns/prov-links#"/>
    ///</summary>
    public const string prov_links = "http://www.w3.org/ns/prov-links#";

    ///<summary>
    ///prov:asInBundle is used to specify which bundle the general entity of a prov:mentionOf property is described.
    ///
    ///When :x prov:mentionOf :y and :y is described in Bundle :b, the triple :x prov:asInBundle :b is also asserted to cite the Bundle in which :y was described.
    ///<see cref="http://www.w3.org/ns/prov#asInBundle"/>
    ///</summary>
    public const string asInBundle = "http://www.w3.org/ns/prov#asInBundle";

    ///<summary>
    ///prov:mentionOf is used to specialize an entity as described in another bundle. It is to be used in conjuction with prov:asInBundle.
    ///
    ///prov:asInBundle is used to cite the Bundle in which the generalization was mentioned.
    ///<see cref="http://www.w3.org/ns/prov#mentionOf"/>
    ///</summary>
    public const string mentionOf = "http://www.w3.org/ns/prov#mentionOf";
}
}