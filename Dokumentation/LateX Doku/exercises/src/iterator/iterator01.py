# Lösung Aufgabe 1 Iteratoren 

#Erstellung einer Liste 
list =["triangle", "square", "circle", "rectangle"]
#Erzeugung des passenden Iterators zum String
iterator = iter(list)

#Zaehler Variable
counter=0

for element in iterator:
    print(element)
    counter=counter+1

print('Counter:')
print(counter)
