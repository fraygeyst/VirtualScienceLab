from sklearn import datasets

# Daten laden
datasetDigits = datasets.load_digits()

# targets wählen und ausgeben
t = datasetDigits.target[[22, 37, 54]]
print("Target (22,37,54): ")
print(t)

# target_names lesen und ausgeben
tn = list(datasetDigits.target_names)
print("Target names: ")
print(tn)
