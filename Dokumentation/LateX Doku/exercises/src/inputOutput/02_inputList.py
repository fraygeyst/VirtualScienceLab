inputList = []

print("Füllen Sie mittels Eingabe eine" 
    + "Liste mit geometrischen Körpern")
print("Bei Eingabe 'exit' wird " + 
    "die Eingabeschleife beendet.\n")

while True:
    inputString = input("Eingabe: ")
    if inputString == "exit":
        break
    inputList.append(inputString)

print("\nDie geometrischen Körper lauten:")
for x in inputList:
    print(x)
