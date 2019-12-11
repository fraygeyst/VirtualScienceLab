import matplotlib.pyplot as pyplot
from sklearn import datasets

# Daten laden
dt_bc = datasets.load_breast_cancer()
bc = dt_bc.data[:20, :2]

# Plot definieren
pyplot.plot(bc)
pyplot.title("Plot from dataset breast_cancer")
pyplot.xlabel("X-Label")
pyplot.ylabel("Y-Label")

# Plot anzeigen
pyplot.show()
