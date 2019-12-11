# chapters/nebenlaufigkeit/src/primzahlen_prozesse.py
# Primzahlen zählen mit Prozessen

import multiprocessing
import math
import time


def countPrims(numbers):
    count = 0
    for number in numbers:
        if 2 == number:
            count += 1
            continue
        if 2 > number or 0 == number % 2:
            continue

        for i in range(3, int(math.sqrt(number) + 2), 2):
            if 0 == number % i:
                break
        else:
            count += 1
    print("Found ", count, " primes")


if __name__ == "__main__":
    processes = []
    amount = 1000000
    for i in range(10):
        numbers = [j + 1 + i * amount for j in range(amount)]
        process = multiprocessing.Process(target=countPrims,
                                          args=(numbers,))
        processes.append(process)

    start = time.clock()
    for process in processes:
        process.start()

    for process in processes:
        process.join()
    end = time.clock()

    print("Finished in ", end - start, " seconds")
