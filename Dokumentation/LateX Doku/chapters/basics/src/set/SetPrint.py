# chapters/basics/src/set/SetPrint.py
# Ausgabe des Inhalts eines Set auf der Konsole

set1 = {1, 2, 3}
print(set1)
for x in set1:
    print(x)
print(set1[0])  # ERROR
set1[1] = 4  # ERROR