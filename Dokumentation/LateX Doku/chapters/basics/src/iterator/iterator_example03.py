# Beispiel 03 Iteratoren - Erstellung eines eigenen Iterators

class Reverse:
#Klasse, welche Daten haelt und selbst als Iterator fungiert
#und Elemente in umgekehrter Reihenfolge zurückliefert
    
    def __init__(self, data):
        self.data = data
        self.index = len(data)

      #Objekt gibt sich selbst als Iterator zueruck 
    def __iter__(self):
        return self

    def __next__(self):
      # Abbruch falls Ende der Daten erreicht
        if self.index == 0:
            raise StopIteration
      #Vermindere Index bei jedem Aufruf von next()
        self.index = self.index - 1

      # Rueckgabe des naechsten Elements
        return self.data[self.index]
