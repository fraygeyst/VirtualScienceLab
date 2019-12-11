set1 = set((4, 5, 6))
print(set1)
# Ausgabe: {4, 5, 6}

set2 = set((3,4,5))
print(set2)
# Ausgabe: {3, 4, 5}


print (set1 & set2)
# Ausgabe: {4, 5}

set3 = set1.union(set2)
print(set3)
# Ausgabe: {3, 4, 5, 6}

set3.add(7)
set3.add(4) #Error
print(set3)
# Ausgabe: {3, 4, 5, 6, 7}
