# exercises/src/nebenlaufigkeit10.py
# Lösung zur Aufgabe

import threading
import queue


def square(queue):
    while True:
        value = queue.get()
        print(value, " -> ", value * value)
        queue.task_done()


queue = queue.Queue(20)
for i in range(1, 21):
    queue.put(i)

for _ in range(3):
    worker = threading.Thread(target=square, args=(queue,),
                daemon=True)
    worker.start()

queue.join()
print("Finished")
