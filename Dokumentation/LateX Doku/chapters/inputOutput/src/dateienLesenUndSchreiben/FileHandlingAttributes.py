# chapters/inputOutput/src/dateienLesenUndSchreiben
# /FileHandlingAttributes.py
# Attribute des Datei-Objekts

with open("datei.txt", "r") as file:
    print(file.closed)
    print(file.mode)
    print(file.name)

# Ausgabe:
# False
# r
# datei.txt
