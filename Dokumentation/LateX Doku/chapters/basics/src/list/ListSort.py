# chapters/basics/src/list/ListSort.py
# Verwendung der sort-Methode

# Lexikographisches Sortieren
liste = ["b", "c", "a"]
print(liste)
liste.sort()
print(liste)

# Sortieren nach Zahlenwert
liste = [6, 1, 2, 3, 4, 5]
print(liste)
liste.sort()
print(liste)

# Umkehren der Sortierreihenfolge
liste.sort(reverse=True)
print(liste)

# Sortieren nach der Laenge einzelner Objekte
liste = ["aa", "aaa", "a"]


def sortFunc(x):
    return len(x)


liste.sort(key=sortFunc)
print(liste)

# Umkehren der Sortierreihenfolge
liste.sort(reverse=True, key=sortFunc)
print(liste)