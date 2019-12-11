import matplotlib.pyplot as pyplot
from sklearn import datasets

# Daten laden
dt_bc = datasets.load_breast_cancer()
bc = dt_bc.data[:, :2]

# Plot definieren
pyplot.hist(bc)
pyplot.title("Histogram of breast_cancer daatset")
pyplot.xlabel("X-Label")
pyplot.ylabel("Y-Label")

# Plot anzeigen
pyplot.show()
