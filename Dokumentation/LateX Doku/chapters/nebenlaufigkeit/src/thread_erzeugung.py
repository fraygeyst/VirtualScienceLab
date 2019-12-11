# chapters/nebenlaufigkeit/src/thread_erzeugung.py
# Beispiel zur Threaderzeugung

import threading


def task():
    # Nebenläufig ausgeführte Aufgabe


thread = threading.Thread(target=task)
thread.start()


class MyThread(threading.Thread):
    def run(self):
        # Nebenläufig ausgeführte Aufgabe


thread = MyThread()
thread.start()
