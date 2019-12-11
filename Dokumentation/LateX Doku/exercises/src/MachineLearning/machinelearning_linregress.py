import numpy as numpy
from sklearn import datasets
from scipy.stats import linregress
import matplotlib.pyplot as plt

# Daten laden
dt = datasets.load_wine()
wine = dt.data[:, :6]
datasetny = numpy.array(wine)

# Lineare Regression berechnen
b, a, r, p, std = linregress(datasetny[3,], datasetny[5,])

# Plot erzeugen
plt.scatter(datasetny[3,], datasetny[5,])
plt.plot([0, 130], [a, a + 130 * b], c="red", alpha=0.3)
plt.plot()
plt.title("Lineare Regression")
plt.xlim(0, 25)
plt.ylim(0, 25)
plt.xlabel("Alcohol")
plt.ylabel("Magnesium")
plt.grid(alpha=0.3)
plt.show()
