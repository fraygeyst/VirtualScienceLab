import math

pow = lambda a: a * a
root = lambda a: math.sqrt(a)

def pythagoras(a, b):
    return root(pow(a) + pow(b))

print(pythagoras(3,2))