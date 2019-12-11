liste = []
i = 1
while i <= 10:
    liste.append(i)
    i = i+1

print(liste)
# Ausgabe: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]



laenge = len(liste)
print(laenge)
# Ausgabe: 10


liste.remove(4)
print(liste)
# Ausgabe: [1, 2, 3, 5, 6, 7, 8, 9, 10]


liste.insert(3,22)
print(liste)
# Ausgabe: [1, 2, 3, 22, 5, 6, 7, 8, 9, 10]


liste.append(1)
print(liste)
# Ausgabe: [1, 2, 3, 22, 5, 6, 7, 8, 9, 10, 1]


liste.sort()
print(liste)
# Ausgabe: [1, 1, 2, 3, 5, 6, 7, 8, 9, 10, 22]