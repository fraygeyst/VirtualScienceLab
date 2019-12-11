# chapters/nebenlaufigkeit/src/condition_variable.py
# Beispiel zur Verwendung von Condition-Objekten

import threading


class Counter:
    def __init__(self):
        self.count = 0
        self.cv = threading.Condition(threading.RLock())

    def increment(self, value=1):
        if 0 >= value:
            return

        with self.cv:
            self.count += 1
            self.increment(value - 1)


class IncrementerThread(threading.Thread):
    def __init__(self, counter):
        threading.Thread.__init__(self)
        self.counter = counter

    def run(self):
        for _ in range(1000000):
            self.counter.increment()


counter = Counter()
threads = []
for _ in range(10):
    thread = IncrementerThread(counter)
    thread.start()
    threads.append(thread)

for thread in threads:
    thread.join()

print(counter.count)
