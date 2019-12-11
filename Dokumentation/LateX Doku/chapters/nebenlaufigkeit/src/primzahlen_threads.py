# chapters/nebenlaufigkeit/src/primzahlen_threads.py
# Primzahlen zählen mit Threads

import threading
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


threads = []
amount = 1000000
for i in range(10):
    numbers = [j + 1 + i * amount for j in range(amount)]
    thread = threading.Thread(target=countPrims,
                              args=(numbers,))
    threads.append(thread)

start = time.clock()
for thread in threads:
    thread.start()

for thread in threads:
    thread.join()
end = time.clock()

print("Finished in ", end - start, " seconds")
