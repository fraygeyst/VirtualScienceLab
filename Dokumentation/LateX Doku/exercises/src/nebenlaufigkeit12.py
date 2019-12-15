# exercises/src/nebenlaufigkeit12.py
# Lösung zur Aufgabe

import multiprocessing
import time
import math


def isPrim(number):
    if 2 == number:
        return 1
    if 2 > number or 0 == number % 2:
        return 0

    for i in range(3, int(math.sqrt(number) + 2), 2):
        if 0 == number % i:
            return 0
    return 1


if __name__ == "__main__":
    multiprocessing.set_start_method("spawn")
    with multiprocessing.Pool(10) as p:
        start = time.clock()

        results = p.imap(isPrim, range(1, 10000001), 1000000)
        print(sum(results))

        end = time.clock()
        print("Finished in ", end - start, " seconds")
