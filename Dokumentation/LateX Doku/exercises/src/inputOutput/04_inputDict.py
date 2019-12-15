bodyList = []
bodyDict = {}

print("Füllen Sie mittels Eingabe eine" 
    + "Liste mit geometrischen Körpern")
print("Bei Eingabe 'exit' wird " + 
    "die Eingabeschleife beendet.\n")

while True:
    inputString = input("Eingabe: ")
    if inputString == "exit":
        break
    bodyList.append(inputString)

print("Geben Sie die Eckzahl zum jeweiligen Objekt an.")

for body in bodyList:
    inputString = input(body + ": ")
    bodyDict.update({inputString : body})

for body in bodyDict:
    print(body + " " + bodyDict[body])






