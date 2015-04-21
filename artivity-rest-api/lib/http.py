"""
  AUTHORS
  - Sebastian Faubel <sebastian@semiodesk.com>
  - Moritz Eberl <moritz@semiodesk.com>
 
  Copyright (c) 2015 Semiodesk GmbH
 
  Released under GNU GPL, see the file 'COPYING' for more information
"""

from logging import getLogger

from werkzeug.wrappers import Request, Response
from werkzeug.serving import run_simple
from werkzeug.exceptions import abort

import json

class HttpServer:
	"""A simple HTTP Server providing a REST API for communication 
	with the Zeitgeist engine via D-Bus"""

	actors = {}

	def __init__(self, uri, name, zeitgeist):
		"""Create a new instance of the class."""

		self.log = getLogger(uri)
		self.uri = uri
		self.name = name
		self.zeitgeist = zeitgeist

	def run(self, host, port):
		self.log.info("Starting HTTP server for REST API..")

		run_simple(host, port, self.dispatch_request)

	@Request.application
	def dispatch_request(self, request):
		"""Calls the functions corresponding to the requested path."""

		if request.path == "/artivity/1.0/activities":
			return self.create_activity(request)
		elif request.path == "/artivity/1.0/status":
			return self.get_status(request)

		return Response()

	def create_activity(self, request):
		"""Handles a request to push a website access event into the Zeitgeist log."""

		if not request.data:
			abort(400)

		param = json.loads(request.data)

		if not param or not 'url' in param or not 'title' in param or not 'actor' in param:
			abort(400)

		url = param['url']
		title = param['title']
		actor = param['actor']

		self.log.info(url)

		self.zeitgeist.log_website_access(actor, url, title);

		return Response()

	def get_status(self, request):
		"""Handles a request to get the current logging status for a given actor."""

		self.log.info(request.data)

		if not request.data:
			abort(400)	

		param = json.loads(request.data)

		self.log.info(param)

		if not param or not 'actor' in param:
			abort(400)

		actor = param['actor']	

		if not actor in self.actors:
			self.actors[actor] = False

		if 'enabled' in param:
			self.actors[actor] = param['enabled']

		self.log.info(self.actors[actor])

		return Response(json.dumps(self.actors[actor]))
