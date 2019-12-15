# exercises/src/nebenlaufigkeit06.py
# L?sung zur Aufgabe

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
            self.cv.notify_all()


class IncrementerThread(threading.Thread):
    def __init__(self, counter, digit):
        threading.Thread.__init__(self)
        self.counter = counter
        self.digit = digit

    def run(self):
        cv = self.counter.cv
        for _ in range(1000000):
            with cv:
                cv.wait_for(self.check_condition)
                print(
                    "digit is: ",
                    self.digit,
                    ", counter is being incremented")
                self.counter.increment()

    def check_condition(self):
        print("digit is: ", self.digit, ", counter is: ",
                self.counter.count)
        return self.counter.count % 10 == self.digit


counter = Counter()
threads = []
for i in range(10):
    thread = IncrementerThread(counter, i)
    thread.start()
    threads.append(thread)

for thread in threads:
    thread.join()

print(counter.count)
