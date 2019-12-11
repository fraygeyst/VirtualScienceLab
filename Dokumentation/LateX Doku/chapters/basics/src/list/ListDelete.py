# chapters/basics/src/list/ListDelete.py
# Beispiel zur Verwendung des del-Operators

liste = [1, 2, 3]
print(liste)

del liste[1]
print(liste)

del liste

print(liste)  # ERROR, da die Liste nicht mehr existiert!
