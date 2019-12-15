# exercises/src/nebenlaufigkeit05.py
# Lösung zur Aufgabe

import threading


class Counter:
    def __init__(self):
        self.count = 0
        self.lock = threading.RLock()

    def increment(self, value=1):
        if 0 >= value:
            return

        with self.lock:
            self.count += 1
            self.increment(value - 1)


class IncrementerThread(threading.Thread):
    def __init__(self, counter):
        threading.Thread.__init__(self)
        self.counter = counter

    def run(self):
        for _ in range(1000000):
            self.counter.increment(5)


counter = Counter()
threads = []
for _ in range(10):
    thread = IncrementerThread(counter)
    thread.start()
    threads.append(thread)

for thread in threads:
    thread.join()

print(counter.count)
