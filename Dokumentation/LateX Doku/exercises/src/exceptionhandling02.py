try:
    numb = int(input("Bitte eine Zahl eingeben: "))
    res = numb * numb
except ValueError as e:
    print("error message: ", e)
    print("Die Eingabe war keine Zahl!")
    print("Das Programm wird beendet.")
else:
    print("Die Zahl " + str(numb)
          + " ergibt quadriert: " + str(res))

# Ausgabe:
# Bitte eine Zahl eingeben: 5
# Die Zahl 5 ergibt quadriert: 25

# Alternative Ausgabe:
# Bitte eine Zahl eingeben: a
# error message:  invalid literal for int() with base 10: 'a'
# Die Eingabe war keine Zahl!
# Das Programm wird beendet.