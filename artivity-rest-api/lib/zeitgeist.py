"""
  AUTHORS
  - Sebastian Faubel <sebastian@semiodesk.com>
  - Moritz Eberl <moritz@semiodesk.com>
 
  Copyright (c) 2015 Semiodesk GmbH
 
  Released under GNU GPL, see the file 'COPYING' for more information
"""

# Since the module is named identically with the global zeitgeist module we disable relative imports.
from __future__ import absolute_import

import gobject

from time import time
from logging import getLogger

from zeitgeist.client import ZeitgeistClient
from zeitgeist.datamodel import *

class ZeitgeistProxy:
	"""Establishes a connection to the local Zeitgeist engine and 
	handles all communication from and to the log."""

	# A logger for program runtime information
	log = None

	# The Uniform Resource Identifier of the client.
	uri = None

	# The human readable name of the client.
	name = None

	# A reference to the Zeitgeist client.
	client = None

	# The thread context of the Zeitgeist client.
	context = None

	def __init__(self, uri, name):
		"""Create a new instance of the class."""
		
		self.log = getLogger(uri)
		self.uri = uri
		self.name = name

	def is_initialized(self):
		"""Checks if the Zeitgeist client is initialized."""

		if not self.client:
			self.log.error("The Zeigeist client is not initialized.")

		if not self.context:
			self.log.error("The GObject thread context is not initialized.")

		return self.client and self.context

	def init(self):
		"""Initializes the Zeitgeist client and its GObject thread context."""

		if not self.client:
			self.log.info("Initializing the Zeitgeist client..")

			subject = Subject()
			subject.interpretation = Interpretation.WEBSITE
			subject.manifestation = Manifestation.WEB_DATA_OBJECT
	
			event = Event()
			event.interpretation = Interpretation.ACCESS_EVENT
			event.manifestation = Manifestation.USER_ACTIVITY
			event.append_subject(subject)

			def on_zeitgeist_status_changed(enabled):
				# Since we are operating the gobject main loop manually,
				# we will not receive this event.
				return

			self.client = ZeitgeistClient()
			self.client.register_data_source(self.uri, self.name, "", [event], on_zeitgeist_status_changed) 

		if not self.context:
			self.log.info("Initializing the GObject main loop..")

			loop = gobject.MainLoop()

			gobject.threads_init()

			self.context = loop.get_context()

	def log_website_access(self, actor, url, title):
		"""Logs a browser access to a website in the Zeitgeist engine."""

		if not self.is_initialized():
			return

		subject = Subject.new_for_values(
			uri = url,
			interpretation = Interpretation.WEBSITE,
			manifestation = Manifestation.WEB_DATA_OBJECT,
			origin = url,
			mimetype = "text/html",
			text = title,
			storage = "net")

		event = Event.new_for_values(
			timestamp = int(time() * 1000),
			interpretation = Interpretation.ACCESS_EVENT,
			manifestation = Manifestation.USER_ACTIVITY,
			actor = actor,
			subjects=[subject])

		def on_zeitgeist_id_received(event_ids):
			# Since we are operating the gobject main loop manually,
			# we will not receive this event.
			return

		self.client.insert_events([event], on_zeitgeist_id_received)
		self.context.iteration(True)

