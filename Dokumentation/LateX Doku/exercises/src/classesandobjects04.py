class Fahrzeug:
    def __init__(self, farbe, leistung):
        self.farbe = farbe
        self.leistung = leistung
        self.kilometerstand = 0

    def fahren(self, kilometer):
        if kilometer > 0:
            self.kilometerstand += kilometer

class Auto(Fahrzeug):
    def __init__(self, kennzeichen, farbe, leistung):
        Fahrzeug.__init__(self, farbe, leistung)
        self.kennzeichen = kennzeichen

class LKW(Fahrzeug):
    def __init__(self, kennzeichen, farbe, leistung):
        Fahrzeug.__init__(self, farbe, leistung)
        self.kennzeichen = kennzeichen

class Motorrad(Fahrzeug):
    def __init__(self, kennzeichen, farbe, leistung):
        Fahrzeug.__init__(self, farbe, leistung)
        self.kennzeichen = kennzeichen
