# exercises/src/nebenlaufigkeit02.py
# Lösung zur Aufgabe

import threading
import time


class MyThread(threading.Thread):
    def __init__(self, name):
        threading.Thread.__init__(self)
        self.name = name

    def run(self):
        for i in range(10):
            print(self.name, str(i + 1))
            time.sleep(1)


for i in range(100):
    thread = MyThread("Thread" + str(i))
    thread.start()
