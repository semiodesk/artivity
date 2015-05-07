#! /usr/bin/python

_URI = "http://arts.ac.uk/artivity/1.0/artivityd"
_NAME = "Artivity REST API Deamon"
_VERSION = "1.0"
_PORT = 8272
_PIDFILE = "~/.artivity-apid.pid"

from lib.zeitgeist import ZeitgeistProxy
from lib.http import HttpServer
from lib.pid import write_pidfile_or_die
from lib.pid import delete_pidfile

import signal
import logging

httpd = None
zeitgeist = None

def init_logging():
	formatter = logging.Formatter('[%(asctime)s] %(levelname)s: %(message)s')

	console = logging.StreamHandler()
	console.setLevel(logging.DEBUG)
	console.setFormatter(formatter)

	logger = logging.getLogger(_URI)
	logger.setLevel(logging.DEBUG)
	logger.addHandler(console)

def handle_sigterm(signal, frame):
	logger = logging.getLogger(_URI)
	logger.info("SIGTERM received. Shutting down.")

	httpd.shutdown()

	delete_pidfile(_PIDFILE, logger)

if __name__ == '__main__':
	init_logging()

	write_pidfile_or_die(_PIDFILE, logging.getLogger(_URI))

	signal.signal(signal.SIGTERM, handle_sigterm)

	print("%s v%s\n" % (_NAME, _VERSION))

	zeitgeist = ZeitgeistProxy(_URI, _NAME)
	zeitgeist.init()

	httpd = HttpServer(_URI, _NAME, zeitgeist)
	httpd.run('localhost', _PORT)
