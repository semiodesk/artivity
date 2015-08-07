import os
import sys

def pid_is_running(pid):
    try:
        os.kill(pid, 0)

    except OSError:
        return

    else:
        return pid

def write_pidfile_or_die(pidfile, log):

    if pidfile.startswith('~'):
	pidfile = pidfile.replace('~', os.path.expanduser('~'))

    if os.path.exists(pidfile):
        pid = int(open(pidfile).read())

        if pid_is_running(pid):
            log.fatal("Detected a running process with id {0}".format(pid))
            raise SystemExit

        else:
            os.remove(pidfile)

    open(pidfile, 'w+').write(str(os.getpid()))

    return pidfile

def delete_pidfile(pidfile, log):

    if pidfile.startswith('~'):
	pidfile = pidfile.replace('~', os.path.expanduser('~'))

    if os.path.exists(pidfile):
	log.info("Deleting pid file of current process..")
	os.remove(pidfile)

    sys.exit(0)
