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

		if(request.path == "/artivity/1.0/activities"):
			self.create_activity(request)

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
