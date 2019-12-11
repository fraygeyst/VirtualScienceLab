# exercises/src/nebenlaufigkeit01.py
# Lösung zur Aufgabe

import threading
import time


class MyThread(threading.Thread):
    def run(self):
        for i in range(10):
            print(str(i + 1))
            time.sleep(1)


thread = MyThread()
thread.start()
