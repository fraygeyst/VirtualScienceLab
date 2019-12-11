# exercises/src/nebenlaufigkeit08.py
# Lösung zur Aufgabe

import threading
import time
import random


class CarPark:
    def __init__(self, capacity):
        self.semaphore = threading.BoundedSemaphore(capacity)

    def is_free(self):
        return 0 < self.semaphore._value

    def enter(self):
        self.semaphore.acquire()
        self.show()

    def exit(self):
        self.semaphore.release()
        self.show()

    def show(self):
        print("CarPool capacity is ", self.semaphore._value)


class Car(threading.Thread):
    def __init__(self, carPark, id, barrier):
        threading.Thread.__init__(self)
        self.carPark = carPark
        self.id = id
        self.barrier = barrier

    def run(self):
        self.barrier.wait()

        while True:
            # Fahre eine zufällige Zeit umher
            time.sleep(random.uniform(0, 10))

            # Fahren in das Parkhaus
            print("Car", self.id, " wants to park")
            self.carPark.enter()
            print("Car", self.id, " entered the car park")

            # Parke eine zufällige Zeit
            print("Car", self.id, " is parking")
            time.sleep(random.uniform(0, 15))

            # Fahre aus dem Parkhaus
            self.carPark.exit()
            print("Car", self.id, " exited the car park")


carPark = CarPark(5)

amountOfCars = 10
barrier = threading.Barrier(amountOfCars)
for i in range(amountOfCars):
    cars = Car(carPark, i, barrier)
    cars.start()
