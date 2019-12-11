class Kreis:
    def __init__(self, radius, farbe):
        self.radius = radius
        self.farbe = farbe

    def getRadius(self):
        return self.radius

    def setRadius(self, radius):
        self.radius = radius

    def getFarbe(self):
        return self.farbe

    def setFarbe(self, farbe):
        self.farbe = farbe

    def getKreis(self):
        print(self.radius, self.farbe)
