// Attention: This file is generated. Any modifications will eventually be overwritten.
// Date: 06.05.2016 16:02:14

using System;
using System.Collections.Generic;
using System.Text;
using Semiodesk.Trinity;

namespace Artivity.DataModel
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
public class prov : Ontology
{
    public static readonly Uri Namespace = new Uri("http://www.w3.org/ns/prov#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "prov";
    public static string GetPrefix() { return Prefix; }     

    ///<summary>
    ///(, en)
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#comment"/>
    ///</summary>
    public static readonly Property comment = new Property(new Uri("http://www.w3.org/2000/01/rdf-schema#comment"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#isDefinedBy"/>
    ///</summary>
    public static readonly Property isDefinedBy = new Property(new Uri("http://www.w3.org/2000/01/rdf-schema#isDefinedBy"));    

    ///<summary>
    ///(, en)
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#label"/>
    ///</summary>
    public static readonly Property label = new Property(new Uri("http://www.w3.org/2000/01/rdf-schema#label"));    

    ///<summary>
    ///(, en)
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
    ///(ActivityInfluence provides additional descriptions of an Activity's binary influence upon any other kind of resource. Instances of ActivityInfluence use the prov:activity property to cite the influencing Activity., en)
    ///<see cref="http://www.w3.org/ns/prov#ActivityInfluence"/>
    ///</summary>
    public static readonly Class ActivityInfluence = new Class(new Uri("http://www.w3.org/ns/prov#ActivityInfluence"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Agent"/>
    ///</summary>
    public static readonly Class Agent = new Class(new Uri("http://www.w3.org/ns/prov#Agent"));    

    ///<summary>
    ///(AgentInfluence provides additional descriptions of an Agent's binary influence upon any other kind of resource. Instances of AgentInfluence use the prov:agent property to cite the influencing Agent., en)
    ///<see cref="http://www.w3.org/ns/prov#AgentInfluence"/>
    ///</summary>
    public static readonly Class AgentInfluence = new Class(new Uri("http://www.w3.org/ns/prov#AgentInfluence"));    

    ///<summary>
    ///(An instance of prov:Association provides additional descriptions about the binary prov:wasAssociatedWith relation from an prov:Activity to some prov:Agent that had some responsiblity for it. For example, :baking prov:wasAssociatedWith :baker; prov:qualifiedAssociation [ a prov:Association; prov:agent :baker; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#Association"/>
    ///</summary>
    public static readonly Class Association = new Class(new Uri("http://www.w3.org/ns/prov#Association"));    

    ///<summary>
    ///(An instance of prov:Attribution provides additional descriptions about the binary prov:wasAttributedTo relation from an prov:Entity to some prov:Agent that had some responsible for it. For example, :cake prov:wasAttributedTo :baker; prov:qualifiedAttribution [ a prov:Attribution; prov:entity :baker; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#Attribution"/>
    ///</summary>
    public static readonly Class Attribution = new Class(new Uri("http://www.w3.org/ns/prov#Attribution"));    

    ///<summary>
    ///(Note that there are kinds of bundles (e.g. handwritten letters, audio recordings, etc.) that are not expressed in PROV-O, but can be still be described by PROV-O., en)
    ///<see cref="http://www.w3.org/ns/prov#Bundle"/>
    ///</summary>
    public static readonly Class Bundle = new Class(new Uri("http://www.w3.org/ns/prov#Bundle"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Collection"/>
    ///</summary>
    public static readonly Class Collection = new Class(new Uri("http://www.w3.org/ns/prov#Collection"));    

    ///<summary>
    ///(An instance of prov:Communication provides additional descriptions about the binary prov:wasInformedBy relation from an informed prov:Activity to the prov:Activity that informed it. For example, :you_jumping_off_bridge prov:wasInformedBy :everyone_else_jumping_off_bridge; prov:qualifiedCommunication [ a prov:Communication; prov:activity :everyone_else_jumping_off_bridge; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#Communication"/>
    ///</summary>
    public static readonly Class Communication = new Class(new Uri("http://www.w3.org/ns/prov#Communication"));    

    ///<summary>
    ///(An instance of prov:Delegation provides additional descriptions about the binary prov:actedOnBehalfOf relation from a performing prov:Agent to some prov:Agent for whom it was performed. For example, :mixing prov:wasAssociatedWith :toddler . :toddler prov:actedOnBehalfOf :mother; prov:qualifiedDelegation [ a prov:Delegation; prov:entity :mother; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#Delegation"/>
    ///</summary>
    public static readonly Class Delegation = new Class(new Uri("http://www.w3.org/ns/prov#Delegation"));    

    ///<summary>
    ///(An instance of prov:Derivation provides additional descriptions about the binary prov:wasDerivedFrom relation from some derived prov:Entity to another prov:Entity from which it was derived. For example, :chewed_bubble_gum prov:wasDerivedFrom :unwrapped_bubble_gum; prov:qualifiedDerivation [ a prov:Derivation; prov:entity :unwrapped_bubble_gum; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#Derivation"/>
    ///</summary>
    public static readonly Class Derivation = new Class(new Uri("http://www.w3.org/ns/prov#Derivation"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#EmptyCollection"/>
    ///</summary>
    public static readonly Class EmptyCollection = new Class(new Uri("http://www.w3.org/ns/prov#EmptyCollection"));    

    ///<summary>
    ///(An instance of prov:End provides additional descriptions about the binary prov:wasEndedBy relation from some ended prov:Activity to an prov:Entity that ended it. For example, :ball_game prov:wasEndedBy :buzzer; prov:qualifiedEnd [ a prov:End; prov:entity :buzzer; :foo :bar; prov:atTime '2012-03-09T08:05:08-05:00'^^xsd:dateTime ]., en)
    ///<see cref="http://www.w3.org/ns/prov#End"/>
    ///</summary>
    public static readonly Class End = new Class(new Uri("http://www.w3.org/ns/prov#End"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Entity"/>
    ///</summary>
    public static readonly Class Entity = new Class(new Uri("http://www.w3.org/ns/prov#Entity"));    

    ///<summary>
    ///(EntityInfluence provides additional descriptions of an Entity's binary influence upon any other kind of resource. Instances of EntityInfluence use the prov:entity property to cite the influencing Entity., en)
    ///<see cref="http://www.w3.org/ns/prov#EntityInfluence"/>
    ///</summary>
    public static readonly Class EntityInfluence = new Class(new Uri("http://www.w3.org/ns/prov#EntityInfluence"));    

    ///<summary>
    ///(An instance of prov:Generation provides additional descriptions about the binary prov:wasGeneratedBy relation from a generated prov:Entity to the prov:Activity that generated it. For example, :cake prov:wasGeneratedBy :baking; prov:qualifiedGeneration [ a prov:Generation; prov:activity :baking; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#Generation"/>
    ///</summary>
    public static readonly Class Generation = new Class(new Uri("http://www.w3.org/ns/prov#Generation"));    

    ///<summary>
    ///(An instance of prov:Influence provides additional descriptions about the binary prov:wasInfluencedBy relation from some influenced Activity, Entity, or Agent to the influencing Activity, Entity, or Agent. For example, :stomach_ache prov:wasInfluencedBy :spoon; prov:qualifiedInfluence [ a prov:Influence; prov:entity :spoon; :foo :bar ] . Because prov:Influence is a broad relation, the more specific relations (Communication, Delegation, End, etc.) should be used when applicable., en)
    ///<see cref="http://www.w3.org/ns/prov#Influence"/>
    ///</summary>
    public static readonly Class Influence = new Class(new Uri("http://www.w3.org/ns/prov#Influence"));    

    ///<summary>
    ///(An instantaneous event, or event for short, happens in the world and marks a change in the world, in its activities and in its entities. The term 'event' is commonly used in process algebra with a similar meaning. Events represent communications or interactions; they are assumed to be atomic and instantaneous., en)
    ///<see cref="http://www.w3.org/ns/prov#InstantaneousEvent"/>
    ///</summary>
    public static readonly Class InstantaneousEvent = new Class(new Uri("http://www.w3.org/ns/prov#InstantaneousEvent"));    

    ///<summary>
    ///(An instance of prov:Invalidation provides additional descriptions about the binary prov:wasInvalidatedBy relation from an invalidated prov:Entity to the prov:Activity that invalidated it. For example, :uncracked_egg prov:wasInvalidatedBy :baking; prov:qualifiedInvalidation [ a prov:Invalidation; prov:activity :baking; :foo :bar ]., en)
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
    ///(There exist no prescriptive requirement on the nature of plans, their representation, the actions or steps they consist of, or their intended goals. Since plans may evolve over time, it may become necessary to track their provenance, so plans themselves are entities. Representing the plan explicitly in the provenance can be useful for various tasks: for example, to validate the execution as represented in the provenance record, to manage expectation failures, or to provide explanations., en)
    ///<see cref="http://www.w3.org/ns/prov#Plan"/>
    ///</summary>
    public static readonly Class Plan = new Class(new Uri("http://www.w3.org/ns/prov#Plan"));    

    ///<summary>
    ///(An instance of prov:PrimarySource provides additional descriptions about the binary prov:hadPrimarySource relation from some secondary prov:Entity to an earlier, primary prov:Entity. For example, :blog prov:hadPrimarySource :newsArticle; prov:qualifiedPrimarySource [ a prov:PrimarySource; prov:entity :newsArticle; :foo :bar ] ., en)
    ///<see cref="http://www.w3.org/ns/prov#PrimarySource"/>
    ///</summary>
    public static readonly Class PrimarySource = new Class(new Uri("http://www.w3.org/ns/prov#PrimarySource"));    

    ///<summary>
    ///(An instance of prov:Quotation provides additional descriptions about the binary prov:wasQuotedFrom relation from some taken prov:Entity from an earlier, larger prov:Entity. For example, :here_is_looking_at_you_kid prov:wasQuotedFrom :casablanca_script; prov:qualifiedQuotation [ a prov:Quotation; prov:entity :casablanca_script; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#Quotation"/>
    ///</summary>
    public static readonly Class Quotation = new Class(new Uri("http://www.w3.org/ns/prov#Quotation"));    

    ///<summary>
    ///(An instance of prov:Revision provides additional descriptions about the binary prov:wasRevisionOf relation from some newer prov:Entity to an earlier prov:Entity. For example, :draft_2 prov:wasRevisionOf :draft_1; prov:qualifiedRevision [ a prov:Revision; prov:entity :draft_1; :foo :bar ]., en)
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
    ///(An instance of prov:Start provides additional descriptions about the binary prov:wasStartedBy relation from some started prov:Activity to an prov:Entity that started it. For example, :foot_race prov:wasStartedBy :bang; prov:qualifiedStart [ a prov:Start; prov:entity :bang; :foo :bar; prov:atTime '2012-03-09T08:05:08-05:00'^^xsd:dateTime ] ., en)
    ///<see cref="http://www.w3.org/ns/prov#Start"/>
    ///</summary>
    public static readonly Class Start = new Class(new Uri("http://www.w3.org/ns/prov#Start"));    

    ///<summary>
    ///(An instance of prov:Usage provides additional descriptions about the binary prov:used relation from some prov:Activity to an prov:Entity that it used. For example, :keynote prov:used :podium; prov:qualifiedUsage [ a prov:Usage; prov:entity :podium; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#Usage"/>
    ///</summary>
    public static readonly Class Usage = new Class(new Uri("http://www.w3.org/ns/prov#Usage"));    

    ///<summary>
    ///(An object property to express the accountability of an agent towards another agent. The subordinate agent acted on behalf of the responsible agent in an actual activity. , en)
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
    ///(The Location of any resource., en)
    ///<see cref="http://www.w3.org/ns/prov#atLocation"/>
    ///</summary>
    public static readonly Property atLocation = new Property(new Uri("http://www.w3.org/ns/prov#atLocation"));    

    ///<summary>
    ///(The time at which an InstantaneousEvent occurred, in the form of xsd:dateTime., en)
    ///<see cref="http://www.w3.org/ns/prov#atTime"/>
    ///</summary>
    public static readonly Property atTime = new Property(new Uri("http://www.w3.org/ns/prov#atTime"));    

    ///<summary>
    ///(Classify prov-o terms into three categories, including 'starting-point', 'qualifed', and 'extended'. This classification is used by the prov-o html document to gently introduce prov-o terms to its users. , en)
    ///<see cref="http://www.w3.org/ns/prov#category"/>
    ///</summary>
    public static readonly Property category = new Property(new Uri("http://www.w3.org/ns/prov#category"));    

    ///<summary>
    ///(Classify prov-o terms into six components according to prov-dm, including 'agents-responsibility', 'alternate', 'annotations', 'collections', 'derivations', and 'entities-activities'. This classification is used so that readers of prov-o specification can find its correspondence with the prov-dm specification., en)
    ///<see cref="http://www.w3.org/ns/prov#component"/>
    ///</summary>
    public static readonly Property component = new Property(new Uri("http://www.w3.org/ns/prov#component"));    

    ///<summary>
    ///(A reference to the principal section of the PROV-CONSTRAINTS document that describes this concept., en)
    ///<see cref="http://www.w3.org/ns/prov#constraints"/>
    ///</summary>
    public static readonly Property constraints = new Property(new Uri("http://www.w3.org/ns/prov#constraints"));    

    ///<summary>
    ///(A definition quoted from PROV-DM or PROV-CONSTRAINTS that describes the concept expressed with this OWL term., en)
    ///<see cref="http://www.w3.org/ns/prov#definition"/>
    ///</summary>
    public static readonly Property definition = new Property(new Uri("http://www.w3.org/ns/prov#definition"));    

    ///<summary>
    ///(A reference to the principal section of the PROV-DM document that describes this concept., en)
    ///<see cref="http://www.w3.org/ns/prov#dm"/>
    ///</summary>
    public static readonly Property dm = new Property(new Uri("http://www.w3.org/ns/prov#dm"));    

    ///<summary>
    ///(A note by the OWL development team about how this term expresses the PROV-DM concept, or how it should be used in context of semantic web or linked data., en)
    ///<see cref="http://www.w3.org/ns/prov#editorialNote"/>
    ///</summary>
    public static readonly Property editorialNote = new Property(new Uri("http://www.w3.org/ns/prov#editorialNote"));    

    ///<summary>
    ///(When the prov-o term does not have a definition drawn from prov-dm, and the prov-o editor provides one., en)
    ///<see cref="http://www.w3.org/ns/prov#editorsDefinition"/>
    ///</summary>
    public static readonly Property editorsDefinition = new Property(new Uri("http://www.w3.org/ns/prov#editorsDefinition"));    

    ///<summary>
    ///(The time at which an activity ended. See also prov:startedAtTime., en)
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
    ///(The time at which an entity was completely created and is available for use., en)
    ///<see cref="http://www.w3.org/ns/prov#generatedAtTime"/>
    ///</summary>
    public static readonly Property generatedAtTime = new Property(new Uri("http://www.w3.org/ns/prov#generatedAtTime"));    

    ///<summary>
    ///(The _optional_ Activity of an Influence, which used, generated, invalidated, or was the responsibility of some Entity. This property is _not_ used by ActivityInfluence (use prov:activity instead)., en)
    ///<see cref="http://www.w3.org/ns/prov#hadActivity"/>
    ///</summary>
    public static readonly Property hadActivity = new Property(new Uri("http://www.w3.org/ns/prov#hadActivity"));    

    ///<summary>
    ///(The _optional_ Generation involved in an Entity's Derivation., en)
    ///<see cref="http://www.w3.org/ns/prov#hadGeneration"/>
    ///</summary>
    public static readonly Property hadGeneration = new Property(new Uri("http://www.w3.org/ns/prov#hadGeneration"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadMember"/>
    ///</summary>
    public static readonly Property hadMember = new Property(new Uri("http://www.w3.org/ns/prov#hadMember"));    

    ///<summary>
    ///(The _optional_ Plan adopted by an Agent in Association with some Activity. Plan specifications are out of the scope of this specification., en)
    ///<see cref="http://www.w3.org/ns/prov#hadPlan"/>
    ///</summary>
    public static readonly Property hadPlan = new Property(new Uri("http://www.w3.org/ns/prov#hadPlan"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadPrimarySource"/>
    ///</summary>
    public static readonly Property hadPrimarySource = new Property(new Uri("http://www.w3.org/ns/prov#hadPrimarySource"));    

    ///<summary>
    ///(The _optional_ Role that an Entity assumed in the context of an Activity. For example, :baking prov:used :spoon; prov:qualified [ a prov:Usage; prov:entity :spoon; prov:hadRole roles:mixing_implement ]., en)
    ///<see cref="http://www.w3.org/ns/prov#hadRole"/>
    ///</summary>
    public static readonly Property hadRole = new Property(new Uri("http://www.w3.org/ns/prov#hadRole"));    

    ///<summary>
    ///(The _optional_ Usage involved in an Entity's Derivation., en)
    ///<see cref="http://www.w3.org/ns/prov#hadUsage"/>
    ///</summary>
    public static readonly Property hadUsage = new Property(new Uri("http://www.w3.org/ns/prov#hadUsage"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#influenced"/>
    ///</summary>
    public static readonly Property influenced = new Property(new Uri("http://www.w3.org/ns/prov#influenced"));    

    ///<summary>
    ///(Subproperties of prov:influencer are used to cite the object of an unqualified PROV-O triple whose predicate is a subproperty of prov:wasInfluencedBy (e.g. prov:used, prov:wasGeneratedBy). prov:influencer is used much like rdf:object is used., en)
    ///<see cref="http://www.w3.org/ns/prov#influencer"/>
    ///</summary>
    public static readonly Property influencer = new Property(new Uri("http://www.w3.org/ns/prov#influencer"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#invalidated"/>
    ///</summary>
    public static readonly Property invalidated = new Property(new Uri("http://www.w3.org/ns/prov#invalidated"));    

    ///<summary>
    ///(The time at which an entity was invalidated (i.e., no longer usable)., en)
    ///<see cref="http://www.w3.org/ns/prov#invalidatedAtTime"/>
    ///</summary>
    public static readonly Property invalidatedAtTime = new Property(new Uri("http://www.w3.org/ns/prov#invalidatedAtTime"));    

    ///<summary>
    ///(PROV-O does not define all property inverses. The directionalities defined in PROV-O should be given preference over those not defined. However, if users wish to name the inverse of a PROV-O property, the local name given by prov:inverse should be used., en)
    ///<see cref="http://www.w3.org/ns/prov#inverse"/>
    ///</summary>
    public static readonly Property inverse = new Property(new Uri("http://www.w3.org/ns/prov#inverse"));    

    ///<summary>
    ///(A reference to the principal section of the PROV-DM document that describes this concept., en)
    ///<see cref="http://www.w3.org/ns/prov#n"/>
    ///</summary>
    public static readonly Property n = new Property(new Uri("http://www.w3.org/ns/prov#n"));    

    ///<summary>
    ///(The position that this OWL term should be listed within documentation. The scope of the documentation (e.g., among all terms, among terms within a prov:category, among properties applying to a particular class, etc.) is unspecified., en)
    ///<see cref="http://www.w3.org/ns/prov#order"/>
    ///</summary>
    public static readonly Property order = new Property(new Uri("http://www.w3.org/ns/prov#order"));    

    ///<summary>
    ///(If this Activity prov:wasAssociatedWith Agent :ag, then it can qualify the Association using prov:qualifiedAssociation [ a prov:Association;  prov:agent :ag; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedAssociation"/>
    ///</summary>
    public static readonly Property qualifiedAssociation = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedAssociation"));    

    ///<summary>
    ///(If this Entity prov:wasAttributedTo Agent :ag, then it can qualify how it was influenced using prov:qualifiedAttribution [ a prov:Attribution;  prov:agent :ag; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedAttribution"/>
    ///</summary>
    public static readonly Property qualifiedAttribution = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedAttribution"));    

    ///<summary>
    ///(If this Activity prov:wasInformedBy Activity :a, then it can qualify how it was influenced using prov:qualifiedCommunication [ a prov:Communication;  prov:activity :a; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedCommunication"/>
    ///</summary>
    public static readonly Property qualifiedCommunication = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedCommunication"));    

    ///<summary>
    ///(If this Agent prov:actedOnBehalfOf Agent :ag, then it can qualify how with prov:qualifiedResponsibility [ a prov:Responsibility;  prov:agent :ag; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedDelegation"/>
    ///</summary>
    public static readonly Property qualifiedDelegation = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedDelegation"));    

    ///<summary>
    ///(If this Entity prov:wasDerivedFrom Entity :e, then it can qualify how it was derived using prov:qualifiedDerivation [ a prov:Derivation;  prov:entity :e; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedDerivation"/>
    ///</summary>
    public static readonly Property qualifiedDerivation = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedDerivation"));    

    ///<summary>
    ///(If this Activity prov:wasEndedBy Entity :e1, then it can qualify how it was ended using prov:qualifiedEnd [ a prov:End;  prov:entity :e1; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedEnd"/>
    ///</summary>
    public static readonly Property qualifiedEnd = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedEnd"));    

    ///<summary>
    ///(This annotation property links a subproperty of prov:wasInfluencedBy with the subclass of prov:Influence and the qualifying property that are used to qualify it. 
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
    ///Note how the value of the unqualified influence (prov:wasGeneratedBy :activity1) is mirrored as the value of the prov:activity (or prov:entity, or prov:agent) property on the influence class., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedForm"/>
    ///</summary>
    public static readonly Property qualifiedForm = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedForm"));    

    ///<summary>
    ///(If this Activity prov:generated Entity :e, then it can qualify how it performed the Generation using prov:qualifiedGeneration [ a prov:Generation;  prov:entity :e; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedGeneration"/>
    ///</summary>
    public static readonly Property qualifiedGeneration = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedGeneration"));    

    ///<summary>
    ///(Because prov:qualifiedInfluence is a broad relation, the more specific relations (qualifiedCommunication, qualifiedDelegation, qualifiedEnd, etc.) should be used when applicable., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedInfluence"/>
    ///</summary>
    public static readonly Property qualifiedInfluence = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedInfluence"));    

    ///<summary>
    ///(If this Entity prov:wasInvalidatedBy Activity :a, then it can qualify how it was invalidated using prov:qualifiedInvalidation [ a prov:Invalidation;  prov:activity :a; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedInvalidation"/>
    ///</summary>
    public static readonly Property qualifiedInvalidation = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedInvalidation"));    

    ///<summary>
    ///(If this Entity prov:hadPrimarySource Entity :e, then it can qualify how using prov:qualifiedPrimarySource [ a prov:PrimarySource; prov:entity :e; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedPrimarySource"/>
    ///</summary>
    public static readonly Property qualifiedPrimarySource = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedPrimarySource"));    

    ///<summary>
    ///(If this Entity prov:wasQuotedFrom Entity :e, then it can qualify how using prov:qualifiedQuotation [ a prov:Quotation;  prov:entity :e; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedQuotation"/>
    ///</summary>
    public static readonly Property qualifiedQuotation = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedQuotation"));    

    ///<summary>
    ///(If this Entity prov:wasRevisionOf Entity :e, then it can qualify how it was revised using prov:qualifiedRevision [ a prov:Revision;  prov:entity :e; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedRevision"/>
    ///</summary>
    public static readonly Property qualifiedRevision = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedRevision"));    

    ///<summary>
    ///(If this Activity prov:wasStartedBy Entity :e1, then it can qualify how it was started using prov:qualifiedStart [ a prov:Start;  prov:entity :e1; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedStart"/>
    ///</summary>
    public static readonly Property qualifiedStart = new Property(new Uri("http://www.w3.org/ns/prov#qualifiedStart"));    

    ///<summary>
    ///(If this Activity prov:used Entity :e, then it can qualify how it used it using prov:qualifiedUsage [ a prov:Usage; prov:entity :e; :foo :bar ]., en)
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
    ///(The time at which an activity started. See also prov:endedAtTime., en)
    ///<see cref="http://www.w3.org/ns/prov#startedAtTime"/>
    ///</summary>
    public static readonly Property startedAtTime = new Property(new Uri("http://www.w3.org/ns/prov#startedAtTime"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#todo"/>
    ///</summary>
    public static readonly Property todo = new Property(new Uri("http://www.w3.org/ns/prov#todo"));    

    ///<summary>
    ///(Classes and properties used to qualify relationships are annotated with prov:unqualifiedForm to indicate the property used to assert an unqualified provenance relation., en)
    ///<see cref="http://www.w3.org/ns/prov#unqualifiedForm"/>
    ///</summary>
    public static readonly Property unqualifiedForm = new Property(new Uri("http://www.w3.org/ns/prov#unqualifiedForm"));    

    ///<summary>
    ///(A prov:Entity that was used by this prov:Activity. For example, :baking prov:used :spoon, :egg, :oven ., en)
    ///<see cref="http://www.w3.org/ns/prov#used"/>
    ///</summary>
    public static readonly Property used = new Property(new Uri("http://www.w3.org/ns/prov#used"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#value"/>
    ///</summary>
    public static readonly Property value = new Property(new Uri("http://www.w3.org/ns/prov#value"));    

    ///<summary>
    ///(An prov:Agent that had some (unspecified) responsibility for the occurrence of this prov:Activity., en)
    ///<see cref="http://www.w3.org/ns/prov#wasAssociatedWith"/>
    ///</summary>
    public static readonly Property wasAssociatedWith = new Property(new Uri("http://www.w3.org/ns/prov#wasAssociatedWith"));    

    ///<summary>
    ///(Attribution is the ascribing of an entity to an agent., en)
    ///<see cref="http://www.w3.org/ns/prov#wasAttributedTo"/>
    ///</summary>
    public static readonly Property wasAttributedTo = new Property(new Uri("http://www.w3.org/ns/prov#wasAttributedTo"));    

    ///<summary>
    ///(The more specific subproperties of prov:wasDerivedFrom (i.e., prov:wasQuotedFrom, prov:wasRevisionOf, prov:hadPrimarySource) should be used when applicable., en)
    ///<see cref="http://www.w3.org/ns/prov#wasDerivedFrom"/>
    ///</summary>
    public static readonly Property wasDerivedFrom = new Property(new Uri("http://www.w3.org/ns/prov#wasDerivedFrom"));    

    ///<summary>
    ///(End is when an activity is deemed to have ended. An end may refer to an entity, known as trigger, that terminated the activity., en)
    ///<see cref="http://www.w3.org/ns/prov#wasEndedBy"/>
    ///</summary>
    public static readonly Property wasEndedBy = new Property(new Uri("http://www.w3.org/ns/prov#wasEndedBy"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasGeneratedBy"/>
    ///</summary>
    public static readonly Property wasGeneratedBy = new Property(new Uri("http://www.w3.org/ns/prov#wasGeneratedBy"));    

    ///<summary>
    ///(Because prov:wasInfluencedBy is a broad relation, its more specific subproperties (e.g. prov:wasInformedBy, prov:actedOnBehalfOf, prov:wasEndedBy, etc.) should be used when applicable., en)
    ///<see cref="http://www.w3.org/ns/prov#wasInfluencedBy"/>
    ///</summary>
    public static readonly Property wasInfluencedBy = new Property(new Uri("http://www.w3.org/ns/prov#wasInfluencedBy"));    

    ///<summary>
    ///(An activity a2 is dependent on or informed by another activity a1, by way of some unspecified entity that is generated by a1 and used by a2., en)
    ///<see cref="http://www.w3.org/ns/prov#wasInformedBy"/>
    ///</summary>
    public static readonly Property wasInformedBy = new Property(new Uri("http://www.w3.org/ns/prov#wasInformedBy"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasInvalidatedBy"/>
    ///</summary>
    public static readonly Property wasInvalidatedBy = new Property(new Uri("http://www.w3.org/ns/prov#wasInvalidatedBy"));    

    ///<summary>
    ///(An entity is derived from an original entity by copying, or 'quoting', some or all of it., en)
    ///<see cref="http://www.w3.org/ns/prov#wasQuotedFrom"/>
    ///</summary>
    public static readonly Property wasQuotedFrom = new Property(new Uri("http://www.w3.org/ns/prov#wasQuotedFrom"));    

    ///<summary>
    ///(A revision is a derivation that revises an entity into a revised version., en)
    ///<see cref="http://www.w3.org/ns/prov#wasRevisionOf"/>
    ///</summary>
    public static readonly Property wasRevisionOf = new Property(new Uri("http://www.w3.org/ns/prov#wasRevisionOf"));    

    ///<summary>
    ///(Start is when an activity is deemed to have started. A start may refer to an entity, known as trigger, that initiated the activity., en)
    ///<see cref="http://www.w3.org/ns/prov#wasStartedBy"/>
    ///</summary>
    public static readonly Property wasStartedBy = new Property(new Uri("http://www.w3.org/ns/prov#wasStartedBy"));    

    ///<summary>
    ///(This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page). 
    ///
    ///If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/Archives/Public/public-prov-comments/). All feedback is welcome., en)
    ///<see cref="http://www.w3.org/ns/prov-o#"/>
    ///</summary>
    public static readonly Resource prov_o = new Resource(new Uri("http://www.w3.org/ns/prov-o#"));    

    ///<summary>
    ///
    ///<see cref="file:///D:/Projects/2016/Artivity/Artivity.DataModel/Ontologies/prov.ttl#"/>
    ///</summary>
    public static readonly Resource prov_ttl = new Resource(new Uri("file:///D:/Projects/2016/Artivity/Artivity.DataModel/Ontologies/prov.ttl#"));    

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
    ///(Indicates anchor URI for a potentially dynamic resource instance., en)
    ///<see cref="http://www.w3.org/ns/prov#has_anchor"/>
    ///</summary>
    public static readonly Property has_anchor = new Property(new Uri("http://www.w3.org/ns/prov#has_anchor"));    

    ///<summary>
    ///(Indicates a provenance-URI for a resource; the resource identified by this property presents a provenance record about its subject or anchor resource., en)
    ///<see cref="http://www.w3.org/ns/prov#has_provenance"/>
    ///</summary>
    public static readonly Property has_provenance = new Property(new Uri("http://www.w3.org/ns/prov#has_provenance"));    

    ///<summary>
    ///(Indicates a provenance query service that can access provenance related to its subject or anchor resource., en)
    ///<see cref="http://www.w3.org/ns/prov#has_query_service"/>
    ///</summary>
    public static readonly Property has_query_service = new Property(new Uri("http://www.w3.org/ns/prov#has_query_service"));    

    ///<summary>
    ///(relates a generic provenance query service resource (type prov:ServiceDescription) to a specific query service description (e.g. a prov:DirectQueryService or a sd:Service)., en)
    ///<see cref="http://www.w3.org/ns/prov#describesService"/>
    ///</summary>
    public static readonly Property describesService = new Property(new Uri("http://www.w3.org/ns/prov#describesService"));    

    ///<summary>
    ///(Relates a provenance service to a URI template string for constructing provenance-URIs., en)
    ///<see cref="http://www.w3.org/ns/prov#provenanceUriTemplate"/>
    ///</summary>
    public static readonly Property provenanceUriTemplate = new Property(new Uri("http://www.w3.org/ns/prov#provenanceUriTemplate"));    

    ///<summary>
    ///(Relates a resource to a provenance pingback service that may receive additional provenance links about the resource., en)
    ///<see cref="http://www.w3.org/ns/prov#pingback"/>
    ///</summary>
    public static readonly Property pingback = new Property(new Uri("http://www.w3.org/ns/prov#pingback"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2002/07/owl#topObjectProperty"/>
    ///</summary>
    public static readonly Property topObjectProperty = new Property(new Uri("http://www.w3.org/2002/07/owl#topObjectProperty"));    

    ///<summary>
    ///(This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page). 
    ///
    ///If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/Archives/Public/public-prov-comments/). All feedback is welcome., en)
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
    ///(This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page). 
    ///
    ///If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/Archives/Public/public-prov-comments/). All feedback is welcome., en)
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
    ///(This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page). If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/Archives/Public/public-prov-comments/
    ///). All feedback is welcome., en)
    ///<see cref="http://www.w3.org/ns/prov-links#"/>
    ///</summary>
    public static readonly Resource prov_links = new Resource(new Uri("http://www.w3.org/ns/prov-links#"));    

    ///<summary>
    ///(prov:asInBundle is used to specify which bundle the general entity of a prov:mentionOf property is described.
    ///
    ///When :x prov:mentionOf :y and :y is described in Bundle :b, the triple :x prov:asInBundle :b is also asserted to cite the Bundle in which :y was described., en)
    ///<see cref="http://www.w3.org/ns/prov#asInBundle"/>
    ///</summary>
    public static readonly Property asInBundle = new Property(new Uri("http://www.w3.org/ns/prov#asInBundle"));    

    ///<summary>
    ///(prov:mentionOf is used to specialize an entity as described in another bundle. It is to be used in conjuction with prov:asInBundle.
    ///
    ///prov:asInBundle is used to cite the Bundle in which the generalization was mentioned., en)
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
    ///(, en)
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#comment"/>
    ///</summary>
    public const string comment = "http://www.w3.org/2000/01/rdf-schema#comment";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#isDefinedBy"/>
    ///</summary>
    public const string isDefinedBy = "http://www.w3.org/2000/01/rdf-schema#isDefinedBy";

    ///<summary>
    ///(, en)
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#label"/>
    ///</summary>
    public const string label = "http://www.w3.org/2000/01/rdf-schema#label";

    ///<summary>
    ///(, en)
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
    ///(ActivityInfluence provides additional descriptions of an Activity's binary influence upon any other kind of resource. Instances of ActivityInfluence use the prov:activity property to cite the influencing Activity., en)
    ///<see cref="http://www.w3.org/ns/prov#ActivityInfluence"/>
    ///</summary>
    public const string ActivityInfluence = "http://www.w3.org/ns/prov#ActivityInfluence";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Agent"/>
    ///</summary>
    public const string Agent = "http://www.w3.org/ns/prov#Agent";

    ///<summary>
    ///(AgentInfluence provides additional descriptions of an Agent's binary influence upon any other kind of resource. Instances of AgentInfluence use the prov:agent property to cite the influencing Agent., en)
    ///<see cref="http://www.w3.org/ns/prov#AgentInfluence"/>
    ///</summary>
    public const string AgentInfluence = "http://www.w3.org/ns/prov#AgentInfluence";

    ///<summary>
    ///(An instance of prov:Association provides additional descriptions about the binary prov:wasAssociatedWith relation from an prov:Activity to some prov:Agent that had some responsiblity for it. For example, :baking prov:wasAssociatedWith :baker; prov:qualifiedAssociation [ a prov:Association; prov:agent :baker; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#Association"/>
    ///</summary>
    public const string Association = "http://www.w3.org/ns/prov#Association";

    ///<summary>
    ///(An instance of prov:Attribution provides additional descriptions about the binary prov:wasAttributedTo relation from an prov:Entity to some prov:Agent that had some responsible for it. For example, :cake prov:wasAttributedTo :baker; prov:qualifiedAttribution [ a prov:Attribution; prov:entity :baker; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#Attribution"/>
    ///</summary>
    public const string Attribution = "http://www.w3.org/ns/prov#Attribution";

    ///<summary>
    ///(Note that there are kinds of bundles (e.g. handwritten letters, audio recordings, etc.) that are not expressed in PROV-O, but can be still be described by PROV-O., en)
    ///<see cref="http://www.w3.org/ns/prov#Bundle"/>
    ///</summary>
    public const string Bundle = "http://www.w3.org/ns/prov#Bundle";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Collection"/>
    ///</summary>
    public const string Collection = "http://www.w3.org/ns/prov#Collection";

    ///<summary>
    ///(An instance of prov:Communication provides additional descriptions about the binary prov:wasInformedBy relation from an informed prov:Activity to the prov:Activity that informed it. For example, :you_jumping_off_bridge prov:wasInformedBy :everyone_else_jumping_off_bridge; prov:qualifiedCommunication [ a prov:Communication; prov:activity :everyone_else_jumping_off_bridge; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#Communication"/>
    ///</summary>
    public const string Communication = "http://www.w3.org/ns/prov#Communication";

    ///<summary>
    ///(An instance of prov:Delegation provides additional descriptions about the binary prov:actedOnBehalfOf relation from a performing prov:Agent to some prov:Agent for whom it was performed. For example, :mixing prov:wasAssociatedWith :toddler . :toddler prov:actedOnBehalfOf :mother; prov:qualifiedDelegation [ a prov:Delegation; prov:entity :mother; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#Delegation"/>
    ///</summary>
    public const string Delegation = "http://www.w3.org/ns/prov#Delegation";

    ///<summary>
    ///(An instance of prov:Derivation provides additional descriptions about the binary prov:wasDerivedFrom relation from some derived prov:Entity to another prov:Entity from which it was derived. For example, :chewed_bubble_gum prov:wasDerivedFrom :unwrapped_bubble_gum; prov:qualifiedDerivation [ a prov:Derivation; prov:entity :unwrapped_bubble_gum; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#Derivation"/>
    ///</summary>
    public const string Derivation = "http://www.w3.org/ns/prov#Derivation";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#EmptyCollection"/>
    ///</summary>
    public const string EmptyCollection = "http://www.w3.org/ns/prov#EmptyCollection";

    ///<summary>
    ///(An instance of prov:End provides additional descriptions about the binary prov:wasEndedBy relation from some ended prov:Activity to an prov:Entity that ended it. For example, :ball_game prov:wasEndedBy :buzzer; prov:qualifiedEnd [ a prov:End; prov:entity :buzzer; :foo :bar; prov:atTime '2012-03-09T08:05:08-05:00'^^xsd:dateTime ]., en)
    ///<see cref="http://www.w3.org/ns/prov#End"/>
    ///</summary>
    public const string End = "http://www.w3.org/ns/prov#End";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#Entity"/>
    ///</summary>
    public const string Entity = "http://www.w3.org/ns/prov#Entity";

    ///<summary>
    ///(EntityInfluence provides additional descriptions of an Entity's binary influence upon any other kind of resource. Instances of EntityInfluence use the prov:entity property to cite the influencing Entity., en)
    ///<see cref="http://www.w3.org/ns/prov#EntityInfluence"/>
    ///</summary>
    public const string EntityInfluence = "http://www.w3.org/ns/prov#EntityInfluence";

    ///<summary>
    ///(An instance of prov:Generation provides additional descriptions about the binary prov:wasGeneratedBy relation from a generated prov:Entity to the prov:Activity that generated it. For example, :cake prov:wasGeneratedBy :baking; prov:qualifiedGeneration [ a prov:Generation; prov:activity :baking; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#Generation"/>
    ///</summary>
    public const string Generation = "http://www.w3.org/ns/prov#Generation";

    ///<summary>
    ///(An instance of prov:Influence provides additional descriptions about the binary prov:wasInfluencedBy relation from some influenced Activity, Entity, or Agent to the influencing Activity, Entity, or Agent. For example, :stomach_ache prov:wasInfluencedBy :spoon; prov:qualifiedInfluence [ a prov:Influence; prov:entity :spoon; :foo :bar ] . Because prov:Influence is a broad relation, the more specific relations (Communication, Delegation, End, etc.) should be used when applicable., en)
    ///<see cref="http://www.w3.org/ns/prov#Influence"/>
    ///</summary>
    public const string Influence = "http://www.w3.org/ns/prov#Influence";

    ///<summary>
    ///(An instantaneous event, or event for short, happens in the world and marks a change in the world, in its activities and in its entities. The term 'event' is commonly used in process algebra with a similar meaning. Events represent communications or interactions; they are assumed to be atomic and instantaneous., en)
    ///<see cref="http://www.w3.org/ns/prov#InstantaneousEvent"/>
    ///</summary>
    public const string InstantaneousEvent = "http://www.w3.org/ns/prov#InstantaneousEvent";

    ///<summary>
    ///(An instance of prov:Invalidation provides additional descriptions about the binary prov:wasInvalidatedBy relation from an invalidated prov:Entity to the prov:Activity that invalidated it. For example, :uncracked_egg prov:wasInvalidatedBy :baking; prov:qualifiedInvalidation [ a prov:Invalidation; prov:activity :baking; :foo :bar ]., en)
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
    ///(There exist no prescriptive requirement on the nature of plans, their representation, the actions or steps they consist of, or their intended goals. Since plans may evolve over time, it may become necessary to track their provenance, so plans themselves are entities. Representing the plan explicitly in the provenance can be useful for various tasks: for example, to validate the execution as represented in the provenance record, to manage expectation failures, or to provide explanations., en)
    ///<see cref="http://www.w3.org/ns/prov#Plan"/>
    ///</summary>
    public const string Plan = "http://www.w3.org/ns/prov#Plan";

    ///<summary>
    ///(An instance of prov:PrimarySource provides additional descriptions about the binary prov:hadPrimarySource relation from some secondary prov:Entity to an earlier, primary prov:Entity. For example, :blog prov:hadPrimarySource :newsArticle; prov:qualifiedPrimarySource [ a prov:PrimarySource; prov:entity :newsArticle; :foo :bar ] ., en)
    ///<see cref="http://www.w3.org/ns/prov#PrimarySource"/>
    ///</summary>
    public const string PrimarySource = "http://www.w3.org/ns/prov#PrimarySource";

    ///<summary>
    ///(An instance of prov:Quotation provides additional descriptions about the binary prov:wasQuotedFrom relation from some taken prov:Entity from an earlier, larger prov:Entity. For example, :here_is_looking_at_you_kid prov:wasQuotedFrom :casablanca_script; prov:qualifiedQuotation [ a prov:Quotation; prov:entity :casablanca_script; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#Quotation"/>
    ///</summary>
    public const string Quotation = "http://www.w3.org/ns/prov#Quotation";

    ///<summary>
    ///(An instance of prov:Revision provides additional descriptions about the binary prov:wasRevisionOf relation from some newer prov:Entity to an earlier prov:Entity. For example, :draft_2 prov:wasRevisionOf :draft_1; prov:qualifiedRevision [ a prov:Revision; prov:entity :draft_1; :foo :bar ]., en)
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
    ///(An instance of prov:Start provides additional descriptions about the binary prov:wasStartedBy relation from some started prov:Activity to an prov:Entity that started it. For example, :foot_race prov:wasStartedBy :bang; prov:qualifiedStart [ a prov:Start; prov:entity :bang; :foo :bar; prov:atTime '2012-03-09T08:05:08-05:00'^^xsd:dateTime ] ., en)
    ///<see cref="http://www.w3.org/ns/prov#Start"/>
    ///</summary>
    public const string Start = "http://www.w3.org/ns/prov#Start";

    ///<summary>
    ///(An instance of prov:Usage provides additional descriptions about the binary prov:used relation from some prov:Activity to an prov:Entity that it used. For example, :keynote prov:used :podium; prov:qualifiedUsage [ a prov:Usage; prov:entity :podium; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#Usage"/>
    ///</summary>
    public const string Usage = "http://www.w3.org/ns/prov#Usage";

    ///<summary>
    ///(An object property to express the accountability of an agent towards another agent. The subordinate agent acted on behalf of the responsible agent in an actual activity. , en)
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
    ///(The Location of any resource., en)
    ///<see cref="http://www.w3.org/ns/prov#atLocation"/>
    ///</summary>
    public const string atLocation = "http://www.w3.org/ns/prov#atLocation";

    ///<summary>
    ///(The time at which an InstantaneousEvent occurred, in the form of xsd:dateTime., en)
    ///<see cref="http://www.w3.org/ns/prov#atTime"/>
    ///</summary>
    public const string atTime = "http://www.w3.org/ns/prov#atTime";

    ///<summary>
    ///(Classify prov-o terms into three categories, including 'starting-point', 'qualifed', and 'extended'. This classification is used by the prov-o html document to gently introduce prov-o terms to its users. , en)
    ///<see cref="http://www.w3.org/ns/prov#category"/>
    ///</summary>
    public const string category = "http://www.w3.org/ns/prov#category";

    ///<summary>
    ///(Classify prov-o terms into six components according to prov-dm, including 'agents-responsibility', 'alternate', 'annotations', 'collections', 'derivations', and 'entities-activities'. This classification is used so that readers of prov-o specification can find its correspondence with the prov-dm specification., en)
    ///<see cref="http://www.w3.org/ns/prov#component"/>
    ///</summary>
    public const string component = "http://www.w3.org/ns/prov#component";

    ///<summary>
    ///(A reference to the principal section of the PROV-CONSTRAINTS document that describes this concept., en)
    ///<see cref="http://www.w3.org/ns/prov#constraints"/>
    ///</summary>
    public const string constraints = "http://www.w3.org/ns/prov#constraints";

    ///<summary>
    ///(A definition quoted from PROV-DM or PROV-CONSTRAINTS that describes the concept expressed with this OWL term., en)
    ///<see cref="http://www.w3.org/ns/prov#definition"/>
    ///</summary>
    public const string definition = "http://www.w3.org/ns/prov#definition";

    ///<summary>
    ///(A reference to the principal section of the PROV-DM document that describes this concept., en)
    ///<see cref="http://www.w3.org/ns/prov#dm"/>
    ///</summary>
    public const string dm = "http://www.w3.org/ns/prov#dm";

    ///<summary>
    ///(A note by the OWL development team about how this term expresses the PROV-DM concept, or how it should be used in context of semantic web or linked data., en)
    ///<see cref="http://www.w3.org/ns/prov#editorialNote"/>
    ///</summary>
    public const string editorialNote = "http://www.w3.org/ns/prov#editorialNote";

    ///<summary>
    ///(When the prov-o term does not have a definition drawn from prov-dm, and the prov-o editor provides one., en)
    ///<see cref="http://www.w3.org/ns/prov#editorsDefinition"/>
    ///</summary>
    public const string editorsDefinition = "http://www.w3.org/ns/prov#editorsDefinition";

    ///<summary>
    ///(The time at which an activity ended. See also prov:startedAtTime., en)
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
    ///(The time at which an entity was completely created and is available for use., en)
    ///<see cref="http://www.w3.org/ns/prov#generatedAtTime"/>
    ///</summary>
    public const string generatedAtTime = "http://www.w3.org/ns/prov#generatedAtTime";

    ///<summary>
    ///(The _optional_ Activity of an Influence, which used, generated, invalidated, or was the responsibility of some Entity. This property is _not_ used by ActivityInfluence (use prov:activity instead)., en)
    ///<see cref="http://www.w3.org/ns/prov#hadActivity"/>
    ///</summary>
    public const string hadActivity = "http://www.w3.org/ns/prov#hadActivity";

    ///<summary>
    ///(The _optional_ Generation involved in an Entity's Derivation., en)
    ///<see cref="http://www.w3.org/ns/prov#hadGeneration"/>
    ///</summary>
    public const string hadGeneration = "http://www.w3.org/ns/prov#hadGeneration";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadMember"/>
    ///</summary>
    public const string hadMember = "http://www.w3.org/ns/prov#hadMember";

    ///<summary>
    ///(The _optional_ Plan adopted by an Agent in Association with some Activity. Plan specifications are out of the scope of this specification., en)
    ///<see cref="http://www.w3.org/ns/prov#hadPlan"/>
    ///</summary>
    public const string hadPlan = "http://www.w3.org/ns/prov#hadPlan";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#hadPrimarySource"/>
    ///</summary>
    public const string hadPrimarySource = "http://www.w3.org/ns/prov#hadPrimarySource";

    ///<summary>
    ///(The _optional_ Role that an Entity assumed in the context of an Activity. For example, :baking prov:used :spoon; prov:qualified [ a prov:Usage; prov:entity :spoon; prov:hadRole roles:mixing_implement ]., en)
    ///<see cref="http://www.w3.org/ns/prov#hadRole"/>
    ///</summary>
    public const string hadRole = "http://www.w3.org/ns/prov#hadRole";

    ///<summary>
    ///(The _optional_ Usage involved in an Entity's Derivation., en)
    ///<see cref="http://www.w3.org/ns/prov#hadUsage"/>
    ///</summary>
    public const string hadUsage = "http://www.w3.org/ns/prov#hadUsage";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#influenced"/>
    ///</summary>
    public const string influenced = "http://www.w3.org/ns/prov#influenced";

    ///<summary>
    ///(Subproperties of prov:influencer are used to cite the object of an unqualified PROV-O triple whose predicate is a subproperty of prov:wasInfluencedBy (e.g. prov:used, prov:wasGeneratedBy). prov:influencer is used much like rdf:object is used., en)
    ///<see cref="http://www.w3.org/ns/prov#influencer"/>
    ///</summary>
    public const string influencer = "http://www.w3.org/ns/prov#influencer";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#invalidated"/>
    ///</summary>
    public const string invalidated = "http://www.w3.org/ns/prov#invalidated";

    ///<summary>
    ///(The time at which an entity was invalidated (i.e., no longer usable)., en)
    ///<see cref="http://www.w3.org/ns/prov#invalidatedAtTime"/>
    ///</summary>
    public const string invalidatedAtTime = "http://www.w3.org/ns/prov#invalidatedAtTime";

    ///<summary>
    ///(PROV-O does not define all property inverses. The directionalities defined in PROV-O should be given preference over those not defined. However, if users wish to name the inverse of a PROV-O property, the local name given by prov:inverse should be used., en)
    ///<see cref="http://www.w3.org/ns/prov#inverse"/>
    ///</summary>
    public const string inverse = "http://www.w3.org/ns/prov#inverse";

    ///<summary>
    ///(A reference to the principal section of the PROV-DM document that describes this concept., en)
    ///<see cref="http://www.w3.org/ns/prov#n"/>
    ///</summary>
    public const string n = "http://www.w3.org/ns/prov#n";

    ///<summary>
    ///(The position that this OWL term should be listed within documentation. The scope of the documentation (e.g., among all terms, among terms within a prov:category, among properties applying to a particular class, etc.) is unspecified., en)
    ///<see cref="http://www.w3.org/ns/prov#order"/>
    ///</summary>
    public const string order = "http://www.w3.org/ns/prov#order";

    ///<summary>
    ///(If this Activity prov:wasAssociatedWith Agent :ag, then it can qualify the Association using prov:qualifiedAssociation [ a prov:Association;  prov:agent :ag; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedAssociation"/>
    ///</summary>
    public const string qualifiedAssociation = "http://www.w3.org/ns/prov#qualifiedAssociation";

    ///<summary>
    ///(If this Entity prov:wasAttributedTo Agent :ag, then it can qualify how it was influenced using prov:qualifiedAttribution [ a prov:Attribution;  prov:agent :ag; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedAttribution"/>
    ///</summary>
    public const string qualifiedAttribution = "http://www.w3.org/ns/prov#qualifiedAttribution";

    ///<summary>
    ///(If this Activity prov:wasInformedBy Activity :a, then it can qualify how it was influenced using prov:qualifiedCommunication [ a prov:Communication;  prov:activity :a; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedCommunication"/>
    ///</summary>
    public const string qualifiedCommunication = "http://www.w3.org/ns/prov#qualifiedCommunication";

    ///<summary>
    ///(If this Agent prov:actedOnBehalfOf Agent :ag, then it can qualify how with prov:qualifiedResponsibility [ a prov:Responsibility;  prov:agent :ag; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedDelegation"/>
    ///</summary>
    public const string qualifiedDelegation = "http://www.w3.org/ns/prov#qualifiedDelegation";

    ///<summary>
    ///(If this Entity prov:wasDerivedFrom Entity :e, then it can qualify how it was derived using prov:qualifiedDerivation [ a prov:Derivation;  prov:entity :e; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedDerivation"/>
    ///</summary>
    public const string qualifiedDerivation = "http://www.w3.org/ns/prov#qualifiedDerivation";

    ///<summary>
    ///(If this Activity prov:wasEndedBy Entity :e1, then it can qualify how it was ended using prov:qualifiedEnd [ a prov:End;  prov:entity :e1; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedEnd"/>
    ///</summary>
    public const string qualifiedEnd = "http://www.w3.org/ns/prov#qualifiedEnd";

    ///<summary>
    ///(This annotation property links a subproperty of prov:wasInfluencedBy with the subclass of prov:Influence and the qualifying property that are used to qualify it. 
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
    ///Note how the value of the unqualified influence (prov:wasGeneratedBy :activity1) is mirrored as the value of the prov:activity (or prov:entity, or prov:agent) property on the influence class., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedForm"/>
    ///</summary>
    public const string qualifiedForm = "http://www.w3.org/ns/prov#qualifiedForm";

    ///<summary>
    ///(If this Activity prov:generated Entity :e, then it can qualify how it performed the Generation using prov:qualifiedGeneration [ a prov:Generation;  prov:entity :e; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedGeneration"/>
    ///</summary>
    public const string qualifiedGeneration = "http://www.w3.org/ns/prov#qualifiedGeneration";

    ///<summary>
    ///(Because prov:qualifiedInfluence is a broad relation, the more specific relations (qualifiedCommunication, qualifiedDelegation, qualifiedEnd, etc.) should be used when applicable., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedInfluence"/>
    ///</summary>
    public const string qualifiedInfluence = "http://www.w3.org/ns/prov#qualifiedInfluence";

    ///<summary>
    ///(If this Entity prov:wasInvalidatedBy Activity :a, then it can qualify how it was invalidated using prov:qualifiedInvalidation [ a prov:Invalidation;  prov:activity :a; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedInvalidation"/>
    ///</summary>
    public const string qualifiedInvalidation = "http://www.w3.org/ns/prov#qualifiedInvalidation";

    ///<summary>
    ///(If this Entity prov:hadPrimarySource Entity :e, then it can qualify how using prov:qualifiedPrimarySource [ a prov:PrimarySource; prov:entity :e; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedPrimarySource"/>
    ///</summary>
    public const string qualifiedPrimarySource = "http://www.w3.org/ns/prov#qualifiedPrimarySource";

    ///<summary>
    ///(If this Entity prov:wasQuotedFrom Entity :e, then it can qualify how using prov:qualifiedQuotation [ a prov:Quotation;  prov:entity :e; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedQuotation"/>
    ///</summary>
    public const string qualifiedQuotation = "http://www.w3.org/ns/prov#qualifiedQuotation";

    ///<summary>
    ///(If this Entity prov:wasRevisionOf Entity :e, then it can qualify how it was revised using prov:qualifiedRevision [ a prov:Revision;  prov:entity :e; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedRevision"/>
    ///</summary>
    public const string qualifiedRevision = "http://www.w3.org/ns/prov#qualifiedRevision";

    ///<summary>
    ///(If this Activity prov:wasStartedBy Entity :e1, then it can qualify how it was started using prov:qualifiedStart [ a prov:Start;  prov:entity :e1; :foo :bar ]., en)
    ///<see cref="http://www.w3.org/ns/prov#qualifiedStart"/>
    ///</summary>
    public const string qualifiedStart = "http://www.w3.org/ns/prov#qualifiedStart";

    ///<summary>
    ///(If this Activity prov:used Entity :e, then it can qualify how it used it using prov:qualifiedUsage [ a prov:Usage; prov:entity :e; :foo :bar ]., en)
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
    ///(The time at which an activity started. See also prov:endedAtTime., en)
    ///<see cref="http://www.w3.org/ns/prov#startedAtTime"/>
    ///</summary>
    public const string startedAtTime = "http://www.w3.org/ns/prov#startedAtTime";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#todo"/>
    ///</summary>
    public const string todo = "http://www.w3.org/ns/prov#todo";

    ///<summary>
    ///(Classes and properties used to qualify relationships are annotated with prov:unqualifiedForm to indicate the property used to assert an unqualified provenance relation., en)
    ///<see cref="http://www.w3.org/ns/prov#unqualifiedForm"/>
    ///</summary>
    public const string unqualifiedForm = "http://www.w3.org/ns/prov#unqualifiedForm";

    ///<summary>
    ///(A prov:Entity that was used by this prov:Activity. For example, :baking prov:used :spoon, :egg, :oven ., en)
    ///<see cref="http://www.w3.org/ns/prov#used"/>
    ///</summary>
    public const string used = "http://www.w3.org/ns/prov#used";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#value"/>
    ///</summary>
    public const string value = "http://www.w3.org/ns/prov#value";

    ///<summary>
    ///(An prov:Agent that had some (unspecified) responsibility for the occurrence of this prov:Activity., en)
    ///<see cref="http://www.w3.org/ns/prov#wasAssociatedWith"/>
    ///</summary>
    public const string wasAssociatedWith = "http://www.w3.org/ns/prov#wasAssociatedWith";

    ///<summary>
    ///(Attribution is the ascribing of an entity to an agent., en)
    ///<see cref="http://www.w3.org/ns/prov#wasAttributedTo"/>
    ///</summary>
    public const string wasAttributedTo = "http://www.w3.org/ns/prov#wasAttributedTo";

    ///<summary>
    ///(The more specific subproperties of prov:wasDerivedFrom (i.e., prov:wasQuotedFrom, prov:wasRevisionOf, prov:hadPrimarySource) should be used when applicable., en)
    ///<see cref="http://www.w3.org/ns/prov#wasDerivedFrom"/>
    ///</summary>
    public const string wasDerivedFrom = "http://www.w3.org/ns/prov#wasDerivedFrom";

    ///<summary>
    ///(End is when an activity is deemed to have ended. An end may refer to an entity, known as trigger, that terminated the activity., en)
    ///<see cref="http://www.w3.org/ns/prov#wasEndedBy"/>
    ///</summary>
    public const string wasEndedBy = "http://www.w3.org/ns/prov#wasEndedBy";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasGeneratedBy"/>
    ///</summary>
    public const string wasGeneratedBy = "http://www.w3.org/ns/prov#wasGeneratedBy";

    ///<summary>
    ///(Because prov:wasInfluencedBy is a broad relation, its more specific subproperties (e.g. prov:wasInformedBy, prov:actedOnBehalfOf, prov:wasEndedBy, etc.) should be used when applicable., en)
    ///<see cref="http://www.w3.org/ns/prov#wasInfluencedBy"/>
    ///</summary>
    public const string wasInfluencedBy = "http://www.w3.org/ns/prov#wasInfluencedBy";

    ///<summary>
    ///(An activity a2 is dependent on or informed by another activity a1, by way of some unspecified entity that is generated by a1 and used by a2., en)
    ///<see cref="http://www.w3.org/ns/prov#wasInformedBy"/>
    ///</summary>
    public const string wasInformedBy = "http://www.w3.org/ns/prov#wasInformedBy";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/ns/prov#wasInvalidatedBy"/>
    ///</summary>
    public const string wasInvalidatedBy = "http://www.w3.org/ns/prov#wasInvalidatedBy";

    ///<summary>
    ///(An entity is derived from an original entity by copying, or 'quoting', some or all of it., en)
    ///<see cref="http://www.w3.org/ns/prov#wasQuotedFrom"/>
    ///</summary>
    public const string wasQuotedFrom = "http://www.w3.org/ns/prov#wasQuotedFrom";

    ///<summary>
    ///(A revision is a derivation that revises an entity into a revised version., en)
    ///<see cref="http://www.w3.org/ns/prov#wasRevisionOf"/>
    ///</summary>
    public const string wasRevisionOf = "http://www.w3.org/ns/prov#wasRevisionOf";

    ///<summary>
    ///(Start is when an activity is deemed to have started. A start may refer to an entity, known as trigger, that initiated the activity., en)
    ///<see cref="http://www.w3.org/ns/prov#wasStartedBy"/>
    ///</summary>
    public const string wasStartedBy = "http://www.w3.org/ns/prov#wasStartedBy";

    ///<summary>
    ///(This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page). 
    ///
    ///If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/Archives/Public/public-prov-comments/). All feedback is welcome., en)
    ///<see cref="http://www.w3.org/ns/prov-o#"/>
    ///</summary>
    public const string prov_o = "http://www.w3.org/ns/prov-o#";

    ///<summary>
    ///
    ///<see cref="file:///D:/Projects/2016/Artivity/Artivity.DataModel/Ontologies/prov.ttl#"/>
    ///</summary>
    public const string prov_ttl = "file:///D:/Projects/2016/Artivity/Artivity.DataModel/Ontologies/prov.ttl#";

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
    ///(Indicates anchor URI for a potentially dynamic resource instance., en)
    ///<see cref="http://www.w3.org/ns/prov#has_anchor"/>
    ///</summary>
    public const string has_anchor = "http://www.w3.org/ns/prov#has_anchor";

    ///<summary>
    ///(Indicates a provenance-URI for a resource; the resource identified by this property presents a provenance record about its subject or anchor resource., en)
    ///<see cref="http://www.w3.org/ns/prov#has_provenance"/>
    ///</summary>
    public const string has_provenance = "http://www.w3.org/ns/prov#has_provenance";

    ///<summary>
    ///(Indicates a provenance query service that can access provenance related to its subject or anchor resource., en)
    ///<see cref="http://www.w3.org/ns/prov#has_query_service"/>
    ///</summary>
    public const string has_query_service = "http://www.w3.org/ns/prov#has_query_service";

    ///<summary>
    ///(relates a generic provenance query service resource (type prov:ServiceDescription) to a specific query service description (e.g. a prov:DirectQueryService or a sd:Service)., en)
    ///<see cref="http://www.w3.org/ns/prov#describesService"/>
    ///</summary>
    public const string describesService = "http://www.w3.org/ns/prov#describesService";

    ///<summary>
    ///(Relates a provenance service to a URI template string for constructing provenance-URIs., en)
    ///<see cref="http://www.w3.org/ns/prov#provenanceUriTemplate"/>
    ///</summary>
    public const string provenanceUriTemplate = "http://www.w3.org/ns/prov#provenanceUriTemplate";

    ///<summary>
    ///(Relates a resource to a provenance pingback service that may receive additional provenance links about the resource., en)
    ///<see cref="http://www.w3.org/ns/prov#pingback"/>
    ///</summary>
    public const string pingback = "http://www.w3.org/ns/prov#pingback";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2002/07/owl#topObjectProperty"/>
    ///</summary>
    public const string topObjectProperty = "http://www.w3.org/2002/07/owl#topObjectProperty";

    ///<summary>
    ///(This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page). 
    ///
    ///If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/Archives/Public/public-prov-comments/). All feedback is welcome., en)
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
    ///(This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page). 
    ///
    ///If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/Archives/Public/public-prov-comments/). All feedback is welcome., en)
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
    ///(This document is published by the Provenance Working Group (http://www.w3.org/2011/prov/wiki/Main_Page). If you wish to make comments regarding this document, please send them to public-prov-comments@w3.org (subscribe public-prov-comments-request@w3.org, archives http://lists.w3.org/Archives/Public/public-prov-comments/
    ///). All feedback is welcome., en)
    ///<see cref="http://www.w3.org/ns/prov-links#"/>
    ///</summary>
    public const string prov_links = "http://www.w3.org/ns/prov-links#";

    ///<summary>
    ///(prov:asInBundle is used to specify which bundle the general entity of a prov:mentionOf property is described.
    ///
    ///When :x prov:mentionOf :y and :y is described in Bundle :b, the triple :x prov:asInBundle :b is also asserted to cite the Bundle in which :y was described., en)
    ///<see cref="http://www.w3.org/ns/prov#asInBundle"/>
    ///</summary>
    public const string asInBundle = "http://www.w3.org/ns/prov#asInBundle";

    ///<summary>
    ///(prov:mentionOf is used to specialize an entity as described in another bundle. It is to be used in conjuction with prov:asInBundle.
    ///
    ///prov:asInBundle is used to cite the Bundle in which the generalization was mentioned., en)
    ///<see cref="http://www.w3.org/ns/prov#mentionOf"/>
    ///</summary>
    public const string mentionOf = "http://www.w3.org/ns/prov#mentionOf";
}
///<summary>
///RDF schema for the XML Infoset
///
///</summary>
public class xml : Ontology
{
    public static readonly Uri Namespace = new Uri("http://www.w3.org/2001/04/infoset#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "xml";
    public static string GetPrefix() { return Prefix; }     

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#"/>
    ///</summary>
    public static readonly Resource infoset = new Resource(new Uri("http://www.w3.org/2001/04/infoset#"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#AttributeType"/>
    ///</summary>
    public static readonly Class AttributeType = new Class(new Uri("http://www.w3.org/2001/04/infoset#AttributeType"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Boolean"/>
    ///</summary>
    public static readonly Class Boolean = new Class(new Uri("http://www.w3.org/2001/04/infoset#Boolean"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Standalone"/>
    ///</summary>
    public static readonly Class Standalone = new Class(new Uri("http://www.w3.org/2001/04/infoset#Standalone"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Unknown"/>
    ///</summary>
    public static readonly Class Unknown = new Class(new Uri("http://www.w3.org/2001/04/infoset#Unknown"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#NoValue"/>
    ///</summary>
    public static readonly Class NoValue = new Class(new Uri("http://www.w3.org/2001/04/infoset#NoValue"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Literal"/>
    ///</summary>
    public static readonly Class Literal = new Class(new Uri("http://www.w3.org/2001/04/infoset#Literal"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Integer"/>
    ///</summary>
    public static readonly Class Integer = new Class(new Uri("http://www.w3.org/2001/04/infoset#Integer"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#InfoItem"/>
    ///</summary>
    public static readonly Class InfoItem = new Class(new Uri("http://www.w3.org/2001/04/infoset#InfoItem"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Document"/>
    ///</summary>
    public static readonly Class Document = new Class(new Uri("http://www.w3.org/2001/04/infoset#Document"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Element"/>
    ///</summary>
    public static readonly Class Element = new Class(new Uri("http://www.w3.org/2001/04/infoset#Element"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Attribute"/>
    ///</summary>
    public static readonly Class Attribute = new Class(new Uri("http://www.w3.org/2001/04/infoset#Attribute"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#ProcessingInstruction"/>
    ///</summary>
    public static readonly Class ProcessingInstruction = new Class(new Uri("http://www.w3.org/2001/04/infoset#ProcessingInstruction"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Character"/>
    ///</summary>
    public static readonly Class Character = new Class(new Uri("http://www.w3.org/2001/04/infoset#Character"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#UnexpandedEntityReference"/>
    ///</summary>
    public static readonly Class UnexpandedEntityReference = new Class(new Uri("http://www.w3.org/2001/04/infoset#UnexpandedEntityReference"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Comment"/>
    ///</summary>
    public static readonly Class Comment = new Class(new Uri("http://www.w3.org/2001/04/infoset#Comment"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#DocumentTypeDeclaration"/>
    ///</summary>
    public static readonly Class DocumentTypeDeclaration = new Class(new Uri("http://www.w3.org/2001/04/infoset#DocumentTypeDeclaration"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#UnparsedEntity"/>
    ///</summary>
    public static readonly Class UnparsedEntity = new Class(new Uri("http://www.w3.org/2001/04/infoset#UnparsedEntity"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Notation"/>
    ///</summary>
    public static readonly Class Notation = new Class(new Uri("http://www.w3.org/2001/04/infoset#Notation"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#InfoItemSet"/>
    ///</summary>
    public static readonly Class InfoItemSet = new Class(new Uri("http://www.w3.org/2001/04/infoset#InfoItemSet"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#AttributeSet"/>
    ///</summary>
    public static readonly Class AttributeSet = new Class(new Uri("http://www.w3.org/2001/04/infoset#AttributeSet"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#UnparsedEntitySet"/>
    ///</summary>
    public static readonly Class UnparsedEntitySet = new Class(new Uri("http://www.w3.org/2001/04/infoset#UnparsedEntitySet"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#NamespaceSet"/>
    ///</summary>
    public static readonly Class NamespaceSet = new Class(new Uri("http://www.w3.org/2001/04/infoset#NamespaceSet"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#NotationSet"/>
    ///</summary>
    public static readonly Class NotationSet = new Class(new Uri("http://www.w3.org/2001/04/infoset#NotationSet"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#InfoItemSeq"/>
    ///</summary>
    public static readonly Class InfoItemSeq = new Class(new Uri("http://www.w3.org/2001/04/infoset#InfoItemSeq"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#References"/>
    ///</summary>
    public static readonly Class References = new Class(new Uri("http://www.w3.org/2001/04/infoset#References"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#allDeclarationsProcessed"/>
    ///</summary>
    public static readonly Resource allDeclarationsProcessed = new Resource(new Uri("http://www.w3.org/2001/04/infoset#allDeclarationsProcessed"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#attributes"/>
    ///</summary>
    public static readonly Resource attributes = new Resource(new Uri("http://www.w3.org/2001/04/infoset#attributes"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#attributeType"/>
    ///</summary>
    public static readonly Resource attributeType = new Resource(new Uri("http://www.w3.org/2001/04/infoset#attributeType"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#baseURI"/>
    ///</summary>
    public static readonly Resource baseURI = new Resource(new Uri("http://www.w3.org/2001/04/infoset#baseURI"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#characterCode"/>
    ///</summary>
    public static readonly Resource characterCode = new Resource(new Uri("http://www.w3.org/2001/04/infoset#characterCode"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#characterEncodingScheme"/>
    ///</summary>
    public static readonly Resource characterEncodingScheme = new Resource(new Uri("http://www.w3.org/2001/04/infoset#characterEncodingScheme"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#children"/>
    ///</summary>
    public static readonly Resource children = new Resource(new Uri("http://www.w3.org/2001/04/infoset#children"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#content"/>
    ///</summary>
    public static readonly Resource content = new Resource(new Uri("http://www.w3.org/2001/04/infoset#content"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#namespaceAttributes"/>
    ///</summary>
    public static readonly Resource namespaceAttributes = new Resource(new Uri("http://www.w3.org/2001/04/infoset#namespaceAttributes"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#declarationBaseURI"/>
    ///</summary>
    public static readonly Resource declarationBaseURI = new Resource(new Uri("http://www.w3.org/2001/04/infoset#declarationBaseURI"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#documentElement"/>
    ///</summary>
    public static readonly Resource documentElement = new Resource(new Uri("http://www.w3.org/2001/04/infoset#documentElement"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#elementContentWhitespace"/>
    ///</summary>
    public static readonly Resource elementContentWhitespace = new Resource(new Uri("http://www.w3.org/2001/04/infoset#elementContentWhitespace"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#unparsedEntities"/>
    ///</summary>
    public static readonly Resource unparsedEntities = new Resource(new Uri("http://www.w3.org/2001/04/infoset#unparsedEntities"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#inScopeNamespaces"/>
    ///</summary>
    public static readonly Resource inScopeNamespaces = new Resource(new Uri("http://www.w3.org/2001/04/infoset#inScopeNamespaces"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#localName"/>
    ///</summary>
    public static readonly Resource localName = new Resource(new Uri("http://www.w3.org/2001/04/infoset#localName"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#name"/>
    ///</summary>
    public static readonly Resource name = new Resource(new Uri("http://www.w3.org/2001/04/infoset#name"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#namespaceName"/>
    ///</summary>
    public static readonly Resource namespaceName = new Resource(new Uri("http://www.w3.org/2001/04/infoset#namespaceName"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#normalizedValue"/>
    ///</summary>
    public static readonly Resource normalizedValue = new Resource(new Uri("http://www.w3.org/2001/04/infoset#normalizedValue"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#notation"/>
    ///</summary>
    public static readonly Resource notation = new Resource(new Uri("http://www.w3.org/2001/04/infoset#notation"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#notationName"/>
    ///</summary>
    public static readonly Resource notationName = new Resource(new Uri("http://www.w3.org/2001/04/infoset#notationName"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#notations"/>
    ///</summary>
    public static readonly Resource notations = new Resource(new Uri("http://www.w3.org/2001/04/infoset#notations"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#ownerElement"/>
    ///</summary>
    public static readonly Resource ownerElement = new Resource(new Uri("http://www.w3.org/2001/04/infoset#ownerElement"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#parent"/>
    ///</summary>
    public static readonly Resource parent = new Resource(new Uri("http://www.w3.org/2001/04/infoset#parent"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#prefix"/>
    ///</summary>
    public static readonly Resource prefix = new Resource(new Uri("http://www.w3.org/2001/04/infoset#prefix"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#publicIdentifier"/>
    ///</summary>
    public static readonly Resource publicIdentifier = new Resource(new Uri("http://www.w3.org/2001/04/infoset#publicIdentifier"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#references"/>
    ///</summary>
    public static readonly Resource references = new Resource(new Uri("http://www.w3.org/2001/04/infoset#references"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#specified"/>
    ///</summary>
    public static readonly Resource specified = new Resource(new Uri("http://www.w3.org/2001/04/infoset#specified"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#standalone"/>
    ///</summary>
    public static readonly Resource standalone = new Resource(new Uri("http://www.w3.org/2001/04/infoset#standalone"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#systemIdentifier"/>
    ///</summary>
    public static readonly Resource systemIdentifier = new Resource(new Uri("http://www.w3.org/2001/04/infoset#systemIdentifier"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#target"/>
    ///</summary>
    public static readonly Resource target = new Resource(new Uri("http://www.w3.org/2001/04/infoset#target"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#version"/>
    ///</summary>
    public static readonly Resource version = new Resource(new Uri("http://www.w3.org/2001/04/infoset#version"));
}
///<summary>
///RDF schema for the XML Infoset
///
///</summary>
public static class XML
{
    public static readonly Uri Namespace = new Uri("http://www.w3.org/2001/04/infoset#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "XML";
    public static string GetPrefix() { return Prefix; } 

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#"/>
    ///</summary>
    public const string infoset = "http://www.w3.org/2001/04/infoset#";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#AttributeType"/>
    ///</summary>
    public const string AttributeType = "http://www.w3.org/2001/04/infoset#AttributeType";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Boolean"/>
    ///</summary>
    public const string Boolean = "http://www.w3.org/2001/04/infoset#Boolean";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Standalone"/>
    ///</summary>
    public const string Standalone = "http://www.w3.org/2001/04/infoset#Standalone";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Unknown"/>
    ///</summary>
    public const string Unknown = "http://www.w3.org/2001/04/infoset#Unknown";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#NoValue"/>
    ///</summary>
    public const string NoValue = "http://www.w3.org/2001/04/infoset#NoValue";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Literal"/>
    ///</summary>
    public const string Literal = "http://www.w3.org/2001/04/infoset#Literal";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Integer"/>
    ///</summary>
    public const string Integer = "http://www.w3.org/2001/04/infoset#Integer";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#InfoItem"/>
    ///</summary>
    public const string InfoItem = "http://www.w3.org/2001/04/infoset#InfoItem";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Document"/>
    ///</summary>
    public const string Document = "http://www.w3.org/2001/04/infoset#Document";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Element"/>
    ///</summary>
    public const string Element = "http://www.w3.org/2001/04/infoset#Element";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Attribute"/>
    ///</summary>
    public const string Attribute = "http://www.w3.org/2001/04/infoset#Attribute";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#ProcessingInstruction"/>
    ///</summary>
    public const string ProcessingInstruction = "http://www.w3.org/2001/04/infoset#ProcessingInstruction";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Character"/>
    ///</summary>
    public const string Character = "http://www.w3.org/2001/04/infoset#Character";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#UnexpandedEntityReference"/>
    ///</summary>
    public const string UnexpandedEntityReference = "http://www.w3.org/2001/04/infoset#UnexpandedEntityReference";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Comment"/>
    ///</summary>
    public const string Comment = "http://www.w3.org/2001/04/infoset#Comment";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#DocumentTypeDeclaration"/>
    ///</summary>
    public const string DocumentTypeDeclaration = "http://www.w3.org/2001/04/infoset#DocumentTypeDeclaration";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#UnparsedEntity"/>
    ///</summary>
    public const string UnparsedEntity = "http://www.w3.org/2001/04/infoset#UnparsedEntity";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#Notation"/>
    ///</summary>
    public const string Notation = "http://www.w3.org/2001/04/infoset#Notation";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#InfoItemSet"/>
    ///</summary>
    public const string InfoItemSet = "http://www.w3.org/2001/04/infoset#InfoItemSet";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#AttributeSet"/>
    ///</summary>
    public const string AttributeSet = "http://www.w3.org/2001/04/infoset#AttributeSet";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#UnparsedEntitySet"/>
    ///</summary>
    public const string UnparsedEntitySet = "http://www.w3.org/2001/04/infoset#UnparsedEntitySet";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#NamespaceSet"/>
    ///</summary>
    public const string NamespaceSet = "http://www.w3.org/2001/04/infoset#NamespaceSet";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#NotationSet"/>
    ///</summary>
    public const string NotationSet = "http://www.w3.org/2001/04/infoset#NotationSet";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#InfoItemSeq"/>
    ///</summary>
    public const string InfoItemSeq = "http://www.w3.org/2001/04/infoset#InfoItemSeq";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#References"/>
    ///</summary>
    public const string References = "http://www.w3.org/2001/04/infoset#References";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#allDeclarationsProcessed"/>
    ///</summary>
    public const string allDeclarationsProcessed = "http://www.w3.org/2001/04/infoset#allDeclarationsProcessed";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#attributes"/>
    ///</summary>
    public const string attributes = "http://www.w3.org/2001/04/infoset#attributes";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#attributeType"/>
    ///</summary>
    public const string attributeType = "http://www.w3.org/2001/04/infoset#attributeType";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#baseURI"/>
    ///</summary>
    public const string baseURI = "http://www.w3.org/2001/04/infoset#baseURI";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#characterCode"/>
    ///</summary>
    public const string characterCode = "http://www.w3.org/2001/04/infoset#characterCode";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#characterEncodingScheme"/>
    ///</summary>
    public const string characterEncodingScheme = "http://www.w3.org/2001/04/infoset#characterEncodingScheme";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#children"/>
    ///</summary>
    public const string children = "http://www.w3.org/2001/04/infoset#children";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#content"/>
    ///</summary>
    public const string content = "http://www.w3.org/2001/04/infoset#content";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#namespaceAttributes"/>
    ///</summary>
    public const string namespaceAttributes = "http://www.w3.org/2001/04/infoset#namespaceAttributes";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#declarationBaseURI"/>
    ///</summary>
    public const string declarationBaseURI = "http://www.w3.org/2001/04/infoset#declarationBaseURI";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#documentElement"/>
    ///</summary>
    public const string documentElement = "http://www.w3.org/2001/04/infoset#documentElement";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#elementContentWhitespace"/>
    ///</summary>
    public const string elementContentWhitespace = "http://www.w3.org/2001/04/infoset#elementContentWhitespace";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#unparsedEntities"/>
    ///</summary>
    public const string unparsedEntities = "http://www.w3.org/2001/04/infoset#unparsedEntities";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#inScopeNamespaces"/>
    ///</summary>
    public const string inScopeNamespaces = "http://www.w3.org/2001/04/infoset#inScopeNamespaces";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#localName"/>
    ///</summary>
    public const string localName = "http://www.w3.org/2001/04/infoset#localName";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#name"/>
    ///</summary>
    public const string name = "http://www.w3.org/2001/04/infoset#name";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#namespaceName"/>
    ///</summary>
    public const string namespaceName = "http://www.w3.org/2001/04/infoset#namespaceName";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#normalizedValue"/>
    ///</summary>
    public const string normalizedValue = "http://www.w3.org/2001/04/infoset#normalizedValue";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#notation"/>
    ///</summary>
    public const string notation = "http://www.w3.org/2001/04/infoset#notation";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#notationName"/>
    ///</summary>
    public const string notationName = "http://www.w3.org/2001/04/infoset#notationName";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#notations"/>
    ///</summary>
    public const string notations = "http://www.w3.org/2001/04/infoset#notations";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#ownerElement"/>
    ///</summary>
    public const string ownerElement = "http://www.w3.org/2001/04/infoset#ownerElement";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#parent"/>
    ///</summary>
    public const string parent = "http://www.w3.org/2001/04/infoset#parent";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#prefix"/>
    ///</summary>
    public const string prefix = "http://www.w3.org/2001/04/infoset#prefix";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#publicIdentifier"/>
    ///</summary>
    public const string publicIdentifier = "http://www.w3.org/2001/04/infoset#publicIdentifier";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#references"/>
    ///</summary>
    public const string references = "http://www.w3.org/2001/04/infoset#references";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#specified"/>
    ///</summary>
    public const string specified = "http://www.w3.org/2001/04/infoset#specified";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#standalone"/>
    ///</summary>
    public const string standalone = "http://www.w3.org/2001/04/infoset#standalone";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#systemIdentifier"/>
    ///</summary>
    public const string systemIdentifier = "http://www.w3.org/2001/04/infoset#systemIdentifier";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#target"/>
    ///</summary>
    public const string target = "http://www.w3.org/2001/04/infoset#target";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2001/04/infoset#version"/>
    ///</summary>
    public const string version = "http://www.w3.org/2001/04/infoset#version";
}
///<summary>
///
///
///</summary>
public class nao : Ontology
{
    public static readonly Uri Namespace = new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "nao";
    public static string GetPrefix() { return Prefix; }     

    ///<summary>
    ///Defines the default static namespace abbreviation for a graph
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasDefaultNamespaceAbbreviation"/>
    ///</summary>
    public static readonly Property hasDefaultNamespaceAbbreviation = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasDefaultNamespaceAbbreviation"));    

    ///<summary>
    ///Generic annotation for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#annotation"/>
    ///</summary>
    public static readonly Property annotation = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#annotation"));    

    ///<summary>
    ///Represents a symbol, a visual representation of a resource. Typically a local or remote file would be double-typed to be used as a symbol. An alternative is nao:FreeDesktopIcon.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#Symbol"/>
    ///</summary>
    public static readonly Class Symbol = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#Symbol"));    

    ///<summary>
    ///Defines a name for a FreeDesktop Icon as defined in the FreeDesktop Icon Naming Standard
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#iconName"/>
    ///</summary>
    public static readonly Property iconName = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#iconName"));    

    ///<summary>
    ///Represents a desktop icon as defined in the FreeDesktop Icon Naming Standard (http://standards.freedesktop.org/icon-naming-spec/icon-naming-spec-latest.html).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#FreeDesktopIcon"/>
    ///</summary>
    public static readonly Class FreeDesktopIcon = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#FreeDesktopIcon"));    

    ///<summary>
    ///Defines a generic identifier for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#identifier"/>
    ///</summary>
    public static readonly Property identifier = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#identifier"));    

    ///<summary>
    ///An authoritative score for an item valued between 0 and 1
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#score"/>
    ///</summary>
    public static readonly Property score = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#score"));    

    ///<summary>
    ///Defines a relationship between two resources, where the subject is a topic of the object
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#isTopicOf"/>
    ///</summary>
    public static readonly Property isTopicOf = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#isTopicOf"));    

    ///<summary>
    ///Defines an annotation for a resource in the form of a relationship between the subject resource and another resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#isRelated"/>
    ///</summary>
    public static readonly Property isRelated = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#isRelated"));    

    ///<summary>
    ///Defines a relationship between two resources, where the object is a topic of the subject
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasTopic"/>
    ///</summary>
    public static readonly Property hasTopic = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasTopic"));    

    ///<summary>
    ///Defines a relationship between a resource and one or more sub resources. Descriptions of sub-resources are only interpretable when the super-resource exists. Deleting a super-resource should then also delete all sub-resources, and transferring a super-resource (for example, sending it to another user) must also include the sub-resource.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasSubResource"/>
    ///</summary>
    public static readonly Property hasSubResource = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasSubResource"));    

    ///<summary>
    ///Defines a relationship between a resource and one or more super resources
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasSuperResource"/>
    ///</summary>
    public static readonly Property hasSuperResource = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasSuperResource"));    

    ///<summary>
    ///States which resources a tag is associated with
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#isTagFor"/>
    ///</summary>
    public static readonly Property isTagFor = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#isTagFor"));    

    ///<summary>
    ///Represents a generic tag
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#Tag"/>
    ///</summary>
    public static readonly Class Tag = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#Tag"));    

    ///<summary>
    ///Defines an existing tag for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasTag"/>
    ///</summary>
    public static readonly Property hasTag = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasTag"));    

    ///<summary>
    ///Links a named graph to the resource for which it contains metadata. Its typical usage would be to link the graph containing extracted file metadata to the file resource. This allows for easy maintenance later on. Inverse property of nao:hasDataGraph.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#isDataGraphFor"/>
    ///</summary>
    public static readonly Property isDataGraphFor = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#isDataGraphFor"));    

    ///<summary>
    ///Links a resource to the graph which contains its metadata. Its typical usage would be to link the file resource to the graph containing its extracted file metadata. This allows for easy maintenance later on. Inverse property of nao:isDataGraphFor.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasDataGraph"/>
    ///</summary>
    public static readonly Property hasDataGraph = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasDataGraph"));    

    ///<summary>
    ///Specifies the version of a graph, in numeric format
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#version"/>
    ///</summary>
    public static readonly Property version = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#version"));    

    ///<summary>
    ///An alternative label alongside the preferred label for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#altLabel"/>
    ///</summary>
    public static readonly Property altLabel = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#altLabel"));    

    ///<summary>
    ///Annotation for a resource in the form of a visual representation. Typically the symbol is a double-typed image file or a nao:FreeDesktopIcon.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasSymbol"/>
    ///</summary>
    public static readonly Property hasSymbol = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasSymbol"));    

    ///<summary>
    ///A unique preferred symbol representation for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#prefSymbol"/>
    ///</summary>
    public static readonly Property prefSymbol = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#prefSymbol"));    

    ///<summary>
    ///An alternative symbol representation for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#altSymbol"/>
    ///</summary>
    public static readonly Property altSymbol = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#altSymbol"));    

    ///<summary>
    ///States the serialization language for a named graph that is represented within a document
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#serializationLanguage"/>
    ///</summary>
    public static readonly Property serializationLanguage = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#serializationLanguage"));    

    ///<summary>
    ///Refers to the single or group of individuals that created the resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#creator"/>
    ///</summary>
    public static readonly Property creator = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#creator"));    

    ///<summary>
    ///Represents a single or a group of individuals
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#Party"/>
    ///</summary>
    public static readonly Class Party = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#Party"));    

    ///<summary>
    /// Annotation for a resource in the form of a numeric rating (float value), allowed values are between 1 and 10 whereas 0 is interpreted as not set
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#numericRating"/>
    ///</summary>
    public static readonly Property numericRating = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#numericRating"));    

    ///<summary>
    ///Annotation for a resource in the form of an unrestricted rating
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#rating"/>
    ///</summary>
    public static readonly Property rating = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#rating"));    

    ///<summary>
    ///A marker property to mark selected properties which are input to a mathematical algorithm to generate scores for resources. Properties are marked by being defined as subproperties of this property
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#scoreParameter"/>
    ///</summary>
    public static readonly Property scoreParameter = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#scoreParameter"));    

    ///<summary>
    ///Refers to a single or a group of individuals that contributed to a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#contributor"/>
    ///</summary>
    public static readonly Property contributor = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#contributor"));    

    ///<summary>
    ///Defines the default static namespace for a graph
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasDefaultNamespace"/>
    ///</summary>
    public static readonly Property hasDefaultNamespace = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasDefaultNamespace"));    

    ///<summary>
    ///States the modification time for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#modified"/>
    ///</summary>
    public static readonly Property modified = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#modified"));    

    ///<summary>
    ///States the creation, or first modification time for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#created"/>
    ///</summary>
    public static readonly Property created = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#created"));    

    ///<summary>
    ///States the last modification time for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#lastModified"/>
    ///</summary>
    public static readonly Property lastModified = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#lastModified"));    

    ///<summary>
    ///Specifies the status of a graph, stable, unstable or testing
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#status"/>
    ///</summary>
    public static readonly Property status = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#status"));    

    ///<summary>
    ///A non-technical textual annotation for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#description"/>
    ///</summary>
    public static readonly Property description = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#description"));    

    ///<summary>
    ///A preferred label for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#prefLabel"/>
    ///</summary>
    public static readonly Property prefLabel = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#prefLabel"));    

    ///<summary>
    ///The plural form of the preferred label for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#pluralPrefLabel"/>
    ///</summary>
    public static readonly Property pluralPrefLabel = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#pluralPrefLabel"));    

    ///<summary>
    ///Specifies the engineering tool used to generate the graph
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#engineeringTool"/>
    ///</summary>
    public static readonly Property engineeringTool = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#engineeringTool"));    

    ///<summary>
    ///Defines a personal string identifier for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#personalIdentifier"/>
    ///</summary>
    public static readonly Property personalIdentifier = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#personalIdentifier"));    

    ///<summary>
    ///If this property is assigned, the subject class, property, or resource, is deprecated and should not be used in production systems any longer. It may be removed without further notice.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#deprecated"/>
    ///</summary>
    public static readonly Property deprecated = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#deprecated"));    

    ///<summary>
    ///Mark a property, class, or even resource as user visible or not. Non-user-visible entities should never be presented to the user. By default everything is user-visible.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#userVisible"/>
    ///</summary>
    public static readonly Property userVisible = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#userVisible"));    

    ///<summary>
    ///The agent that maintains this resource, ie. created it and knows what to do with it.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#maintainedBy"/>
    ///</summary>
    public static readonly Property maintainedBy = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#maintainedBy"));    

    ///<summary>
    ///An agent is the artificial counterpart to nao:Party. It can be a software component or some service.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#Agent"/>
    ///</summary>
    public static readonly Class Agent = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#Agent"));    

    ///<summary>
    ///A generalised trust level assigned to an agent, based on a combination of direct and network trust values it possesses. Allowed values range from 0 (no trust) to 1 (very high trust).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#trustLevel"/>
    ///</summary>
    public static readonly Property trustLevel = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#trustLevel"));    

    ///<summary>
    ///A direct trust value assigned to an agent, either manually by a user or semi-/automatically by a system. Allowed values range from 0 (no trust) to 1 (very high trust).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#directTrust"/>
    ///</summary>
    public static readonly Property directTrust = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#directTrust"));    

    ///<summary>
    ///A network-derived trust value assigned to an agent, based on the shared direct trust values for the same agent, as set by participating agents in a network. Allowed values range from 0 (no trust) to 1 (very high trust).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#networkTrust"/>
    ///</summary>
    public static readonly Property networkTrust = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#networkTrust"));    

    ///<summary>
    ///A privacy level as defined for a resource. Allowed values range from 0 (private) to 1 (public).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#privacyLevel"/>
    ///</summary>
    public static readonly Property privacyLevel = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#privacyLevel"));    

    ///<summary>
    ///An external identifier for a resource that has been retreived from an external source.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#externalIdentifier"/>
    ///</summary>
    public static readonly Property externalIdentifier = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#externalIdentifier"));    

    ///<summary>
    ///Signifies social endorsment by an agent, by way of marking the resource as a favourite.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#favouritedBy"/>
    ///</summary>
    public static readonly Property favouritedBy = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#favouritedBy"));    

    ///<summary>
    ///Signifies social endorsment of a resource by a specific agent. Endorsement includes social actions like favouriting, liking, voting for, starring a resource.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#endorsedBy"/>
    ///</summary>
    public static readonly Property endorsedBy = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#endorsedBy"));
}
///<summary>
///
///
///</summary>
public static class NAO
{
    public static readonly Uri Namespace = new Uri("http://www.semanticdesktop.org/ontologies/2007/08/15/nao#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "NAO";
    public static string GetPrefix() { return Prefix; } 

    ///<summary>
    ///Defines the default static namespace abbreviation for a graph
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasDefaultNamespaceAbbreviation"/>
    ///</summary>
    public const string hasDefaultNamespaceAbbreviation = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasDefaultNamespaceAbbreviation";

    ///<summary>
    ///Generic annotation for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#annotation"/>
    ///</summary>
    public const string annotation = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#annotation";

    ///<summary>
    ///Represents a symbol, a visual representation of a resource. Typically a local or remote file would be double-typed to be used as a symbol. An alternative is nao:FreeDesktopIcon.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#Symbol"/>
    ///</summary>
    public const string Symbol = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#Symbol";

    ///<summary>
    ///Defines a name for a FreeDesktop Icon as defined in the FreeDesktop Icon Naming Standard
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#iconName"/>
    ///</summary>
    public const string iconName = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#iconName";

    ///<summary>
    ///Represents a desktop icon as defined in the FreeDesktop Icon Naming Standard (http://standards.freedesktop.org/icon-naming-spec/icon-naming-spec-latest.html).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#FreeDesktopIcon"/>
    ///</summary>
    public const string FreeDesktopIcon = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#FreeDesktopIcon";

    ///<summary>
    ///Defines a generic identifier for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#identifier"/>
    ///</summary>
    public const string identifier = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#identifier";

    ///<summary>
    ///An authoritative score for an item valued between 0 and 1
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#score"/>
    ///</summary>
    public const string score = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#score";

    ///<summary>
    ///Defines a relationship between two resources, where the subject is a topic of the object
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#isTopicOf"/>
    ///</summary>
    public const string isTopicOf = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#isTopicOf";

    ///<summary>
    ///Defines an annotation for a resource in the form of a relationship between the subject resource and another resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#isRelated"/>
    ///</summary>
    public const string isRelated = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#isRelated";

    ///<summary>
    ///Defines a relationship between two resources, where the object is a topic of the subject
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasTopic"/>
    ///</summary>
    public const string hasTopic = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasTopic";

    ///<summary>
    ///Defines a relationship between a resource and one or more sub resources. Descriptions of sub-resources are only interpretable when the super-resource exists. Deleting a super-resource should then also delete all sub-resources, and transferring a super-resource (for example, sending it to another user) must also include the sub-resource.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasSubResource"/>
    ///</summary>
    public const string hasSubResource = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasSubResource";

    ///<summary>
    ///Defines a relationship between a resource and one or more super resources
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasSuperResource"/>
    ///</summary>
    public const string hasSuperResource = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasSuperResource";

    ///<summary>
    ///States which resources a tag is associated with
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#isTagFor"/>
    ///</summary>
    public const string isTagFor = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#isTagFor";

    ///<summary>
    ///Represents a generic tag
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#Tag"/>
    ///</summary>
    public const string Tag = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#Tag";

    ///<summary>
    ///Defines an existing tag for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasTag"/>
    ///</summary>
    public const string hasTag = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasTag";

    ///<summary>
    ///Links a named graph to the resource for which it contains metadata. Its typical usage would be to link the graph containing extracted file metadata to the file resource. This allows for easy maintenance later on. Inverse property of nao:hasDataGraph.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#isDataGraphFor"/>
    ///</summary>
    public const string isDataGraphFor = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#isDataGraphFor";

    ///<summary>
    ///Links a resource to the graph which contains its metadata. Its typical usage would be to link the file resource to the graph containing its extracted file metadata. This allows for easy maintenance later on. Inverse property of nao:isDataGraphFor.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasDataGraph"/>
    ///</summary>
    public const string hasDataGraph = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasDataGraph";

    ///<summary>
    ///Specifies the version of a graph, in numeric format
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#version"/>
    ///</summary>
    public const string version = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#version";

    ///<summary>
    ///An alternative label alongside the preferred label for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#altLabel"/>
    ///</summary>
    public const string altLabel = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#altLabel";

    ///<summary>
    ///Annotation for a resource in the form of a visual representation. Typically the symbol is a double-typed image file or a nao:FreeDesktopIcon.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasSymbol"/>
    ///</summary>
    public const string hasSymbol = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasSymbol";

    ///<summary>
    ///A unique preferred symbol representation for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#prefSymbol"/>
    ///</summary>
    public const string prefSymbol = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#prefSymbol";

    ///<summary>
    ///An alternative symbol representation for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#altSymbol"/>
    ///</summary>
    public const string altSymbol = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#altSymbol";

    ///<summary>
    ///States the serialization language for a named graph that is represented within a document
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#serializationLanguage"/>
    ///</summary>
    public const string serializationLanguage = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#serializationLanguage";

    ///<summary>
    ///Refers to the single or group of individuals that created the resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#creator"/>
    ///</summary>
    public const string creator = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#creator";

    ///<summary>
    ///Represents a single or a group of individuals
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#Party"/>
    ///</summary>
    public const string Party = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#Party";

    ///<summary>
    /// Annotation for a resource in the form of a numeric rating (float value), allowed values are between 1 and 10 whereas 0 is interpreted as not set
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#numericRating"/>
    ///</summary>
    public const string numericRating = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#numericRating";

    ///<summary>
    ///Annotation for a resource in the form of an unrestricted rating
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#rating"/>
    ///</summary>
    public const string rating = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#rating";

    ///<summary>
    ///A marker property to mark selected properties which are input to a mathematical algorithm to generate scores for resources. Properties are marked by being defined as subproperties of this property
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#scoreParameter"/>
    ///</summary>
    public const string scoreParameter = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#scoreParameter";

    ///<summary>
    ///Refers to a single or a group of individuals that contributed to a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#contributor"/>
    ///</summary>
    public const string contributor = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#contributor";

    ///<summary>
    ///Defines the default static namespace for a graph
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasDefaultNamespace"/>
    ///</summary>
    public const string hasDefaultNamespace = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#hasDefaultNamespace";

    ///<summary>
    ///States the modification time for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#modified"/>
    ///</summary>
    public const string modified = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#modified";

    ///<summary>
    ///States the creation, or first modification time for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#created"/>
    ///</summary>
    public const string created = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#created";

    ///<summary>
    ///States the last modification time for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#lastModified"/>
    ///</summary>
    public const string lastModified = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#lastModified";

    ///<summary>
    ///Specifies the status of a graph, stable, unstable or testing
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#status"/>
    ///</summary>
    public const string status = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#status";

    ///<summary>
    ///A non-technical textual annotation for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#description"/>
    ///</summary>
    public const string description = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#description";

    ///<summary>
    ///A preferred label for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#prefLabel"/>
    ///</summary>
    public const string prefLabel = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#prefLabel";

    ///<summary>
    ///The plural form of the preferred label for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#pluralPrefLabel"/>
    ///</summary>
    public const string pluralPrefLabel = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#pluralPrefLabel";

    ///<summary>
    ///Specifies the engineering tool used to generate the graph
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#engineeringTool"/>
    ///</summary>
    public const string engineeringTool = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#engineeringTool";

    ///<summary>
    ///Defines a personal string identifier for a resource
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#personalIdentifier"/>
    ///</summary>
    public const string personalIdentifier = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#personalIdentifier";

    ///<summary>
    ///If this property is assigned, the subject class, property, or resource, is deprecated and should not be used in production systems any longer. It may be removed without further notice.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#deprecated"/>
    ///</summary>
    public const string deprecated = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#deprecated";

    ///<summary>
    ///Mark a property, class, or even resource as user visible or not. Non-user-visible entities should never be presented to the user. By default everything is user-visible.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#userVisible"/>
    ///</summary>
    public const string userVisible = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#userVisible";

    ///<summary>
    ///The agent that maintains this resource, ie. created it and knows what to do with it.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#maintainedBy"/>
    ///</summary>
    public const string maintainedBy = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#maintainedBy";

    ///<summary>
    ///An agent is the artificial counterpart to nao:Party. It can be a software component or some service.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#Agent"/>
    ///</summary>
    public const string Agent = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#Agent";

    ///<summary>
    ///A generalised trust level assigned to an agent, based on a combination of direct and network trust values it possesses. Allowed values range from 0 (no trust) to 1 (very high trust).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#trustLevel"/>
    ///</summary>
    public const string trustLevel = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#trustLevel";

    ///<summary>
    ///A direct trust value assigned to an agent, either manually by a user or semi-/automatically by a system. Allowed values range from 0 (no trust) to 1 (very high trust).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#directTrust"/>
    ///</summary>
    public const string directTrust = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#directTrust";

    ///<summary>
    ///A network-derived trust value assigned to an agent, based on the shared direct trust values for the same agent, as set by participating agents in a network. Allowed values range from 0 (no trust) to 1 (very high trust).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#networkTrust"/>
    ///</summary>
    public const string networkTrust = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#networkTrust";

    ///<summary>
    ///A privacy level as defined for a resource. Allowed values range from 0 (private) to 1 (public).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#privacyLevel"/>
    ///</summary>
    public const string privacyLevel = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#privacyLevel";

    ///<summary>
    ///An external identifier for a resource that has been retreived from an external source.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#externalIdentifier"/>
    ///</summary>
    public const string externalIdentifier = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#externalIdentifier";

    ///<summary>
    ///Signifies social endorsment by an agent, by way of marking the resource as a favourite.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#favouritedBy"/>
    ///</summary>
    public const string favouritedBy = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#favouritedBy";

    ///<summary>
    ///Signifies social endorsment of a resource by a specific agent. Endorsement includes social actions like favouriting, liking, voting for, starring a resource.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/08/15/nao#endorsedBy"/>
    ///</summary>
    public const string endorsedBy = "http://www.semanticdesktop.org/ontologies/2007/08/15/nao#endorsedBy";
}
///<summary>
///NEPOMUK Information Element Core Ontology
///
///</summary>
public class nie : Ontology
{
    public static readonly Uri Namespace = new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "nie";
    public static string GetPrefix() { return Prefix; }     

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#"/>
    ///</summary>
    public static readonly Resource _22_rdf_syntax_ns = new Resource(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));    

    ///<summary>
    ///Characterset in which the content of the InformationElement was created. Example: ISO-8859-1, UTF-8. One of the registered character sets at http://www.iana.org/assignments/character-sets. This characterSet is used to interpret any textual parts of the content. If more than one characterSet is used within one data object, use more specific properties.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#characterSet"/>
    ///</summary>
    public static readonly Property characterSet = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#characterSet"));    

    ///<summary>
    ///A unit of content the user works with. This is a superclass for all interpretations of a DataObject.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#InformationElement"/>
    ///</summary>
    public static readonly Class InformationElement = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#InformationElement"));    

    ///<summary>
    ///DataObjects extracted from a single data source are organized into a containment tree. This property links the root of that tree with the datasource it has been extracted from
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#rootElementOf"/>
    ///</summary>
    public static readonly Property rootElementOf = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#rootElementOf"));    

    ///<summary>
    ///A superclass for all entities from which DataObjects can be extracted. Each entity represents a native application or some other system that manages information that may be of interest to the user of the Semantic Desktop. Subclasses may include FileSystems, Mailboxes, Calendars, websites etc. The exact choice of subclasses and their properties is considered application-specific. Each data extraction application is supposed to provide it's own DataSource ontology. Such an ontology should contain supported data source types coupled with properties necessary for the application to gain access to the data sources.  (paths, urls, passwords  etc...)
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#DataSource"/>
    ///</summary>
    public static readonly Class DataSource = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#DataSource"));    

    ///<summary>
    ///A point or period of time associated with an event in the lifecycle of an Information Element. A common superproperty for all date-related properties of InformationElements in the NIE Framework.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#informationElementDate"/>
    ///</summary>
    public static readonly Property informationElementDate = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#informationElementDate"));    

    ///<summary>
    ///A common superproperty for all properties that point at legal information about an Information Element
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#legal"/>
    ///</summary>
    public static readonly Property legal = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#legal"));    

    ///<summary>
    ///Links the information element with the DataObject it is stored in.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#isStoredAs"/>
    ///</summary>
    public static readonly Property isStoredAs = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#isStoredAs"));    

    ///<summary>
    ///A unit of data that is created, annotated and processed on the user desktop. It represents a native structure the user works with. The usage of the term 'native' is important. It means that a DataObject can be directly mapped to a data structure maintained by a native application. This may be a file, a set of files or a part of a file. The granularity depends on the user. This class is not intended to be instantiated by itself. Use more specific subclasses.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#DataObject"/>
    ///</summary>
    public static readonly Class DataObject = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#DataObject"));    

    ///<summary>
    ///Links the DataObject with the InformationElement it is interpreted as.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#interpretedAs"/>
    ///</summary>
    public static readonly Property interpretedAs = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#interpretedAs"));    

    ///<summary>
    ///Language the InformationElement is expressed in. This property applies to the data object in its entirety. If the data object is divisible into parts expressed in multiple languages - more specific properties should be used. Users are encouraged to use the two-letter code specified in the RFC 3066
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#language"/>
    ///</summary>
    public static readonly Property language = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#language"));    

    ///<summary>
    ///Content copyright
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#copyright"/>
    ///</summary>
    public static readonly Property copyright = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#copyright"));    

    ///<summary>
    ///Date the DataObject was changed in any way.  Note that this date refers to the modification of the DataObject itself (i.e. the physical representation). Compare with nie:contentModified.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#modified"/>
    ///</summary>
    public static readonly Property modified = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#modified"));    

    ///<summary>
    ///Date of creation of the DataObject. Note that this date refers to the creation of the DataObject itself (i.e. the physical representation). Compare with nie:contentCreated.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#created"/>
    ///</summary>
    public static readonly Property created = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#created"));    

    ///<summary>
    ///Last modification date of the DataObject. Note that this date refers to the modification of the DataObject itself (i.e. the physical representation). Compare with nie:contentLastModified.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#lastModified"/>
    ///</summary>
    public static readonly Property lastModified = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#lastModified"));    

    ///<summary>
    ///The mime type of the resource, if available. Example: "text/plain". See http://www.iana.org/assignments/media-types/. This property applies to data objects that can be described with one mime type. In cases where the object as a whole has one mime type, while it's parts have other mime types, or there is no mime type that can be applied to the object as a whole, but some parts of the content have mime types - use more specific properties.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#mimeType"/>
    ///</summary>
    public static readonly Property mimeType = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#mimeType"));    

    ///<summary>
    ///The current version of the given data object. Exact semantics is unspecified at this level. Use more specific subproperties if needed.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#version"/>
    ///</summary>
    public static readonly Property version = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#version"));    

    ///<summary>
    ///A linking relation. A piece of content links/mentions a piece of data
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#links"/>
    ///</summary>
    public static readonly Property links = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#links"));    

    ///<summary>
    ///A common superproperty for all relations between a piece of content and other pieces of data (which may be interpreted as other pieces of content).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#relatedTo"/>
    ///</summary>
    public static readonly Property relatedTo = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#relatedTo"));    

    ///<summary>
    ///Software used to "generate" the contents. E.g. a word processor name.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#generator"/>
    ///</summary>
    public static readonly Property generator = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#generator"));    

    ///<summary>
    ///Generic property used to express containment relationships between DataObjects. NIE extensions are encouraged to provide more specific subproperties of this one. It is advisable for actual instances of DataObjects to use those specific subproperties. Note to the developers: Please be aware of the distinction between containment relation and provenance. The isPartOf relation models physical containment, a nie:DataObject (e.g. an nfo:Attachment) is a 'physical' part of an nie:InformationElement (a nmo:Message). Also, please note the difference between physical containment (isPartOf) and logical containment (isLogicalPartOf) the former has more strict meaning. They may occur independently of each other.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#isPartOf"/>
    ///</summary>
    public static readonly Property isPartOf = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#isPartOf"));    

    ///<summary>
    ///Generic property used to express 'physical' containment relationships between DataObjects. NIE extensions are encouraged to provide more specific subproperties of this one. It is advisable for actual instances of DataObjects to use those specific subproperties. Note to the developers: Please be aware of the distinction between containment relation and provenance. The hasPart relation models physical containment, an InformationElement (a nmo:Message) can have a 'physical' part (an nfo:Attachment).  Also, please note the difference between physical containment (hasPart) and logical containment (hasLogicalPart) the former has more strict meaning. They may occur independently of each other.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#hasPart"/>
    ///</summary>
    public static readonly Property hasPart = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#hasPart"));    

    ///<summary>
    ///A disclaimer
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#disclaimer"/>
    ///</summary>
    public static readonly Property disclaimer = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#disclaimer"));    

    ///<summary>
    ///A common superproperty for all settings used by the generating software. This may include compression settings, algorithms, autosave, interlaced/non-interlaced etc. Note that this property has no range specified and therefore should not be used directly. Always use more specific properties.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#generatorOption"/>
    ///</summary>
    public static readonly Property generatorOption = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#generatorOption"));    

    ///<summary>
    ///A textual description of the resource. This property may be used for any metadata fields that provide some meta-information or comment about a resource in the form of a passage of text. This property is not to be confused with nie:plainTextContent. Use more specific subproperties wherever possible.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#description"/>
    ///</summary>
    public static readonly Property description = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#description"));    

    ///<summary>
    ///The date of the content creation. This may not necessarily be equal to the date when the DataObject (i.e. the physical representation) itself was created. Compare with nie:created property.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#contentCreated"/>
    ///</summary>
    public static readonly Property contentCreated = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#contentCreated"));    

    ///<summary>
    ///The date of a modification of the original content (not its corresponding DataObject or local copy). Compare with nie:modified.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#contentModified"/>
    ///</summary>
    public static readonly Property contentModified = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#contentModified"));    

    ///<summary>
    ///Name given to an InformationElement
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#title"/>
    ///</summary>
    public static readonly Property title = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#title"));    

    ///<summary>
    ///Date when information about this data object was retrieved (for the first time) or last refreshed from the data source. This property is important for metadata extraction applications that don't receive any notifications of changes in the data source and have to poll it regularly. This may lead to information becoming out of date. In these cases this property may be used to determine the age of data, which is an important element of it's dependability. 
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#lastRefreshed"/>
    ///</summary>
    public static readonly Property lastRefreshed = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#lastRefreshed"));    

    ///<summary>
    ///Marks the provenance of a DataObject, what source does a data object come from.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#dataSource"/>
    ///</summary>
    public static readonly Property dataSource = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#dataSource"));    

    ///<summary>
    ///Dependency relation. A piece of content depends on another piece of data in order to be properly understood/used/interpreted.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#depends"/>
    ///</summary>
    public static readonly Property depends = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#depends"));    

    ///<summary>
    ///The date of the last modification of the original content (not its corresponding DataObject or local copy). Compare with nie:lastModified.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#contentLastModified"/>
    ///</summary>
    public static readonly Property contentLastModified = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#contentLastModified"));    

    ///<summary>
    ///Adapted DublinCore: The topic of the content of the resource, as keyword. No sentences here. Recommended best practice is to select a value from a controlled vocabulary or formal classification scheme. 
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#keyword"/>
    ///</summary>
    public static readonly Property keyword = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#keyword"));    

    ///<summary>
    ///Generic property used to express 'logical' containment relationships between DataObjects. NIE extensions are encouraged to provide more specific subproperties of this one. It is advisable for actual instances of InformationElement to use those specific subproperties. Note the difference between 'physical' containment (isPartOf) and logical containment (isLogicalPartOf)
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#isLogicalPartOf"/>
    ///</summary>
    public static readonly Property isLogicalPartOf = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#isLogicalPartOf"));    

    ///<summary>
    ///Generic property used to express 'logical' containment relationships between InformationElements. NIE extensions are encouraged to provide more specific subproperties of this one. It is advisable for actual instances of InformationElement to use those specific subproperties. Note the difference between 'physical' containment (hasPart) and logical containment (hasLogicalPart)
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#hasLogicalPart"/>
    ///</summary>
    public static readonly Property hasLogicalPart = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#hasLogicalPart"));    

    ///<summary>
    ///An unambiguous reference to the InformationElement within a given context. Recommended best practice is to identify the resource by means of a string conforming to a formal identification system.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#identifier"/>
    ///</summary>
    public static readonly Property identifier = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#identifier"));    

    ///<summary>
    ///Plain-text representation of the content of a InformationElement with all markup removed. The main purpose of this property is full-text indexing and search. Its exact content is considered application-specific. The user can make no assumptions about what is and what is not contained within. Applications should use more specific properties wherever possible.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#plainTextContent"/>
    ///</summary>
    public static readonly Property plainTextContent = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#plainTextContent"));    

    ///<summary>
    ///The HTML content of an information element. This property can be used to store text including formatting in a generic fashion.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#htmlContent"/>
    ///</summary>
    public static readonly Property htmlContent = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#htmlContent"));    

    ///<summary>
    ///A user comment about an InformationElement.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#comment"/>
    ///</summary>
    public static readonly Property comment = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#comment"));    

    ///<summary>
    ///The size of the content. This property can be used whenever the size of the content of an InformationElement differs from the size of the DataObject. (e.g. because of compression, encoding, encryption or any other representation issues). The contentSize in expressed in bytes.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#contentSize"/>
    ///</summary>
    public static readonly Property contentSize = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#contentSize"));    

    ///<summary>
    ///Terms and intellectual property rights licensing conditions.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#license"/>
    ///</summary>
    public static readonly Property license = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#license"));    

    ///<summary>
    ///An overall topic of the content of a InformationElement
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#subject"/>
    ///</summary>
    public static readonly Property subject = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#subject"));    

    ///<summary>
    ///Connects the data object with the graph that contains information about it. Deprecated in favor of a more generic nao:isDataGraphFor.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#coreGraph"/>
    ///</summary>
    public static readonly Property coreGraph = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#coreGraph"));    

    ///<summary>
    ///The type of the license. Possible values for this field may include "GPL", "BSD", "Creative Commons" etc.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#licenseType"/>
    ///</summary>
    public static readonly Property licenseType = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#licenseType"));    

    ///<summary>
    ///The overall size of the data object in bytes. That means the space taken by the DataObject in its container, and not the size of the content that is of interest to the user. For cases where the content size is different (e.g. in compressed files the content is larger, in messages the content excludes headings and is smaller) use more specific properties, not necessarily subproperties of this one.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#byteSize"/>
    ///</summary>
    public static readonly Property byteSize = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#byteSize"));    

    ///<summary>
    ///URL of a DataObject. It points to the location of the object. A typial usage is FileDataObject. In cases where creating a simple file:// or http:// URL for a file is difficult (e.g. for files inside compressed archives) the applications are encouraged to use conventions defined by Apache Commons VFS Project at http://jakarta.apache.org/  commons/ vfs/ filesystems.html.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#url"/>
    ///</summary>
    public static readonly Property url = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#url"));    

    ///<summary>
    ///Represents the sum of all information that has been obtained from a data source. Each data source has its own personal information graph. When a data source is deleted, the graph becomes redundant and should also be deleted. If two or more items in two or more data source graphs are determined to be equivalent, they are integrated at the PIMO level, and the integrated representation plus the links to the original items are stored in the pimo:PersonalInformationModel graph.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#DataSourceGraph"/>
    ///</summary>
    public static readonly Resource DataSourceGraph = new Resource(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#DataSourceGraph"));    

    ///<summary>
    ///Represents a number of applicable modes for a data source.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#sourceMode"/>
    ///</summary>
    public static readonly Property sourceMode = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#sourceMode"));    

    ///<summary>
    ///Representation for a standard set of device/application/service modes, corresponding to various sets of modes that are either inbuilt in a device (e.g. inbuilt phone modes such as silent, loud, general, vibrate, etc.) or available for applications and online services (e.g. IM system modes such as busy, available, invisible, etc.)
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#Mode"/>
    ///</summary>
    public static readonly Class Mode = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#Mode"));
}
///<summary>
///NEPOMUK Information Element Core Ontology
///
///</summary>
public static class NIE
{
    public static readonly Uri Namespace = new Uri("http://www.semanticdesktop.org/ontologies/2007/01/19/nie#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "NIE";
    public static string GetPrefix() { return Prefix; } 

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#"/>
    ///</summary>
    public const string _22_rdf_syntax_ns = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";

    ///<summary>
    ///Characterset in which the content of the InformationElement was created. Example: ISO-8859-1, UTF-8. One of the registered character sets at http://www.iana.org/assignments/character-sets. This characterSet is used to interpret any textual parts of the content. If more than one characterSet is used within one data object, use more specific properties.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#characterSet"/>
    ///</summary>
    public const string characterSet = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#characterSet";

    ///<summary>
    ///A unit of content the user works with. This is a superclass for all interpretations of a DataObject.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#InformationElement"/>
    ///</summary>
    public const string InformationElement = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#InformationElement";

    ///<summary>
    ///DataObjects extracted from a single data source are organized into a containment tree. This property links the root of that tree with the datasource it has been extracted from
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#rootElementOf"/>
    ///</summary>
    public const string rootElementOf = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#rootElementOf";

    ///<summary>
    ///A superclass for all entities from which DataObjects can be extracted. Each entity represents a native application or some other system that manages information that may be of interest to the user of the Semantic Desktop. Subclasses may include FileSystems, Mailboxes, Calendars, websites etc. The exact choice of subclasses and their properties is considered application-specific. Each data extraction application is supposed to provide it's own DataSource ontology. Such an ontology should contain supported data source types coupled with properties necessary for the application to gain access to the data sources.  (paths, urls, passwords  etc...)
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#DataSource"/>
    ///</summary>
    public const string DataSource = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#DataSource";

    ///<summary>
    ///A point or period of time associated with an event in the lifecycle of an Information Element. A common superproperty for all date-related properties of InformationElements in the NIE Framework.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#informationElementDate"/>
    ///</summary>
    public const string informationElementDate = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#informationElementDate";

    ///<summary>
    ///A common superproperty for all properties that point at legal information about an Information Element
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#legal"/>
    ///</summary>
    public const string legal = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#legal";

    ///<summary>
    ///Links the information element with the DataObject it is stored in.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#isStoredAs"/>
    ///</summary>
    public const string isStoredAs = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#isStoredAs";

    ///<summary>
    ///A unit of data that is created, annotated and processed on the user desktop. It represents a native structure the user works with. The usage of the term 'native' is important. It means that a DataObject can be directly mapped to a data structure maintained by a native application. This may be a file, a set of files or a part of a file. The granularity depends on the user. This class is not intended to be instantiated by itself. Use more specific subclasses.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#DataObject"/>
    ///</summary>
    public const string DataObject = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#DataObject";

    ///<summary>
    ///Links the DataObject with the InformationElement it is interpreted as.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#interpretedAs"/>
    ///</summary>
    public const string interpretedAs = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#interpretedAs";

    ///<summary>
    ///Language the InformationElement is expressed in. This property applies to the data object in its entirety. If the data object is divisible into parts expressed in multiple languages - more specific properties should be used. Users are encouraged to use the two-letter code specified in the RFC 3066
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#language"/>
    ///</summary>
    public const string language = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#language";

    ///<summary>
    ///Content copyright
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#copyright"/>
    ///</summary>
    public const string copyright = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#copyright";

    ///<summary>
    ///Date the DataObject was changed in any way.  Note that this date refers to the modification of the DataObject itself (i.e. the physical representation). Compare with nie:contentModified.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#modified"/>
    ///</summary>
    public const string modified = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#modified";

    ///<summary>
    ///Date of creation of the DataObject. Note that this date refers to the creation of the DataObject itself (i.e. the physical representation). Compare with nie:contentCreated.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#created"/>
    ///</summary>
    public const string created = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#created";

    ///<summary>
    ///Last modification date of the DataObject. Note that this date refers to the modification of the DataObject itself (i.e. the physical representation). Compare with nie:contentLastModified.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#lastModified"/>
    ///</summary>
    public const string lastModified = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#lastModified";

    ///<summary>
    ///The mime type of the resource, if available. Example: "text/plain". See http://www.iana.org/assignments/media-types/. This property applies to data objects that can be described with one mime type. In cases where the object as a whole has one mime type, while it's parts have other mime types, or there is no mime type that can be applied to the object as a whole, but some parts of the content have mime types - use more specific properties.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#mimeType"/>
    ///</summary>
    public const string mimeType = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#mimeType";

    ///<summary>
    ///The current version of the given data object. Exact semantics is unspecified at this level. Use more specific subproperties if needed.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#version"/>
    ///</summary>
    public const string version = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#version";

    ///<summary>
    ///A linking relation. A piece of content links/mentions a piece of data
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#links"/>
    ///</summary>
    public const string links = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#links";

    ///<summary>
    ///A common superproperty for all relations between a piece of content and other pieces of data (which may be interpreted as other pieces of content).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#relatedTo"/>
    ///</summary>
    public const string relatedTo = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#relatedTo";

    ///<summary>
    ///Software used to "generate" the contents. E.g. a word processor name.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#generator"/>
    ///</summary>
    public const string generator = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#generator";

    ///<summary>
    ///Generic property used to express containment relationships between DataObjects. NIE extensions are encouraged to provide more specific subproperties of this one. It is advisable for actual instances of DataObjects to use those specific subproperties. Note to the developers: Please be aware of the distinction between containment relation and provenance. The isPartOf relation models physical containment, a nie:DataObject (e.g. an nfo:Attachment) is a 'physical' part of an nie:InformationElement (a nmo:Message). Also, please note the difference between physical containment (isPartOf) and logical containment (isLogicalPartOf) the former has more strict meaning. They may occur independently of each other.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#isPartOf"/>
    ///</summary>
    public const string isPartOf = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#isPartOf";

    ///<summary>
    ///Generic property used to express 'physical' containment relationships between DataObjects. NIE extensions are encouraged to provide more specific subproperties of this one. It is advisable for actual instances of DataObjects to use those specific subproperties. Note to the developers: Please be aware of the distinction between containment relation and provenance. The hasPart relation models physical containment, an InformationElement (a nmo:Message) can have a 'physical' part (an nfo:Attachment).  Also, please note the difference between physical containment (hasPart) and logical containment (hasLogicalPart) the former has more strict meaning. They may occur independently of each other.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#hasPart"/>
    ///</summary>
    public const string hasPart = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#hasPart";

    ///<summary>
    ///A disclaimer
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#disclaimer"/>
    ///</summary>
    public const string disclaimer = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#disclaimer";

    ///<summary>
    ///A common superproperty for all settings used by the generating software. This may include compression settings, algorithms, autosave, interlaced/non-interlaced etc. Note that this property has no range specified and therefore should not be used directly. Always use more specific properties.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#generatorOption"/>
    ///</summary>
    public const string generatorOption = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#generatorOption";

    ///<summary>
    ///A textual description of the resource. This property may be used for any metadata fields that provide some meta-information or comment about a resource in the form of a passage of text. This property is not to be confused with nie:plainTextContent. Use more specific subproperties wherever possible.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#description"/>
    ///</summary>
    public const string description = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#description";

    ///<summary>
    ///The date of the content creation. This may not necessarily be equal to the date when the DataObject (i.e. the physical representation) itself was created. Compare with nie:created property.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#contentCreated"/>
    ///</summary>
    public const string contentCreated = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#contentCreated";

    ///<summary>
    ///The date of a modification of the original content (not its corresponding DataObject or local copy). Compare with nie:modified.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#contentModified"/>
    ///</summary>
    public const string contentModified = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#contentModified";

    ///<summary>
    ///Name given to an InformationElement
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#title"/>
    ///</summary>
    public const string title = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#title";

    ///<summary>
    ///Date when information about this data object was retrieved (for the first time) or last refreshed from the data source. This property is important for metadata extraction applications that don't receive any notifications of changes in the data source and have to poll it regularly. This may lead to information becoming out of date. In these cases this property may be used to determine the age of data, which is an important element of it's dependability. 
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#lastRefreshed"/>
    ///</summary>
    public const string lastRefreshed = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#lastRefreshed";

    ///<summary>
    ///Marks the provenance of a DataObject, what source does a data object come from.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#dataSource"/>
    ///</summary>
    public const string dataSource = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#dataSource";

    ///<summary>
    ///Dependency relation. A piece of content depends on another piece of data in order to be properly understood/used/interpreted.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#depends"/>
    ///</summary>
    public const string depends = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#depends";

    ///<summary>
    ///The date of the last modification of the original content (not its corresponding DataObject or local copy). Compare with nie:lastModified.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#contentLastModified"/>
    ///</summary>
    public const string contentLastModified = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#contentLastModified";

    ///<summary>
    ///Adapted DublinCore: The topic of the content of the resource, as keyword. No sentences here. Recommended best practice is to select a value from a controlled vocabulary or formal classification scheme. 
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#keyword"/>
    ///</summary>
    public const string keyword = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#keyword";

    ///<summary>
    ///Generic property used to express 'logical' containment relationships between DataObjects. NIE extensions are encouraged to provide more specific subproperties of this one. It is advisable for actual instances of InformationElement to use those specific subproperties. Note the difference between 'physical' containment (isPartOf) and logical containment (isLogicalPartOf)
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#isLogicalPartOf"/>
    ///</summary>
    public const string isLogicalPartOf = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#isLogicalPartOf";

    ///<summary>
    ///Generic property used to express 'logical' containment relationships between InformationElements. NIE extensions are encouraged to provide more specific subproperties of this one. It is advisable for actual instances of InformationElement to use those specific subproperties. Note the difference between 'physical' containment (hasPart) and logical containment (hasLogicalPart)
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#hasLogicalPart"/>
    ///</summary>
    public const string hasLogicalPart = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#hasLogicalPart";

    ///<summary>
    ///An unambiguous reference to the InformationElement within a given context. Recommended best practice is to identify the resource by means of a string conforming to a formal identification system.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#identifier"/>
    ///</summary>
    public const string identifier = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#identifier";

    ///<summary>
    ///Plain-text representation of the content of a InformationElement with all markup removed. The main purpose of this property is full-text indexing and search. Its exact content is considered application-specific. The user can make no assumptions about what is and what is not contained within. Applications should use more specific properties wherever possible.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#plainTextContent"/>
    ///</summary>
    public const string plainTextContent = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#plainTextContent";

    ///<summary>
    ///The HTML content of an information element. This property can be used to store text including formatting in a generic fashion.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#htmlContent"/>
    ///</summary>
    public const string htmlContent = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#htmlContent";

    ///<summary>
    ///A user comment about an InformationElement.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#comment"/>
    ///</summary>
    public const string comment = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#comment";

    ///<summary>
    ///The size of the content. This property can be used whenever the size of the content of an InformationElement differs from the size of the DataObject. (e.g. because of compression, encoding, encryption or any other representation issues). The contentSize in expressed in bytes.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#contentSize"/>
    ///</summary>
    public const string contentSize = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#contentSize";

    ///<summary>
    ///Terms and intellectual property rights licensing conditions.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#license"/>
    ///</summary>
    public const string license = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#license";

    ///<summary>
    ///An overall topic of the content of a InformationElement
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#subject"/>
    ///</summary>
    public const string subject = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#subject";

    ///<summary>
    ///Connects the data object with the graph that contains information about it. Deprecated in favor of a more generic nao:isDataGraphFor.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#coreGraph"/>
    ///</summary>
    public const string coreGraph = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#coreGraph";

    ///<summary>
    ///The type of the license. Possible values for this field may include "GPL", "BSD", "Creative Commons" etc.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#licenseType"/>
    ///</summary>
    public const string licenseType = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#licenseType";

    ///<summary>
    ///The overall size of the data object in bytes. That means the space taken by the DataObject in its container, and not the size of the content that is of interest to the user. For cases where the content size is different (e.g. in compressed files the content is larger, in messages the content excludes headings and is smaller) use more specific properties, not necessarily subproperties of this one.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#byteSize"/>
    ///</summary>
    public const string byteSize = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#byteSize";

    ///<summary>
    ///URL of a DataObject. It points to the location of the object. A typial usage is FileDataObject. In cases where creating a simple file:// or http:// URL for a file is difficult (e.g. for files inside compressed archives) the applications are encouraged to use conventions defined by Apache Commons VFS Project at http://jakarta.apache.org/  commons/ vfs/ filesystems.html.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#url"/>
    ///</summary>
    public const string url = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#url";

    ///<summary>
    ///Represents the sum of all information that has been obtained from a data source. Each data source has its own personal information graph. When a data source is deleted, the graph becomes redundant and should also be deleted. If two or more items in two or more data source graphs are determined to be equivalent, they are integrated at the PIMO level, and the integrated representation plus the links to the original items are stored in the pimo:PersonalInformationModel graph.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#DataSourceGraph"/>
    ///</summary>
    public const string DataSourceGraph = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#DataSourceGraph";

    ///<summary>
    ///Represents a number of applicable modes for a data source.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#sourceMode"/>
    ///</summary>
    public const string sourceMode = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#sourceMode";

    ///<summary>
    ///Representation for a standard set of device/application/service modes, corresponding to various sets of modes that are either inbuilt in a device (e.g. inbuilt phone modes such as silent, loud, general, vibrate, etc.) or available for applications and online services (e.g. IM system modes such as busy, available, invisible, etc.)
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/01/19/nie#Mode"/>
    ///</summary>
    public const string Mode = "http://www.semanticdesktop.org/ontologies/2007/01/19/nie#Mode";
}
///<summary>
///NEPOMUK File Ontology (NFO)
///
///</summary>
public class nfo : Ontology
{
    public static readonly Uri Namespace = new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "nfo";
    public static string GetPrefix() { return Prefix; }     

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#"/>
    ///</summary>
    public static readonly Resource _22_rdf_syntax_ns = new Resource(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));    

    ///<summary>
    ///Horizontal resolution of an image (if printed). Expressed in DPI.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#horizontalResolution"/>
    ///</summary>
    public static readonly Property horizontalResolution = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#horizontalResolution"));    

    ///<summary>
    ///A file containing an image.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Image"/>
    ///</summary>
    public static readonly Class Image = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Image"));    

    ///<summary>
    ///The amount of audio samples per second.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#sampleRate"/>
    ///</summary>
    public static readonly Property sampleRate = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#sampleRate"));    

    ///<summary>
    ///A file containing audio content
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Audio"/>
    ///</summary>
    public static readonly Class Audio = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Audio"));    

    ///<summary>
    ///A piece of media content. This class may be used to express complex media containers with many streams of various media content (both aural and visual).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Media"/>
    ///</summary>
    public static readonly Class Media = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Media"));    

    ///<summary>
    ///A common superproperty for all properties specifying the media rate. Examples of subproperties may include frameRate for video and sampleRate for audio. This property is expressed in units per second.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#rate"/>
    ///</summary>
    public static readonly Property rate = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#rate"));    

    ///<summary>
    ///A partition on a hard disk
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#HardDiskPartition"/>
    ///</summary>
    public static readonly Class HardDiskPartition = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#HardDiskPartition"));    

    ///<summary>
    ///Name of the file, together with the extension
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileName"/>
    ///</summary>
    public static readonly Property fileName = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileName"));    

    ///<summary>
    ///A resource containing a finite sequence of bytes with arbitrary information, that is available to a computer program and is usually based on some kind of durable storage. A file is durable in the sense that it remains available for programs to use after the current program has finished.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#FileDataObject"/>
    ///</summary>
    public static readonly Class FileDataObject = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#FileDataObject"));    

    ///<summary>
    ///A stream of multimedia content, usually contained within a media container such as a movie (containing both audio and video) or a DVD (possibly containing many streams of audio and video). Most common interpretations for such a DataObject include Audio and Video.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#MediaStream"/>
    ///</summary>
    public static readonly Class MediaStream = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#MediaStream"));    

    ///<summary>
    ///A Presentation made by some presentation software (Corel Presentations, OpenOffice Impress, MS Powerpoint etc.)
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Presentation"/>
    ///</summary>
    public static readonly Class Presentation = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Presentation"));    

    ///<summary>
    ///A generic document. A common superclass for all documents on the desktop.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Document"/>
    ///</summary>
    public static readonly Class Document = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Document"));    

    ///<summary>
    ///Name of the algorithm used to compute the hash value. Examples might include CRC32, MD5, SHA, TTH etc.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hashAlgorithm"/>
    ///</summary>
    public static readonly Property hashAlgorithm = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hashAlgorithm"));    

    ///<summary>
    ///A fingerprint of the file, generated by some hashing function.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#FileHash"/>
    ///</summary>
    public static readonly Class FileHash = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#FileHash"));    

    ///<summary>
    ///The amount of character in comments i.e. characters ignored by the compiler/interpreter.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#commentCharacterCount"/>
    ///</summary>
    public static readonly Property commentCharacterCount = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#commentCharacterCount"));    

    ///<summary>
    ///Code in a compilable or interpreted programming language.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#SourceCode"/>
    ///</summary>
    public static readonly Class SourceCode = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#SourceCode"));    

    ///<summary>
    ///A file containing plain text (ASCII, Unicode or other encodings). Examples may include TXT, HTML, XML, program source code etc.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#PlainTextDocument"/>
    ///</summary>
    public static readonly Class PlainTextDocument = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#PlainTextDocument"));    

    ///<summary>
    ///A text document
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#TextDocument"/>
    ///</summary>
    public static readonly Class TextDocument = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#TextDocument"));    

    ///<summary>
    ///The foundry, the organization that created the font.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#foundry"/>
    ///</summary>
    public static readonly Property foundry = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#foundry"));    

    ///<summary>
    ///A font.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Font"/>
    ///</summary>
    public static readonly Class Font = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Font"));    

    ///<summary>
    ///
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#losslessCompressionType"/>
    ///</summary>
    public static readonly Resource losslessCompressionType = new Resource(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#losslessCompressionType"));    

    ///<summary>
    ///Number of side channels
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#sideChannels"/>
    ///</summary>
    public static readonly Property sideChannels = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#sideChannels"));    

    ///<summary>
    ///Number of channels. This property is to be used directly if no detailed information is necessary. Otherwise use more detailed subproperties.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#channels"/>
    ///</summary>
    public static readonly Property channels = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#channels"));    

    ///<summary>
    ///True if the image is interlaced, false if not.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#interlaceMode"/>
    ///</summary>
    public static readonly Property interlaceMode = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#interlaceMode"));    

    ///<summary>
    ///File containing visual content.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Visual"/>
    ///</summary>
    public static readonly Class Visual = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Visual"));    

    ///<summary>
    ///Visual content width in pixels.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#width"/>
    ///</summary>
    public static readonly Property width = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#width"));    

    ///<summary>
    ///The amount of frames in a video sequence.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#frameCount"/>
    ///</summary>
    public static readonly Property frameCount = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#frameCount"));    

    ///<summary>
    ///A video file.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Video"/>
    ///</summary>
    public static readonly Class Video = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Video"));    

    ///<summary>
    ///A common superproperty for all properties signifying the amount of atomic media data units. Examples of subproperties may include sampleCount and frameCount.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#count"/>
    ///</summary>
    public static readonly Property count = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#count"));    

    ///<summary>
    ///A filesystem. Examples of filesystems include hard disk partitions, removable media, but also images thereof stored in files such as ISO.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Filesystem"/>
    ///</summary>
    public static readonly Class Filesystem = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Filesystem"));    

    ///<summary>
    ///A superclass for all entities, whose primary purpose is to serve as containers for other data object. They usually don't have any "meaning" by themselves. Examples include folders, archives and optical disc images.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#DataContainer"/>
    ///</summary>
    public static readonly Class DataContainer = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#DataContainer"));    

    ///<summary>
    ///Type of filesystem such as ext3 and ntfs.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#filesystemType"/>
    ///</summary>
    public static readonly Property filesystemType = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#filesystemType"));    

    ///<summary>
    ///Total storage space of the filesystem, which can be different from nie:contentSize because the latter includes filesystem format overhead.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#totalSpace"/>
    ///</summary>
    public static readonly Property totalSpace = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#totalSpace"));    

    ///<summary>
    ///Unoccupied storage space of the filesystem.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#freeSpace"/>
    ///</summary>
    public static readonly Property freeSpace = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#freeSpace"));    

    ///<summary>
    ///Occupied storage space of the filesystem.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#occupiedSpace"/>
    ///</summary>
    public static readonly Property occupiedSpace = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#occupiedSpace"));    

    ///<summary>
    ///Universally unique identifier of the filesystem. In the future, this property may have its parent changed to a more generic class.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#uuid"/>
    ///</summary>
    public static readonly Property uuid = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#uuid"));    

    ///<summary>
    ///A name of a function/method defined in the given source code file.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#definesFunction"/>
    ///</summary>
    public static readonly Property definesFunction = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#definesFunction"));    

    ///<summary>
    ///A string containing the permissions of a file. A feature common in many UNIX-like operating systems.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#permissions"/>
    ///</summary>
    public static readonly Property permissions = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#permissions"));    

    ///<summary>
    ///The amount of lines in a text document
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#lineCount"/>
    ///</summary>
    public static readonly Property lineCount = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#lineCount"));    

    ///<summary>
    ///A DataObject representing a piece of software. Examples of interpretations of a SoftwareItem include an Application and an OperatingSystem.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#SoftwareItem"/>
    ///</summary>
    public static readonly Class SoftwareItem = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#SoftwareItem"));    

    ///<summary>
    ///The amount of words in a text document.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#wordCount"/>
    ///</summary>
    public static readonly Property wordCount = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#wordCount"));    

    ///<summary>
    ///The address of the linked object. Usually a web URI.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#bookmarks"/>
    ///</summary>
    public static readonly Property bookmarks = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#bookmarks"));    

    ///<summary>
    ///A bookmark of a webbrowser. Use nie:title for the name/label, nie:contentCreated to represent the date when the user added the bookmark, and nie:contentLastModified for modifications. nfo:bookmarks to store the link.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Bookmark"/>
    ///</summary>
    public static readonly Class Bookmark = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Bookmark"));    

    ///<summary>
    ///Character position of the bookmark.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#characterPosition"/>
    ///</summary>
    public static readonly Property characterPosition = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#characterPosition"));    

    ///<summary>
    ///Page linked by the bookmark.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#pageNumber"/>
    ///</summary>
    public static readonly Property pageNumber = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#pageNumber"));    

    ///<summary>
    ///Stream position of the bookmark, suitable for e.g. audio books. Expressed in milliseconds
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#streamPosition"/>
    ///</summary>
    public static readonly Property streamPosition = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#streamPosition"));    

    ///<summary>
    ///An address specifying a remote host and port. Such an address can be interpreted in many ways (examples of such interpretations include mailboxes, websites, remote calendars or filesystems), depending on an interpretation, various kinds of data may be extracted from such an address.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#RemotePortAddress"/>
    ///</summary>
    public static readonly Class RemotePortAddress = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#RemotePortAddress"));    

    ///<summary>
    ///A file attached to another data object. Many data formats allow for attachments: emails, vcards, ical events, id3 and exif...
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Attachment"/>
    ///</summary>
    public static readonly Class Attachment = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Attachment"));    

    ///<summary>
    ///A file embedded in another data object. There are many ways in which a file may be embedded in another one. Use this class directly only in cases if none of the subclasses gives a better description of your case.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#EmbeddedFileDataObject"/>
    ///</summary>
    public static readonly Class EmbeddedFileDataObject = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#EmbeddedFileDataObject"));    

    ///<summary>
    ///The amount of characters in the document.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#characterCount"/>
    ///</summary>
    public static readonly Property characterCount = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#characterCount"));    

    ///<summary>
    ///Time when the file was last accessed.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileLastAccessed"/>
    ///</summary>
    public static readonly Property fileLastAccessed = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileLastAccessed"));    

    ///<summary>
    ///States that a piece of software supercedes another piece of software.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#supercedes"/>
    ///</summary>
    public static readonly Property supercedes = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#supercedes"));    

    ///<summary>
    ///A piece of software. Examples may include applications and the operating system. This interpretation most commonly applies to SoftwareItems.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Software"/>
    ///</summary>
    public static readonly Class Software = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Software"));    

    ///<summary>
    ///Indicates the name of the programming language this source code file is written in. Examples might include 'C', 'C++', 'Java' etc.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#programmingLanguage"/>
    ///</summary>
    public static readonly Property programmingLanguage = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#programmingLanguage"));    

    ///<summary>
    ///An application
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Application"/>
    ///</summary>
    public static readonly Class Application = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Application"));    

    ///<summary>
    ///The amount of samples in an audio clip.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#sampleCount"/>
    ///</summary>
    public static readonly Property sampleCount = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#sampleCount"));    

    ///<summary>
    ///Visual content height in pixels.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#height"/>
    ///</summary>
    public static readonly Property height = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#height"));    

    ///<summary>
    ///Number of front channels.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#frontChannels"/>
    ///</summary>
    public static readonly Property frontChannels = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#frontChannels"));    

    ///<summary>
    ///An image of a filesystem. Instances of this class may include CD images, DVD images or hard disk partition images created by various pieces of software (e.g. Norton Ghost). Deprecated in favor of nfo:Filesystem.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#FilesystemImage"/>
    ///</summary>
    public static readonly Class FilesystemImage = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#FilesystemImage"));    

    ///<summary>
    ///Number of rear channels.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#rearChannels"/>
    ///</summary>
    public static readonly Property rearChannels = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#rearChannels"));    

    ///<summary>
    ///Amount of bits in each audio sample.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#bitsPerSample"/>
    ///</summary>
    public static readonly Property bitsPerSample = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#bitsPerSample"));    

    ///<summary>
    ///A common superproperty for all properties signifying the amount of bits for an atomic unit of data. Examples of subproperties may include bitsPerSample and bitsPerPixel
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#bitDepth"/>
    ///</summary>
    public static readonly Property bitDepth = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#bitDepth"));    

    ///<summary>
    ///A HTML document, may contain links to other files.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#HtmlDocument"/>
    ///</summary>
    public static readonly Class HtmlDocument = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#HtmlDocument"));    

    ///<summary>
    ///Duration of a media piece.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#duration"/>
    ///</summary>
    public static readonly Property duration = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#duration"));    

    ///<summary>
    ///Number of Low Frequency Expansion (subwoofer) channels.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#lfeChannels"/>
    ///</summary>
    public static readonly Property lfeChannels = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#lfeChannels"));    

    ///<summary>
    ///Connects a media container with a single media stream contained within.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hasMediaStream"/>
    ///</summary>
    public static readonly Property hasMediaStream = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hasMediaStream"));    

    ///<summary>
    ///A spreadsheet, created by a spreadsheet application. Examples might include Gnumeric, OpenOffice Calc or MS Excel.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Spreadsheet"/>
    ///</summary>
    public static readonly Class Spreadsheet = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Spreadsheet"));    

    ///<summary>
    ///States if a given resource is password-protected.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#isPasswordProtected"/>
    ///</summary>
    public static readonly Property isPasswordProtected = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#isPasswordProtected"));    

    ///<summary>
    ///A file entity inside an archive.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#ArchiveItem"/>
    ///</summary>
    public static readonly Class ArchiveItem = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#ArchiveItem"));    

    ///<summary>
    ///The actual value of the hash.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hashValue"/>
    ///</summary>
    public static readonly Property hashValue = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hashValue"));    

    ///<summary>
    ///
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#encryptedStatus"/>
    ///</summary>
    public static readonly Resource encryptedStatus = new Resource(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#encryptedStatus"));    

    ///<summary>
    ///Uncompressed size of the content of a compressed file.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#uncompressedSize"/>
    ///</summary>
    public static readonly Property uncompressedSize = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#uncompressedSize"));    

    ///<summary>
    ///A compressed file. May contain other files or folder inside. 
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Archive"/>
    ///</summary>
    public static readonly Class Archive = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Archive"));    

    ///<summary>
    ///The date and time of the deletion.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#deletionDate"/>
    ///</summary>
    public static readonly Property deletionDate = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#deletionDate"));    

    ///<summary>
    ///A file entity that has been deleted from the original source. Usually such entities are stored within various kinds of 'Trash' or 'Recycle Bin' folders.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#DeletedResource"/>
    ///</summary>
    public static readonly Class DeletedResource = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#DeletedResource"));    

    ///<summary>
    ///A MindMap, created by a mind-mapping utility. Examples might include FreeMind or mind mapper.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#MindMap"/>
    ///</summary>
    public static readonly Class MindMap = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#MindMap"));    

    ///<summary>
    ///A service published by a piece of software, either by an operating system or an application. Examples of such services may include calendar, addressbook and mailbox managed by a PIM application. This category is introduced to distinguish between data available directly from the applications (Via some Interprocess Communication Mechanisms) and data available from files on a disk. In either case both DataObjects would receive a similar interpretation (e.g. a Mailbox) and wouldn't differ on the content level.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#SoftwareService"/>
    ///</summary>
    public static readonly Class SoftwareService = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#SoftwareService"));    

    ///<summary>
    ///
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#decryptedStatus"/>
    ///</summary>
    public static readonly Resource decryptedStatus = new Resource(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#decryptedStatus"));    

    ///<summary>
    ///The original location of the deleted resource.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#originalLocation"/>
    ///</summary>
    public static readonly Property originalLocation = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#originalLocation"));    

    ///<summary>
    ///A website, usually a container for remote resources, that may be interpreted as HTMLDocuments, images or other types of content.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Website"/>
    ///</summary>
    public static readonly Class Website = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Website"));    

    ///<summary>
    ///
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#VectorImage"/>
    ///</summary>
    public static readonly Class VectorImage = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#VectorImage"));    

    ///<summary>
    ///A Cursor.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Cursor"/>
    ///</summary>
    public static readonly Class Cursor = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Cursor"));    

    ///<summary>
    ///A raster image.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#RasterImage"/>
    ///</summary>
    public static readonly Class RasterImage = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#RasterImage"));    

    ///<summary>
    ///This property is intended to point to an RDF list of MediaFiles.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hasMediaFileListEntry"/>
    ///</summary>
    public static readonly Property hasMediaFileListEntry = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hasMediaFileListEntry"));    

    ///<summary>
    ///A file containing a list of media files.e.g. a playlist
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#MediaList"/>
    ///</summary>
    public static readonly Class MediaList = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#MediaList"));    

    ///<summary>
    ///A single node in the list of media files contained within an MediaList instance. This class is intended to provide a type all those links have. In valid NRL untyped resources cannot be linked. There are no properties defined for this class but the application may expect rdf:first and rdf:last links. The former points to the DataObject instance, interpreted as Media the latter points at another MediaFileListEntr. At the end of the list there is a link to rdf:nil.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#MediaFileListEntry"/>
    ///</summary>
    public static readonly Class MediaFileListEntry = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#MediaFileListEntry"));    

    ///<summary>
    ///A folder with bookmarks of a webbrowser. Use nfo:containsBookmark to relate Bookmarks. Folders can contain subfolders, use containsBookmarkFolder to relate them.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#BookmarkFolder"/>
    ///</summary>
    public static readonly Class BookmarkFolder = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#BookmarkFolder"));    

    ///<summary>
    ///Amount of bits used to express the color of each pixel.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#colorDepth"/>
    ///</summary>
    public static readonly Property colorDepth = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#colorDepth"));    

    ///<summary>
    ///The average overall bitrate of a media container. (i.e. the size of the piece of media in bits, divided by it's duration expressed in seconds).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#averageBitrate"/>
    ///</summary>
    public static readonly Property averageBitrate = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#averageBitrate"));    

    ///<summary>
    ///An Icon (regardless of whether it's a raster or a vector icon. A resource representing an icon could have two types (Icon and Raster, or Icon and Vector) if required.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Icon"/>
    ///</summary>
    public static readonly Class Icon = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Icon"));    

    ///<summary>
    ///The owner of the file as defined by the file system access rights feature.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileOwner"/>
    ///</summary>
    public static readonly Property fileOwner = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileOwner"));    

    ///<summary>
    ///Visual content aspect ratio. (Width divided by Height)
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#aspectRatio"/>
    ///</summary>
    public static readonly Property aspectRatio = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#aspectRatio"));    

    ///<summary>
    ///The folder contains a bookmark folder.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#containsBookmarkFolder"/>
    ///</summary>
    public static readonly Property containsBookmarkFolder = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#containsBookmarkFolder"));    

    ///<summary>
    ///Models the containment relations between Files and Folders (or CompressedFiles).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#belongsToContainer"/>
    ///</summary>
    public static readonly Property belongsToContainer = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#belongsToContainer"));    

    ///<summary>
    ///Vertical resolution of an Image (if printed). Expressed in DPI
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#verticalResolution"/>
    ///</summary>
    public static readonly Property verticalResolution = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#verticalResolution"));    

    ///<summary>
    ///URL of the file. It points at the location of the file. In cases where creating a simple file:// or http:// URL for a file is difficult (e.g. for files inside compressed archives) the applications are encouraged to use conventions defined by Apache Commons VFS Project at http://jakarta.apache.org/  commons/ vfs/ filesystems.html.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileUrl"/>
    ///</summary>
    public static readonly Property fileUrl = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileUrl"));    

    ///<summary>
    ///Amount of video frames per second.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#frameRate"/>
    ///</summary>
    public static readonly Property frameRate = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#frameRate"));    

    ///<summary>
    ///The name of the font family.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fontFamily"/>
    ///</summary>
    public static readonly Property fontFamily = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fontFamily"));    

    ///<summary>
    ///File creation date
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileCreated"/>
    ///</summary>
    public static readonly Property fileCreated = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileCreated"));    

    ///<summary>
    ///The type of the bitrate. Examples may include CBR and VBR.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#bitrateType"/>
    ///</summary>
    public static readonly Property bitrateType = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#bitrateType"));    

    ///<summary>
    ///The encoding used for the Embedded File. Examples might include BASE64 or UUEncode
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#encoding"/>
    ///</summary>
    public static readonly Property encoding = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#encoding"));    

    ///<summary>
    ///A folder/directory. Examples of folders include folders on a filesystem and message folders in a mailbox.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Folder"/>
    ///</summary>
    public static readonly Class Folder = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Folder"));    

    ///<summary>
    ///Links the file with it's hash value.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hasHash"/>
    ///</summary>
    public static readonly Property hasHash = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hasHash"));    

    ///<summary>
    ///The name of the codec necessary to decode a piece of media.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#codec"/>
    ///</summary>
    public static readonly Property codec = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#codec"));    

    ///<summary>
    ///last modification date
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileLastModified"/>
    ///</summary>
    public static readonly Property fileLastModified = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileLastModified"));    

    ///<summary>
    ///The type of the compression. Values include, 'lossy' and 'lossless'.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#compressionType"/>
    ///</summary>
    public static readonly Property compressionType = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#compressionType"));    

    ///<summary>
    ///Type of compression. Instances of this class represent the limited set of values allowed for the nfo:compressionType property.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#CompressionType"/>
    ///</summary>
    public static readonly Class CompressionType = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#CompressionType"));    

    ///<summary>
    ///Number of pages.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#pageCount"/>
    ///</summary>
    public static readonly Property pageCount = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#pageCount"));    

    ///<summary>
    ///A file containing a text document, that is unambiguously divided into pages. Examples might include PDF, DOC, PS, DVI etc.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#PaginatedTextDocument"/>
    ///</summary>
    public static readonly Class PaginatedTextDocument = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#PaginatedTextDocument"));    

    ///<summary>
    ///Name of a global variable defined within the source code file.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#definesGlobalVariable"/>
    ///</summary>
    public static readonly Property definesGlobalVariable = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#definesGlobalVariable"));    

    ///<summary>
    ///Represents a container for deleted files, a feature common in modern operating systems.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Trash"/>
    ///</summary>
    public static readonly Class Trash = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Trash"));    

    ///<summary>
    ///States that a piece of software is in conflict with another piece of software.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#conflicts"/>
    ///</summary>
    public static readonly Property conflicts = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#conflicts"));    

    ///<summary>
    ///The status of the encryption of the InformationElement.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#encryptionStatus"/>
    ///</summary>
    public static readonly Property encryptionStatus = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#encryptionStatus"));    

    ///<summary>
    ///The status of the encryption of an InformationElement. nfo:encryptedStatus means that the InformationElement has been encrypted and couldn't be decrypted by the extraction software, thus no content is available. nfo:decryptedStatus means that decryption was successfull and the content is available.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#EncryptionStatus"/>
    ///</summary>
    public static readonly Class EncryptionStatus = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#EncryptionStatus"));    

    ///<summary>
    ///The folder contains a bookmark.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#containsBookmark"/>
    ///</summary>
    public static readonly Property containsBookmark = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#containsBookmark"));    

    ///<summary>
    ///An executable file.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Executable"/>
    ///</summary>
    public static readonly Class Executable = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Executable"));    

    ///<summary>
    ///Name of a class defined in the source code file.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#definesClass"/>
    ///</summary>
    public static readonly Property definesClass = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#definesClass"));    

    ///<summary>
    ///
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#lossyCompressionType"/>
    ///</summary>
    public static readonly Resource lossyCompressionType = new Resource(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#lossyCompressionType"));    

    ///<summary>
    ///An OperatingSystem
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#OperatingSystem"/>
    ///</summary>
    public static readonly Class OperatingSystem = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#OperatingSystem"));    

    ///<summary>
    ///The size of the file in bytes. For compressed files it means the size of the packed file, not of the contents. For folders it means the aggregated size of all contained files and folders 
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileSize"/>
    ///</summary>
    public static readonly Property fileSize = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileSize"));    

    ///<summary>
    ///A file data object stored at a remote location. Don't confuse this class with a RemotePortAddress. This one applies to a particular resource, RemotePortAddress applies to an address, that can have various interpretations.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#RemoteDataObject"/>
    ///</summary>
    public static readonly Class RemoteDataObject = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#RemoteDataObject"));    

    ///<summary>
    ///The number of colors used/available in a raster image.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#colorCount"/>
    ///</summary>
    public static readonly Property colorCount = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#colorCount"));    

    ///<summary>
    ///The number of colors defined in palette of the raster image.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#paletteSize"/>
    ///</summary>
    public static readonly Property paletteSize = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#paletteSize"));    

    ///<summary>
    ///An information resources of which representations (files, streams) can be retrieved through a web server. They may be generated at retrieval time. Typical examples are pages served by PHP or AJAX or mp3 streams.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#WebDataObject"/>
    ///</summary>
    public static readonly Class WebDataObject = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#WebDataObject"));    

    ///<summary>
    ///A local file data object which is stored on a local file system. Its nie:url always uses the file:/ protocol. The main use of this class is to distinguish local and non-local files.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#LocalFileDataObject"/>
    ///</summary>
    public static readonly Class LocalFileDataObject = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#LocalFileDataObject"));    

    ///<summary>
    ///Relates an information element to an image which depicts said element.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#depiction"/>
    ///</summary>
    public static readonly Property depiction = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#depiction"));    

    ///<summary>
    ///Relates an image to the information elements it depicts.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#depicts"/>
    ///</summary>
    public static readonly Property depicts = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#depicts"));    

    ///<summary>
    ///Containment relation between placemark containers (files) and placemarks within.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#containsPlacemark"/>
    ///</summary>
    public static readonly Property containsPlacemark = new Property(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#containsPlacemark"));    

    ///<summary>
    ///A data object containing placemark(s). Use nie:contentCreated to represent the date when the user created the dataobject, nao:creator for defining the creator, nie:contentLastModified for modifications. nfo:containsPlacemark to refer to individual placemarks within.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#PlacemarkContainer"/>
    ///</summary>
    public static readonly Class PlacemarkContainer = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#PlacemarkContainer"));    

    ///<summary>
    ///One placemark within a placemark container/file. Use nie:title for the name/label, nao:creator for defining the creator.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Placemark"/>
    ///</summary>
    public static readonly Class Placemark = new Class(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Placemark"));
}
///<summary>
///NEPOMUK File Ontology (NFO)
///
///</summary>
public static class NFO
{
    public static readonly Uri Namespace = new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "NFO";
    public static string GetPrefix() { return Prefix; } 

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#"/>
    ///</summary>
    public const string _22_rdf_syntax_ns = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";

    ///<summary>
    ///Horizontal resolution of an image (if printed). Expressed in DPI.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#horizontalResolution"/>
    ///</summary>
    public const string horizontalResolution = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#horizontalResolution";

    ///<summary>
    ///A file containing an image.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Image"/>
    ///</summary>
    public const string Image = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Image";

    ///<summary>
    ///The amount of audio samples per second.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#sampleRate"/>
    ///</summary>
    public const string sampleRate = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#sampleRate";

    ///<summary>
    ///A file containing audio content
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Audio"/>
    ///</summary>
    public const string Audio = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Audio";

    ///<summary>
    ///A piece of media content. This class may be used to express complex media containers with many streams of various media content (both aural and visual).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Media"/>
    ///</summary>
    public const string Media = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Media";

    ///<summary>
    ///A common superproperty for all properties specifying the media rate. Examples of subproperties may include frameRate for video and sampleRate for audio. This property is expressed in units per second.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#rate"/>
    ///</summary>
    public const string rate = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#rate";

    ///<summary>
    ///A partition on a hard disk
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#HardDiskPartition"/>
    ///</summary>
    public const string HardDiskPartition = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#HardDiskPartition";

    ///<summary>
    ///Name of the file, together with the extension
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileName"/>
    ///</summary>
    public const string fileName = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileName";

    ///<summary>
    ///A resource containing a finite sequence of bytes with arbitrary information, that is available to a computer program and is usually based on some kind of durable storage. A file is durable in the sense that it remains available for programs to use after the current program has finished.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#FileDataObject"/>
    ///</summary>
    public const string FileDataObject = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#FileDataObject";

    ///<summary>
    ///A stream of multimedia content, usually contained within a media container such as a movie (containing both audio and video) or a DVD (possibly containing many streams of audio and video). Most common interpretations for such a DataObject include Audio and Video.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#MediaStream"/>
    ///</summary>
    public const string MediaStream = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#MediaStream";

    ///<summary>
    ///A Presentation made by some presentation software (Corel Presentations, OpenOffice Impress, MS Powerpoint etc.)
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Presentation"/>
    ///</summary>
    public const string Presentation = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Presentation";

    ///<summary>
    ///A generic document. A common superclass for all documents on the desktop.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Document"/>
    ///</summary>
    public const string Document = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Document";

    ///<summary>
    ///Name of the algorithm used to compute the hash value. Examples might include CRC32, MD5, SHA, TTH etc.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hashAlgorithm"/>
    ///</summary>
    public const string hashAlgorithm = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hashAlgorithm";

    ///<summary>
    ///A fingerprint of the file, generated by some hashing function.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#FileHash"/>
    ///</summary>
    public const string FileHash = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#FileHash";

    ///<summary>
    ///The amount of character in comments i.e. characters ignored by the compiler/interpreter.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#commentCharacterCount"/>
    ///</summary>
    public const string commentCharacterCount = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#commentCharacterCount";

    ///<summary>
    ///Code in a compilable or interpreted programming language.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#SourceCode"/>
    ///</summary>
    public const string SourceCode = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#SourceCode";

    ///<summary>
    ///A file containing plain text (ASCII, Unicode or other encodings). Examples may include TXT, HTML, XML, program source code etc.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#PlainTextDocument"/>
    ///</summary>
    public const string PlainTextDocument = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#PlainTextDocument";

    ///<summary>
    ///A text document
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#TextDocument"/>
    ///</summary>
    public const string TextDocument = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#TextDocument";

    ///<summary>
    ///The foundry, the organization that created the font.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#foundry"/>
    ///</summary>
    public const string foundry = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#foundry";

    ///<summary>
    ///A font.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Font"/>
    ///</summary>
    public const string Font = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Font";

    ///<summary>
    ///
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#losslessCompressionType"/>
    ///</summary>
    public const string losslessCompressionType = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#losslessCompressionType";

    ///<summary>
    ///Number of side channels
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#sideChannels"/>
    ///</summary>
    public const string sideChannels = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#sideChannels";

    ///<summary>
    ///Number of channels. This property is to be used directly if no detailed information is necessary. Otherwise use more detailed subproperties.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#channels"/>
    ///</summary>
    public const string channels = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#channels";

    ///<summary>
    ///True if the image is interlaced, false if not.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#interlaceMode"/>
    ///</summary>
    public const string interlaceMode = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#interlaceMode";

    ///<summary>
    ///File containing visual content.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Visual"/>
    ///</summary>
    public const string Visual = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Visual";

    ///<summary>
    ///Visual content width in pixels.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#width"/>
    ///</summary>
    public const string width = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#width";

    ///<summary>
    ///The amount of frames in a video sequence.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#frameCount"/>
    ///</summary>
    public const string frameCount = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#frameCount";

    ///<summary>
    ///A video file.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Video"/>
    ///</summary>
    public const string Video = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Video";

    ///<summary>
    ///A common superproperty for all properties signifying the amount of atomic media data units. Examples of subproperties may include sampleCount and frameCount.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#count"/>
    ///</summary>
    public const string count = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#count";

    ///<summary>
    ///A filesystem. Examples of filesystems include hard disk partitions, removable media, but also images thereof stored in files such as ISO.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Filesystem"/>
    ///</summary>
    public const string Filesystem = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Filesystem";

    ///<summary>
    ///A superclass for all entities, whose primary purpose is to serve as containers for other data object. They usually don't have any "meaning" by themselves. Examples include folders, archives and optical disc images.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#DataContainer"/>
    ///</summary>
    public const string DataContainer = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#DataContainer";

    ///<summary>
    ///Type of filesystem such as ext3 and ntfs.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#filesystemType"/>
    ///</summary>
    public const string filesystemType = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#filesystemType";

    ///<summary>
    ///Total storage space of the filesystem, which can be different from nie:contentSize because the latter includes filesystem format overhead.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#totalSpace"/>
    ///</summary>
    public const string totalSpace = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#totalSpace";

    ///<summary>
    ///Unoccupied storage space of the filesystem.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#freeSpace"/>
    ///</summary>
    public const string freeSpace = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#freeSpace";

    ///<summary>
    ///Occupied storage space of the filesystem.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#occupiedSpace"/>
    ///</summary>
    public const string occupiedSpace = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#occupiedSpace";

    ///<summary>
    ///Universally unique identifier of the filesystem. In the future, this property may have its parent changed to a more generic class.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#uuid"/>
    ///</summary>
    public const string uuid = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#uuid";

    ///<summary>
    ///A name of a function/method defined in the given source code file.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#definesFunction"/>
    ///</summary>
    public const string definesFunction = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#definesFunction";

    ///<summary>
    ///A string containing the permissions of a file. A feature common in many UNIX-like operating systems.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#permissions"/>
    ///</summary>
    public const string permissions = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#permissions";

    ///<summary>
    ///The amount of lines in a text document
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#lineCount"/>
    ///</summary>
    public const string lineCount = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#lineCount";

    ///<summary>
    ///A DataObject representing a piece of software. Examples of interpretations of a SoftwareItem include an Application and an OperatingSystem.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#SoftwareItem"/>
    ///</summary>
    public const string SoftwareItem = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#SoftwareItem";

    ///<summary>
    ///The amount of words in a text document.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#wordCount"/>
    ///</summary>
    public const string wordCount = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#wordCount";

    ///<summary>
    ///The address of the linked object. Usually a web URI.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#bookmarks"/>
    ///</summary>
    public const string bookmarks = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#bookmarks";

    ///<summary>
    ///A bookmark of a webbrowser. Use nie:title for the name/label, nie:contentCreated to represent the date when the user added the bookmark, and nie:contentLastModified for modifications. nfo:bookmarks to store the link.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Bookmark"/>
    ///</summary>
    public const string Bookmark = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Bookmark";

    ///<summary>
    ///Character position of the bookmark.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#characterPosition"/>
    ///</summary>
    public const string characterPosition = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#characterPosition";

    ///<summary>
    ///Page linked by the bookmark.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#pageNumber"/>
    ///</summary>
    public const string pageNumber = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#pageNumber";

    ///<summary>
    ///Stream position of the bookmark, suitable for e.g. audio books. Expressed in milliseconds
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#streamPosition"/>
    ///</summary>
    public const string streamPosition = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#streamPosition";

    ///<summary>
    ///An address specifying a remote host and port. Such an address can be interpreted in many ways (examples of such interpretations include mailboxes, websites, remote calendars or filesystems), depending on an interpretation, various kinds of data may be extracted from such an address.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#RemotePortAddress"/>
    ///</summary>
    public const string RemotePortAddress = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#RemotePortAddress";

    ///<summary>
    ///A file attached to another data object. Many data formats allow for attachments: emails, vcards, ical events, id3 and exif...
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Attachment"/>
    ///</summary>
    public const string Attachment = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Attachment";

    ///<summary>
    ///A file embedded in another data object. There are many ways in which a file may be embedded in another one. Use this class directly only in cases if none of the subclasses gives a better description of your case.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#EmbeddedFileDataObject"/>
    ///</summary>
    public const string EmbeddedFileDataObject = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#EmbeddedFileDataObject";

    ///<summary>
    ///The amount of characters in the document.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#characterCount"/>
    ///</summary>
    public const string characterCount = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#characterCount";

    ///<summary>
    ///Time when the file was last accessed.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileLastAccessed"/>
    ///</summary>
    public const string fileLastAccessed = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileLastAccessed";

    ///<summary>
    ///States that a piece of software supercedes another piece of software.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#supercedes"/>
    ///</summary>
    public const string supercedes = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#supercedes";

    ///<summary>
    ///A piece of software. Examples may include applications and the operating system. This interpretation most commonly applies to SoftwareItems.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Software"/>
    ///</summary>
    public const string Software = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Software";

    ///<summary>
    ///Indicates the name of the programming language this source code file is written in. Examples might include 'C', 'C++', 'Java' etc.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#programmingLanguage"/>
    ///</summary>
    public const string programmingLanguage = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#programmingLanguage";

    ///<summary>
    ///An application
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Application"/>
    ///</summary>
    public const string Application = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Application";

    ///<summary>
    ///The amount of samples in an audio clip.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#sampleCount"/>
    ///</summary>
    public const string sampleCount = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#sampleCount";

    ///<summary>
    ///Visual content height in pixels.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#height"/>
    ///</summary>
    public const string height = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#height";

    ///<summary>
    ///Number of front channels.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#frontChannels"/>
    ///</summary>
    public const string frontChannels = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#frontChannels";

    ///<summary>
    ///An image of a filesystem. Instances of this class may include CD images, DVD images or hard disk partition images created by various pieces of software (e.g. Norton Ghost). Deprecated in favor of nfo:Filesystem.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#FilesystemImage"/>
    ///</summary>
    public const string FilesystemImage = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#FilesystemImage";

    ///<summary>
    ///Number of rear channels.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#rearChannels"/>
    ///</summary>
    public const string rearChannels = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#rearChannels";

    ///<summary>
    ///Amount of bits in each audio sample.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#bitsPerSample"/>
    ///</summary>
    public const string bitsPerSample = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#bitsPerSample";

    ///<summary>
    ///A common superproperty for all properties signifying the amount of bits for an atomic unit of data. Examples of subproperties may include bitsPerSample and bitsPerPixel
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#bitDepth"/>
    ///</summary>
    public const string bitDepth = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#bitDepth";

    ///<summary>
    ///A HTML document, may contain links to other files.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#HtmlDocument"/>
    ///</summary>
    public const string HtmlDocument = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#HtmlDocument";

    ///<summary>
    ///Duration of a media piece.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#duration"/>
    ///</summary>
    public const string duration = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#duration";

    ///<summary>
    ///Number of Low Frequency Expansion (subwoofer) channels.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#lfeChannels"/>
    ///</summary>
    public const string lfeChannels = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#lfeChannels";

    ///<summary>
    ///Connects a media container with a single media stream contained within.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hasMediaStream"/>
    ///</summary>
    public const string hasMediaStream = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hasMediaStream";

    ///<summary>
    ///A spreadsheet, created by a spreadsheet application. Examples might include Gnumeric, OpenOffice Calc or MS Excel.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Spreadsheet"/>
    ///</summary>
    public const string Spreadsheet = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Spreadsheet";

    ///<summary>
    ///States if a given resource is password-protected.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#isPasswordProtected"/>
    ///</summary>
    public const string isPasswordProtected = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#isPasswordProtected";

    ///<summary>
    ///A file entity inside an archive.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#ArchiveItem"/>
    ///</summary>
    public const string ArchiveItem = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#ArchiveItem";

    ///<summary>
    ///The actual value of the hash.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hashValue"/>
    ///</summary>
    public const string hashValue = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hashValue";

    ///<summary>
    ///
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#encryptedStatus"/>
    ///</summary>
    public const string encryptedStatus = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#encryptedStatus";

    ///<summary>
    ///Uncompressed size of the content of a compressed file.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#uncompressedSize"/>
    ///</summary>
    public const string uncompressedSize = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#uncompressedSize";

    ///<summary>
    ///A compressed file. May contain other files or folder inside. 
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Archive"/>
    ///</summary>
    public const string Archive = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Archive";

    ///<summary>
    ///The date and time of the deletion.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#deletionDate"/>
    ///</summary>
    public const string deletionDate = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#deletionDate";

    ///<summary>
    ///A file entity that has been deleted from the original source. Usually such entities are stored within various kinds of 'Trash' or 'Recycle Bin' folders.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#DeletedResource"/>
    ///</summary>
    public const string DeletedResource = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#DeletedResource";

    ///<summary>
    ///A MindMap, created by a mind-mapping utility. Examples might include FreeMind or mind mapper.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#MindMap"/>
    ///</summary>
    public const string MindMap = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#MindMap";

    ///<summary>
    ///A service published by a piece of software, either by an operating system or an application. Examples of such services may include calendar, addressbook and mailbox managed by a PIM application. This category is introduced to distinguish between data available directly from the applications (Via some Interprocess Communication Mechanisms) and data available from files on a disk. In either case both DataObjects would receive a similar interpretation (e.g. a Mailbox) and wouldn't differ on the content level.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#SoftwareService"/>
    ///</summary>
    public const string SoftwareService = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#SoftwareService";

    ///<summary>
    ///
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#decryptedStatus"/>
    ///</summary>
    public const string decryptedStatus = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#decryptedStatus";

    ///<summary>
    ///The original location of the deleted resource.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#originalLocation"/>
    ///</summary>
    public const string originalLocation = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#originalLocation";

    ///<summary>
    ///A website, usually a container for remote resources, that may be interpreted as HTMLDocuments, images or other types of content.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Website"/>
    ///</summary>
    public const string Website = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Website";

    ///<summary>
    ///
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#VectorImage"/>
    ///</summary>
    public const string VectorImage = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#VectorImage";

    ///<summary>
    ///A Cursor.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Cursor"/>
    ///</summary>
    public const string Cursor = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Cursor";

    ///<summary>
    ///A raster image.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#RasterImage"/>
    ///</summary>
    public const string RasterImage = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#RasterImage";

    ///<summary>
    ///This property is intended to point to an RDF list of MediaFiles.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hasMediaFileListEntry"/>
    ///</summary>
    public const string hasMediaFileListEntry = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hasMediaFileListEntry";

    ///<summary>
    ///A file containing a list of media files.e.g. a playlist
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#MediaList"/>
    ///</summary>
    public const string MediaList = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#MediaList";

    ///<summary>
    ///A single node in the list of media files contained within an MediaList instance. This class is intended to provide a type all those links have. In valid NRL untyped resources cannot be linked. There are no properties defined for this class but the application may expect rdf:first and rdf:last links. The former points to the DataObject instance, interpreted as Media the latter points at another MediaFileListEntr. At the end of the list there is a link to rdf:nil.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#MediaFileListEntry"/>
    ///</summary>
    public const string MediaFileListEntry = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#MediaFileListEntry";

    ///<summary>
    ///A folder with bookmarks of a webbrowser. Use nfo:containsBookmark to relate Bookmarks. Folders can contain subfolders, use containsBookmarkFolder to relate them.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#BookmarkFolder"/>
    ///</summary>
    public const string BookmarkFolder = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#BookmarkFolder";

    ///<summary>
    ///Amount of bits used to express the color of each pixel.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#colorDepth"/>
    ///</summary>
    public const string colorDepth = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#colorDepth";

    ///<summary>
    ///The average overall bitrate of a media container. (i.e. the size of the piece of media in bits, divided by it's duration expressed in seconds).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#averageBitrate"/>
    ///</summary>
    public const string averageBitrate = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#averageBitrate";

    ///<summary>
    ///An Icon (regardless of whether it's a raster or a vector icon. A resource representing an icon could have two types (Icon and Raster, or Icon and Vector) if required.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Icon"/>
    ///</summary>
    public const string Icon = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Icon";

    ///<summary>
    ///The owner of the file as defined by the file system access rights feature.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileOwner"/>
    ///</summary>
    public const string fileOwner = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileOwner";

    ///<summary>
    ///Visual content aspect ratio. (Width divided by Height)
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#aspectRatio"/>
    ///</summary>
    public const string aspectRatio = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#aspectRatio";

    ///<summary>
    ///The folder contains a bookmark folder.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#containsBookmarkFolder"/>
    ///</summary>
    public const string containsBookmarkFolder = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#containsBookmarkFolder";

    ///<summary>
    ///Models the containment relations between Files and Folders (or CompressedFiles).
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#belongsToContainer"/>
    ///</summary>
    public const string belongsToContainer = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#belongsToContainer";

    ///<summary>
    ///Vertical resolution of an Image (if printed). Expressed in DPI
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#verticalResolution"/>
    ///</summary>
    public const string verticalResolution = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#verticalResolution";

    ///<summary>
    ///URL of the file. It points at the location of the file. In cases where creating a simple file:// or http:// URL for a file is difficult (e.g. for files inside compressed archives) the applications are encouraged to use conventions defined by Apache Commons VFS Project at http://jakarta.apache.org/  commons/ vfs/ filesystems.html.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileUrl"/>
    ///</summary>
    public const string fileUrl = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileUrl";

    ///<summary>
    ///Amount of video frames per second.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#frameRate"/>
    ///</summary>
    public const string frameRate = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#frameRate";

    ///<summary>
    ///The name of the font family.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fontFamily"/>
    ///</summary>
    public const string fontFamily = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fontFamily";

    ///<summary>
    ///File creation date
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileCreated"/>
    ///</summary>
    public const string fileCreated = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileCreated";

    ///<summary>
    ///The type of the bitrate. Examples may include CBR and VBR.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#bitrateType"/>
    ///</summary>
    public const string bitrateType = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#bitrateType";

    ///<summary>
    ///The encoding used for the Embedded File. Examples might include BASE64 or UUEncode
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#encoding"/>
    ///</summary>
    public const string encoding = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#encoding";

    ///<summary>
    ///A folder/directory. Examples of folders include folders on a filesystem and message folders in a mailbox.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Folder"/>
    ///</summary>
    public const string Folder = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Folder";

    ///<summary>
    ///Links the file with it's hash value.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hasHash"/>
    ///</summary>
    public const string hasHash = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#hasHash";

    ///<summary>
    ///The name of the codec necessary to decode a piece of media.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#codec"/>
    ///</summary>
    public const string codec = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#codec";

    ///<summary>
    ///last modification date
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileLastModified"/>
    ///</summary>
    public const string fileLastModified = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileLastModified";

    ///<summary>
    ///The type of the compression. Values include, 'lossy' and 'lossless'.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#compressionType"/>
    ///</summary>
    public const string compressionType = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#compressionType";

    ///<summary>
    ///Type of compression. Instances of this class represent the limited set of values allowed for the nfo:compressionType property.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#CompressionType"/>
    ///</summary>
    public const string CompressionType = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#CompressionType";

    ///<summary>
    ///Number of pages.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#pageCount"/>
    ///</summary>
    public const string pageCount = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#pageCount";

    ///<summary>
    ///A file containing a text document, that is unambiguously divided into pages. Examples might include PDF, DOC, PS, DVI etc.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#PaginatedTextDocument"/>
    ///</summary>
    public const string PaginatedTextDocument = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#PaginatedTextDocument";

    ///<summary>
    ///Name of a global variable defined within the source code file.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#definesGlobalVariable"/>
    ///</summary>
    public const string definesGlobalVariable = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#definesGlobalVariable";

    ///<summary>
    ///Represents a container for deleted files, a feature common in modern operating systems.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Trash"/>
    ///</summary>
    public const string Trash = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Trash";

    ///<summary>
    ///States that a piece of software is in conflict with another piece of software.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#conflicts"/>
    ///</summary>
    public const string conflicts = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#conflicts";

    ///<summary>
    ///The status of the encryption of the InformationElement.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#encryptionStatus"/>
    ///</summary>
    public const string encryptionStatus = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#encryptionStatus";

    ///<summary>
    ///The status of the encryption of an InformationElement. nfo:encryptedStatus means that the InformationElement has been encrypted and couldn't be decrypted by the extraction software, thus no content is available. nfo:decryptedStatus means that decryption was successfull and the content is available.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#EncryptionStatus"/>
    ///</summary>
    public const string EncryptionStatus = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#EncryptionStatus";

    ///<summary>
    ///The folder contains a bookmark.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#containsBookmark"/>
    ///</summary>
    public const string containsBookmark = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#containsBookmark";

    ///<summary>
    ///An executable file.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Executable"/>
    ///</summary>
    public const string Executable = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Executable";

    ///<summary>
    ///Name of a class defined in the source code file.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#definesClass"/>
    ///</summary>
    public const string definesClass = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#definesClass";

    ///<summary>
    ///
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#lossyCompressionType"/>
    ///</summary>
    public const string lossyCompressionType = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#lossyCompressionType";

    ///<summary>
    ///An OperatingSystem
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#OperatingSystem"/>
    ///</summary>
    public const string OperatingSystem = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#OperatingSystem";

    ///<summary>
    ///The size of the file in bytes. For compressed files it means the size of the packed file, not of the contents. For folders it means the aggregated size of all contained files and folders 
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileSize"/>
    ///</summary>
    public const string fileSize = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#fileSize";

    ///<summary>
    ///A file data object stored at a remote location. Don't confuse this class with a RemotePortAddress. This one applies to a particular resource, RemotePortAddress applies to an address, that can have various interpretations.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#RemoteDataObject"/>
    ///</summary>
    public const string RemoteDataObject = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#RemoteDataObject";

    ///<summary>
    ///The number of colors used/available in a raster image.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#colorCount"/>
    ///</summary>
    public const string colorCount = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#colorCount";

    ///<summary>
    ///The number of colors defined in palette of the raster image.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#paletteSize"/>
    ///</summary>
    public const string paletteSize = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#paletteSize";

    ///<summary>
    ///An information resources of which representations (files, streams) can be retrieved through a web server. They may be generated at retrieval time. Typical examples are pages served by PHP or AJAX or mp3 streams.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#WebDataObject"/>
    ///</summary>
    public const string WebDataObject = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#WebDataObject";

    ///<summary>
    ///A local file data object which is stored on a local file system. Its nie:url always uses the file:/ protocol. The main use of this class is to distinguish local and non-local files.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#LocalFileDataObject"/>
    ///</summary>
    public const string LocalFileDataObject = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#LocalFileDataObject";

    ///<summary>
    ///Relates an information element to an image which depicts said element.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#depiction"/>
    ///</summary>
    public const string depiction = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#depiction";

    ///<summary>
    ///Relates an image to the information elements it depicts.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#depicts"/>
    ///</summary>
    public const string depicts = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#depicts";

    ///<summary>
    ///Containment relation between placemark containers (files) and placemarks within.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#containsPlacemark"/>
    ///</summary>
    public const string containsPlacemark = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#containsPlacemark";

    ///<summary>
    ///A data object containing placemark(s). Use nie:contentCreated to represent the date when the user created the dataobject, nao:creator for defining the creator, nie:contentLastModified for modifications. nfo:containsPlacemark to refer to individual placemarks within.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#PlacemarkContainer"/>
    ///</summary>
    public const string PlacemarkContainer = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#PlacemarkContainer";

    ///<summary>
    ///One placemark within a placemark container/file. Use nie:title for the name/label, nao:creator for defining the creator.
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Placemark"/>
    ///</summary>
    public const string Placemark = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#Placemark";
}
///<summary>
///
///
///</summary>
public class art : Ontology
{
    public static readonly Uri Namespace = new Uri("http://semiodesk.com/artivity/1.0/");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "art";
    public static string GetPrefix() { return Prefix; }     

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/"/>
    ///</summary>
    public static readonly Resource _1_0 = new Resource(new Uri("http://semiodesk.com/artivity/1.0/"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Database"/>
    ///</summary>
    public static readonly Class Database = new Class(new Uri("http://semiodesk.com/artivity/1.0/Database"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/isMonitoringEnabled"/>
    ///</summary>
    public static readonly Property isMonitoringEnabled = new Property(new Uri("http://semiodesk.com/artivity/1.0/isMonitoringEnabled"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/hadState"/>
    ///</summary>
    public static readonly Property hadState = new Property(new Uri("http://semiodesk.com/artivity/1.0/hadState"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/DatabaseState"/>
    ///</summary>
    public static readonly Class DatabaseState = new Class(new Uri("http://semiodesk.com/artivity/1.0/DatabaseState"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/atTime"/>
    ///</summary>
    public static readonly Property atTime = new Property(new Uri("http://semiodesk.com/artivity/1.0/atTime"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/factsCount"/>
    ///</summary>
    public static readonly Property factsCount = new Property(new Uri("http://semiodesk.com/artivity/1.0/factsCount"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Browse"/>
    ///</summary>
    public static readonly Class Browse = new Class(new Uri("http://semiodesk.com/artivity/1.0/Browse"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/CreateFile"/>
    ///</summary>
    public static readonly Class CreateFile = new Class(new Uri("http://semiodesk.com/artivity/1.0/CreateFile"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/DeleteFile"/>
    ///</summary>
    public static readonly Class DeleteFile = new Class(new Uri("http://semiodesk.com/artivity/1.0/DeleteFile"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/EditFile"/>
    ///</summary>
    public static readonly Class EditFile = new Class(new Uri("http://semiodesk.com/artivity/1.0/EditFile"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Add"/>
    ///</summary>
    public static readonly Class Add = new Class(new Uri("http://semiodesk.com/artivity/1.0/Add"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Edit"/>
    ///</summary>
    public static readonly Class Edit = new Class(new Uri("http://semiodesk.com/artivity/1.0/Edit"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Redo"/>
    ///</summary>
    public static readonly Class Redo = new Class(new Uri("http://semiodesk.com/artivity/1.0/Redo"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Remove"/>
    ///</summary>
    public static readonly Class Remove = new Class(new Uri("http://semiodesk.com/artivity/1.0/Remove"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Undo"/>
    ///</summary>
    public static readonly Class Undo = new Class(new Uri("http://semiodesk.com/artivity/1.0/Undo"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/View"/>
    ///</summary>
    public static readonly Class View = new Class(new Uri("http://semiodesk.com/artivity/1.0/View"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Layer"/>
    ///</summary>
    public static readonly Class Layer = new Class(new Uri("http://semiodesk.com/artivity/1.0/Layer"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/selectedLayer"/>
    ///</summary>
    public static readonly Property selectedLayer = new Property(new Uri("http://semiodesk.com/artivity/1.0/selectedLayer"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/isCaptureEnabled"/>
    ///</summary>
    public static readonly Property isCaptureEnabled = new Property(new Uri("http://semiodesk.com/artivity/1.0/isCaptureEnabled"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/executableName"/>
    ///</summary>
    public static readonly Property executableName = new Property(new Uri("http://semiodesk.com/artivity/1.0/executableName"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/executablePath"/>
    ///</summary>
    public static readonly Property executablePath = new Property(new Uri("http://semiodesk.com/artivity/1.0/executablePath"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/hasColourCode"/>
    ///</summary>
    public static readonly Property hasColourCode = new Property(new Uri("http://semiodesk.com/artivity/1.0/hasColourCode"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/coordinateSystem"/>
    ///</summary>
    public static readonly Property coordinateSystem = new Property(new Uri("http://semiodesk.com/artivity/1.0/coordinateSystem"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/CoordinateSystem"/>
    ///</summary>
    public static readonly Class CoordinateSystem = new Class(new Uri("http://semiodesk.com/artivity/1.0/CoordinateSystem"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/CartesianCoordinateSystem"/>
    ///</summary>
    public static readonly Class CartesianCoordinateSystem = new Class(new Uri("http://semiodesk.com/artivity/1.0/CartesianCoordinateSystem"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/lengthUnit"/>
    ///</summary>
    public static readonly Property lengthUnit = new Property(new Uri("http://semiodesk.com/artivity/1.0/lengthUnit"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/transformationMatrix"/>
    ///</summary>
    public static readonly Property transformationMatrix = new Property(new Uri("http://semiodesk.com/artivity/1.0/transformationMatrix"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Point"/>
    ///</summary>
    public static readonly Class Point = new Class(new Uri("http://semiodesk.com/artivity/1.0/Point"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/x"/>
    ///</summary>
    public static readonly Property x = new Property(new Uri("http://semiodesk.com/artivity/1.0/x"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/y"/>
    ///</summary>
    public static readonly Property y = new Property(new Uri("http://semiodesk.com/artivity/1.0/y"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/z"/>
    ///</summary>
    public static readonly Property z = new Property(new Uri("http://semiodesk.com/artivity/1.0/z"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Geometry"/>
    ///</summary>
    public static readonly Class Geometry = new Class(new Uri("http://semiodesk.com/artivity/1.0/Geometry"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/coordinateDimension"/>
    ///</summary>
    public static readonly Property coordinateDimension = new Property(new Uri("http://semiodesk.com/artivity/1.0/coordinateDimension"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/width"/>
    ///</summary>
    public static readonly Property width = new Property(new Uri("http://semiodesk.com/artivity/1.0/width"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/height"/>
    ///</summary>
    public static readonly Property height = new Property(new Uri("http://semiodesk.com/artivity/1.0/height"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/depth"/>
    ///</summary>
    public static readonly Property depth = new Property(new Uri("http://semiodesk.com/artivity/1.0/depth"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/position"/>
    ///</summary>
    public static readonly Property position = new Property(new Uri("http://semiodesk.com/artivity/1.0/position"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Rectangle"/>
    ///</summary>
    public static readonly Class Rectangle = new Class(new Uri("http://semiodesk.com/artivity/1.0/Rectangle"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Cube"/>
    ///</summary>
    public static readonly Class Cube = new Class(new Uri("http://semiodesk.com/artivity/1.0/Cube"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/canvas"/>
    ///</summary>
    public static readonly Property canvas = new Property(new Uri("http://semiodesk.com/artivity/1.0/canvas"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Canvas"/>
    ///</summary>
    public static readonly Class Canvas = new Class(new Uri("http://semiodesk.com/artivity/1.0/Canvas"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/hadBoundaries"/>
    ///</summary>
    public static readonly Property hadBoundaries = new Property(new Uri("http://semiodesk.com/artivity/1.0/hadBoundaries"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/BoundingRectangle"/>
    ///</summary>
    public static readonly Class BoundingRectangle = new Class(new Uri("http://semiodesk.com/artivity/1.0/BoundingRectangle"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/BoundingCube"/>
    ///</summary>
    public static readonly Class BoundingCube = new Class(new Uri("http://semiodesk.com/artivity/1.0/BoundingCube"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/hadViewport"/>
    ///</summary>
    public static readonly Property hadViewport = new Property(new Uri("http://semiodesk.com/artivity/1.0/hadViewport"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/thumbnailUrl"/>
    ///</summary>
    public static readonly Property thumbnailUrl = new Property(new Uri("http://semiodesk.com/artivity/1.0/thumbnailUrl"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/thumbnailPosition"/>
    ///</summary>
    public static readonly Property thumbnailPosition = new Property(new Uri("http://semiodesk.com/artivity/1.0/thumbnailPosition"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Viewport"/>
    ///</summary>
    public static readonly Class Viewport = new Class(new Uri("http://semiodesk.com/artivity/1.0/Viewport"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/zoomFactor"/>
    ///</summary>
    public static readonly Property zoomFactor = new Property(new Uri("http://semiodesk.com/artivity/1.0/zoomFactor"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/LengthUnit"/>
    ///</summary>
    public static readonly Class LengthUnit = new Class(new Uri("http://semiodesk.com/artivity/1.0/LengthUnit"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/m"/>
    ///</summary>
    public static readonly Class m = new Class(new Uri("http://semiodesk.com/artivity/1.0/m"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/cm"/>
    ///</summary>
    public static readonly Class cm = new Class(new Uri("http://semiodesk.com/artivity/1.0/cm"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/mm"/>
    ///</summary>
    public static readonly Class mm = new Class(new Uri("http://semiodesk.com/artivity/1.0/mm"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/pt"/>
    ///</summary>
    public static readonly Class pt = new Class(new Uri("http://semiodesk.com/artivity/1.0/pt"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/in"/>
    ///</summary>
    public static readonly Class _in = new Class(new Uri("http://semiodesk.com/artivity/1.0/in"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/ft"/>
    ///</summary>
    public static readonly Class ft = new Class(new Uri("http://semiodesk.com/artivity/1.0/ft"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/px"/>
    ///</summary>
    public static readonly Class px = new Class(new Uri("http://semiodesk.com/artivity/1.0/px"));    

    ///<summary>
    ///
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#FileDataObject"/>
    ///</summary>
    public static readonly Resource FileDataObject = new Resource(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#FileDataObject"));    

    ///<summary>
    ///
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#WebDataObject"/>
    ///</summary>
    public static readonly Resource WebDataObject = new Resource(new Uri("http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#WebDataObject"));    

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/OrcidAccount"/>
    ///</summary>
    public static readonly Class OrcidAccount = new Class(new Uri("http://semiodesk.com/artivity/1.0/OrcidAccount"));
}
///<summary>
///
///
///</summary>
public static class ART
{
    public static readonly Uri Namespace = new Uri("http://semiodesk.com/artivity/1.0/");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "ART";
    public static string GetPrefix() { return Prefix; } 

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/"/>
    ///</summary>
    public const string _1_0 = "http://semiodesk.com/artivity/1.0/";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Database"/>
    ///</summary>
    public const string Database = "http://semiodesk.com/artivity/1.0/Database";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/isMonitoringEnabled"/>
    ///</summary>
    public const string isMonitoringEnabled = "http://semiodesk.com/artivity/1.0/isMonitoringEnabled";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/hadState"/>
    ///</summary>
    public const string hadState = "http://semiodesk.com/artivity/1.0/hadState";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/DatabaseState"/>
    ///</summary>
    public const string DatabaseState = "http://semiodesk.com/artivity/1.0/DatabaseState";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/atTime"/>
    ///</summary>
    public const string atTime = "http://semiodesk.com/artivity/1.0/atTime";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/factsCount"/>
    ///</summary>
    public const string factsCount = "http://semiodesk.com/artivity/1.0/factsCount";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Browse"/>
    ///</summary>
    public const string Browse = "http://semiodesk.com/artivity/1.0/Browse";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/CreateFile"/>
    ///</summary>
    public const string CreateFile = "http://semiodesk.com/artivity/1.0/CreateFile";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/DeleteFile"/>
    ///</summary>
    public const string DeleteFile = "http://semiodesk.com/artivity/1.0/DeleteFile";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/EditFile"/>
    ///</summary>
    public const string EditFile = "http://semiodesk.com/artivity/1.0/EditFile";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Add"/>
    ///</summary>
    public const string Add = "http://semiodesk.com/artivity/1.0/Add";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Edit"/>
    ///</summary>
    public const string Edit = "http://semiodesk.com/artivity/1.0/Edit";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Redo"/>
    ///</summary>
    public const string Redo = "http://semiodesk.com/artivity/1.0/Redo";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Remove"/>
    ///</summary>
    public const string Remove = "http://semiodesk.com/artivity/1.0/Remove";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Undo"/>
    ///</summary>
    public const string Undo = "http://semiodesk.com/artivity/1.0/Undo";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/View"/>
    ///</summary>
    public const string View = "http://semiodesk.com/artivity/1.0/View";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Layer"/>
    ///</summary>
    public const string Layer = "http://semiodesk.com/artivity/1.0/Layer";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/selectedLayer"/>
    ///</summary>
    public const string selectedLayer = "http://semiodesk.com/artivity/1.0/selectedLayer";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/isCaptureEnabled"/>
    ///</summary>
    public const string isCaptureEnabled = "http://semiodesk.com/artivity/1.0/isCaptureEnabled";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/executableName"/>
    ///</summary>
    public const string executableName = "http://semiodesk.com/artivity/1.0/executableName";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/executablePath"/>
    ///</summary>
    public const string executablePath = "http://semiodesk.com/artivity/1.0/executablePath";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/hasColourCode"/>
    ///</summary>
    public const string hasColourCode = "http://semiodesk.com/artivity/1.0/hasColourCode";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/coordinateSystem"/>
    ///</summary>
    public const string coordinateSystem = "http://semiodesk.com/artivity/1.0/coordinateSystem";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/CoordinateSystem"/>
    ///</summary>
    public const string CoordinateSystem = "http://semiodesk.com/artivity/1.0/CoordinateSystem";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/CartesianCoordinateSystem"/>
    ///</summary>
    public const string CartesianCoordinateSystem = "http://semiodesk.com/artivity/1.0/CartesianCoordinateSystem";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/lengthUnit"/>
    ///</summary>
    public const string lengthUnit = "http://semiodesk.com/artivity/1.0/lengthUnit";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/transformationMatrix"/>
    ///</summary>
    public const string transformationMatrix = "http://semiodesk.com/artivity/1.0/transformationMatrix";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Point"/>
    ///</summary>
    public const string Point = "http://semiodesk.com/artivity/1.0/Point";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/x"/>
    ///</summary>
    public const string x = "http://semiodesk.com/artivity/1.0/x";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/y"/>
    ///</summary>
    public const string y = "http://semiodesk.com/artivity/1.0/y";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/z"/>
    ///</summary>
    public const string z = "http://semiodesk.com/artivity/1.0/z";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Geometry"/>
    ///</summary>
    public const string Geometry = "http://semiodesk.com/artivity/1.0/Geometry";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/coordinateDimension"/>
    ///</summary>
    public const string coordinateDimension = "http://semiodesk.com/artivity/1.0/coordinateDimension";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/width"/>
    ///</summary>
    public const string width = "http://semiodesk.com/artivity/1.0/width";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/height"/>
    ///</summary>
    public const string height = "http://semiodesk.com/artivity/1.0/height";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/depth"/>
    ///</summary>
    public const string depth = "http://semiodesk.com/artivity/1.0/depth";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/position"/>
    ///</summary>
    public const string position = "http://semiodesk.com/artivity/1.0/position";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Rectangle"/>
    ///</summary>
    public const string Rectangle = "http://semiodesk.com/artivity/1.0/Rectangle";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Cube"/>
    ///</summary>
    public const string Cube = "http://semiodesk.com/artivity/1.0/Cube";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/canvas"/>
    ///</summary>
    public const string canvas = "http://semiodesk.com/artivity/1.0/canvas";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Canvas"/>
    ///</summary>
    public const string Canvas = "http://semiodesk.com/artivity/1.0/Canvas";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/hadBoundaries"/>
    ///</summary>
    public const string hadBoundaries = "http://semiodesk.com/artivity/1.0/hadBoundaries";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/BoundingRectangle"/>
    ///</summary>
    public const string BoundingRectangle = "http://semiodesk.com/artivity/1.0/BoundingRectangle";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/BoundingCube"/>
    ///</summary>
    public const string BoundingCube = "http://semiodesk.com/artivity/1.0/BoundingCube";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/hadViewport"/>
    ///</summary>
    public const string hadViewport = "http://semiodesk.com/artivity/1.0/hadViewport";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/thumbnailUrl"/>
    ///</summary>
    public const string thumbnailUrl = "http://semiodesk.com/artivity/1.0/thumbnailUrl";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/thumbnailPosition"/>
    ///</summary>
    public const string thumbnailPosition = "http://semiodesk.com/artivity/1.0/thumbnailPosition";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/Viewport"/>
    ///</summary>
    public const string Viewport = "http://semiodesk.com/artivity/1.0/Viewport";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/zoomFactor"/>
    ///</summary>
    public const string zoomFactor = "http://semiodesk.com/artivity/1.0/zoomFactor";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/LengthUnit"/>
    ///</summary>
    public const string LengthUnit = "http://semiodesk.com/artivity/1.0/LengthUnit";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/m"/>
    ///</summary>
    public const string m = "http://semiodesk.com/artivity/1.0/m";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/cm"/>
    ///</summary>
    public const string cm = "http://semiodesk.com/artivity/1.0/cm";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/mm"/>
    ///</summary>
    public const string mm = "http://semiodesk.com/artivity/1.0/mm";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/pt"/>
    ///</summary>
    public const string pt = "http://semiodesk.com/artivity/1.0/pt";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/in"/>
    ///</summary>
    public const string _in = "http://semiodesk.com/artivity/1.0/in";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/ft"/>
    ///</summary>
    public const string ft = "http://semiodesk.com/artivity/1.0/ft";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/px"/>
    ///</summary>
    public const string px = "http://semiodesk.com/artivity/1.0/px";

    ///<summary>
    ///
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#FileDataObject"/>
    ///</summary>
    public const string FileDataObject = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#FileDataObject";

    ///<summary>
    ///
    ///<see cref="http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#WebDataObject"/>
    ///</summary>
    public const string WebDataObject = "http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#WebDataObject";

    ///<summary>
    ///
    ///<see cref="http://semiodesk.com/artivity/1.0/OrcidAccount"/>
    ///</summary>
    public const string OrcidAccount = "http://semiodesk.com/artivity/1.0/OrcidAccount";
}
///<summary>
///Friend of a Friend (FOAF) vocabulary
///
///</summary>
public class foaf : Ontology
{
    public static readonly Uri Namespace = new Uri("http://xmlns.com/foaf/0.1/");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "foaf";
    public static string GetPrefix() { return Prefix; }     

    ///<summary>
    ///
    ///<see cref="http://xmlns.com/foaf/0.1/"/>
    ///</summary>
    public static readonly Resource _0_1 = new Resource(new Uri("http://xmlns.com/foaf/0.1/"));    

    ///<summary>
    ///
    ///<see cref="http://xmlns.com/wot/0.1/assurance"/>
    ///</summary>
    public static readonly Property assurance = new Property(new Uri("http://xmlns.com/wot/0.1/assurance"));    

    ///<summary>
    ///
    ///<see cref="http://xmlns.com/wot/0.1/src_assurance"/>
    ///</summary>
    public static readonly Property src_assurance = new Property(new Uri("http://xmlns.com/wot/0.1/src_assurance"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2003/06/sw-vocab-status/ns#term_status"/>
    ///</summary>
    public static readonly Property term_status = new Property(new Uri("http://www.w3.org/2003/06/sw-vocab-status/ns#term_status"));    

    ///<summary>
    ///
    ///<see cref="http://purl.org/dc/elements/1.1/description"/>
    ///</summary>
    public static readonly Property description = new Property(new Uri("http://purl.org/dc/elements/1.1/description"));    

    ///<summary>
    ///
    ///<see cref="http://purl.org/dc/elements/1.1/title"/>
    ///</summary>
    public static readonly Property title = new Property(new Uri("http://purl.org/dc/elements/1.1/title"));    

    ///<summary>
    ///
    ///<see cref="http://purl.org/dc/elements/1.1/date"/>
    ///</summary>
    public static readonly Property date = new Property(new Uri("http://purl.org/dc/elements/1.1/date"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#Class"/>
    ///</summary>
    public static readonly Class Class = new Class(new Uri("http://www.w3.org/2000/01/rdf-schema#Class"));    

    ///<summary>
    ///A foaf:LabelProperty is any RDF property with texual values that serve as labels.
    ///<see cref="http://xmlns.com/foaf/0.1/LabelProperty"/>
    ///</summary>
    public static readonly Class LabelProperty = new Class(new Uri("http://xmlns.com/foaf/0.1/LabelProperty"));    

    ///<summary>
    ///A person.
    ///<see cref="http://xmlns.com/foaf/0.1/Person"/>
    ///</summary>
    public static readonly Class Person = new Class(new Uri("http://xmlns.com/foaf/0.1/Person"));    

    ///<summary>
    ///An agent (eg. person, group, software or physical artifact).
    ///<see cref="http://xmlns.com/foaf/0.1/Agent"/>
    ///</summary>
    public static readonly Class Agent = new Class(new Uri("http://xmlns.com/foaf/0.1/Agent"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2003/01/geo/wgs84_pos#SpatialThing"/>
    ///</summary>
    public static readonly Class SpatialThing = new Class(new Uri("http://www.w3.org/2003/01/geo/wgs84_pos#SpatialThing"));    

    ///<summary>
    ///A document.
    ///<see cref="http://xmlns.com/foaf/0.1/Document"/>
    ///</summary>
    public static readonly Class Document = new Class(new Uri("http://xmlns.com/foaf/0.1/Document"));    

    ///<summary>
    ///An organization.
    ///<see cref="http://xmlns.com/foaf/0.1/Organization"/>
    ///</summary>
    public static readonly Class Organization = new Class(new Uri("http://xmlns.com/foaf/0.1/Organization"));    

    ///<summary>
    ///A class of Agents.
    ///<see cref="http://xmlns.com/foaf/0.1/Group"/>
    ///</summary>
    public static readonly Class Group = new Class(new Uri("http://xmlns.com/foaf/0.1/Group"));    

    ///<summary>
    ///A project (a collective endeavour of some kind).
    ///<see cref="http://xmlns.com/foaf/0.1/Project"/>
    ///</summary>
    public static readonly Class Project = new Class(new Uri("http://xmlns.com/foaf/0.1/Project"));    

    ///<summary>
    ///An image.
    ///<see cref="http://xmlns.com/foaf/0.1/Image"/>
    ///</summary>
    public static readonly Class Image = new Class(new Uri("http://xmlns.com/foaf/0.1/Image"));    

    ///<summary>
    ///A personal profile RDF document.
    ///<see cref="http://xmlns.com/foaf/0.1/PersonalProfileDocument"/>
    ///</summary>
    public static readonly Class PersonalProfileDocument = new Class(new Uri("http://xmlns.com/foaf/0.1/PersonalProfileDocument"));    

    ///<summary>
    ///An online account.
    ///<see cref="http://xmlns.com/foaf/0.1/OnlineAccount"/>
    ///</summary>
    public static readonly Class OnlineAccount = new Class(new Uri("http://xmlns.com/foaf/0.1/OnlineAccount"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2002/07/owl#Thing"/>
    ///</summary>
    public static readonly Resource Thing = new Resource(new Uri("http://www.w3.org/2002/07/owl#Thing"));    

    ///<summary>
    ///An online gaming account.
    ///<see cref="http://xmlns.com/foaf/0.1/OnlineGamingAccount"/>
    ///</summary>
    public static readonly Class OnlineGamingAccount = new Class(new Uri("http://xmlns.com/foaf/0.1/OnlineGamingAccount"));    

    ///<summary>
    ///An online e-commerce account.
    ///<see cref="http://xmlns.com/foaf/0.1/OnlineEcommerceAccount"/>
    ///</summary>
    public static readonly Class OnlineEcommerceAccount = new Class(new Uri("http://xmlns.com/foaf/0.1/OnlineEcommerceAccount"));    

    ///<summary>
    ///An online chat account.
    ///<see cref="http://xmlns.com/foaf/0.1/OnlineChatAccount"/>
    ///</summary>
    public static readonly Class OnlineChatAccount = new Class(new Uri("http://xmlns.com/foaf/0.1/OnlineChatAccount"));    

    ///<summary>
    ///A  personal mailbox, ie. an Internet mailbox associated with exactly one owner, the first owner of this mailbox. This is a 'static inverse functional property', in that  there is (across time and change) at most one individual that ever has any particular value for foaf:mbox.
    ///<see cref="http://xmlns.com/foaf/0.1/mbox"/>
    ///</summary>
    public static readonly Property mbox = new Property(new Uri("http://xmlns.com/foaf/0.1/mbox"));    

    ///<summary>
    ///The sha1sum of the URI of an Internet mailbox associated with exactly one owner, the  first owner of the mailbox.
    ///<see cref="http://xmlns.com/foaf/0.1/mbox_sha1sum"/>
    ///</summary>
    public static readonly Property mbox_sha1sum = new Property(new Uri("http://xmlns.com/foaf/0.1/mbox_sha1sum"));    

    ///<summary>
    ///The gender of this Agent (typically but not necessarily 'male' or 'female').
    ///<see cref="http://xmlns.com/foaf/0.1/gender"/>
    ///</summary>
    public static readonly Property gender = new Property(new Uri("http://xmlns.com/foaf/0.1/gender"));    

    ///<summary>
    ///A textual geekcode for this person, see http://www.geekcode.com/geek.html
    ///<see cref="http://xmlns.com/foaf/0.1/geekcode"/>
    ///</summary>
    public static readonly Property geekcode = new Property(new Uri("http://xmlns.com/foaf/0.1/geekcode"));    

    ///<summary>
    ///A checksum for the DNA of some thing. Joke.
    ///<see cref="http://xmlns.com/foaf/0.1/dnaChecksum"/>
    ///</summary>
    public static readonly Property dnaChecksum = new Property(new Uri("http://xmlns.com/foaf/0.1/dnaChecksum"));    

    ///<summary>
    ///A sha1sum hash, in hex.
    ///<see cref="http://xmlns.com/foaf/0.1/sha1"/>
    ///</summary>
    public static readonly Property sha1 = new Property(new Uri("http://xmlns.com/foaf/0.1/sha1"));    

    ///<summary>
    ///A location that something is based near, for some broadly human notion of near.
    ///<see cref="http://xmlns.com/foaf/0.1/based_near"/>
    ///</summary>
    public static readonly Property based_near = new Property(new Uri("http://xmlns.com/foaf/0.1/based_near"));    

    ///<summary>
    ///Title (Mr, Mrs, Ms, Dr. etc)
    ///<see cref="http://xmlns.com/foaf/0.1/title"/>
    ///</summary>
    public static readonly Property title_0 = new Property(new Uri("http://xmlns.com/foaf/0.1/title"));    

    ///<summary>
    ///A short informal nickname characterising an agent (includes login identifiers, IRC and other chat nicknames).
    ///<see cref="http://xmlns.com/foaf/0.1/nick"/>
    ///</summary>
    public static readonly Property nick = new Property(new Uri("http://xmlns.com/foaf/0.1/nick"));    

    ///<summary>
    ///A jabber ID for something.
    ///<see cref="http://xmlns.com/foaf/0.1/jabberID"/>
    ///</summary>
    public static readonly Property jabberID = new Property(new Uri("http://xmlns.com/foaf/0.1/jabberID"));    

    ///<summary>
    ///An AIM chat ID
    ///<see cref="http://xmlns.com/foaf/0.1/aimChatID"/>
    ///</summary>
    public static readonly Property aimChatID = new Property(new Uri("http://xmlns.com/foaf/0.1/aimChatID"));    

    ///<summary>
    ///A Skype ID
    ///<see cref="http://xmlns.com/foaf/0.1/skypeID"/>
    ///</summary>
    public static readonly Property skypeID = new Property(new Uri("http://xmlns.com/foaf/0.1/skypeID"));    

    ///<summary>
    ///An ICQ chat ID
    ///<see cref="http://xmlns.com/foaf/0.1/icqChatID"/>
    ///</summary>
    public static readonly Property icqChatID = new Property(new Uri("http://xmlns.com/foaf/0.1/icqChatID"));    

    ///<summary>
    ///A Yahoo chat ID
    ///<see cref="http://xmlns.com/foaf/0.1/yahooChatID"/>
    ///</summary>
    public static readonly Property yahooChatID = new Property(new Uri("http://xmlns.com/foaf/0.1/yahooChatID"));    

    ///<summary>
    ///An MSN chat ID
    ///<see cref="http://xmlns.com/foaf/0.1/msnChatID"/>
    ///</summary>
    public static readonly Property msnChatID = new Property(new Uri("http://xmlns.com/foaf/0.1/msnChatID"));    

    ///<summary>
    ///A name for some thing.
    ///<see cref="http://xmlns.com/foaf/0.1/name"/>
    ///</summary>
    public static readonly Property name = new Property(new Uri("http://xmlns.com/foaf/0.1/name"));    

    ///<summary>
    ///The first name of a person.
    ///<see cref="http://xmlns.com/foaf/0.1/firstName"/>
    ///</summary>
    public static readonly Property firstName = new Property(new Uri("http://xmlns.com/foaf/0.1/firstName"));    

    ///<summary>
    ///The last name of a person.
    ///<see cref="http://xmlns.com/foaf/0.1/lastName"/>
    ///</summary>
    public static readonly Property lastName = new Property(new Uri("http://xmlns.com/foaf/0.1/lastName"));    

    ///<summary>
    ///The given name of some person.
    ///<see cref="http://xmlns.com/foaf/0.1/givenName"/>
    ///</summary>
    public static readonly Property givenName = new Property(new Uri("http://xmlns.com/foaf/0.1/givenName"));    

    ///<summary>
    ///The given name of some person.
    ///<see cref="http://xmlns.com/foaf/0.1/givenname"/>
    ///</summary>
    public static readonly Property givenname = new Property(new Uri("http://xmlns.com/foaf/0.1/givenname"));    

    ///<summary>
    ///The surname of some person.
    ///<see cref="http://xmlns.com/foaf/0.1/surname"/>
    ///</summary>
    public static readonly Property surname = new Property(new Uri("http://xmlns.com/foaf/0.1/surname"));    

    ///<summary>
    ///The family name of some person.
    ///<see cref="http://xmlns.com/foaf/0.1/family_name"/>
    ///</summary>
    public static readonly Property family_name = new Property(new Uri("http://xmlns.com/foaf/0.1/family_name"));    

    ///<summary>
    ///The family name of some person.
    ///<see cref="http://xmlns.com/foaf/0.1/familyName"/>
    ///</summary>
    public static readonly Property familyName = new Property(new Uri("http://xmlns.com/foaf/0.1/familyName"));    

    ///<summary>
    ///A phone,  specified using fully qualified tel: URI scheme (refs: http://www.w3.org/Addressing/schemes.html#tel).
    ///<see cref="http://xmlns.com/foaf/0.1/phone"/>
    ///</summary>
    public static readonly Property phone = new Property(new Uri("http://xmlns.com/foaf/0.1/phone"));    

    ///<summary>
    ///A homepage for some thing.
    ///<see cref="http://xmlns.com/foaf/0.1/homepage"/>
    ///</summary>
    public static readonly Property homepage = new Property(new Uri("http://xmlns.com/foaf/0.1/homepage"));    

    ///<summary>
    ///A weblog of some thing (whether person, group, company etc.).
    ///<see cref="http://xmlns.com/foaf/0.1/weblog"/>
    ///</summary>
    public static readonly Property weblog = new Property(new Uri("http://xmlns.com/foaf/0.1/weblog"));    

    ///<summary>
    ///An OpenID for an Agent.
    ///<see cref="http://xmlns.com/foaf/0.1/openid"/>
    ///</summary>
    public static readonly Property openid = new Property(new Uri("http://xmlns.com/foaf/0.1/openid"));    

    ///<summary>
    ///A tipjar document for this agent, describing means for payment and reward.
    ///<see cref="http://xmlns.com/foaf/0.1/tipjar"/>
    ///</summary>
    public static readonly Property tipjar = new Property(new Uri("http://xmlns.com/foaf/0.1/tipjar"));    

    ///<summary>
    ///A .plan comment, in the tradition of finger and '.plan' files.
    ///<see cref="http://xmlns.com/foaf/0.1/plan"/>
    ///</summary>
    public static readonly Property plan = new Property(new Uri("http://xmlns.com/foaf/0.1/plan"));    

    ///<summary>
    ///Something that was made by this agent.
    ///<see cref="http://xmlns.com/foaf/0.1/made"/>
    ///</summary>
    public static readonly Property made = new Property(new Uri("http://xmlns.com/foaf/0.1/made"));    

    ///<summary>
    ///An agent that  made this thing.
    ///<see cref="http://xmlns.com/foaf/0.1/maker"/>
    ///</summary>
    public static readonly Property maker = new Property(new Uri("http://xmlns.com/foaf/0.1/maker"));    

    ///<summary>
    ///An image that can be used to represent some thing (ie. those depictions which are particularly representative of something, eg. one's photo on a homepage).
    ///<see cref="http://xmlns.com/foaf/0.1/img"/>
    ///</summary>
    public static readonly Property img = new Property(new Uri("http://xmlns.com/foaf/0.1/img"));    

    ///<summary>
    ///A depiction of some thing.
    ///<see cref="http://xmlns.com/foaf/0.1/depiction"/>
    ///</summary>
    public static readonly Property depiction = new Property(new Uri("http://xmlns.com/foaf/0.1/depiction"));    

    ///<summary>
    ///A thing depicted in this representation.
    ///<see cref="http://xmlns.com/foaf/0.1/depicts"/>
    ///</summary>
    public static readonly Property depicts = new Property(new Uri("http://xmlns.com/foaf/0.1/depicts"));    

    ///<summary>
    ///A derived thumbnail image.
    ///<see cref="http://xmlns.com/foaf/0.1/thumbnail"/>
    ///</summary>
    public static readonly Property thumbnail = new Property(new Uri("http://xmlns.com/foaf/0.1/thumbnail"));    

    ///<summary>
    ///A Myers Briggs (MBTI) personality classification.
    ///<see cref="http://xmlns.com/foaf/0.1/myersBriggs"/>
    ///</summary>
    public static readonly Property myersBriggs = new Property(new Uri("http://xmlns.com/foaf/0.1/myersBriggs"));    

    ///<summary>
    ///A workplace homepage of some person; the homepage of an organization they work for.
    ///<see cref="http://xmlns.com/foaf/0.1/workplaceHomepage"/>
    ///</summary>
    public static readonly Property workplaceHomepage = new Property(new Uri("http://xmlns.com/foaf/0.1/workplaceHomepage"));    

    ///<summary>
    ///A work info homepage of some person; a page about their work for some organization.
    ///<see cref="http://xmlns.com/foaf/0.1/workInfoHomepage"/>
    ///</summary>
    public static readonly Property workInfoHomepage = new Property(new Uri("http://xmlns.com/foaf/0.1/workInfoHomepage"));    

    ///<summary>
    ///A homepage of a school attended by the person.
    ///<see cref="http://xmlns.com/foaf/0.1/schoolHomepage"/>
    ///</summary>
    public static readonly Property schoolHomepage = new Property(new Uri("http://xmlns.com/foaf/0.1/schoolHomepage"));    

    ///<summary>
    ///A person known by this person (indicating some level of reciprocated interaction between the parties).
    ///<see cref="http://xmlns.com/foaf/0.1/knows"/>
    ///</summary>
    public static readonly Property knows = new Property(new Uri("http://xmlns.com/foaf/0.1/knows"));    

    ///<summary>
    ///A page about a topic of interest to this person.
    ///<see cref="http://xmlns.com/foaf/0.1/interest"/>
    ///</summary>
    public static readonly Property interest = new Property(new Uri("http://xmlns.com/foaf/0.1/interest"));    

    ///<summary>
    ///A thing of interest to this person.
    ///<see cref="http://xmlns.com/foaf/0.1/topic_interest"/>
    ///</summary>
    public static readonly Property topic_interest = new Property(new Uri("http://xmlns.com/foaf/0.1/topic_interest"));    

    ///<summary>
    ///A link to the publications of this person.
    ///<see cref="http://xmlns.com/foaf/0.1/publications"/>
    ///</summary>
    public static readonly Property publications = new Property(new Uri("http://xmlns.com/foaf/0.1/publications"));    

    ///<summary>
    ///A current project this person works on.
    ///<see cref="http://xmlns.com/foaf/0.1/currentProject"/>
    ///</summary>
    public static readonly Property currentProject = new Property(new Uri("http://xmlns.com/foaf/0.1/currentProject"));    

    ///<summary>
    ///A project this person has previously worked on.
    ///<see cref="http://xmlns.com/foaf/0.1/pastProject"/>
    ///</summary>
    public static readonly Property pastProject = new Property(new Uri("http://xmlns.com/foaf/0.1/pastProject"));    

    ///<summary>
    ///An organization funding a project or person.
    ///<see cref="http://xmlns.com/foaf/0.1/fundedBy"/>
    ///</summary>
    public static readonly Property fundedBy = new Property(new Uri("http://xmlns.com/foaf/0.1/fundedBy"));    

    ///<summary>
    ///A logo representing some thing.
    ///<see cref="http://xmlns.com/foaf/0.1/logo"/>
    ///</summary>
    public static readonly Property logo = new Property(new Uri("http://xmlns.com/foaf/0.1/logo"));    

    ///<summary>
    ///A topic of some page or document.
    ///<see cref="http://xmlns.com/foaf/0.1/topic"/>
    ///</summary>
    public static readonly Property topic = new Property(new Uri("http://xmlns.com/foaf/0.1/topic"));    

    ///<summary>
    ///The primary topic of some page or document.
    ///<see cref="http://xmlns.com/foaf/0.1/primaryTopic"/>
    ///</summary>
    public static readonly Property primaryTopic = new Property(new Uri("http://xmlns.com/foaf/0.1/primaryTopic"));    

    ///<summary>
    ///The underlying or 'focal' entity associated with some SKOS-described concept.
    ///<see cref="http://xmlns.com/foaf/0.1/focus"/>
    ///</summary>
    public static readonly Property focus = new Property(new Uri("http://xmlns.com/foaf/0.1/focus"));    

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2004/02/skos/core#Concept"/>
    ///</summary>
    public static readonly Resource Concept = new Resource(new Uri("http://www.w3.org/2004/02/skos/core#Concept"));    

    ///<summary>
    ///A document that this thing is the primary topic of.
    ///<see cref="http://xmlns.com/foaf/0.1/isPrimaryTopicOf"/>
    ///</summary>
    public static readonly Property isPrimaryTopicOf = new Property(new Uri("http://xmlns.com/foaf/0.1/isPrimaryTopicOf"));    

    ///<summary>
    ///A page or document about this thing.
    ///<see cref="http://xmlns.com/foaf/0.1/page"/>
    ///</summary>
    public static readonly Property page = new Property(new Uri("http://xmlns.com/foaf/0.1/page"));    

    ///<summary>
    ///A theme.
    ///<see cref="http://xmlns.com/foaf/0.1/theme"/>
    ///</summary>
    public static readonly Property theme = new Property(new Uri("http://xmlns.com/foaf/0.1/theme"));    

    ///<summary>
    ///Indicates an account held by this agent.
    ///<see cref="http://xmlns.com/foaf/0.1/account"/>
    ///</summary>
    public static readonly Property account = new Property(new Uri("http://xmlns.com/foaf/0.1/account"));    

    ///<summary>
    ///Indicates an account held by this agent.
    ///<see cref="http://xmlns.com/foaf/0.1/holdsAccount"/>
    ///</summary>
    public static readonly Property holdsAccount = new Property(new Uri("http://xmlns.com/foaf/0.1/holdsAccount"));    

    ///<summary>
    ///Indicates a homepage of the service provide for this online account.
    ///<see cref="http://xmlns.com/foaf/0.1/accountServiceHomepage"/>
    ///</summary>
    public static readonly Property accountServiceHomepage = new Property(new Uri("http://xmlns.com/foaf/0.1/accountServiceHomepage"));    

    ///<summary>
    ///Indicates the name (identifier) associated with this online account.
    ///<see cref="http://xmlns.com/foaf/0.1/accountName"/>
    ///</summary>
    public static readonly Property accountName = new Property(new Uri("http://xmlns.com/foaf/0.1/accountName"));    

    ///<summary>
    ///Indicates a member of a Group
    ///<see cref="http://xmlns.com/foaf/0.1/member"/>
    ///</summary>
    public static readonly Property member = new Property(new Uri("http://xmlns.com/foaf/0.1/member"));    

    ///<summary>
    ///Indicates the class of individuals that are a member of a Group
    ///<see cref="http://xmlns.com/foaf/0.1/membershipClass"/>
    ///</summary>
    public static readonly Property membershipClass = new Property(new Uri("http://xmlns.com/foaf/0.1/membershipClass"));    

    ///<summary>
    ///The birthday of this Agent, represented in mm-dd string form, eg. '12-31'.
    ///<see cref="http://xmlns.com/foaf/0.1/birthday"/>
    ///</summary>
    public static readonly Property birthday = new Property(new Uri("http://xmlns.com/foaf/0.1/birthday"));    

    ///<summary>
    ///The age in years of some agent.
    ///<see cref="http://xmlns.com/foaf/0.1/age"/>
    ///</summary>
    public static readonly Property age = new Property(new Uri("http://xmlns.com/foaf/0.1/age"));    

    ///<summary>
    ///A string expressing what the user is happy for the general public (normally) to know about their current activity.
    ///<see cref="http://xmlns.com/foaf/0.1/status"/>
    ///</summary>
    public static readonly Property status = new Property(new Uri("http://xmlns.com/foaf/0.1/status"));
}
///<summary>
///Friend of a Friend (FOAF) vocabulary
///
///</summary>
public static class FOAF
{
    public static readonly Uri Namespace = new Uri("http://xmlns.com/foaf/0.1/");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "FOAF";
    public static string GetPrefix() { return Prefix; } 

    ///<summary>
    ///
    ///<see cref="http://xmlns.com/foaf/0.1/"/>
    ///</summary>
    public const string _0_1 = "http://xmlns.com/foaf/0.1/";

    ///<summary>
    ///
    ///<see cref="http://xmlns.com/wot/0.1/assurance"/>
    ///</summary>
    public const string assurance = "http://xmlns.com/wot/0.1/assurance";

    ///<summary>
    ///
    ///<see cref="http://xmlns.com/wot/0.1/src_assurance"/>
    ///</summary>
    public const string src_assurance = "http://xmlns.com/wot/0.1/src_assurance";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2003/06/sw-vocab-status/ns#term_status"/>
    ///</summary>
    public const string term_status = "http://www.w3.org/2003/06/sw-vocab-status/ns#term_status";

    ///<summary>
    ///
    ///<see cref="http://purl.org/dc/elements/1.1/description"/>
    ///</summary>
    public const string description = "http://purl.org/dc/elements/1.1/description";

    ///<summary>
    ///
    ///<see cref="http://purl.org/dc/elements/1.1/title"/>
    ///</summary>
    public const string title = "http://purl.org/dc/elements/1.1/title";

    ///<summary>
    ///
    ///<see cref="http://purl.org/dc/elements/1.1/date"/>
    ///</summary>
    public const string date = "http://purl.org/dc/elements/1.1/date";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2000/01/rdf-schema#Class"/>
    ///</summary>
    public const string Class = "http://www.w3.org/2000/01/rdf-schema#Class";

    ///<summary>
    ///A foaf:LabelProperty is any RDF property with texual values that serve as labels.
    ///<see cref="http://xmlns.com/foaf/0.1/LabelProperty"/>
    ///</summary>
    public const string LabelProperty = "http://xmlns.com/foaf/0.1/LabelProperty";

    ///<summary>
    ///A person.
    ///<see cref="http://xmlns.com/foaf/0.1/Person"/>
    ///</summary>
    public const string Person = "http://xmlns.com/foaf/0.1/Person";

    ///<summary>
    ///An agent (eg. person, group, software or physical artifact).
    ///<see cref="http://xmlns.com/foaf/0.1/Agent"/>
    ///</summary>
    public const string Agent = "http://xmlns.com/foaf/0.1/Agent";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2003/01/geo/wgs84_pos#SpatialThing"/>
    ///</summary>
    public const string SpatialThing = "http://www.w3.org/2003/01/geo/wgs84_pos#SpatialThing";

    ///<summary>
    ///A document.
    ///<see cref="http://xmlns.com/foaf/0.1/Document"/>
    ///</summary>
    public const string Document = "http://xmlns.com/foaf/0.1/Document";

    ///<summary>
    ///An organization.
    ///<see cref="http://xmlns.com/foaf/0.1/Organization"/>
    ///</summary>
    public const string Organization = "http://xmlns.com/foaf/0.1/Organization";

    ///<summary>
    ///A class of Agents.
    ///<see cref="http://xmlns.com/foaf/0.1/Group"/>
    ///</summary>
    public const string Group = "http://xmlns.com/foaf/0.1/Group";

    ///<summary>
    ///A project (a collective endeavour of some kind).
    ///<see cref="http://xmlns.com/foaf/0.1/Project"/>
    ///</summary>
    public const string Project = "http://xmlns.com/foaf/0.1/Project";

    ///<summary>
    ///An image.
    ///<see cref="http://xmlns.com/foaf/0.1/Image"/>
    ///</summary>
    public const string Image = "http://xmlns.com/foaf/0.1/Image";

    ///<summary>
    ///A personal profile RDF document.
    ///<see cref="http://xmlns.com/foaf/0.1/PersonalProfileDocument"/>
    ///</summary>
    public const string PersonalProfileDocument = "http://xmlns.com/foaf/0.1/PersonalProfileDocument";

    ///<summary>
    ///An online account.
    ///<see cref="http://xmlns.com/foaf/0.1/OnlineAccount"/>
    ///</summary>
    public const string OnlineAccount = "http://xmlns.com/foaf/0.1/OnlineAccount";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2002/07/owl#Thing"/>
    ///</summary>
    public const string Thing = "http://www.w3.org/2002/07/owl#Thing";

    ///<summary>
    ///An online gaming account.
    ///<see cref="http://xmlns.com/foaf/0.1/OnlineGamingAccount"/>
    ///</summary>
    public const string OnlineGamingAccount = "http://xmlns.com/foaf/0.1/OnlineGamingAccount";

    ///<summary>
    ///An online e-commerce account.
    ///<see cref="http://xmlns.com/foaf/0.1/OnlineEcommerceAccount"/>
    ///</summary>
    public const string OnlineEcommerceAccount = "http://xmlns.com/foaf/0.1/OnlineEcommerceAccount";

    ///<summary>
    ///An online chat account.
    ///<see cref="http://xmlns.com/foaf/0.1/OnlineChatAccount"/>
    ///</summary>
    public const string OnlineChatAccount = "http://xmlns.com/foaf/0.1/OnlineChatAccount";

    ///<summary>
    ///A  personal mailbox, ie. an Internet mailbox associated with exactly one owner, the first owner of this mailbox. This is a 'static inverse functional property', in that  there is (across time and change) at most one individual that ever has any particular value for foaf:mbox.
    ///<see cref="http://xmlns.com/foaf/0.1/mbox"/>
    ///</summary>
    public const string mbox = "http://xmlns.com/foaf/0.1/mbox";

    ///<summary>
    ///The sha1sum of the URI of an Internet mailbox associated with exactly one owner, the  first owner of the mailbox.
    ///<see cref="http://xmlns.com/foaf/0.1/mbox_sha1sum"/>
    ///</summary>
    public const string mbox_sha1sum = "http://xmlns.com/foaf/0.1/mbox_sha1sum";

    ///<summary>
    ///The gender of this Agent (typically but not necessarily 'male' or 'female').
    ///<see cref="http://xmlns.com/foaf/0.1/gender"/>
    ///</summary>
    public const string gender = "http://xmlns.com/foaf/0.1/gender";

    ///<summary>
    ///A textual geekcode for this person, see http://www.geekcode.com/geek.html
    ///<see cref="http://xmlns.com/foaf/0.1/geekcode"/>
    ///</summary>
    public const string geekcode = "http://xmlns.com/foaf/0.1/geekcode";

    ///<summary>
    ///A checksum for the DNA of some thing. Joke.
    ///<see cref="http://xmlns.com/foaf/0.1/dnaChecksum"/>
    ///</summary>
    public const string dnaChecksum = "http://xmlns.com/foaf/0.1/dnaChecksum";

    ///<summary>
    ///A sha1sum hash, in hex.
    ///<see cref="http://xmlns.com/foaf/0.1/sha1"/>
    ///</summary>
    public const string sha1 = "http://xmlns.com/foaf/0.1/sha1";

    ///<summary>
    ///A location that something is based near, for some broadly human notion of near.
    ///<see cref="http://xmlns.com/foaf/0.1/based_near"/>
    ///</summary>
    public const string based_near = "http://xmlns.com/foaf/0.1/based_near";

    ///<summary>
    ///Title (Mr, Mrs, Ms, Dr. etc)
    ///<see cref="http://xmlns.com/foaf/0.1/title"/>
    ///</summary>
    public const string title_0 = "http://xmlns.com/foaf/0.1/title";

    ///<summary>
    ///A short informal nickname characterising an agent (includes login identifiers, IRC and other chat nicknames).
    ///<see cref="http://xmlns.com/foaf/0.1/nick"/>
    ///</summary>
    public const string nick = "http://xmlns.com/foaf/0.1/nick";

    ///<summary>
    ///A jabber ID for something.
    ///<see cref="http://xmlns.com/foaf/0.1/jabberID"/>
    ///</summary>
    public const string jabberID = "http://xmlns.com/foaf/0.1/jabberID";

    ///<summary>
    ///An AIM chat ID
    ///<see cref="http://xmlns.com/foaf/0.1/aimChatID"/>
    ///</summary>
    public const string aimChatID = "http://xmlns.com/foaf/0.1/aimChatID";

    ///<summary>
    ///A Skype ID
    ///<see cref="http://xmlns.com/foaf/0.1/skypeID"/>
    ///</summary>
    public const string skypeID = "http://xmlns.com/foaf/0.1/skypeID";

    ///<summary>
    ///An ICQ chat ID
    ///<see cref="http://xmlns.com/foaf/0.1/icqChatID"/>
    ///</summary>
    public const string icqChatID = "http://xmlns.com/foaf/0.1/icqChatID";

    ///<summary>
    ///A Yahoo chat ID
    ///<see cref="http://xmlns.com/foaf/0.1/yahooChatID"/>
    ///</summary>
    public const string yahooChatID = "http://xmlns.com/foaf/0.1/yahooChatID";

    ///<summary>
    ///An MSN chat ID
    ///<see cref="http://xmlns.com/foaf/0.1/msnChatID"/>
    ///</summary>
    public const string msnChatID = "http://xmlns.com/foaf/0.1/msnChatID";

    ///<summary>
    ///A name for some thing.
    ///<see cref="http://xmlns.com/foaf/0.1/name"/>
    ///</summary>
    public const string name = "http://xmlns.com/foaf/0.1/name";

    ///<summary>
    ///The first name of a person.
    ///<see cref="http://xmlns.com/foaf/0.1/firstName"/>
    ///</summary>
    public const string firstName = "http://xmlns.com/foaf/0.1/firstName";

    ///<summary>
    ///The last name of a person.
    ///<see cref="http://xmlns.com/foaf/0.1/lastName"/>
    ///</summary>
    public const string lastName = "http://xmlns.com/foaf/0.1/lastName";

    ///<summary>
    ///The given name of some person.
    ///<see cref="http://xmlns.com/foaf/0.1/givenName"/>
    ///</summary>
    public const string givenName = "http://xmlns.com/foaf/0.1/givenName";

    ///<summary>
    ///The given name of some person.
    ///<see cref="http://xmlns.com/foaf/0.1/givenname"/>
    ///</summary>
    public const string givenname = "http://xmlns.com/foaf/0.1/givenname";

    ///<summary>
    ///The surname of some person.
    ///<see cref="http://xmlns.com/foaf/0.1/surname"/>
    ///</summary>
    public const string surname = "http://xmlns.com/foaf/0.1/surname";

    ///<summary>
    ///The family name of some person.
    ///<see cref="http://xmlns.com/foaf/0.1/family_name"/>
    ///</summary>
    public const string family_name = "http://xmlns.com/foaf/0.1/family_name";

    ///<summary>
    ///The family name of some person.
    ///<see cref="http://xmlns.com/foaf/0.1/familyName"/>
    ///</summary>
    public const string familyName = "http://xmlns.com/foaf/0.1/familyName";

    ///<summary>
    ///A phone,  specified using fully qualified tel: URI scheme (refs: http://www.w3.org/Addressing/schemes.html#tel).
    ///<see cref="http://xmlns.com/foaf/0.1/phone"/>
    ///</summary>
    public const string phone = "http://xmlns.com/foaf/0.1/phone";

    ///<summary>
    ///A homepage for some thing.
    ///<see cref="http://xmlns.com/foaf/0.1/homepage"/>
    ///</summary>
    public const string homepage = "http://xmlns.com/foaf/0.1/homepage";

    ///<summary>
    ///A weblog of some thing (whether person, group, company etc.).
    ///<see cref="http://xmlns.com/foaf/0.1/weblog"/>
    ///</summary>
    public const string weblog = "http://xmlns.com/foaf/0.1/weblog";

    ///<summary>
    ///An OpenID for an Agent.
    ///<see cref="http://xmlns.com/foaf/0.1/openid"/>
    ///</summary>
    public const string openid = "http://xmlns.com/foaf/0.1/openid";

    ///<summary>
    ///A tipjar document for this agent, describing means for payment and reward.
    ///<see cref="http://xmlns.com/foaf/0.1/tipjar"/>
    ///</summary>
    public const string tipjar = "http://xmlns.com/foaf/0.1/tipjar";

    ///<summary>
    ///A .plan comment, in the tradition of finger and '.plan' files.
    ///<see cref="http://xmlns.com/foaf/0.1/plan"/>
    ///</summary>
    public const string plan = "http://xmlns.com/foaf/0.1/plan";

    ///<summary>
    ///Something that was made by this agent.
    ///<see cref="http://xmlns.com/foaf/0.1/made"/>
    ///</summary>
    public const string made = "http://xmlns.com/foaf/0.1/made";

    ///<summary>
    ///An agent that  made this thing.
    ///<see cref="http://xmlns.com/foaf/0.1/maker"/>
    ///</summary>
    public const string maker = "http://xmlns.com/foaf/0.1/maker";

    ///<summary>
    ///An image that can be used to represent some thing (ie. those depictions which are particularly representative of something, eg. one's photo on a homepage).
    ///<see cref="http://xmlns.com/foaf/0.1/img"/>
    ///</summary>
    public const string img = "http://xmlns.com/foaf/0.1/img";

    ///<summary>
    ///A depiction of some thing.
    ///<see cref="http://xmlns.com/foaf/0.1/depiction"/>
    ///</summary>
    public const string depiction = "http://xmlns.com/foaf/0.1/depiction";

    ///<summary>
    ///A thing depicted in this representation.
    ///<see cref="http://xmlns.com/foaf/0.1/depicts"/>
    ///</summary>
    public const string depicts = "http://xmlns.com/foaf/0.1/depicts";

    ///<summary>
    ///A derived thumbnail image.
    ///<see cref="http://xmlns.com/foaf/0.1/thumbnail"/>
    ///</summary>
    public const string thumbnail = "http://xmlns.com/foaf/0.1/thumbnail";

    ///<summary>
    ///A Myers Briggs (MBTI) personality classification.
    ///<see cref="http://xmlns.com/foaf/0.1/myersBriggs"/>
    ///</summary>
    public const string myersBriggs = "http://xmlns.com/foaf/0.1/myersBriggs";

    ///<summary>
    ///A workplace homepage of some person; the homepage of an organization they work for.
    ///<see cref="http://xmlns.com/foaf/0.1/workplaceHomepage"/>
    ///</summary>
    public const string workplaceHomepage = "http://xmlns.com/foaf/0.1/workplaceHomepage";

    ///<summary>
    ///A work info homepage of some person; a page about their work for some organization.
    ///<see cref="http://xmlns.com/foaf/0.1/workInfoHomepage"/>
    ///</summary>
    public const string workInfoHomepage = "http://xmlns.com/foaf/0.1/workInfoHomepage";

    ///<summary>
    ///A homepage of a school attended by the person.
    ///<see cref="http://xmlns.com/foaf/0.1/schoolHomepage"/>
    ///</summary>
    public const string schoolHomepage = "http://xmlns.com/foaf/0.1/schoolHomepage";

    ///<summary>
    ///A person known by this person (indicating some level of reciprocated interaction between the parties).
    ///<see cref="http://xmlns.com/foaf/0.1/knows"/>
    ///</summary>
    public const string knows = "http://xmlns.com/foaf/0.1/knows";

    ///<summary>
    ///A page about a topic of interest to this person.
    ///<see cref="http://xmlns.com/foaf/0.1/interest"/>
    ///</summary>
    public const string interest = "http://xmlns.com/foaf/0.1/interest";

    ///<summary>
    ///A thing of interest to this person.
    ///<see cref="http://xmlns.com/foaf/0.1/topic_interest"/>
    ///</summary>
    public const string topic_interest = "http://xmlns.com/foaf/0.1/topic_interest";

    ///<summary>
    ///A link to the publications of this person.
    ///<see cref="http://xmlns.com/foaf/0.1/publications"/>
    ///</summary>
    public const string publications = "http://xmlns.com/foaf/0.1/publications";

    ///<summary>
    ///A current project this person works on.
    ///<see cref="http://xmlns.com/foaf/0.1/currentProject"/>
    ///</summary>
    public const string currentProject = "http://xmlns.com/foaf/0.1/currentProject";

    ///<summary>
    ///A project this person has previously worked on.
    ///<see cref="http://xmlns.com/foaf/0.1/pastProject"/>
    ///</summary>
    public const string pastProject = "http://xmlns.com/foaf/0.1/pastProject";

    ///<summary>
    ///An organization funding a project or person.
    ///<see cref="http://xmlns.com/foaf/0.1/fundedBy"/>
    ///</summary>
    public const string fundedBy = "http://xmlns.com/foaf/0.1/fundedBy";

    ///<summary>
    ///A logo representing some thing.
    ///<see cref="http://xmlns.com/foaf/0.1/logo"/>
    ///</summary>
    public const string logo = "http://xmlns.com/foaf/0.1/logo";

    ///<summary>
    ///A topic of some page or document.
    ///<see cref="http://xmlns.com/foaf/0.1/topic"/>
    ///</summary>
    public const string topic = "http://xmlns.com/foaf/0.1/topic";

    ///<summary>
    ///The primary topic of some page or document.
    ///<see cref="http://xmlns.com/foaf/0.1/primaryTopic"/>
    ///</summary>
    public const string primaryTopic = "http://xmlns.com/foaf/0.1/primaryTopic";

    ///<summary>
    ///The underlying or 'focal' entity associated with some SKOS-described concept.
    ///<see cref="http://xmlns.com/foaf/0.1/focus"/>
    ///</summary>
    public const string focus = "http://xmlns.com/foaf/0.1/focus";

    ///<summary>
    ///
    ///<see cref="http://www.w3.org/2004/02/skos/core#Concept"/>
    ///</summary>
    public const string Concept = "http://www.w3.org/2004/02/skos/core#Concept";

    ///<summary>
    ///A document that this thing is the primary topic of.
    ///<see cref="http://xmlns.com/foaf/0.1/isPrimaryTopicOf"/>
    ///</summary>
    public const string isPrimaryTopicOf = "http://xmlns.com/foaf/0.1/isPrimaryTopicOf";

    ///<summary>
    ///A page or document about this thing.
    ///<see cref="http://xmlns.com/foaf/0.1/page"/>
    ///</summary>
    public const string page = "http://xmlns.com/foaf/0.1/page";

    ///<summary>
    ///A theme.
    ///<see cref="http://xmlns.com/foaf/0.1/theme"/>
    ///</summary>
    public const string theme = "http://xmlns.com/foaf/0.1/theme";

    ///<summary>
    ///Indicates an account held by this agent.
    ///<see cref="http://xmlns.com/foaf/0.1/account"/>
    ///</summary>
    public const string account = "http://xmlns.com/foaf/0.1/account";

    ///<summary>
    ///Indicates an account held by this agent.
    ///<see cref="http://xmlns.com/foaf/0.1/holdsAccount"/>
    ///</summary>
    public const string holdsAccount = "http://xmlns.com/foaf/0.1/holdsAccount";

    ///<summary>
    ///Indicates a homepage of the service provide for this online account.
    ///<see cref="http://xmlns.com/foaf/0.1/accountServiceHomepage"/>
    ///</summary>
    public const string accountServiceHomepage = "http://xmlns.com/foaf/0.1/accountServiceHomepage";

    ///<summary>
    ///Indicates the name (identifier) associated with this online account.
    ///<see cref="http://xmlns.com/foaf/0.1/accountName"/>
    ///</summary>
    public const string accountName = "http://xmlns.com/foaf/0.1/accountName";

    ///<summary>
    ///Indicates a member of a Group
    ///<see cref="http://xmlns.com/foaf/0.1/member"/>
    ///</summary>
    public const string member = "http://xmlns.com/foaf/0.1/member";

    ///<summary>
    ///Indicates the class of individuals that are a member of a Group
    ///<see cref="http://xmlns.com/foaf/0.1/membershipClass"/>
    ///</summary>
    public const string membershipClass = "http://xmlns.com/foaf/0.1/membershipClass";

    ///<summary>
    ///The birthday of this Agent, represented in mm-dd string form, eg. '12-31'.
    ///<see cref="http://xmlns.com/foaf/0.1/birthday"/>
    ///</summary>
    public const string birthday = "http://xmlns.com/foaf/0.1/birthday";

    ///<summary>
    ///The age in years of some agent.
    ///<see cref="http://xmlns.com/foaf/0.1/age"/>
    ///</summary>
    public const string age = "http://xmlns.com/foaf/0.1/age";

    ///<summary>
    ///A string expressing what the user is happy for the general public (normally) to know about their current activity.
    ///<see cref="http://xmlns.com/foaf/0.1/status"/>
    ///</summary>
    public const string status = "http://xmlns.com/foaf/0.1/status";
}
///<summary>
///
///
///</summary>
public class dces : Ontology
{
    public static readonly Uri Namespace = new Uri("http://purl.org/dc/elements/1.1/");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "dces";
    public static string GetPrefix() { return Prefix; }     

    ///<summary>
    ///
    ///<see cref="http://purl.org/dc/elements/1.1/"/>
    ///</summary>
    public static readonly Resource _1_1 = new Resource(new Uri("http://purl.org/dc/elements/1.1/"));    

    ///<summary>
    ///(An entity responsible for making contributions to the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/contributor"/>
    ///</summary>
    public static readonly Property contributor = new Property(new Uri("http://purl.org/dc/elements/1.1/contributor"));    

    ///<summary>
    ///(The spatial or temporal topic of the resource, the spatial applicability of the resource, or the jurisdiction under which the resource is relevant., en)
    ///<see cref="http://purl.org/dc/elements/1.1/coverage"/>
    ///</summary>
    public static readonly Property coverage = new Property(new Uri("http://purl.org/dc/elements/1.1/coverage"));    

    ///<summary>
    ///(An entity primarily responsible for making the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/creator"/>
    ///</summary>
    public static readonly Property creator = new Property(new Uri("http://purl.org/dc/elements/1.1/creator"));    

    ///<summary>
    ///(A point or period of time associated with an event in the lifecycle of the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/date"/>
    ///</summary>
    public static readonly Property date = new Property(new Uri("http://purl.org/dc/elements/1.1/date"));    

    ///<summary>
    ///(An account of the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/description"/>
    ///</summary>
    public static readonly Property description = new Property(new Uri("http://purl.org/dc/elements/1.1/description"));    

    ///<summary>
    ///(The file format, physical medium, or dimensions of the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/format"/>
    ///</summary>
    public static readonly Property format = new Property(new Uri("http://purl.org/dc/elements/1.1/format"));    

    ///<summary>
    ///(An unambiguous reference to the resource within a given context., en)
    ///<see cref="http://purl.org/dc/elements/1.1/identifier"/>
    ///</summary>
    public static readonly Property identifier = new Property(new Uri("http://purl.org/dc/elements/1.1/identifier"));    

    ///<summary>
    ///(A language of the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/language"/>
    ///</summary>
    public static readonly Property language = new Property(new Uri("http://purl.org/dc/elements/1.1/language"));    

    ///<summary>
    ///(An entity responsible for making the resource available., en)
    ///<see cref="http://purl.org/dc/elements/1.1/publisher"/>
    ///</summary>
    public static readonly Property publisher = new Property(new Uri("http://purl.org/dc/elements/1.1/publisher"));    

    ///<summary>
    ///(A related resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/relation"/>
    ///</summary>
    public static readonly Property relation = new Property(new Uri("http://purl.org/dc/elements/1.1/relation"));    

    ///<summary>
    ///(Information about rights held in and over the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/rights"/>
    ///</summary>
    public static readonly Property rights = new Property(new Uri("http://purl.org/dc/elements/1.1/rights"));    

    ///<summary>
    ///(A related resource from which the described resource is derived., en)
    ///<see cref="http://purl.org/dc/elements/1.1/source"/>
    ///</summary>
    public static readonly Property source = new Property(new Uri("http://purl.org/dc/elements/1.1/source"));    

    ///<summary>
    ///(The topic of the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/subject"/>
    ///</summary>
    public static readonly Property subject = new Property(new Uri("http://purl.org/dc/elements/1.1/subject"));    

    ///<summary>
    ///(A name given to the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/title"/>
    ///</summary>
    public static readonly Property title = new Property(new Uri("http://purl.org/dc/elements/1.1/title"));    

    ///<summary>
    ///(The nature or genre of the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/type"/>
    ///</summary>
    public static readonly Property type = new Property(new Uri("http://purl.org/dc/elements/1.1/type"));
}
///<summary>
///
///
///</summary>
public static class DCES
{
    public static readonly Uri Namespace = new Uri("http://purl.org/dc/elements/1.1/");
    public static Uri GetNamespace() { return Namespace; }
    
    public static readonly string Prefix = "DCES";
    public static string GetPrefix() { return Prefix; } 

    ///<summary>
    ///
    ///<see cref="http://purl.org/dc/elements/1.1/"/>
    ///</summary>
    public const string _1_1 = "http://purl.org/dc/elements/1.1/";

    ///<summary>
    ///(An entity responsible for making contributions to the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/contributor"/>
    ///</summary>
    public const string contributor = "http://purl.org/dc/elements/1.1/contributor";

    ///<summary>
    ///(The spatial or temporal topic of the resource, the spatial applicability of the resource, or the jurisdiction under which the resource is relevant., en)
    ///<see cref="http://purl.org/dc/elements/1.1/coverage"/>
    ///</summary>
    public const string coverage = "http://purl.org/dc/elements/1.1/coverage";

    ///<summary>
    ///(An entity primarily responsible for making the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/creator"/>
    ///</summary>
    public const string creator = "http://purl.org/dc/elements/1.1/creator";

    ///<summary>
    ///(A point or period of time associated with an event in the lifecycle of the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/date"/>
    ///</summary>
    public const string date = "http://purl.org/dc/elements/1.1/date";

    ///<summary>
    ///(An account of the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/description"/>
    ///</summary>
    public const string description = "http://purl.org/dc/elements/1.1/description";

    ///<summary>
    ///(The file format, physical medium, or dimensions of the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/format"/>
    ///</summary>
    public const string format = "http://purl.org/dc/elements/1.1/format";

    ///<summary>
    ///(An unambiguous reference to the resource within a given context., en)
    ///<see cref="http://purl.org/dc/elements/1.1/identifier"/>
    ///</summary>
    public const string identifier = "http://purl.org/dc/elements/1.1/identifier";

    ///<summary>
    ///(A language of the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/language"/>
    ///</summary>
    public const string language = "http://purl.org/dc/elements/1.1/language";

    ///<summary>
    ///(An entity responsible for making the resource available., en)
    ///<see cref="http://purl.org/dc/elements/1.1/publisher"/>
    ///</summary>
    public const string publisher = "http://purl.org/dc/elements/1.1/publisher";

    ///<summary>
    ///(A related resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/relation"/>
    ///</summary>
    public const string relation = "http://purl.org/dc/elements/1.1/relation";

    ///<summary>
    ///(Information about rights held in and over the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/rights"/>
    ///</summary>
    public const string rights = "http://purl.org/dc/elements/1.1/rights";

    ///<summary>
    ///(A related resource from which the described resource is derived., en)
    ///<see cref="http://purl.org/dc/elements/1.1/source"/>
    ///</summary>
    public const string source = "http://purl.org/dc/elements/1.1/source";

    ///<summary>
    ///(The topic of the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/subject"/>
    ///</summary>
    public const string subject = "http://purl.org/dc/elements/1.1/subject";

    ///<summary>
    ///(A name given to the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/title"/>
    ///</summary>
    public const string title = "http://purl.org/dc/elements/1.1/title";

    ///<summary>
    ///(The nature or genre of the resource., en)
    ///<see cref="http://purl.org/dc/elements/1.1/type"/>
    ///</summary>
    public const string type = "http://purl.org/dc/elements/1.1/type";
}
}