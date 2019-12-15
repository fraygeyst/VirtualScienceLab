import matplotlib.pyplot as pyplot
from sklearn import datasets

# Daten laden
dt_bc = datasets.load_breast_cancer()
bc = dt_bc.data[:25, :3]

# Plot definieren
fig = pyplot.figure()
ax = fig.add_subplot(111)

# Plot mit Daten versorgen
ax.scatter(bc[:, 0], bc[:, 1], bc[:, 2],
           c = 'r', marker = '*')

# Achsenbeschriftungen festlegen
ax.set_title("Scatter plot from "
             "breast_cancer dataset")
ax.set_xlabel("X-Label")
ax.set_ylabel("Y-Label")

# Plot anzeigen
pyplot.show()
