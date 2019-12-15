# Beispiel 04 Iteratoren - Erstellung eines eigenen Iterators

def reverse(data):
   #Hinweis: Die range(start, stop, step)-Anweisung
   #generiert eine Liste mit Nummern
   #von start bis stop mit einer Schrittweite von step 
    for index in range(len(data)-1, -1, -1):
        yield data[index]


#Aufruf des Generators und Ausgabe der gelieferten Elemente
string = 'ILoveTriangles'      
for char in reverse(string):
    print(char)
