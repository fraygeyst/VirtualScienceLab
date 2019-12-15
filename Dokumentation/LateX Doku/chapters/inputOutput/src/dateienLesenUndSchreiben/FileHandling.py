# chapters/inputOutput/src/dateienLesenUndSchreiben
# /FileHandling.py
# Typ des Datei-Objekts festlegen
# Beispiel am "r"-Modus

file = open("datei.txt", "rt")  # Text
# oder
file = open("datei.txt", "r")  # Text

file = open("datei.txt", "rb")  # Binaer
file.close()