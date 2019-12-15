liste = [1, 6, 3, 10, 5, 2, 7, 8, 9, 4]

total = 0
for x in liste:
    total = total + x

print(total)
# Ausgabe: 55

maxValue = liste[0]
for x in liste:
    if x > maxValue:
        maxValue = x

print(maxValue)
# Ausgabe: 10

fib = [0, 1]
for i in range(0, 8):
    fib.append(fib[-2] + fib[-1])

print(fib)
# Ausgabe: [0, 1, 1, 2, 3, 5, 8, 13, 21, 34]
