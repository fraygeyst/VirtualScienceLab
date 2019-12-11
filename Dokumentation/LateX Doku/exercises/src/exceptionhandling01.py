a = range(11)  # Laenge 11
b = range(11, 22)  # Laenge 11

i = 10
try:
    file = open("result.txt", "w")
    while i >= 0:
        res = round(b[i] / a[i], 2)
        file.write("Ergebnis: " + str(b[i]) + " / "
                   + str(a[i]) + " = " + str(res) + "\n")
        i = i - 1
except (ZeroDivisionError, FileNotFoundError) as e:
    print("error message: ", e)
    print("Das Programm wird beendet.")
finally:
    file.close()
    print("Die Datei wurde geschlossen.")
# Ausgabe auf der Konsole:
# error message:  division by zero
# Das Programm wird beendet.
# Die Datei wurde geschlossen.

# Dateiinhalt der result.txt:
# Ergebnis: 21 / 10 = 2.1
# Ergebnis: 20 / 9 = 2.22
# Ergebnis: 19 / 8 = 2.38
# Ergebnis: 18 / 7 = 2.57
# Ergebnis: 17 / 6 = 2.83
# Ergebnis: 16 / 5 = 3.2
# Ergebnis: 15 / 4 = 3.75
# Ergebnis: 14 / 3 = 4.67
# Ergebnis: 13 / 2 = 6.5
# Ergebnis: 12 / 1 = 12.0