"""
  AUTHORS
  - Sebastian Faubel <sebastian@semiodesk.com>
  - Moritz Eberl <moritz@semiodesk.com>
 
  Copyright (c) 2015 Semiodesk GmbH
 
  Released under GNU GPL, see the file 'COPYING' for more information
"""

import os
import json

def get_config_dir(log):
	configdir = os.path.expanduser('~') + '/.artivity/'

	if not os.path.isdir(configdir):
		try:
			os.makedirs(configdir)
		except IOError:
			log.error("Cannot create configuration directory: {0}".format(configdir))

	return configdir

def get_actors_file(log):
	configdir = get_config_dir(log)

	if os.path.isdir(configdir):
		return configdir + 'actors.json'
	else:
		return ""

def load_actors(log):
	configdir = get_config_dir(log)

	if not os.path.isdir(configdir):
		log.warning("Failed to load logging state of actors because configuration directory does not exist.")
		return;

	actorsfile = get_actors_file(log)

	if os.path.isfile(actorsfile):
		log.info("Loading logging state of actors..")

		return json.load(open(actorsfile, 'r'))
	else:
		return {}

def save_actors(log, actors):
	configdir = get_config_dir(log)

	if not os.path.isdir(configdir):
		log.warning("Failed to save logging state of actors because configuration directory does not exist.")
		return;

	actorsfile = get_actors_file(log)

	with open(actorsfile, 'w+') as f:
		log.info("Saving logging state of actors..")

		f.write(json.dumps(actors))
