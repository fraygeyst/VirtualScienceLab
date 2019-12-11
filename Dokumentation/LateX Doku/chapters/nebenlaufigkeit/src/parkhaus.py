# chapters/nebenlaufigkeit/src/parkhaus.py
# Anschauungsbeispiel: Parkhaus

import threading
import time
import random


class CarPark:
    def __init__(self, capacity):
        self.cv = threading.Condition()
        self.capacity = capacity
        self.occupied = 0

    def is_free(self):
        return self.occupied < self.capacity

    def enter(self):
        with self.cv:
            self.cv.wait_for(self.is_free)
            self.occupied += 1
            self.show()

    def exit(self):
        with self.cv:
            self.occupied -= 1
            self.cv.notify()
            self.show()
    
    def show(self):
        currentCapacity = self.capacity - self.occupied
        print("CarPool capacity is ", currentCapacity)


class Car(threading.Thread):
    def __init__(self, carPark, id):
        threading.Thread.__init__(self)
        self.carPark = carPark
        self.id = id

    def run(self):
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
for i in range(10):
    cars = Car(carPark, i)
    cars.start()
