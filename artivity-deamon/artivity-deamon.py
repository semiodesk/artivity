#! /usr/bin/python

_URI = "http://artivity.arts.ac.uk/1.0/artivity-deamon"
_NAME = "Artivity Deamon"
_VERSION = "1.0"
_PORT = 8272

import gobject
import time

from zeitgeist.client import ZeitgeistClient
from zeitgeist.datamodel import *

zeitgeist = ZeitgeistClient()

def on_zeitgeist_status_changed(enabled):
	if enabled:
		print "Artivity deamon READY"
	else:
		print "Artivity deamon DISABLED"

def initialize_zeitgeist():
	uri = _URI
	name = _NAME
	
	subject = Subject()
	subject.interpretation = Interpretation.WEBSITE
	subject.manifestation = Manifestation.WEB_DATA_OBJECT
	
	event = Event()
	event.interpretation = Interpretation.ACCESS_EVENT
	event.manifestation = Manifestation.USER_ACTIVITY
	event.append_subject(subject)

	zeitgeist.register_data_source(uri, name, "", [event], on_zeitgeist_status_changed) 

def log(title, url):
	subject = Subject.new_for_values(
		uri = url,
		interpretation = Interpretation.WEBSITE,
		manifestation = Manifestation.WEB_DATA_OBJECT,
		origin = "application://google-chrome.desktop",
		mimetype = "text/html",
		text = title,
		storage = "net")

	event = Event.new_for_values(
		timestamp = int(time.time() * 1000),
		interpretation = Interpretation.ACCESS_EVENT,
		manifestation = Manifestation.USER_ACTIVITY,
		actor = "application://google-chrome.desktop",
		subjects=[subject])

	def on_zeitgeist_id_received(event_ids):
		print 'Logged %s with event id %d.' % (title, event_ids[0])

	zeitgeist.insert_events([event], on_zeitgeist_id_received)

from flask import Flask
from flask import request

app = Flask(_NAME)
context = None

@app.route('/artivity/1.0/activities', methods=['POST'])
def create_activity():
	if not request.json or not 'url' in request.json:
		abort(400)

	print request.json['title']
	print request.json['url']

	log(request.json['title'], request.json['url']);

	context.iteration(True)

	return ""

import Queue

if __name__ == '__main__':
	initialize_zeitgeist()

	loop = gobject.MainLoop()
	gobject.threads_init()

	context = loop.get_context()

	app.run(debug=True, port=_PORT)
