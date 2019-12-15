# exercises/src/testmod.py
# Zu testende Funktion im Kapitel Testen


def testDiv(a, b):
    if(b == 0):
        raise ZeroDivisionError()
    return a // b


def testMod(a, b):
    if(b == 0):
        raise ZeroDivisionError()
    return a % b
