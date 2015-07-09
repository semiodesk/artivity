#! /usr/bin/python

import time

class Timer:
    def __init__(self):
        self.__start_time = 0
        self.__stop_time = 0
        self.total_seconds = 0

    def start(self):
        self.__start_time = time.time()

    def stop(self):
        self.__stop_time = time.time()
        self.total_seconds = self.__stop_time - self.__start_time

    def reset(self):
        self.__start_time = 0
        self.__stop_time = 0
        self.total_seconds = 0
