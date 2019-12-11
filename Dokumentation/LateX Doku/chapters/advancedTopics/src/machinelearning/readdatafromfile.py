# Bibliothek einbinden
import numpy as numpy		

def readDataSet(filename):

	# Datei-Stream vorbereiten
	fr = open(filename)

	# Anzahl der Zeilen ermitteln
	numberOfLines = len(fr.readlines())

	# Eine Numpy-Matrix erzeugen
	mymatrix = numpy.zeros((numberOfLines-1,3))	

	fr = open(filename)
	index = 0

	# Zeilen nach und nach aus Datei lesen
	for line in fr.readlines():	
		# Kopfzeile weg lassen
		if index != 0:				
			line = line.strip()
			# Zeile in temporäre Liste splitten
			listFromLine = (line.split(';')) 
			
			# Liste in Matrix einfügen
			mymatrix[index-1,:] = 
				listFromLine[1:4]		
			
			# Kategorien
			classLabel = listFromLine[4] 
			
			if classLabel == "foo":
				color = 'yellow'
			elif classLabel == "blub":
				color = 'blue'
			else:
				color = 'red'
			
			# Kategorie als Text-Label
			classLabelVector.append(classLabel) 
			classColorVector.append(color)
			
			index += 1
				
	return mymatrix, classLabelVector, classColorVector


# Aufruf der Methode
dataset, classLabelVector, classColorVector =
	readDataSet("SampleFile.txt")