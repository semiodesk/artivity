#! /usr/bin/python

class art:
    BeginEditingEvent = "http://purl.org/ontologies/art/1.0/terms#BeginEditingEvent"
    EndEditingEvent = "http://purl.org/ontologies/art/1.0/terms#EndEditingEvent"
    EditEvent = "http://purl.org/ontologies/art/1.0/terms#EditEvent"
    UndoEvent = "http://purl.org/ontologies/art/1.0/terms#UndoEvent"
    RedoEvent = "http://purl.org/ontologies/art/1.0/terms#RedoEvent"

class zg:
    AccessEvent = "http://www.zeitgeist-project.com/ontologies/2010/01/27/zg#AccessEvent"
    ModifyEvent = "http://www.zeitgeist-project.com/ontologies/2010/01/27/zg#ModifyEvent"
    MoveEvent = "http://www.zeitgeist-project.com/ontologies/2010/01/27/zg#MoveEvent"

def abbreviate(uri):
    u = uri.replace("http://www.zeitgeist-project.com/ontologies/2010/01/27/zg#", "zg:")
    u = u.replace("http://purl.org/ontologies/art/1.0/terms#", "art:")
    return u
