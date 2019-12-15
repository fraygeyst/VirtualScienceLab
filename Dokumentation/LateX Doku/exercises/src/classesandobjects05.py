class Fahrzeug:
    counter = 0

    def __init__(self, farbe, leistung):
        self.farbe = farbe
        self.leistung = leistung
        self.kilometerstand = 0
        Fahrzeug.counter += 1

    def __del__(self):
        Fahrzeug.counter -= 1

    def fahren(self, kilometer):
        if kilometer > 0:
            self.kilometerstand += kilometer

class Auto(Fahrzeug):
    counter = 0

    def __init__(self, kennzeichen, farbe, leistung):
        Fahrzeug.__init__(self, farbe, leistung)
        self.kennzeichen = kennzeichen
        Auto.counter += 1

    def __del__(self):
        Fahrzeug.__del__(self)
        Auto.counter -= 1

class LKW(Fahrzeug):
    counter = 0

    def __init__(self, kennzeichen, farbe, leistung):
        Fahrzeug.__init__(self, farbe, leistung)
        self.kennzeichen = kennzeichen
        LKW.counter += 1

    def __del__(self):
        Fahrzeug.__del__(self)
        LKW.counter -= 1

class Motorrad(Fahrzeug):
    counter = 0

    def __init__(self, kennzeichen, farbe, leistung):
        Fahrzeug.__init__(self, farbe, leistung)
        self.kennzeichen = kennzeichen
        Motorrad.counter += 1

    def __del__(self):
        Fahrzeug.__del__(self)
        Motorrad.counter -= 1
