# exercises/src/nebenlaufigkeit13.py
# Lösung zur Aufgabe

import multiprocessing


def square(queue):
    while True:
        value = queue.get()
        print(value, " -> ", value * value)
        queue.task_done()


if __name__ == "__main__":
    queue = multiprocessing.JoinableQueue(20)
    for i in range(1, 21):
        queue.put(i)

    for _ in range(3):
        worker = multiprocessing.Process(
            target=square, args=(queue,), daemon=True)
        worker.start()

    queue.join()
    print("Finished")
