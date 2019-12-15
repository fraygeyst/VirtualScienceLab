# Uebung 1
def greet(target = "World"):
    print("Hello, " + target)

# Uebung 2
def greetMultiple(*targets):
    for target in targets:
        print("Hello, " + target)

# Uebung 3
def getGeometryWithMostCorners(*geoBodys):
    global geometryWithMostCorners
    geometryWithMostCorners = geoBodys[0]
    for x in geoBodys:
        if(geometryWithMostCorners[0] < x[0]):
            geometryWithMostCorners = x

# Uebung 4
import math
pow = lambda a: a * a
root = lambda a: math.sqrt(a)
def pythagoras(a, b):
    return root(pow(a) + pow(b))

# Uebung 5
def sum(*numbers):
    value = 0
    for number in numbers:
        value = value + number
    return value

def product(*numbers):
    value = 1
    for number in numbers:
        value = value * number
    return value